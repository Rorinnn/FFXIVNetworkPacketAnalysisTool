using System.Runtime.InteropServices;

namespace FFXIVNetworkPacketAnalysisTool.PacketStructures;

/// <summary>
/// 心跳包（客户端定期发送）
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0x08)]
public struct UP_Heartbeat
{
    [FieldOffset(0x00)] public uint TimeMs;
}
