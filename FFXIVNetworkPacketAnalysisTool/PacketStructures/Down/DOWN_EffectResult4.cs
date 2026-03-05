using System.Runtime.InteropServices;

namespace FFXIVNetworkPacketAnalysisTool.PacketStructures;

/// <summary>
/// 效果结果4包（4条目版）
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0x164)]
public unsafe struct DOWN_EffectResult4
{
    [FieldOffset(0x00)] public byte Count;
    [FieldOffset(0x04)] public fixed byte Results[352]; // 4 × 0x58
}
