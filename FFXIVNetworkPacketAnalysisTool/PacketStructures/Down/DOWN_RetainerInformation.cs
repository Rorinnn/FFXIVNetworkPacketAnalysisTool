using System.Runtime.InteropServices;

namespace FFXIVNetworkPacketAnalysisTool.PacketStructures;

/// <summary>
/// 雇员信息包
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0x48)]
public unsafe struct DOWN_RetainerInformation
{
    [FieldOffset(0x00)] public ulong RetainerId;
    [FieldOffset(0x08)] public byte HireOrder;
    [FieldOffset(0x09)] public byte ClassJob;
    [FieldOffset(0x0A)] public byte Level;
    [FieldOffset(0x0B)] public byte Padding;
    [FieldOffset(0x0C)] public uint Gil;
    [FieldOffset(0x10)] public byte CityId;
    [FieldOffset(0x11)] public byte MarketItemCount;
    [FieldOffset(0x14)] public uint VentureId;
    [FieldOffset(0x18)] public uint VentureCompleteTime;
    [FieldOffset(0x1C)] public fixed byte RetainerName[32];
}
