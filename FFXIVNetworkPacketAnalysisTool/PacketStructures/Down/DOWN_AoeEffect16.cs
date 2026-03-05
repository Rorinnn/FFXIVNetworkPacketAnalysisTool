using System.Runtime.InteropServices;

namespace FFXIVNetworkPacketAnalysisTool.PacketStructures;

/// <summary>
/// 16目标AOE技能效果包结构体
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0x4B8)]
public unsafe struct DOWN_AoeEffect16
{
    [FieldOffset(0x00)] public uint MainTargetId;
    [FieldOffset(0x08)] public uint RealActionId;
    [FieldOffset(0x0C)] public uint ResponseId;
    [FieldOffset(0x10)] public float LockTime;
    [FieldOffset(0x14)] public uint BallistaTargetId;
    [FieldOffset(0x18)] public ushort RequestId;
    [FieldOffset(0x1A)] public ushort Facing;
    [FieldOffset(0x1C)] public ushort ActionId;
    [FieldOffset(0x1E)] public byte ActionVariant;
    [FieldOffset(0x1F)] public byte ActionKind;
    [FieldOffset(0x20)] public byte Flag;
    [FieldOffset(0x21)] public byte TargetCount;
    [FieldOffset(0x2A)] public fixed byte Effects[1024];
    [FieldOffset(0x430)] public fixed ulong TargetIds[16];
    [FieldOffset(0x4B0)] public fixed ushort Pos[3];

    public void ApplyPacketFix(uint baseValue) => RealActionId += baseValue;
}
