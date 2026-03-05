using System.Runtime.InteropServices;

namespace FFXIVNetworkPacketAnalysisTool.PacketStructures;

/// <summary>
/// Actor量表包结构体（职业特殊量表）
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0x10)]
public unsafe struct DOWN_ActorGauge
{
    [FieldOffset(0x00)] public fixed byte Buffer[16];
}
