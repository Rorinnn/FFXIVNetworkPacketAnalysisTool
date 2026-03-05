using System.Runtime.InteropServices;

namespace FFXIVNetworkPacketAnalysisTool.PacketStructures;

/// <summary>
/// 货币/水晶信息包
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0x08)]
public struct DOWN_CurrencyCrystalInfo
{
    [FieldOffset(0x00)] public uint ContainerId;
    [FieldOffset(0x04)] public uint Quantity;
}
