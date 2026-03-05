using System.Runtime.InteropServices;

namespace FFXIVNetworkPacketAnalysisTool.PacketStructures;

/// <summary>
/// 效果结果包结构体
/// Python 中定义为可变长度，最多支持 16 个结果条目
/// 实际大小: 0x04 (header) + 16 * 0x58 (results) = 0x584
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0x584)]
public unsafe struct DOWN_EffectResult
{
    [FieldOffset(0x00)] public byte Count;
    [FieldOffset(0x04)] public fixed byte Results[1408]; // 16 个 EffectResultEntry (16 * 0x58 = 0x580)

    public EffectResultEntry GetResult(int index) // 获取指定索引的 EffectResultEntry（需要手动解析 fixed byte 数组）
    {
        if (index < 0 || index >= Count)
            throw new System.IndexOutOfRangeException();

        fixed (byte* ptr = Results)
        {
            return ((EffectResultEntry*)(ptr + index * 0x58))[0];
        }
    }
}
