using System.Runtime.InteropServices;

namespace FFXIVNetworkPacketAnalysisTool.PacketStructures;

/// <summary>
/// 设置搜索信息处理器
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0xD0)]
public unsafe struct UP_SetSearchInfoHandler
{
    [FieldOffset(0x00)] public ulong OnlineStatus;
    [FieldOffset(0x08)] public ulong SelectClassId;
    [FieldOffset(0x10)] public byte Region;
    [FieldOffset(0x11)] public fixed byte SearchComment[193];
}
