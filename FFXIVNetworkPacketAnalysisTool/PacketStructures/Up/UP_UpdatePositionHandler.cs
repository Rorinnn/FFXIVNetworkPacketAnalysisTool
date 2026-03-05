using System.Numerics;
using System.Runtime.InteropServices;

namespace FFXIVNetworkPacketAnalysisTool.PacketStructures;

/// <summary>
/// 更新位置处理器
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 0x28)]
public struct UP_UpdatePositionHandler
{
    [FieldOffset(0x0)] public float Rotation;
    [FieldOffset(0x4)] public MoveType Flag;
    [FieldOffset(0x8)] public Vector3 Position;

    public enum MoveType : uint
    {
        NormalMove0 = 0,
        NormalMove1 = 0x10000,
        NormalMove2 = 0x20000,
        NormalMove3 = 0x30000,
        Fly0 = 1,
        Fly1 = 0x10001,
        Fly2 = 0x20001,
        Fly3 = 0x30001,
        WalkOrSlowSwim0 = 2,
        WalkOrSlowSwim1 = 0x10002,
        WalkOrSlowSwim2 = 0x20002,
        WalkOrSlowSwim3 = 0x30002,
        SlowFly0 = 3,
        SlowFly1 = 0x10003,
        SlowFly2 = 0x20003,
        SlowFly3 = 0x30003,
        JumpStart0 = 0x100,
        JumpStart1 = 0x10100,
        JumpStart2 = 0x20100,
        JumpStart3 = 0x30100,
        JumpStartWalk0 = 0x102,
        JumpStartWalk1 = 0x10102,
        JumpStartWalk2 = 0x20102,
        JumpStartWalk3 = 0x30102,
        JumpEnd0 = 0x200,
        JumpEnd1 = 0x10200,
        JumpEnd2 = 0x20200,
        JumpEnd3 = 0x30200,
        JumpEndWalk0 = 0x202,
        JumpEndWalk1 = 0x10202,
        JumpEndWalk2 = 0x20202,
        JumpEndWalk3 = 0x30202,
        JumpProcess0 = 0x100000,
        JumpProcess1 = 0x110000,
        JumpProcess2 = 0x120000,
        JumpProcess3 = 0x130000,
        JumpProcessWalk0 = 0x100002,
        JumpProcessWalk1 = 0x110002,
        JumpProcessWalk2 = 0x120002,
        JumpProcessWalk3 = 0x130002,
        JumpHighestPoint0 = 0x100400,
        JumpHighestPoint1 = 0x110400,
        JumpHighestPoint2 = 0x120400,
        JumpHighestPoint3 = 0x130400,
        JumpHighestPointWalk0 = 0x100402,
        JumpHighestPointWalk1 = 0x110402,
        JumpHighestPointWalk2 = 0x120402,
        JumpHighestPointWalk3 = 0x130402,
        ActionMove0 = 0x200000,
        ActionMove1 = 0x210000,
        ActionMove2 = 0x220000,
        ActionMove3 = 0x230000,
        ActionMoveEnd0 = 0x1000,
        ActionMoveEnd1 = 0x11000,
        ActionMoveEnd2 = 0x21000,
        ActionMoveEnd3 = 0x31000
    }
}
