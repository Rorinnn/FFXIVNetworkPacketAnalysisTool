using System.Runtime.InteropServices;

namespace FFXIVNetworkPacketAnalysisTool.PacketStructures;

/// <summary>
/// 市场板历史记录条目
/// 对应 Sapphire ZoneProtoDownItemHistoryData
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0x30)]
public unsafe struct MarketBoardHistoryEntry
{
    [FieldOffset(0x00)] public uint ItemId;
    [FieldOffset(0x04)] public uint SellPrice;
    [FieldOffset(0x08)] public uint PurchaseTime;
    [FieldOffset(0x0C)] public uint Quantity;
    [FieldOffset(0x10)] public byte IsHq;
    [FieldOffset(0x11)] public byte MateriaCount;
    [FieldOffset(0x12)] public fixed byte BuyerName[32];
}
