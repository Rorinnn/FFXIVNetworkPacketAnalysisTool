using System.Runtime.InteropServices;

namespace FFXIVNetworkPacketAnalysisTool.PacketStructures;

/// <summary>
/// 事件动作
/// 注意：Python 中 _size_ = 0x10 表示基础结构体大小，args 是动态数组
/// 实际包大小 = 0x10 + arg_cnt * 4
/// 这里定义最大容量：0x10 (base) + 255 * 4 (args) = 0x408
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0x10)]
public unsafe struct UP_EventAction
{
    [FieldOffset(0x00)] public uint HandlerId;
    [FieldOffset(0x04)] public ushort SceneId;
    [FieldOffset(0x06)] public byte Result;
    [FieldOffset(0x07)] public byte ArgCount;
    [FieldOffset(0x08)] public fixed uint Args[255];
}
