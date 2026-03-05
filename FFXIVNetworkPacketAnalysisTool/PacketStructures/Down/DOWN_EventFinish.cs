using System.Runtime.InteropServices;

namespace FFXIVNetworkPacketAnalysisTool.PacketStructures;

/// <summary>
/// 事件结束包结构体
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0x10)]
public struct DOWN_EventFinish
{
    [FieldOffset(0x00)] public uint HandlerId;
    [FieldOffset(0x04)] public byte Type;
    [FieldOffset(0x05)] public byte Res;
    [FieldOffset(0x08)] public uint Arg;
}
