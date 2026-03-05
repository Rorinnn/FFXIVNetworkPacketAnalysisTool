using System.Runtime.InteropServices;

namespace FFXIVNetworkPacketAnalysisTool.PacketStructures;

/// <summary>
/// 队伍更新包结构体（karashiiro: UpdateParty）
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0xE18)]
public unsafe struct DOWN_UpdateParty
{
    [FieldOffset(0x00)] public fixed byte Members[3584]; // 8 个 PartyMember (8 * 0x1C0)
    [FieldOffset(0xE00)] public ulong PartyId;
    [FieldOffset(0xE08)] public ulong ChatChannel;
    [FieldOffset(0xE10)] public byte LeaderIndex;
    [FieldOffset(0xE11)] public byte PartyCount;
}
