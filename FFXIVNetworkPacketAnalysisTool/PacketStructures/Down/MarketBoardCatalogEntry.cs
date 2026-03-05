using System.Runtime.InteropServices;

namespace FFXIVNetworkPacketAnalysisTool.PacketStructures;

/// <summary>
/// 市场板搜索结果条目
/// 对应 Sapphire ZoneProtoDownCatalogSearchData
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0x08)]
public struct MarketBoardCatalogEntry
{
    [FieldOffset(0x00)] public uint ItemId;
    [FieldOffset(0x04)] public ushort ListingCount;
    [FieldOffset(0x06)] public ushort DemandCount;
}
