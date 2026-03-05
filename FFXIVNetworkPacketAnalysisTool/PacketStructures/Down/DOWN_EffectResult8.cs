using System.Runtime.InteropServices;

namespace FFXIVNetworkPacketAnalysisTool.PacketStructures;

/// <summary>
/// 效果结果8包（8条目版）
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0x2C4)]
public unsafe struct DOWN_EffectResult8
{
    [FieldOffset(0x00)] public byte Count;
    [FieldOffset(0x04)] public fixed byte Results[704]; // 8 × 0x58
}
