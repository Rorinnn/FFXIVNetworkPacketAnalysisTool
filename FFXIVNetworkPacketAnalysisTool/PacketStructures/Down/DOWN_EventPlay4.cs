using System.Runtime.InteropServices;

namespace FFXIVNetworkPacketAnalysisTool.PacketStructures;

/// <summary>
/// 事件播放包（4参数版）
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0x28)]
public unsafe struct DOWN_EventPlay4
{
    [FieldOffset(0x00)] public ulong ActorId;
    [FieldOffset(0x08)] public uint EventId;
    [FieldOffset(0x0C)] public ushort SceneId;
    [FieldOffset(0x10)] public uint Flags;
    [FieldOffset(0x14)] public byte ParamCount;
    [FieldOffset(0x18)] public fixed uint Params[4];
}
