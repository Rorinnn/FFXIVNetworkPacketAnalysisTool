using System.Runtime.InteropServices;

namespace FFXIVNetworkPacketAnalysisTool.PacketStructures;

/// <summary>
/// 市场板商品列表包（服务器→客户端）
/// 返回当前搜索条件下的商品列表，最多 10 个条目/包
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0x508)]
public unsafe struct DOWN_MarketBoardItemListing
{
    [FieldOffset(0x00)] public fixed byte Listings[1280]; // 10 × MarketBoardListingEntry (10 × 0x80)
    [FieldOffset(0x500)] public byte ListingCount;
    [FieldOffset(0x501)] public byte RequestId;
    [FieldOffset(0x502)] public byte RequestKey;
}
