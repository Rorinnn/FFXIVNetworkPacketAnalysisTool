using System.Numerics;
using System.Runtime.InteropServices;
using Dalamud.Game.Text;

namespace FFXIVNetworkPacketAnalysisTool.PacketStructures;

/// <summary>
/// 公共频道发言
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 1072)]
public unsafe struct UP_ChatHandler
{
    [FieldOffset(0x00)] public int Unknown0;
    [FieldOffset(0x04)] public uint EntityId;
    [FieldOffset(0x08)] public Vector3 Position;
    [FieldOffset(0x14)] public float Rotation;
    [FieldOffset(0x18)] public XivChatType ChatType;
    [FieldOffset(0x1A)] public fixed byte Message[1046];
}
