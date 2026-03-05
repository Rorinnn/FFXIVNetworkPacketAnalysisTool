using System.Runtime.InteropServices;

namespace FFXIVNetworkPacketAnalysisTool.PacketStructures;

/// <summary>
/// Actor控制包结构体
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0x18)]
public struct DOWN_ActorControl
{
    [FieldOffset(0x00)] public ActorControlId Id;
    [FieldOffset(0x04)] public uint Arg0;
    [FieldOffset(0x08)] public uint Arg1;
    [FieldOffset(0x0C)] public uint Arg2;
    [FieldOffset(0x10)] public uint Arg3;

    public ActorControlId ControlId => Id;

    public string ControlIdName
    {
        get
        {
            if (System.Enum.IsDefined(typeof(ActorControlId), Id))
                return Id.ToString();
            return $"Unknown(0x{(ushort)Id:X4})";
        }
    }

    public void ApplyPacketFix(uint baseValue)
    {
        if (ControlId == ActorControlId.SetLockOn)
        {
            Arg0 += baseValue;
        }
    }
}
