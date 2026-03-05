using Dalamud.Hooking;
using FFXIVClientStructs.FFXIV.Application.Network;
using FFXIVClientStructs.FFXIV.Client.Network;
using FFXIVClientStructs.FFXIV.Client.System.Framework;
using FFXIVNetworkPacketAnalysisTool;
using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Runtime.InteropServices;

namespace FFXIVNetworkPacketAnalysisTool.Utils;

/// <summary>
/// 网络包收发 Hook 管理器，负责拦截并记录游戏网络数据包。
/// </summary>
public unsafe class NetRe : IDisposable
{
    private readonly Configuration configuration;

    public ConcurrentQueue<PacketInfo> PacketQueue { get; } = new(); // 已捕获的待处理包队列。

    public bool CaptureEnabled { get; set; } = true; // 是否启用包捕获。

    private const int MaxQueueSize = 10000;
    private readonly ConcurrentDictionary<string, int> packetLengthCache = new();

    private static readonly CompSig sendPacketInternalSig =
        new("48 83 EC ?? 48 8B 89 ?? ?? ?? ?? 48 85 C9 74 ?? 44 89 44 24 ?? 4C 8D 44 24 ?? 44 89 4C 24 ?? 44 0F B6 4C 24");
    private delegate bool SendPacketInternalDelegate(ZoneClient* module, nint packet, uint a3, uint a4, bool priority);
    private static Hook<SendPacketInternalDelegate>? sendPacketInternalHook;

    private delegate void ReceivePacketInternalDelegate(PacketDispatcher* dispatcher, uint targetID, byte* packet);
    private static Hook<ReceivePacketInternalDelegate>? receivePacketInternalHook;

    private delegate bool ManualSendDelegate(NetworkModuleProxy* module, byte* packet, uint a3, uint a4);
    private static readonly ManualSendDelegate? manualSend =
        new CompSig("E8 ?? ?? ?? ?? 48 8B D6 48 8B CF E8 ?? ?? ?? ?? 48 8B 8C 24").GetDelegate<ManualSendDelegate>();

    public unsafe nint GetVFuncByName<T>(T* vtablePtr, string fieldName) where T : unmanaged // 通过字段名从虚函数表指针中获取对应的函数地址。
    {
        var vtType = typeof(T);
        var fi = vtType.GetField(fieldName, BindingFlags.Public | BindingFlags.Instance);
        if (fi == null)
            throw new MissingFieldException(vtType.FullName, fieldName);

        var offAttr = fi.GetCustomAttribute<FieldOffsetAttribute>();
        if (offAttr == null)
            throw new InvalidOperationException($"Field {fieldName} has no FieldOffset");

        var offset = offAttr.Value;

        return *(nint*)((byte*)vtablePtr + offset);
    }

    public NetRe(Configuration configuration) // 初始化网络 Hook 并开始监听收发包。
    {
        this.configuration = configuration;
        sendPacketInternalHook ??= sendPacketInternalSig.GetHook<SendPacketInternalDelegate>(SendPacketInternalDetour);
        sendPacketInternalHook.Enable();

        receivePacketInternalHook ??=
            Plugin.Hook.HookFromAddress<ReceivePacketInternalDelegate>(GetVFuncByName(PacketDispatcher.StaticVirtualTablePointer, "OnReceivePacket"),
                                                                         ReceivePacketInternalDetour);
        receivePacketInternalHook.Enable();


        Plugin.Log.Debug("FFXIVNetworkPacketAnalysisTool 加载完成。");
    }

    public void Dispose() // 释放所有网络 Hook 资源。
    {
        sendPacketInternalHook?.Dispose();
        sendPacketInternalHook = null;

        receivePacketInternalHook?.Dispose();
        receivePacketInternalHook = null;
        Plugin.Log.Debug("FFXIVNetworkPacketAnalysisTool Successfully Uninstalled.");
    }

    public interface IGamePacket // 可发送的游戏数据包接口。
    {
        public string Log(); // 返回数据包的日志描述文本。

        public void Send(); // 将数据包发送至服务器。
    }

    public void SendPacket<T>(T data) where T : unmanaged, IGamePacket => // 向服务器发送指定结构体的数据包。
        manualSend!(Framework.Instance()->NetworkModuleProxy, (byte*)&data, 0, 0x114514);

    private bool SendPacketInternalDetour(ZoneClient* module, nint packet, uint a3, uint a4, bool priority)
    {
        // 优先让原始数据通过，减少游戏延迟
        var original = sendPacketInternalHook!.Original(module, packet, a3, a4, priority);

        // 在原始调用之后再进行捕获和日志（异步处理，不阻塞游戏）
        if (CaptureEnabled)
        {
            var pktPtr = (byte*)packet;
            var opcode = *(ushort*)pktPtr;
            var opcodeName = GetUpOpcodeName(opcode);

            // 尝试从结构体定义中获取包长度
            // 发包包体前有 0x10 (16) 字节的包头，结构体长度需要加上这个偏移
            var structLength = *(uint*)(pktPtr + 8); // GetPacketLengthFromStruct(opcodeName);
            var length = structLength + 0x20; // 加上包头长度

            CapturePacket(opcode, pktPtr, (int)length, (ushort)(priority ? 1 : 0), PacketDirection.Send);
        }

        return original;
    }

