using System.Runtime.InteropServices;

namespace FFXIVNetworkPacketAnalysisTool.PacketStructures;

/// <summary>
/// 查看搜索信息包
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0xD0)]
public unsafe struct DOWN_ExamineSearchInfo
{
    [FieldOffset(0x00)] public uint EntityId;
    [FieldOffset(0x04)] public fixed byte SearchComment[193];
}
