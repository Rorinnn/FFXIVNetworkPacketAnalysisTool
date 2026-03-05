using System.Numerics;
using System.Runtime.InteropServices;

namespace FFXIVNetworkPacketAnalysisTool.PacketStructures;

/// <summary>
/// 动作发送（坐标/地面目标）
/// 参照 OmenTools UseActionLocationPacket 布局
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0x20)]
public struct UP_ActionRequestGroundTargeted
{
    [FieldOffset(0x00)] public uint ActionId;
    [FieldOffset(0x04)] public byte CastBuff;
    [FieldOffset(0x05)] public byte ActionType;
    [FieldOffset(0x06)] public ushort Sequence;
    [FieldOffset(0x08)] public ushort Rotation;
    [FieldOffset(0x0A)] public ushort RotationNew;
    [FieldOffset(0x0C)] public Vector3 Location;
}
