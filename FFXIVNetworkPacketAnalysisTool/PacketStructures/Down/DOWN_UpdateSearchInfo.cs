using System.Runtime.InteropServices;

namespace FFXIVNetworkPacketAnalysisTool.PacketStructures;

/// <summary>
/// 更新搜索信息包
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0x10)]
public struct DOWN_UpdateSearchInfo
{
    [FieldOffset(0x00)] public ulong OnlineStatus;
    [FieldOffset(0x08)] public ulong SelectClassId;
}
