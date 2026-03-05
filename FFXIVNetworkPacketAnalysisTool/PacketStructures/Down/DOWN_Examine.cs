using System.Runtime.InteropServices;

namespace FFXIVNetworkPacketAnalysisTool.PacketStructures;

/// <summary>
/// 查看详情包
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0x08)]
public struct DOWN_Examine
{
    [FieldOffset(0x00)] public uint TargetEntityId;
    [FieldOffset(0x04)] public byte ObjType;
}
