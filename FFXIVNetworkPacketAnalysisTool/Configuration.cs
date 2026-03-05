using Dalamud.Configuration;
using System;
using System.Collections.Generic;

namespace FFXIVNetworkPacketAnalysisTool;

/// <summary>
/// 插件持久化配置，由 Dalamud 自动序列化存储。
/// </summary>
[Serializable]
public class Configuration : IPluginConfiguration
{
    public int Version { get; set; } = 0; // 配置版本号。

    public string GameVersion { get; set; } = ""; // 当前游戏版本字符串。

    public string OpcodeSource { get; set; } = ""; // Opcode 数据来源描述文本。

    public Dictionary<int, string> UpOpcodes { get; set; } = new(); // 上行（客户端→服务器）Opcode 映射表：opcode → 名称。

    public Dictionary<int, string> DownOpcodes { get; set; } = new(); // 下行（服务器→客户端）Opcode 映射表：opcode → 名称。

    public bool ShowSendPackets { get; set; } = true; // 是否在包列表中显示发包。

    public bool ShowReceivePackets { get; set; } = true; // 是否在包列表中显示收包。

    public bool ShowOnlyKnownOpcodes { get; set; } = false; // 是否仅显示已知 Opcode 的包。

    public bool AutoScroll { get; set; } = true; // 是否自动滚动到包列表底部。

    public bool CaptureEnabled { get; set; } = true; // 是否启用包捕获。

    public int MaxPacketsPerSession { get; set; } = 5000; // 单个会话最大包数量，超出后丢弃最早的包。

    public void Save() // 将当前配置持久化保存到磁盘。
    {
        Plugin.PluginInterface.SavePluginConfig(this);
    }
}
