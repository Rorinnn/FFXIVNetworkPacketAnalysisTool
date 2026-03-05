using System.Runtime.InteropServices;

namespace FFXIVNetworkPacketAnalysisTool.PacketStructures;

/// <summary>
/// 登出包
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0x08)]
public struct DOWN_Logout
{
    [FieldOffset(0x00)] public uint Flags;
}
