using System.Runtime.InteropServices;

namespace FFXIVNetworkPacketAnalysisTool.PacketStructures;

/// <summary>
/// 单个效果结果条目
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0x58)]
public struct EffectResultEntry
{
    [FieldOffset(0x00)] public uint ResponseId;
    [FieldOffset(0x04)] public uint TargetId;
    [FieldOffset(0x08)] public uint CurrentHp;
    [FieldOffset(0x0C)] public uint MaxHp;
    [FieldOffset(0x10)] public ushort CurrentMp;
    [FieldOffset(0x13)] public byte ClassJob;
    [FieldOffset(0x14)] public byte Shield;
    [FieldOffset(0x15)] public byte StatusCount;

    [FieldOffset(0x18)] public EffectResultStatus Status0;
    [FieldOffset(0x28)] public EffectResultStatus Status1;
    [FieldOffset(0x38)] public EffectResultStatus Status2;
    [FieldOffset(0x48)] public EffectResultStatus Status3;

    public EffectResultStatus GetStatus(int index) // 获取指定索引的状态（根据 StatusCount 使用）
    {
        return index switch
        {
            0 => Status0,
            1 => Status1,
            2 => Status2,
            3 => Status3,
            _ => throw new System.IndexOutOfRangeException("Status index must be 0-3")
        };
    }
}
