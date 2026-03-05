using Dalamud.Game;
using Dalamud.Game.ClientState.Objects;
using Dalamud.Game.Command;
using Dalamud.Interface.Windowing;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using FFXIVNetworkPacketAnalysisTool.Utils;
using FFXIVNetworkPacketAnalysisTool.Windows;
using OmenTools;
using System;
using System.IO;

namespace FFXIVNetworkPacketAnalysisTool;

/// <summary>
/// 插件主入口，负责初始化与管理所有窗口、命令和网络捕获服务。
/// </summary>
public sealed class Plugin : IDalamudPlugin
{
    [PluginService] internal static IDalamudPluginInterface PluginInterface { get; private set; } = null!;
    [PluginService] internal static ITextureProvider TextureProvider { get; private set; } = null!;
    [PluginService] internal static ICommandManager CommandManager { get; private set; } = null!;
    [PluginService] public static IPluginLog Log { get; private set; } = null!; // Dalamud 日志服务。
    [PluginService] public static IDataManager DataManager { get; private set; } = null!; // Dalamud 游戏数据管理服务。
    [PluginService] public static IObjectTable ObjectTable { get; private set; } = null!; // Dalamud 游戏对象表服务。
    [PluginService] public static ITargetManager TargetManager { get; private set; } = null!; // Dalamud 目标管理服务。
    [PluginService] public static IClientState ClientState { get; private set; } = null!; // Dalamud 客户端状态服务（用于判断国服/国际服）。
    [PluginService] public static ISigScanner SigScanner { get; private set; } = null!; // Dalamud 内存签名扫描服务。
    [PluginService] public static IGameInteropProvider Hook { get; private set; } = null!; // Dalamud 游戏函数 Hook 服务。
    [PluginService] public static ICondition Condition { get; private set; } = null!; // Dalamud 角色条件状态服务。
    [PluginService] public static IGameGui GameGui { get; private set; } = null!; // Dalamud 游戏 GUI 服务。
    [PluginService] public static IChatGui ChatGui { get; private set; } = null!; // Dalamud 聊天窗口服务。
    [PluginService] public static IAddonLifecycle AddonLifecycle { get; private set; } = null!; // Dalamud Addon 生命周期服务。
    [PluginService] public static IFramework Framework { get; private set; } = null!; // Dalamud 游戏框架服务。
    private const string CommandName = "/FFNPAT";



    public Configuration Configuration { get; init; } // 插件持久化配置实例。

    public readonly WindowSystem WindowSystem = new("FFXIVNetworkPacketAnalysisTool");
    private ConfigWindow ConfigWindow { get; init; }
    private MainWindow MainWindow { get; init; }

    private OnlineOpcode onlineOpcode;
    public static Lumina.GameData LuminaGameData => DataManager.GameData; // Lumina 游戏数据访问入口。
    public NetRe MyNetRe { get; private set; } = null!; // 网络包捕获与 Hook 管理器实例。

    public Plugin()
    {
        Configuration = PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();

        // 注入 Dalamud 服务到 OmenTools DService（仅注入，不做完整初始化）
        try { PluginInterface.Inject(DService.Instance()); }
        catch (Exception ex) { Log.Warning($"[FFNPAT] OmenTools DService 注入失败: {ex.Message}"); }

        var goatImagePath = Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "background.png");

        ConfigWindow = new ConfigWindow(this);
        MainWindow = new MainWindow(this, goatImagePath);

        WindowSystem.AddWindow(ConfigWindow);
        WindowSystem.AddWindow(MainWindow);

        CommandManager.AddHandler(CommandName, new CommandInfo(OnCommand)
        {
            HelpMessage = "打开主窗口"
        });

        PluginInterface.UiBuilder.Draw += DrawUI;

        PluginInterface.UiBuilder.OpenConfigUi += ToggleConfigUI;

        PluginInterface.UiBuilder.OpenMainUi += ToggleMainUI;

        onlineOpcode = new OnlineOpcode(this);

        onlineOpcode.Run();

        MyNetRe = new NetRe(Configuration);

    }

    public void Dispose() // 释放所有窗口、Hook 和命令资源。
    {
        WindowSystem.RemoveAllWindows();

        ConfigWindow.Dispose();
        MainWindow.Dispose();
        MyNetRe?.Dispose();
        CommandManager.RemoveHandler(CommandName);
    }

    private void OnCommand(string command, string args)
    {
        ToggleMainUI();
    }

    private void DrawUI() => WindowSystem.Draw();

    public void ToggleConfigUI() => ConfigWindow.Toggle(); // 切换配置窗口的显示/隐藏状态。
    public void ToggleMainUI() => MainWindow.Toggle(); // 切换主窗口的显示/隐藏状态。

}
