using System.Runtime.InteropServices;

namespace FFXIVNetworkPacketAnalysisTool.PacketStructures;

/// <summary>
/// 市场板购买请求
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0x20)]
public struct UP_MarketBoardPurchaseHandler
{
    [FieldOffset(0x00)] public ulong ListingId;
    [FieldOffset(0x08)] public uint ItemId;
    [FieldOffset(0x0C)] public uint Quantity;
    [FieldOffset(0x10)] public uint PricePerUnit;
}
