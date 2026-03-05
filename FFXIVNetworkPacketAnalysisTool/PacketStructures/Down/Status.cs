using System.Runtime.InteropServices;

namespace FFXIVNetworkPacketAnalysisTool.PacketStructures;

/// <summary>
/// 状态（Buff/Debuff）结构体
/// 参照 OmenTools StatusEffectListEntry + Sapphire StatusWork
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0x0C)]
public struct Status
{
    [FieldOffset(0x00)] public ushort StatusId;
    [FieldOffset(0x02)] public short StackCount;
    [FieldOffset(0x04)] public float RemainingTime;
    [FieldOffset(0x08)] public uint SourceId;
}
