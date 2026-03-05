using System.Runtime.InteropServices;

namespace FFXIVNetworkPacketAnalysisTool.PacketStructures;

/// <summary>
/// 物品栏事务完成包
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0x08)]
public struct DOWN_InventoryTransactionFinish
{
    [FieldOffset(0x00)] public uint ContextId;
    [FieldOffset(0x04)] public uint OperationId;
}
