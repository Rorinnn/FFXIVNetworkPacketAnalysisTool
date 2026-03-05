using System.Runtime.InteropServices;

namespace FFXIVNetworkPacketAnalysisTool.PacketStructures;

/// <summary>
/// 放置场地标记包（单个标记）
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0x10)]
public struct DOWN_PlaceFieldMarker
{
    [FieldOffset(0x00)] public byte Active;
    [FieldOffset(0x01)] public byte MarkerId;
    [FieldOffset(0x04)] public int PosX; // 乘以 1000 的整数坐标
    [FieldOffset(0x08)] public int PosY;
    [FieldOffset(0x0C)] public int PosZ;
}