    private void ReceivePacketInternalDetour(PacketDispatcher* dispatcher, uint targetID, byte* packet)
    {
        packet -= 16;

        // 优先让原始数据通过，减少游戏延迟
        receivePacketInternalHook!.Original(dispatcher, targetID, packet + 16);

        // 在原始调用之后再进行捕获和日志（异步处理，不阻塞游戏）
        if (CaptureEnabled)
        {
            var opcode = Marshal.ReadInt16((nint)packet, 18);
            var opcodeName = GetDownOpcodeName((ushort)opcode);

            // 尝试从结构体定义中获取包长度
            // 收包包体前有 0x20 (32) 字节的包头，结构体长度需要加上这个偏移
            var structLength = GetPacketLengthFromStruct(opcodeName);
            var length = structLength + 0x20; // 加上包头长度

            CapturePacket((ushort)opcode, packet, length, 0, PacketDirection.Receive, targetID);
        }
    }


    private string GetUpOpcodeName(ushort opcode)
        => configuration.UpOpcodes.TryGetValue(opcode, out var name) ? name : "Unknown";

    private string GetDownOpcodeName(ushort opcode)
        => configuration.DownOpcodes.TryGetValue(opcode, out var name) ? name : "Unknown";

    private void CapturePacket(ushort opcode, byte* packet, int length, ushort priority, PacketDirection direction, uint targetID = 0)
    {
        try
        {
            // 快速检查队列是否已满，避免过多的入队操作
            if (PacketQueue.Count >= MaxQueueSize)
            {
                // 队列满了就直接丢弃，不阻塞
                return;
            }

            // 限制复制的数据大小，减少内存分配
            int captureLength = Math.Min(length, 4096); // 限制到4KB以提高性能

            var packetInfo = new PacketInfo
            {
                SessionId = DateTime.Now.Ticks,
                Timestamp = DateTime.Now,
                Direction = direction,
                Opcode = opcode,
                OpcodeName = direction == PacketDirection.Send ? GetUpOpcodeName(opcode) : GetDownOpcodeName(opcode),
                RawData = PacketInfo.CloneData(packet, captureLength), // 快速复制
                PacketLength = (uint)length, // 记录实际长度
                Priority = priority,
                TargetID = targetID
            };

            PacketQueue.Enqueue(packetInfo);
        }
        catch
        {
            // 静默失败，不影响游戏性能
        }
    }

    private int GetPacketLengthFromStruct(string opcodeName) // 从结构体定义中获取包长度，找到对应结构体返回其 Size，否则返回默认值 512。
    {
        // 检查缓存
        if (packetLengthCache.TryGetValue(opcodeName, out var cachedLength))
        {
            return cachedLength;
        }

        // Unknown 的包直接返回默认值
        if (opcodeName == "Unknown")
        {
            packetLengthCache[opcodeName] = 512;
            return 512;
        }

        try
        {
            // 在当前程序集中查找对应的结构体
            var assembly = Assembly.GetExecutingAssembly();
            var types = assembly.GetTypes();

            foreach (var type in types)
            {
                // 检查类型名称是否匹配，且是值类型（struct）
                if (type.Name == opcodeName && type.IsValueType)
                {
                    // 获取 StructLayout 特性
                    var structLayoutAttr = type.GetCustomAttribute<StructLayoutAttribute>();
                    if (structLayoutAttr != null && structLayoutAttr.Size > 0)
                    {
                        int size = structLayoutAttr.Size;
                        packetLengthCache[opcodeName] = size;
                        Plugin.Log.Debug($"[包长度] 从结构体 {opcodeName} 获取长度: {size} 字节");
                        return size;
                    }

                    try // 如果没有显式指定 Size，使用 Marshal.SizeOf
                    {
                        int size = Marshal.SizeOf(type);
                        packetLengthCache[opcodeName] = size;
                        Plugin.Log.Debug($"[包长度] 从结构体 {opcodeName} 计算长度: {size} 字节");
                        return size;
                    }
                    catch
                    {
                        // Marshal.SizeOf 可能失败（例如包含托管类型）
                        break;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Plugin.Log.Warning($"查找结构体 {opcodeName} 时出错: {ex.Message}");
        }

        // 未找到结构体定义，使用默认值
        packetLengthCache[opcodeName] = 512;
        return 512;
    }

}
