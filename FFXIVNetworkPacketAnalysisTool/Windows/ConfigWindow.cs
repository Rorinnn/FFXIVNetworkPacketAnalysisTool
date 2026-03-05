using System;
using System.Collections.Generic;
using System.Numerics;
using Dalamud.Interface.Windowing;
using Dalamud.Bindings.ImGui;

namespace FFXIVNetworkPacketAnalysisTool.Windows;

/// <summary>
/// 插件关于/配置窗口，显示版本信息、更新日志和 Opcode 加载状态。
/// </summary>
public class ConfigWindow : Window, IDisposable
{
    private Configuration configuration;

    private float animationTime = 0f;

    private readonly List<ChangelogEntry> changelog = new List<ChangelogEntry>
    {
        new ChangelogEntry
        {
            Version = "v1.2.0",
            Date = "2025-10-08",
            Type = ChangeType.Feature,
            Changes = new List<string>
            {
                "新增多会话支持，可同时管理多个抓包会话",
                "新增包过滤功能，支持发包/收包/已知Opcode过滤",
                "新增结构体自动解析，自动识别已定义的包结构",
                "新增多选功能，支持 Ctrl/Shift 批量选择和删除",
                "优化UI界面，添加颜色区分和鼠标悬停效果"
            }
        },
        new ChangelogEntry
        {
            Version = "v1.1.5",
            Date = "2025-10-07",
            Type = ChangeType.Fix,
            Changes = new List<string>
            {
                "修复长时间运行导致的内存泄漏问题",
                "修复结构体解析时的偏移量计算错误",
                "优化包捕获性能，减少CPU占用",
                "修复包列表滚动时的卡顿问题"
            }
        },
        new ChangelogEntry
        {
            Version = "v1.1.0",
            Date = "2025-10-06",
            Type = ChangeType.Feature,
            Changes = new List<string>
            {
                "新增包体结构体解析功能",
                "新增十六进制视图和C#数组导出",
                "新增自动滚动开关",
                "新增配置持久化保存"
            }
        },
        new ChangelogEntry
        {
            Version = "v1.0.0",
            Date = "2025-10-06",
            Type = ChangeType.Initial,
            Changes = new List<string>
            {
                "首个正式版本发布",
                "实现基础网络包捕获功能",
                "实现包列表展示和详情查看",
                "支持Opcode名称映射"
            }
        }
    };

    public ConfigWindow(Plugin plugin) : base("About FFXIV NPATool###AboutWindow") // 初始化配置窗口。
    {
        Flags = ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse;

        Size = new Vector2(750, 650);
        SizeCondition = ImGuiCond.FirstUseEver;
        SizeConstraints = new WindowSizeConstraints { MinimumSize = new Vector2(600, 500), MaximumSize = new Vector2(1200, 900) };

        configuration = plugin.Configuration;
    }

    public void Dispose() // 释放配置窗口资源。
    {
    }

    public override void PreDraw() // 每帧绘制前更新动画计时器。
    {
        animationTime += ImGui.GetIO().DeltaTime;
    }

    public override void Draw() // 绘制配置窗口 UI。
    {
        DrawHeader();
        ImGui.Spacing();
        ImGui.Separator();
        ImGui.Spacing();

        DrawTabBar();
    }

