using System.Runtime.InteropServices;

namespace FFXIVNetworkPacketAnalysisTool.PacketStructures;

/// <summary>
/// 地图效果包（8条目版）
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0x60)]
public unsafe struct DOWN_MapEffect8
{
    [FieldOffset(0x00)] public uint DirectorId;
    [FieldOffset(0x04)] public byte Count;
    [FieldOffset(0x08)] public fixed uint States[8];
    [FieldOffset(0x28)] public fixed uint PlayStates[8];
    [FieldOffset(0x48)] public fixed byte Indexes[8];
}
