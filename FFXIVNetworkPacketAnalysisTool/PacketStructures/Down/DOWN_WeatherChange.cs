using System.Runtime.InteropServices;

namespace FFXIVNetworkPacketAnalysisTool.PacketStructures;

/// <summary>
/// 天气变化包
/// 对应 Sapphire WeatherId
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0x08)]
public struct DOWN_WeatherChange
{
    [FieldOffset(0x00)] public byte WeatherId;
    [FieldOffset(0x04)] public float TransitionTime;
}
