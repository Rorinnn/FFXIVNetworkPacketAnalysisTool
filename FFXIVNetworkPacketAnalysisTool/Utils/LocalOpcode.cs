using System;
using System.Collections.Generic;
using System.Reflection;
using Dalamud.Memory;
using Serilog;

namespace FFXIVNetworkPacketAnalysisTool.Utils;

/// <summary>
/// 通过内存签名扫描获取联网数据中未收录的 opcode。
/// 大部分 UP opcode 已通过 OmenTools UpstreamOpcode 在 OnlineOpcode 中处理，此处仅保留未覆盖的额外 sig。
/// 在字段上标注 OpcodeAttribute 即可自动注册。
/// </summary>
public static class LocalOpcode
{
    public static void SetLocalUpOpcode(Dictionary<int, string> opcodes) // 扫描所有标注了 OpcodeAttribute 的 CompSig 字段，将结果写入 opcodes。
    {
        const BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

        foreach (var field in typeof(LocalOpcode).GetFields(flags))
        {
            if (field.FieldType != typeof(CompSig)) continue;
            if (field.GetValue(null) is not CompSig sig) continue;

            var attr = field.GetCustomAttribute<OpcodeAttribute>();
            if (attr is null) continue;

            var scanned = ScanSig(sig, attr.Offset);
            if (scanned == 0) continue;

            if (opcodes.TryAdd(scanned, attr.Name))
                Log.Debug($"[LocalOpcode] 已添加 sig opcode: {scanned} → {attr.Name}");
        }
    }

    private static int ScanSig(CompSig sig, int offset)
    {
        var addr = sig.ScanText();
        if (addr == nint.Zero) return 0;
        try { return MemoryHelper.Read<int>(addr + offset); }
        catch (Exception ex)
        {
            Log.Warning($"[LocalOpcode] 读取 sig 偏移失败 offset=0x{offset:X}: {ex.Message}");
            return 0;
        }
    }
}

/// <summary>
/// 标注在 CompSig 字段上，用于声明该签名对应的 Opcode 名称和读取偏移。
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public sealed class OpcodeAttribute(string name, int offset) : Attribute
{
    public string Name { get; } = name; // Opcode 名称（例如 "UP_EventStart"）。

    public int Offset { get; } = offset; // 签名匹配后读取 Opcode 值的字节偏移。
}
