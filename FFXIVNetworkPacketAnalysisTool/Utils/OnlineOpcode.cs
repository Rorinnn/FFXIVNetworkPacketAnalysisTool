using Framework = FFXIVClientStructs.FFXIV.Client.System.Framework;
using FFXIVNetworkPacketAnalysisTool;
using OmenTools.Infos;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace FFXIVNetworkPacketAnalysisTool.Utils;

/// <summary>
/// 从 karashiiro/FFXIVOpcodes 获取 CN 区 opcode，
/// 并补充 zhyupe/ffxiv-opcode-worker CSV 中额外的已知 opcode。
/// </summary>
public sealed class OnlineOpcode
{
    private const string Region = "CN";
    private const int HttpTimeoutSec = 15;

    private static readonly string[] sourceUrls = // 数据源 URL，按访问优先级排列（jsDelivr 国内更快）。
    [
        "https://cdn.jsdelivr.net/gh/karashiiro/FFXIVOpcodes@latest/opcodes.min.json",
        "https://raw.githubusercontent.com/karashiiro/FFXIVOpcodes/master/opcodes.min.json",
    ];

    private readonly Configuration config;

    public OnlineOpcode(Plugin plugin) // 初始化在线 Opcode 获取器，绑定插件配置。
    {
        config = plugin.Configuration;
    }

    public async void Run() // 异步拉取 Opcode 数据并写入配置，完成后在聊天窗口显示结果。
    {
        string gameVersion;
        unsafe { gameVersion = Framework.Framework.Instance()->GameVersionString; }
        config.GameVersion = gameVersion;
        Plugin.ChatGui.Print($"[FFNPAT] 游戏版本: {gameVersion}");

        config.UpOpcodes.Clear();
        config.DownOpcodes.Clear();
        config.OpcodeSource = "";

        if (await TryLoadOpcodes())
        {
            LocalOpcode.SetLocalUpOpcode(config.UpOpcodes);
            Plugin.ChatGui.Print($"[FFNPAT] Opcode 已加载 | {config.OpcodeSource}");
            Plugin.ChatGui.Print($"[FFNPAT]   UP: {config.UpOpcodes.Count}  DOWN: {config.DownOpcodes.Count}");
        }
        else
        {
            Plugin.ChatGui.PrintError("[FFNPAT] 获取 Opcode 失败，请检查网络连接。");
        }
    }

    private async Task<bool> TryLoadOpcodes()
    {
        var json = await FetchFirstAvailable(sourceUrls);
        if (json is null) return false;

        try
        {
            using var doc = JsonDocument.Parse(json);
            if (doc.RootElement.ValueKind != JsonValueKind.Array) return false;

            var (entry, version) = FindLatestRegionEntry(doc.RootElement, Region);
            if (entry is null) return false;

            var lists = entry.Value.GetProperty("lists");

            // 优先级: OmenTools sig > karashiiro JSON > 硬编码回退
            AddSigScannedUpOpcodes();

            if (lists.TryGetProperty("ServerZoneIpcType", out var serverZone))
                ParseOpcodeArray(serverZone, "DOWN_", config.DownOpcodes);

            if (lists.TryGetProperty("ClientZoneIpcType", out var clientZone))
                ParseOpcodeArray(clientZone, "UP_", config.UpOpcodes);

            AddSupplementaryOpcodes();

            config.OpcodeSource = $"karashiiro/FFXIVOpcodes {Region} {version}";
            return config.UpOpcodes.Count > 0 || config.DownOpcodes.Count > 0;
        }
        catch (Exception ex)
        {
            Plugin.ChatGui.PrintError($"[FFNPAT] 解析 Opcode 失败: {ex.Message}");
            return false;
        }
    }

    private static (JsonElement? entry, string? version) FindLatestRegionEntry(JsonElement root, string region) // 在 JSON 数组中查找指定区域版本号最大的条目。
    {
        JsonElement? best = null;
        string? bestVer = null;

        foreach (var el in root.EnumerateArray())
        {
            if (!el.TryGetProperty("region", out var r) || r.GetString() != region) continue;
            var v = el.GetProperty("version").GetString();
            if (bestVer is null || string.Compare(v, bestVer, StringComparison.Ordinal) > 0)
            {
                bestVer = v;
                best = el;
            }
        }

        return (best, bestVer);
    }

    private static void ParseOpcodeArray(JsonElement array, string prefix, Dictionary<int, string> target) // 将 [{"name":"X","opcode":123}, ...] 写入 opcodeDict，自动加前缀。TryAdd 不覆盖已有值。
    {
        foreach (var item in array.EnumerateArray())
        {
            var name = item.GetProperty("name").GetString();
            if (string.IsNullOrEmpty(name)) continue;
            target.TryAdd(item.GetProperty("opcode").GetInt32(), prefix + name);
        }
    }

