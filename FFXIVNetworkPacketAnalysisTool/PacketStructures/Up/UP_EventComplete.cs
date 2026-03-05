using System.Runtime.InteropServices;

namespace FFXIVNetworkPacketAnalysisTool.PacketStructures;

/// <summary>
/// 事件完成/选项应答（客户端确认事件或选择对话选项）
/// 参照 DailyRoutines FieldEntryCommand 的 EventCompletePackt 布局
/// 注意：此结构体与 UP_EventFinish 不同，EventFinish 用于结束事件回调
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0x14)]
public struct UP_EventComplete
{
    [FieldOffset(0x00)] public uint EventId;
    [FieldOffset(0x04)] public uint Category;
    [FieldOffset(0x08)] public uint Param0;
    [FieldOffset(0x0C)] public uint Param1;
    [FieldOffset(0x10)] public uint Param2;
}
