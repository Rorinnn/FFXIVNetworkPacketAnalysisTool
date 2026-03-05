using System.Runtime.InteropServices;

namespace FFXIVNetworkPacketAnalysisTool.PacketStructures;

/// <summary>
/// 玩家属性包（50 个 Param + 6 个原始值）
/// 对应 Sapphire BaseParam
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0xE0)]
public unsafe struct DOWN_PlayerStats
{
    [FieldOffset(0x00)] public fixed uint Param[50];   // STR, DEX, VIT, INT, MND 等
    [FieldOffset(0xC8)] public fixed uint OriginalParam[6];
}
