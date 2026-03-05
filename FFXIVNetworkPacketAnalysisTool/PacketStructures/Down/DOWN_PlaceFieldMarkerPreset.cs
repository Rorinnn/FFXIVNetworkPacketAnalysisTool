using System.Runtime.InteropServices;

namespace FFXIVNetworkPacketAnalysisTool.PacketStructures;

/// <summary>
/// 放置场地标记预设包（8个标记一次性设置）
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0x68)]
public unsafe struct DOWN_PlaceFieldMarkerPreset
{
    [FieldOffset(0x00)] public byte ActiveFlags; // 每 bit 表示一个标记是否启用
    [FieldOffset(0x04)] public fixed int PosX[8]; // 乘以 1000 的整数坐标
    [FieldOffset(0x24)] public fixed int PosY[8];
    [FieldOffset(0x44)] public fixed int PosZ[8];
}
