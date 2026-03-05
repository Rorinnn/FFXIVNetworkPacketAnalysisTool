using System.Runtime.InteropServices;

namespace FFXIVNetworkPacketAnalysisTool.PacketStructures;

/// <summary>
/// 倒计时发起包
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0x30)]
public unsafe struct DOWN_CountdownInitiate
{
    [FieldOffset(0x00)] public uint EntityId;
    [FieldOffset(0x04)] public ushort Time;        // 秒数
    [FieldOffset(0x08)] public fixed byte Name[32]; // 发起者名称
}
