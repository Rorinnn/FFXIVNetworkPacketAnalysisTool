using System.Runtime.InteropServices;

namespace FFXIVNetworkPacketAnalysisTool.PacketStructures;

/// <summary>
/// 批量开始动作时间线包结构体
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0x40)]
public unsafe struct DOWN_StartActionTimelineMulti
{
    [FieldOffset(0x00)] public fixed uint Ids[10];
    [FieldOffset(0x28)] public fixed ushort TimelineIds[10];
}
