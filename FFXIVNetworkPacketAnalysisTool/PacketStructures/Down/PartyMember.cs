using System.Runtime.InteropServices;

namespace FFXIVNetworkPacketAnalysisTool.PacketStructures;

/// <summary>
/// 队伍成员信息结构体
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0x1C0)]
public unsafe struct PartyMember
{
    [FieldOffset(0x00)] public fixed byte NameBytes[32];
    [FieldOffset(0x20)] public ulong AccountId;
    [FieldOffset(0x28)] public ulong CharacterId;
    [FieldOffset(0x30)] public uint ActorId;
    [FieldOffset(0x34)] public uint PetId;
    [FieldOffset(0x38)] public uint BuddyId;
    [FieldOffset(0x3C)] public uint CurrentHp;
    [FieldOffset(0x40)] public uint MaxHp;
    [FieldOffset(0x44)] public ushort CurrentMp;
    [FieldOffset(0x46)] public ushort MaxMp;
    [FieldOffset(0x48)] public ushort HomeWorldId;
    [FieldOffset(0x4A)] public ushort TerritoryId;
    [FieldOffset(0x4C)] public byte Flag;
    [FieldOffset(0x4D)] public byte ClassJob;
    [FieldOffset(0x4E)] public byte Sex;
    [FieldOffset(0x4F)] public byte Level;
    [FieldOffset(0x50)] public byte LevelSync;
    [FieldOffset(0x51)] public byte PlatformType;
    [FieldOffset(0x54)] public fixed byte Status[360]; // 30 个 Status
}
