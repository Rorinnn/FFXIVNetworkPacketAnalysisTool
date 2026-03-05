using System.Runtime.InteropServices;

namespace FFXIVNetworkPacketAnalysisTool.PacketStructures;

/// <summary>
/// Actor移动包结构体
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0x0C)]
public unsafe struct DOWN_ActorMove
{
    [FieldOffset(0x00)] public ushort Facing;
    [FieldOffset(0x02)] public ushort Flag;
    [FieldOffset(0x04)] public byte Speed;
    [FieldOffset(0x06)] public fixed ushort Pos[3];
}
