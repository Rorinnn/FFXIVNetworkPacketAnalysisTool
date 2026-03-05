using System.Runtime.InteropServices;

namespace FFXIVNetworkPacketAnalysisTool.PacketStructures;

/// <summary>
/// NPC生成2包结构体（扩展状态）
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0x3F0)]
public unsafe struct DOWN_NpcSpawn2
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

    // 扩展状态数组（30 个 Status，从 0x284 开始）
    [FieldOffset(0x284)] public fixed byte ExpandStatus[360]; // 30 * 0x0C
}
