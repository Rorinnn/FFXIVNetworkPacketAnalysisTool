using System.Runtime.InteropServices;

namespace FFXIVNetworkPacketAnalysisTool.PacketStructures;

/// <summary>
/// 市场板商品列表数量包
/// 在请求商品列表前发送，告知客户端有多少条结果
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0x10)]
public struct DOWN_MarketBoardItemListingCount
{
    [FieldOffset(0x00)] public uint ItemId;
    [FieldOffset(0x04)] public uint Unknown1;
    [FieldOffset(0x08)] public ushort RequestId;
    [FieldOffset(0x0A)] public ushort Quantity;
}
