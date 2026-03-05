using System.Runtime.InteropServices;

namespace FFXIVNetworkPacketAnalysisTool.PacketStructures;

/// <summary>
/// 环境控制包（灯光、门等场景控件）
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0x10)]
public struct DOWN_EnvironmentControl
{
    [FieldOffset(0x00)] public uint DirectorId;
    [FieldOffset(0x04)] public uint State;
    [FieldOffset(0x08)] public byte Index;
}
