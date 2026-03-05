using System.Runtime.InteropServices;

namespace FFXIVNetworkPacketAnalysisTool.PacketStructures;

/// <summary>
/// 更新职业信息包
/// 对应 Sapphire PlayerStatusUpdate / ChangeClass
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0x10)]
public struct DOWN_UpdateClassInfo
{
    [FieldOffset(0x00)] public byte ClassJob;
    [FieldOffset(0x02)] public ushort Level;
    [FieldOffset(0x04)] public ushort SyncedLevel;
    [FieldOffset(0x08)] public uint Exp;
    [FieldOffset(0x0C)] public uint RestPoint;
}
