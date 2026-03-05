using System.Runtime.InteropServices;

namespace FFXIVNetworkPacketAnalysisTool.PacketStructures;

/// <summary>
/// 玩家生成包结构体
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0x278)]
public struct DOWN_PlayerSpawn
{
    [FieldOffset(0x00)] public ushort TitleId;
    [FieldOffset(0x02)] public ushort PlayingActionTimelineId;
    [FieldOffset(0x04)] public ushort WorldId;
    [FieldOffset(0x06)] public ushort HomeWorldId;
    [FieldOffset(0x08)] public byte GmLevel;
    [FieldOffset(0x09)] public byte GrandCompany;
    [FieldOffset(0x0A)] public byte GrandCompanyLevel;
    [FieldOffset(0x0B)] public byte OnlineStatus;
    [FieldOffset(0x0C)] public byte PoseEmote;
}
