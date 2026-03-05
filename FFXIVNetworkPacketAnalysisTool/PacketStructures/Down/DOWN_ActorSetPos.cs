using System.Runtime.InteropServices;

namespace FFXIVNetworkPacketAnalysisTool.PacketStructures;

/// <summary>
/// Actor设置位置包结构体
/// 参照 OmenTools ActorSetPosPacket
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0x18)]
public unsafe struct DOWN_ActorSetPos
{
    [FieldOffset(0x00)] public ushort Facing;
    [FieldOffset(0x02)] public byte TerritoryTransportType;
    [FieldOffset(0x03)] public byte CharacterMode;
    [FieldOffset(0x04)] public uint TransitionTerritoryFilterKey;
    [FieldOffset(0x08)] public fixed float Pos[3];
}
