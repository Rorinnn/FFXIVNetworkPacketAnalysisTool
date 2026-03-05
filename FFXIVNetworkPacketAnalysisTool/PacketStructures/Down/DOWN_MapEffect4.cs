using System.Runtime.InteropServices;

namespace FFXIVNetworkPacketAnalysisTool.PacketStructures;

/// <summary>
/// 地图效果包（4条目版）
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0x40)]
public unsafe struct DOWN_MapEffect4
{
    [FieldOffset(0x00)] public uint DirectorId;
    [FieldOffset(0x04)] public byte Count;
    [FieldOffset(0x08)] public fixed uint States[4];
    [FieldOffset(0x18)] public fixed uint PlayStates[4];
    [FieldOffset(0x28)] public fixed byte Indexes[4];
}