    private void DrawHeader()
    {
        var windowWidth = ImGui.GetWindowWidth();

        var drawList = ImGui.GetWindowDrawList();
        var headerMin = ImGui.GetCursorScreenPos();
        var headerMax = new Vector2(headerMin.X + windowWidth - 15, headerMin.Y + 120);

        uint color1 = ImGui.GetColorU32(new Vector4(0.2f, 0.4f, 0.8f, 0.3f));
        uint color2 = ImGui.GetColorU32(new Vector4(0.6f, 0.3f, 0.9f, 0.3f));
        uint color3 = ImGui.GetColorU32(new Vector4(0.3f, 0.7f, 0.9f, 0.3f));

        drawList.AddRectFilledMultiColor(headerMin, headerMax, color1, color2, color3, color1);

        ImGui.Dummy(new Vector2(0, 10));

        var titleText = "FFXIV Network Packet Analysis Tool";
        var titleSize = ImGui.CalcTextSize(titleText);
        ImGui.SetCursorPosX((windowWidth - titleSize.X) * 0.5f);

        // 彩虹色动画
        float hue = (animationTime * 0.3f) % 1.0f;
        var titleColor = ColorFromHSV(hue, 0.8f, 1.0f);
        ImGui.PushStyleColor(ImGuiCol.Text, titleColor);
        ImGui.PushFont(ImGui.GetIO().Fonts.Fonts[0]);
        ImGui.Text(titleText);
        ImGui.PopFont();
        ImGui.PopStyleColor();

        ImGui.Spacing();

        var subtitleText = "FF14 网络包分析工具";
        var subtitleSize = ImGui.CalcTextSize(subtitleText);
        ImGui.SetCursorPosX((windowWidth - subtitleSize.X) * 0.5f);
        ImGui.TextColored(new Vector4(0.7f, 0.7f, 0.7f, 1f), subtitleText);

        ImGui.Spacing();

        var versionText = "Version 1.2.0 | By Siren";
        var versionSize = ImGui.CalcTextSize(versionText);
        ImGui.SetCursorPosX((windowWidth - versionSize.X) * 0.5f);
        ImGui.TextColored(new Vector4(0.5f, 0.8f, 1.0f, 1f), versionText);

        ImGui.Spacing();

        float pulse = (float)Math.Sin(animationTime * 3.0f) * 0.3f + 0.7f;
        var heartText = "Made with ♥ for Dalamud Plugin Developer";
        var heartSize = ImGui.CalcTextSize(heartText);
        ImGui.SetCursorPosX((windowWidth - heartSize.X) * 0.5f);
        ImGui.TextColored(new Vector4(1.0f, 0.4f, 0.4f, pulse), heartText);

        ImGui.Dummy(new Vector2(0, 10));
    }

    private void DrawTabBar()
    {
        if (ImGui.BeginTabBar("AboutTabs", ImGuiTabBarFlags.None))
        {
            if (ImGui.BeginTabItem("更新日志"))
            {
                DrawChangelogTab();
                ImGui.EndTabItem();
            }

            if (ImGui.BeginTabItem("Opcode 状态"))
            {
                DrawOpcodeStatusTab();
                ImGui.EndTabItem();
            }

            ImGui.EndTabBar();
        }
    }

    private void DrawOpcodeStatusTab()
    {
        ImGui.BeginChild("OpcodeStatusScroll", new Vector2(0, -1), true);

        ImGui.Spacing();
        ImGui.TextColored(new Vector4(0.4f, 0.8f, 1f, 1f), "游戏版本信息");
        ImGui.Separator();
        ImGui.Spacing();

        var gameVer = configuration.GameVersion;
        if (string.IsNullOrEmpty(gameVer))
        {
            ImGui.TextColored(new Vector4(1f, 0.6f, 0.2f, 1f), "游戏版本字符串未获取（插件尚未完成初始化？）");
        }
        else
        {
            ImGui.Text("游戏版本字符串:");
            ImGui.SameLine();
            ImGui.TextColored(new Vector4(0.4f, 1f, 0.4f, 1f), gameVer);
            if (ImGui.IsItemClicked())
                ImGui.SetClipboardText(gameVer);
            if (ImGui.IsItemHovered())
                ImGui.SetTooltip("点击可复制");
        }

        ImGui.Spacing();
        ImGui.Separator();
        ImGui.Spacing();
        ImGui.TextColored(new Vector4(0.4f, 0.8f, 1f, 1f), "Opcode 加载状态");
        ImGui.Separator();
        ImGui.Spacing();

        if (!string.IsNullOrEmpty(configuration.OpcodeSource))
        {
            ImGui.Text("数据来源:"); ImGui.SameLine();
            ImGui.TextColored(new Vector4(0.8f, 0.8f, 0.4f, 1f), configuration.OpcodeSource);
        }

        int upCount = configuration.UpOpcodes.Count;
        int downCount = configuration.DownOpcodes.Count;

        if (upCount == 0 && downCount == 0)
        {
            ImGui.TextColored(new Vector4(1f, 0.3f, 0.3f, 1f), "未加载任何 Opcode！");
            ImGui.TextWrapped("插件从 karashiiro/FFXIVOpcodes 获取 CN 区 Opcode，请检查网络连接。");
        }
        else
        {
            ImGui.Text($"上行 (UP_) Opcode:"); ImGui.SameLine();
            ImGui.TextColored(new Vector4(0.4f, 1f, 0.4f, 1f), $"{upCount} 个");

            ImGui.Text($"下行 (DOWN_) Opcode:"); ImGui.SameLine();
            ImGui.TextColored(new Vector4(0.4f, 1f, 0.4f, 1f), $"{downCount} 个");
        }

        ImGui.Spacing();
        ImGui.Separator();
        ImGui.Spacing();
        ImGui.TextColored(new Vector4(0.7f, 0.7f, 0.7f, 1f),
            "数据源: karashiiro/FFXIVOpcodes (CN)\n" +
            "CN Opcode 数据来自 zhyupe/ffxiv-opcode-worker，经 karashiiro 仓库自动发布。");

        ImGui.EndChild();
    }

