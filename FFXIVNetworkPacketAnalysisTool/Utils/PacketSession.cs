using System;
using System.Collections.Generic;

namespace FFXIVNetworkPacketAnalysisTool.Utils;

/// <summary>
/// 包捕获会话
/// 每个会话都是独立的，可以像浏览器标签页一样管理
/// </summary>
public class PacketSession : IDisposable
{
    public long SessionId { get; set; } // 会话唯一标识（基于创建时间的 Ticks）。

    public string SessionName { get; set; } // 会话显示名称。

    public DateTime CreateTime { get; set; } // 会话创建时间。

    public List<PacketInfo> Packets { get; set; } // 会话中已捕获的数据包列表。

    public bool IsActive { get; set; } // 会话是否为当前活跃会话（正在接收新包）。

    public PacketSession(long sessionId, string name) // 创建新的包捕获会话。
    {
        SessionId = sessionId;
        SessionName = name;
        CreateTime = DateTime.Now;
        Packets = new List<PacketInfo>();
        IsActive = true;
    }

    public string GetDisplayName() // 获取带包数量的会话显示名称（用于标签页标题）。
    {
        return $"{SessionName} ({Packets.Count} 包)";
    }

    public void Dispose() // 清空并释放会话中的所有包数据。
    {
        Packets.Clear();
        Packets = null!;
    }
}
