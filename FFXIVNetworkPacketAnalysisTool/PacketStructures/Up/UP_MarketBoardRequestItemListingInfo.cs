using System.Runtime.InteropServices;

namespace FFXIVNetworkPacketAnalysisTool.PacketStructures;

/// <summary>
/// 市场板请求商品列表信息
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0x08)]
public struct UP_MarketBoardRequestItemListingInfo
{
    [FieldOffset(0x00)] public uint ItemId;
    [FieldOffset(0x04)] public byte RequestId;
}
