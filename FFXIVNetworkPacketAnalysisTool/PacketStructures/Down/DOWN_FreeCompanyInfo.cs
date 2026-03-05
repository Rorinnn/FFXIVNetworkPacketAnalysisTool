using System.Runtime.InteropServices;

namespace FFXIVNetworkPacketAnalysisTool.PacketStructures;

/// <summary>
/// 免费公司信息包
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0x10)]
public unsafe struct DOWN_FreeCompanyInfo
{
    [FieldOffset(0x00)] public ulong Crest;
    [FieldOffset(0x08)] public fixed byte Tag[6];
}
