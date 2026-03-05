using System.Runtime.InteropServices;

namespace FFXIVNetworkPacketAnalysisTool.PacketStructures;

/// <summary>
/// NPC 生成包结构体
/// 此结构体定义了 DOWN_NpcSpawn 包的数据布局
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0x290)]
public unsafe struct DOWN_NpcSpawn
{
    [FieldOffset(0x10)] public ulong MainTarget;
    [FieldOffset(0x40)] public uint DataID;
    [FieldOffset(0x44)] public uint OwnerID;
    [FieldOffset(0x54)] public uint FlagOrState;
    [FieldOffset(0x5C)] public uint CurrentHp;
    [FieldOffset(0x60)] public uint MaxHp;
    [FieldOffset(0x72)] public float Rotation;
    [FieldOffset(0x7F)] public byte ModelScale;
    [FieldOffset(0x81)] public byte ObjectKind;
    [FieldOffset(0x82)] public byte ObjectType;
    [FieldOffset(0x85)] public byte BattalionType;
    [FieldOffset(0x86)] public byte Level;
    [FieldOffset(0x200)] public float PosX;
    [FieldOffset(0x204)] public float PosY;
    [FieldOffset(0x208)] public float PosZ;
    [FieldOffset(0x242)] public fixed byte NameBytes[32];
}
