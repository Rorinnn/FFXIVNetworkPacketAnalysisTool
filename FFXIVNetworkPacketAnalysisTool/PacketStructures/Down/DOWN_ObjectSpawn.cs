using System.Runtime.InteropServices;

namespace FFXIVNetworkPacketAnalysisTool.PacketStructures;

/// <summary>
/// 对象生成包结构体
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0x40)]
public unsafe struct DOWN_ObjectSpawn
{
    [FieldOffset(0x00)] public byte Index;
    [FieldOffset(0x01)] public byte Kind;
    [FieldOffset(0x02)] public byte Flag;
    [FieldOffset(0x03)] public byte InvisibilityGroup;
    [FieldOffset(0x04)] public uint BaseId;
    [FieldOffset(0x08)] public uint Id;
    [FieldOffset(0x0C)] public uint LayoutId;
    [FieldOffset(0x10)] public uint ContentId;
    [FieldOffset(0x14)] public uint OwnerId;
    [FieldOffset(0x18)] public uint BindLayoutId;
    [FieldOffset(0x1C)] public float Scale;
    [FieldOffset(0x20)] public ushort SharedGroupTimelineState;
    [FieldOffset(0x22)] public ushort Facing;
    [FieldOffset(0x24)] public ushort Fate;
    [FieldOffset(0x26)] public byte PermissionInvisibility;
    [FieldOffset(0x27)] public byte Arg1;
    [FieldOffset(0x28)] public uint Arg2;
    [FieldOffset(0x2C)] public uint Arg3;
    [FieldOffset(0x30)] public fixed float Pos[3];
}
