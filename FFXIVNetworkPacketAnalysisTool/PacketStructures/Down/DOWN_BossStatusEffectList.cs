using System.Runtime.InteropServices;

namespace FFXIVNetworkPacketAnalysisTool.PacketStructures;

/// <summary>
/// 三层状态效果列表包（BOSS 等使用）
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0x168)]
public unsafe struct DOWN_BossStatusEffectList
{
    [FieldOffset(0x00)] public byte ClassJob;
    [FieldOffset(0x02)] public byte Level;
    [FieldOffset(0x04)] public uint Hp;
    [FieldOffset(0x08)] public uint HpMax;
    [FieldOffset(0x0C)] public ushort Mp;
    [FieldOffset(0x0E)] public ushort MpMax;
    [FieldOffset(0x10)] public ushort Shield;
    [FieldOffset(0x14)] public fixed byte Statuses[360]; // 30 × Status (30 × 0x0C)
}
