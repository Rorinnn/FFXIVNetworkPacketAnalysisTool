using System.Runtime.InteropServices;

namespace FFXIVNetworkPacketAnalysisTool.PacketStructures;

/// <summary>
/// 技能效果嵌套结构体（单个效果）
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0x08)]
public struct ActionEffectEntry
{
    [FieldOffset(0x00)] public byte Type;
    [FieldOffset(0x01)] public byte Arg0;
    [FieldOffset(0x02)] public byte Arg1;
    [FieldOffset(0x03)] public byte Arg2;
    [FieldOffset(0x04)] public byte Arg3;
    [FieldOffset(0x05)] public byte Flag;
    [FieldOffset(0x06)] public ushort Value;
}
