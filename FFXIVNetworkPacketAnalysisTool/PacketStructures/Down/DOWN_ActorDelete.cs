using System.Runtime.InteropServices;

namespace FFXIVNetworkPacketAnalysisTool.PacketStructures;

/// <summary>
/// Actor删除包结构体
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0x08)]
public struct DOWN_ActorDelete
{
    [FieldOffset(0x00)] public byte Index;
    [FieldOffset(0x04)] public uint ActorId;
}
