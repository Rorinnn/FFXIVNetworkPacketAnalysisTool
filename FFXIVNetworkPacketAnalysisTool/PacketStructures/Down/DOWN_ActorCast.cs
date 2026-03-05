using System.Runtime.InteropServices;

namespace FFXIVNetworkPacketAnalysisTool.PacketStructures;

/// <summary>
/// Actor施法包结构体
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0x20)]
public unsafe struct DOWN_ActorCast
{
    [FieldOffset(0x00)] public ushort ActionId;
    [FieldOffset(0x02)] public byte ActionKind;
    [FieldOffset(0x03)] public byte DisplayDelay;
    [FieldOffset(0x04)] public uint RealActionId;
    [FieldOffset(0x08)] public float CastTime;
    [FieldOffset(0x0C)] public uint TargetId;
    [FieldOffset(0x10)] public ushort Facing;
    [FieldOffset(0x12)] public byte CanInterrupt;
    [FieldOffset(0x18)] public fixed ushort Pos[3];

    public void ApplyPacketFix(uint baseValue) => RealActionId += baseValue;
}
