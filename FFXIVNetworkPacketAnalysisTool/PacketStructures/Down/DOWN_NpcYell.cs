using System.Runtime.InteropServices;

namespace FFXIVNetworkPacketAnalysisTool.PacketStructures;

/// <summary>
/// NPC对话包结构体
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0x20)]
public unsafe struct DOWN_NpcYell
{
    [FieldOffset(0x00)] public ulong ActorId;
    [FieldOffset(0x08)] public uint NameId;
    [FieldOffset(0x0C)] public ushort NpcYellId;
    [FieldOffset(0x10)] public fixed int Args[4];
}
