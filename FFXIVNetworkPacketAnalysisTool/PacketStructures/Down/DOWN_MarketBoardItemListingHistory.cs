using System.Runtime.InteropServices;

namespace FFXIVNetworkPacketAnalysisTool.PacketStructures;

/// <summary>
/// 市场板商品历史记录包
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0x3C4)]
public unsafe struct DOWN_MarketBoardItemListingHistory
{
    [FieldOffset(0x00)] public uint ItemId;
    [FieldOffset(0x04)] public fixed byte Entries[960]; // 20 × MarketBoardHistoryEntry (20 × 0x30)
}
