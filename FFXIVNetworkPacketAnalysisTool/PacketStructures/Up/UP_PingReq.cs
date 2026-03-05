using System.Runtime.InteropServices;

namespace FFXIVNetworkPacketAnalysisTool.PacketStructures;

/// <summary>
/// Ping 请求
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0x18)]
public struct UP_PingReq
{
    [FieldOffset(0x00)] public uint TimeMs;
}
