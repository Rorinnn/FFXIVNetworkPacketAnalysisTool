using System.Runtime.InteropServices;

namespace FFXIVNetworkPacketAnalysisTool.PacketStructures;

/// <summary>
/// 物品栏事务包
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0x30)]
public struct DOWN_InventoryTransaction
{
    [FieldOffset(0x00)] public uint ContextId;
    [FieldOffset(0x04)] public byte OperationType;
    [FieldOffset(0x08)] public uint SrcEntityId;
    [FieldOffset(0x0C)] public uint SrcContainerId;
    [FieldOffset(0x10)] public short SrcSlotIndex;
    [FieldOffset(0x14)] public uint SrcQuantity;
    [FieldOffset(0x18)] public uint SrcItemId;
    [FieldOffset(0x1C)] public uint DstEntityId;
    [FieldOffset(0x20)] public uint DstContainerId;
    [FieldOffset(0x24)] public short DstSlotIndex;
    [FieldOffset(0x28)] public uint DstQuantity;
    [FieldOffset(0x2C)] public uint DstItemId;
}
