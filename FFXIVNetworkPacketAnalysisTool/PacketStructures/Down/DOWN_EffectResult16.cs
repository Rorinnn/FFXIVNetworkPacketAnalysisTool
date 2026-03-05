using System.Runtime.InteropServices;

namespace FFXIVNetworkPacketAnalysisTool.PacketStructures;

/// <summary>
/// 效果结果16包（16条目版）
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0x584)]
public unsafe struct DOWN_EffectResult16
{
    [FieldOffset(0x00)] public byte Count;
    [FieldOffset(0x04)] public fixed byte Results[1408]; // 16 × 0x58
}
