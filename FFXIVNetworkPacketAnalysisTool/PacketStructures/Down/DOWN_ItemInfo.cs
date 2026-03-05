using System.Runtime.InteropServices;

namespace FFXIVNetworkPacketAnalysisTool.PacketStructures;

/// <summary>
/// 物品信息包（单个物品详情）
/// 对应 Sapphire NormalItem
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0x38)]
public unsafe struct DOWN_ItemInfo
{
    [FieldOffset(0x00)] public uint ContextId;
    [FieldOffset(0x04)] public ushort ContainerId;
    [FieldOffset(0x06)] public ushort SlotIndex;
    [FieldOffset(0x08)] public uint Quantity;
    [FieldOffset(0x0C)] public uint CatalogId;
    [FieldOffset(0x10)] public ulong SignatureId;
    [FieldOffset(0x18)] public byte Flags;        // HQ, Crafted, etc.
    [FieldOffset(0x1A)] public ushort Durability;
    [FieldOffset(0x1C)] public ushort Spiritbond;
    [FieldOffset(0x1E)] public byte Stain;
    [FieldOffset(0x20)] public uint GlamourId;
    [FieldOffset(0x24)] public fixed ushort Materia[5];
    [FieldOffset(0x2E)] public fixed byte MateriaGrade[5];
}
