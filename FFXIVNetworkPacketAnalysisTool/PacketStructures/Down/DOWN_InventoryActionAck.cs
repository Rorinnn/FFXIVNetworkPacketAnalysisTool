using System.Runtime.InteropServices;

namespace FFXIVNetworkPacketAnalysisTool.PacketStructures;

/// <summary>
/// 物品栏操作确认包
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0x10)]
public struct DOWN_InventoryActionAck
{
    [FieldOffset(0x00)] public uint ContextId;
    [FieldOffset(0x04)] public byte OperationType;
    [FieldOffset(0x05)] public byte ErrorType;
    [FieldOffset(0x06)] public byte PacketNum;
}
