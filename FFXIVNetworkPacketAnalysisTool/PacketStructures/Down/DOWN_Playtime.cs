using System.Runtime.InteropServices;

namespace FFXIVNetworkPacketAnalysisTool.PacketStructures;

/// <summary>
/// 玩家在线时间包
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0x04)]
public struct DOWN_Playtime
{
    [FieldOffset(0x00)] public uint PlaytimeMinutes;
}
