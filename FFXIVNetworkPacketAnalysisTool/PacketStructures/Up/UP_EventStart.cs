using System.Runtime.InteropServices;

namespace FFXIVNetworkPacketAnalysisTool.PacketStructures;

/// <summary>
/// 事件开始（客户端请求与NPC/事件对象交互）
/// 参照 DailyRoutines FieldEntryCommand 的 EventStartPackt 布局
/// 用于发起对话、进入副本等交互请求
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0x14)]
public struct UP_EventStart
{
    [FieldOffset(0x00)] public ulong GameObjectId;
    [FieldOffset(0x08)] public uint EventId;
    [FieldOffset(0x0C)] public uint Category;
    [FieldOffset(0x10)] public uint Param;
}
