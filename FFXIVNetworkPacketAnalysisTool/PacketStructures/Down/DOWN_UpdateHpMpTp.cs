using System.Runtime.InteropServices;

namespace FFXIVNetworkPacketAnalysisTool.PacketStructures;

/// <summary>
/// 更新HP/MP/GP包结构体
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0x08)]
public struct DOWN_UpdateHpMpTp
{
    [FieldOffset(0x00)] public uint Hp;
    [FieldOffset(0x04)] public ushort Mp;
    [FieldOffset(0x06)] public ushort Gp;
}
