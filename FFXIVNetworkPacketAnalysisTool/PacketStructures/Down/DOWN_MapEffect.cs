using System.Runtime.InteropServices;

namespace FFXIVNetworkPacketAnalysisTool.PacketStructures;

/// <summary>
/// 地图效果包结构体
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0x10)]
public struct DOWN_MapEffect
{
    [FieldOffset(0x00)] public uint DirectorId;
    [FieldOffset(0x04)] public ushort State;
    [FieldOffset(0x06)] public ushort PlayState;
    [FieldOffset(0x08)] public byte Index;
}
