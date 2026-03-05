using System.Runtime.InteropServices;

namespace FFXIVNetworkPacketAnalysisTool.PacketStructures;

/// <summary>
/// 进入区域初始化包
/// 对应 Sapphire InitZone
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0x30)]
public unsafe struct DOWN_InitZone
{
    [FieldOffset(0x00)] public ushort ZoneId;
    [FieldOffset(0x02)] public ushort TerritoryType;
    [FieldOffset(0x04)] public ushort TerritoryIndex;
    [FieldOffset(0x08)] public uint LayerSetId;
    [FieldOffset(0x0C)] public uint LayoutId;
    [FieldOffset(0x10)] public byte WeatherId;
    [FieldOffset(0x11)] public byte Flag;
    [FieldOffset(0x12)] public ushort FestivalId0;
    [FieldOffset(0x14)] public ushort FestivalPhase0;
    [FieldOffset(0x16)] public ushort FestivalId1;
    [FieldOffset(0x18)] public ushort FestivalPhase1;
    [FieldOffset(0x20)] public fixed float Pos[3]; // X, Y, Z
}
