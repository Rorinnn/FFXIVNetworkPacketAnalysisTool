using System.Runtime.InteropServices;

namespace FFXIVNetworkPacketAnalysisTool.PacketStructures;

/// <summary>
/// 地图效果包（12条目版）
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0x80)]
public unsafe struct DOWN_MapEffect12
{
    [FieldOffset(0x00)] public uint DirectorId;
    [FieldOffset(0x04)] public byte Count;
    [FieldOffset(0x08)] public fixed uint States[12];
    [FieldOffset(0x38)] public fixed uint PlayStates[12];
    [FieldOffset(0x68)] public fixed byte Indexes[12];
}
