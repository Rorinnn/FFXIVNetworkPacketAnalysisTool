using System.Numerics;
using System.Runtime.InteropServices;

namespace FFXIVNetworkPacketAnalysisTool.PacketStructures;

/// <summary>
/// 潜水TP包
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0x30)]
public struct UP_DiveStart
{
    [FieldOffset(0x00)] public float Rotation;
    [FieldOffset(0x04)] public Vector3 Position;
}
