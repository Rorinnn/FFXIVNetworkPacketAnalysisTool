using System.Runtime.InteropServices;

namespace FFXIVNetworkPacketAnalysisTool.PacketStructures;

/// <summary>
/// 市场板购买结果包
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0x10)]
public struct DOWN_MarketBoardPurchase
{
    [FieldOffset(0x00)] public uint ItemId;
    [FieldOffset(0x04)] public uint Result;
    [FieldOffset(0x08)] public byte IsHq;
    [FieldOffset(0x09)] public byte MateriaCount;
    [FieldOffset(0x0A)] public byte Quantity;
}
