using System.Runtime.InteropServices;

namespace FFXIVNetworkPacketAnalysisTool.PacketStructures;

/// <summary>
/// RSV 数据包（Resource String Value）
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0x410)]
public unsafe struct DOWN_RSV
{
    [FieldOffset(0x00)] public uint Key;
    [FieldOffset(0x04)] public fixed byte Value[1036]; // 可变长字符串
}
