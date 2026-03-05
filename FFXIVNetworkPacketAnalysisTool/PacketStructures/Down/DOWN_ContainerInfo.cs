using System.Runtime.InteropServices;

namespace FFXIVNetworkPacketAnalysisTool.PacketStructures;

/// <summary>
/// 容器信息包（新背包开始同步）
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0x10)]
public struct DOWN_ContainerInfo
{
    [FieldOffset(0x00)] public uint ContextId;
    [FieldOffset(0x04)] public uint ContainerSize;
    [FieldOffset(0x08)] public uint ContainerId;
    [FieldOffset(0x0C)] public ushort ContainerType;
}