    private void DrawChangelogTab()
    {
        ImGui.BeginChild("ChangelogScroll", new Vector2(0, -1), true);

        foreach (var entry in changelog)
        {
            DrawChangelogHeader(entry);

            ImGui.Indent(20);

            foreach (var change in entry.Changes)
            {
                ImGui.TextWrapped(change);
                ImGui.Spacing();
            }

            ImGui.Unindent(20);
            ImGui.Spacing();
            ImGui.Separator();
            ImGui.Spacing();
        }

        ImGui.EndChild();
    }

    private void DrawChangelogHeader(ChangelogEntry entry)
    {
        Vector4 typeColor = entry.Type switch
        {
            ChangeType.Feature => new Vector4(0.4f, 0.8f, 0.4f, 1f),
            ChangeType.Fix => new Vector4(1.0f, 0.6f, 0.2f, 1f),
            ChangeType.Breaking => new Vector4(1.0f, 0.3f, 0.3f, 1f),
            ChangeType.Initial => new Vector4(0.6f, 0.4f, 1.0f, 1f),
            _ => new Vector4(0.7f, 0.7f, 0.7f, 1f)
        };

        string typeIcon = entry.Type switch
        {
            ChangeType.Feature => "新功能",
            ChangeType.Fix => "修复",
            ChangeType.Breaking => "破坏性变更",
            ChangeType.Initial => "初始版本",
            _ => "更新"
        };

        ImGui.PushStyleColor(ImGuiCol.Text, typeColor);
        ImGui.Text($"{entry.Version} - {typeIcon}");
        ImGui.PopStyleColor();

        ImGui.SameLine();
        ImGui.TextColored(new Vector4(0.5f, 0.5f, 0.5f, 1f), $"({entry.Date})");
    }

    private Vector4 ColorFromHSV(float h, float s, float v)
    {
        float r = 0, g = 0, b = 0;

        if (s == 0.0f)
        {
            r = g = b = v;
        }
        else
        {
            h = h * 6.0f;
            int i = (int)h;
            float f = h - i;
            float p = v * (1.0f - s);
            float q = v * (1.0f - s * f);
            float t = v * (1.0f - s * (1.0f - f));

            switch (i)
            {
                case 0: r = v; g = t; b = p; break;
                case 1: r = q; g = v; b = p; break;
                case 2: r = p; g = v; b = t; break;
                case 3: r = p; g = q; b = v; break;
                case 4: r = t; g = p; b = v; break;
                default: r = v; g = p; b = q; break;
            }
        }

        return new Vector4(r, g, b, 1.0f);
    }
}

/// <summary>更新日志条目。</summary>
public class ChangelogEntry
{
    public string Version { get; set; } = ""; // 版本号（如 "v1.3.0"）。

    public string Date { get; set; } = ""; // 发布日期。

    public ChangeType Type { get; set; } // 更新类型（新功能/修复/破坏性变更等）。

    public List<string> Changes { get; set; } = new List<string>(); // 变更说明列表。
}

/// <summary>更新日志的变更类型。</summary>
public enum ChangeType
{
    Feature,    // 新功能
    Fix,        // 修复
    Breaking,   // 破坏性变更
    Initial,    // 初始版本
    Other       // 其他
}
