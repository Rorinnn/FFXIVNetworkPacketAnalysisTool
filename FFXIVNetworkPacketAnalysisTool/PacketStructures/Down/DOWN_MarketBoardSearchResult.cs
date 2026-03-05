using System.Runtime.InteropServices;

namespace FFXIVNetworkPacketAnalysisTool.PacketStructures;

/// <summary>
/// 市场板搜索结果包
/// 对应 Sapphire CatalogSearchResult
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0xB0)]
public unsafe struct DOWN_MarketBoardSearchResult
{
    [FieldOffset(0x00)] public fixed byte Items[160]; // 20 × MarketBoardCatalogEntry (20 × 0x08)
    [FieldOffset(0xA0)] public uint NextIndex;
    [FieldOffset(0xA4)] public uint Result;
    [FieldOffset(0xA8)] public uint Index;
    [FieldOffset(0xAC)] public byte RequestKey;
    [FieldOffset(0xAD)] public byte Type;
}
