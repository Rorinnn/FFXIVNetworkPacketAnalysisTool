using System.Runtime.InteropServices;

namespace FFXIVNetworkPacketAnalysisTool.PacketStructures;

/// <summary>
/// 系统日志消息包（基础版 - 参数在子类中展开）
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0x10)]
public struct DOWN_SystemLogMessage
{
    [FieldOffset(0x00)] public uint EntityId;
    [FieldOffset(0x04)] public uint MessageId;
    [FieldOffset(0x08)] public byte ArgCount;
    [FieldOffset(0x0C)] public uint Arg0;
}
