using System.Runtime.InteropServices;

namespace FFXIVNetworkPacketAnalysisTool.PacketStructures;

/// <summary>
/// FATE信息包
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0x10)]
public struct DOWN_FateInfo
{
    [FieldOffset(0x00)] public uint FateId;
    [FieldOffset(0x04)] public uint Progress;
}
