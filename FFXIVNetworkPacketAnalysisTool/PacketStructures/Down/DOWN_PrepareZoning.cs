using System.Runtime.InteropServices;

namespace FFXIVNetworkPacketAnalysisTool.PacketStructures;

/// <summary>
/// 准备转区包
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0x08)]
public struct DOWN_PrepareZoning
{
    [FieldOffset(0x00)] public uint LogMessageId;
    [FieldOffset(0x04)] public ushort TerritoryType;
}
