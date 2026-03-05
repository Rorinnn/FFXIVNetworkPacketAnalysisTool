using System.Runtime.InteropServices;

namespace FFXIVNetworkPacketAnalysisTool.PacketStructures;

/// <summary>
/// RSF 数据包（Resource String File）
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0x410)]
public unsafe struct DOWN_RSF
{
    [FieldOffset(0x00)] public uint Key;
    [FieldOffset(0x04)] public fixed byte Value[1036];
}
