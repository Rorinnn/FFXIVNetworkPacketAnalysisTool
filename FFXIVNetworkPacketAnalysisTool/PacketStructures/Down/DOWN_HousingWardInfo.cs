using System.Runtime.InteropServices;

namespace FFXIVNetworkPacketAnalysisTool.PacketStructures;

/// <summary>
/// 住房信息包
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0x08)]
public struct DOWN_HousingWardInfo
{
    [FieldOffset(0x00)] public ushort LandSetId;
    [FieldOffset(0x02)] public ushort WardId;
    [FieldOffset(0x04)] public ushort TerritoryType;
    [FieldOffset(0x06)] public ushort WorldId;
}
