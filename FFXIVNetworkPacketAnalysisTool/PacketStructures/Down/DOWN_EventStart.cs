using System.Runtime.InteropServices;

namespace FFXIVNetworkPacketAnalysisTool.PacketStructures;

/// <summary>
/// 事件开始包结构体
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0x18)]
public struct DOWN_EventStart
{
    [FieldOffset(0x00)] public ulong TargetCommonId;
    [FieldOffset(0x08)] public uint HandlerId;
    [FieldOffset(0x0C)] public byte Type;
    [FieldOffset(0x0D)] public byte Flags;
    [FieldOffset(0x10)] public uint Arg;
}
