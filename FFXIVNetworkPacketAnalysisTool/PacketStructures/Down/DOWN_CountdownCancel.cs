using System.Runtime.InteropServices;

namespace FFXIVNetworkPacketAnalysisTool.PacketStructures;

/// <summary>
/// 倒计时取消包
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0x28)]
public unsafe struct DOWN_CountdownCancel
{
    [FieldOffset(0x00)] public uint EntityId;
    [FieldOffset(0x04)] public fixed byte Name[32];
}