    // IDA 逆向确认的 CN 7.45 硬编码回退 + 变体 opcode + zhyupe CSV 补充 DOWN opcode。
    private void AddSupplementaryOpcodes() // TryAdd 保证不覆盖 OmenTools sig / karashiiro 已有的值。
    {
        config.UpOpcodes.TryAdd(0x026F, "UP_EventAction");   // ≤ 2 个参数
        config.UpOpcodes.TryAdd(0x0210, "UP_EventAction");   // ≤ 4
        config.UpOpcodes.TryAdd(0x03D6, "UP_EventAction");   // ≤ 8
        config.UpOpcodes.TryAdd(0x02CB, "UP_EventAction");   // ≤ 16
        config.UpOpcodes.TryAdd(0x0330, "UP_EventAction");   // ≤ 32
        config.UpOpcodes.TryAdd(0x03A3, "UP_EventAction");   // ≤ 64
        config.UpOpcodes.TryAdd(0x026E, "UP_EventAction");   // ≤ 128
        config.UpOpcodes.TryAdd(0x01AC, "UP_EventAction");   // ≤ 255
        // 带坐标变体
        config.UpOpcodes.TryAdd(0x00D4, "UP_EventAction");   // 仅坐标
        config.UpOpcodes.TryAdd(0x00AC, "UP_EventAction");   // 坐标 + 参数
        config.UpOpcodes.TryAdd(0x019A, "UP_EventAction");   // 坐标 + 扩展
        config.UpOpcodes.TryAdd(0x00B7, "UP_EventAction");   // 坐标 + 参数 + 数据

        // UP_EventFinish 变体（OmenTools 称 EventComplete）—— 可变长参数
        config.UpOpcodes.TryAdd(0x03CE, "UP_EventFinish");   // 特殊
        config.UpOpcodes.TryAdd(0x0114, "UP_EventFinish");   // 基础 (≤ 2)
        config.UpOpcodes.TryAdd(0x014E, "UP_EventFinish");   // ≤ 4
        config.UpOpcodes.TryAdd(0x0186, "UP_EventFinish");   // ≤ 8
        config.UpOpcodes.TryAdd(0x00C8, "UP_EventFinish");   // ≤ 16
        config.UpOpcodes.TryAdd(0x02FA, "UP_EventFinish");   // ≤ 32
        config.UpOpcodes.TryAdd(0x00D0, "UP_EventFinish");   // ≤ 64
        config.UpOpcodes.TryAdd(0x013A, "UP_EventFinish");   // ≤ 128
        config.UpOpcodes.TryAdd(0x03BE, "UP_EventFinish");   // ≤ 255

        config.DownOpcodes.TryAdd(0x00C9, "DOWN_RSF");
        config.DownOpcodes.TryAdd(0x03DB, "DOWN_EffectResult4");
        config.DownOpcodes.TryAdd(0x03BA, "DOWN_EffectResult8");
        config.DownOpcodes.TryAdd(0x019F, "DOWN_EffectResult16");
        config.DownOpcodes.TryAdd(0x00DC, "DOWN_EurekaStatusEffectList");
        config.DownOpcodes.TryAdd(0x02A1, "DOWN_UpdateParty");
        config.DownOpcodes.TryAdd(0x0037, "DOWN_SystemLogMessage32");
        config.DownOpcodes.TryAdd(0x009A, "DOWN_SystemLogMessage48");
        config.DownOpcodes.TryAdd(0x0106, "DOWN_SystemLogMessage80");
        config.DownOpcodes.TryAdd(0x0154, "DOWN_SystemLogMessage144");
    }

    // 尝试从 OmenTools UpstreamOpcode 读取运行时 sig 扫描结果。
    private void AddSigScannedUpOpcodes() // 仅添加 C7-based sigs（offset 0x4 直读立即数），E8-based sigs 在国服偏移不匹配，由硬编码回退覆盖。
    {
        try
        {
            TryAddSig(UpstreamOpcode.EventStartOpcode, "UP_EventStart");
            TryAddSig(UpstreamOpcode.DiveStartOpcode, "UP_DiveStart");
            TryAddSig(UpstreamOpcode.HeartbeatOpcode, "UP_Heartbeat");
            TryAddSig(UpstreamOpcode.UseActionOpcode, "UP_ActionRequest");
            TryAddSig(UpstreamOpcode.UseActionLocationOpcode, "UP_ActionRequestGroundTargeted");
            TryAddSig(UpstreamOpcode.PositionUpdateInstanceOpcode, "UP_UpdatePositionInstance");
            TryAddSig(UpstreamOpcode.PositionUpdateOpcode, "UP_UpdatePositionHandler");
            TryAddSig(UpstreamOpcode.TreasureOpenOpcode, "UP_TreasureOpen");
            TryAddSig(UpstreamOpcode.MJIInteractOpcode, "UP_MJIInteract");
            TryAddSig(UpstreamOpcode.CharaCardOpenOpcode, "UP_CharaCardOpen");
        }
        catch (Exception ex)
        {
            Plugin.ChatGui.PrintError($"[FFNPAT] OmenTools sig 扫描失败: {ex.InnerException?.Message ?? ex.Message}");
        }
    }

    private void TryAddSig(int opcode, string name) // 仅当 opcode 在合理范围 (0, 0x400] 时添加。
    {
        if (opcode > 0 && opcode <= 0x0400)
            config.UpOpcodes.TryAdd(opcode, name);
    }

    private static async Task<string?> FetchFirstAvailable(string[] urls) // 依次尝试多个 URL，返回第一个成功的响应体；全部失败返回 null。
    {
        using var http = new HttpClient { Timeout = TimeSpan.FromSeconds(HttpTimeoutSec) };
        foreach (var url in urls)
        {
            try { return await http.GetStringAsync(url); }
            catch { /* 继续尝试 */ }
        }
        return null;
    }
}
