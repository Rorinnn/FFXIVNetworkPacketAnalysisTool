using System.Runtime.InteropServices;

namespace FFXIVNetworkPacketAnalysisTool.PacketStructures;

/// <summary>
/// 市场板单个商品条目（嵌套在 MarketBoardItemListing 中）
/// 对应 Sapphire ZoneProtoDownItemSearchData
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0x80)]
public unsafe struct MarketBoardListingEntry
{
    [FieldOffset(0x00)] public ulong ListingId;
    [FieldOffset(0x08)] public ulong RetainerId;
    [FieldOffset(0x10)] public ulong OwnerContentId;
    [FieldOffset(0x18)] public ulong ArtisanId;
    [FieldOffset(0x20)] public uint UnitPrice;
    [FieldOffset(0x24)] public uint TotalTax;
    [FieldOffset(0x28)] public uint Quantity;
    [FieldOffset(0x2C)] public uint ItemId;
    [FieldOffset(0x30)] public uint LastReviewTime;
    [FieldOffset(0x34)] public ushort ContainerId;
    [FieldOffset(0x36)] public ushort SlotId;
    [FieldOffset(0x38)] public ushort Durability;
    [FieldOffset(0x3A)] public ushort Spiritbond;
    [FieldOffset(0x3C)] public fixed ushort Materia[5];
    [FieldOffset(0x46)] public fixed byte RetainerName[32];
    [FieldOffset(0x66)] public byte IsHq;
    [FieldOffset(0x67)] public byte MateriaCount;
    [FieldOffset(0x69)] public byte TownId;
    [FieldOffset(0x6A)] public byte Stain0Id;
    [FieldOffset(0x6B)] public byte Stain1Id;
}
