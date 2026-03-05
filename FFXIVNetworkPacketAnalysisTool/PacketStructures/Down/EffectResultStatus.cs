using System.Runtime.InteropServices;

namespace FFXIVNetworkPacketAnalysisTool.PacketStructures;

/// <summary>
/// 效果结果状态结构体
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0x10)]
public struct EffectResultStatus
{
    [FieldOffset(0x00)] public byte StatusSlot;
    [FieldOffset(0x02)] public ushort StatusId;
    [FieldOffset(0x04)] public short Param;
    [FieldOffset(0x08)] public float Time;
    [FieldOffset(0x0C)] public uint SourceId;
}
