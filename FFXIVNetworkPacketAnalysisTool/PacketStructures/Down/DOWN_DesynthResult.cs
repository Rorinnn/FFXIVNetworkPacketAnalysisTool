using System.Runtime.InteropServices;

namespace FFXIVNetworkPacketAnalysisTool.PacketStructures;

/// <summary>
/// 分解结果包
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0x20)]
public struct DOWN_DesynthResult
{
    [FieldOffset(0x00)] public uint ItemId;
    [FieldOffset(0x04)] public uint ResultItemId;
    [FieldOffset(0x08)] public uint ResultItemCount;
}
