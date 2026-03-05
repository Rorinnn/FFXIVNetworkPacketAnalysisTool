<p align="center">
  <img src="https://raw.githubusercontent.com/extrant/IMGSave/refs/heads/main/FFXivStorkLauncher/NPATool.png" alt="LOGO">
</p>

# FFXIV Network Packet Analysis Tool

FFXIV Network Packet Analysis Tool（以下简称 FFXIV NPATool）是一个用于分析最终幻想14游戏内网络数据包的 Dalamud 插件，主要面向开发人员使用。

## 贡献者

<p align="center">
  <a href="https://github.com/extrant/FFXIVNetworkPacketAnalysisTool/graphs/contributors">
    <img src="https://contrib.rocks/image?repo=extrant/FFXIVNetworkPacketAnalysisTool" />
  </a>
</p>

## 项目结构

```
FFXIVNetworkPacketAnalysisTool/
├── OmenTools/                   # 外部工具库（Git 子模块）
├── FFXIVNetworkPacketAnalysisTool/
│   ├── Plugin.cs                # 插件主入口
│   ├── Configuration.cs         # 持久化配置
│   ├── Utils/
│   │   ├── CompSig.cs           # 内存签名扫描封装
│   │   ├── NetManager.cs        # 网络包收发 Hook 管理器
│   │   ├── OnlineOpcode.cs      # 在线 Opcode 加载
│   │   ├── LocalOpcode.cs       # 本地 Opcode 加载
│   │   ├── PacketInfo.cs        # 数据包信息模型
│   │   ├── PacketSession.cs     # 数据包会话管理
│   │   └── GameImage.cs         # 游戏图片加载
│   ├── Windows/
│   │   ├── MainWindow.cs        # 主窗口（数据包列表与详情）
│   │   ├── ConfigWindow.cs      # 设置窗口
│   │   └── PacketResendWindow.cs # 数据包重发窗口
│   └── PacketStructures/
│       ├── Down/                # 收包（服务器→客户端）结构体
│       ├── Up/                  # 发包（客户端→服务器）结构体
│       ├── ActorControlId.cs    # ActorControl 子类型枚举
│       └── ClientTriggerFlag.cs # ClientTrigger 子类型枚举
└── FFXIVNetworkPacketAnalysisTool.sln
```

## 功能特性

- **实时网络包捕获**：拦截游戏客户端与服务器之间的网络数据包（收包/发包），以时间轴展示。Opcode 名称解析依赖 [FFXIV.EXE Opcode](https://github.com/extrant/FFXIV.EXE/blob/main/Opcode/all_opcodes.json)，不保证实时性。建议自行维护 Opcode 或为该项目贡献更新。交流频道：[Discord](https://discord.gg/g8QKPAnCBa)（FFXIV NPATool 类别）

- **多会话管理**：支持创建多个独立捕获会话，方便对比不同时段的数据。

- **数据包过滤**：按方向（发包/收包）、Opcode 名称搜索、仅显示已知 Opcode。

- **数据包详细分析**：
  - 十六进制数据查看，支持复制为十六进制文本或 C# 字节数组格式。
  - 自动匹配结构体定义并解析字段，显示偏移、类型和值。
  - 收包/发包数据均自动跳过 0x20 字节包头偏移。

- **数据包重发**：选中已捕获的发包后可打开重发窗口，编辑字段值后重新发送至服务器。

- **便捷操作**：暂停/继续捕获、多选删除（Ctrl/Shift）、自动滚动至最新包。

## 安装

### 使用库链接安装

将以下链接粘贴到 Dalamud 第三方库链接中，在插件安装器中搜索 `FFXIV Network Packet Analysis Tool` 安装：

```
https://raw.githubusercontent.com/extrant/DalamudPlugins/main/pluginmaster.json
```

### 从源码构建

```bash
git clone --recursive https://github.com/extrant/FFXIVNetworkPacketAnalysisTool.git
```

使用 Visual Studio 2022 或 JetBrains Rider 打开 `FFXIVNetworkPacketAnalysisTool.sln`，编译即可。


## 使用方法

### 基本操作

游戏内输入 `/FFNPAT` 打开主窗口，默认自动开始捕获网络数据包。

<p align="center">
  <img src="https://raw.githubusercontent.com/extrant/IMGSave/refs/heads/main/FFXIV%20NPATool/%E5%9F%BA%E6%9C%AC%E4%B8%BB%E7%95%8C%E9%9D%A2.png" alt="基本主界面">
</p>

### 界面说明

#### 会话管理标签栏
- 活跃会话显示未保存标记（小圆点）
- 可创建多个独立会话，非活跃会话可关闭（至少保留一个）

#### 控制面板
- **暂停/继续**：控制实时捕获
- **清空日志**：清空当前会话所有数据包
- **新建会话**：创建新捕获会话
- **删除选中**：删除选中数据包（Ctrl 多选、Shift 范围选择）
- **自动滚动**：自动滚动到最新数据包
- **启用捕获**：全局捕获开关

#### 数据包列表（左侧面板）

| 背景色 | 含义 |
|--------|------|
| 蓝色 | 发包（客户端→服务器） |
| 绿色 | 收包（服务器→客户端） |
| 红色 | 已选中 |

支持 Ctrl 点击多选、Shift 范围选择、右键删除单个包。

<p align="center">
  <img src="https://raw.githubusercontent.com/extrant/IMGSave/refs/heads/main/FFXIV%20NPATool/%E6%94%B6%E5%8F%91%E5%8C%85%E7%95%8C%E9%9D%A2.png" alt="收发包界面">
</p>

#### 数据包详情（右侧面板）

<p align="center">
  <img src="https://raw.githubusercontent.com/extrant/IMGSave/refs/heads/main/FFXIV%20NPATool/%E9%80%89%E4%B8%AD%E5%8C%85%E4%BD%93%E8%A7%A3%E6%9E%90%E7%95%8C%E9%9D%A2.png" alt="数据包详情">
</p>

- **十六进制数据**：完整的十六进制转储，可复制为十六进制文本或 C# 字节数组
- **结构体解析**：自动匹配结构体定义，显示字段偏移、类型和值

<p align="center">
  <img src="https://raw.githubusercontent.com/extrant/IMGSave/refs/heads/main/FFXIV%20NPATool/%E7%BB%93%E6%9E%84%E4%BD%93%E7%95%8C%E9%9D%A2.png" alt="结构体">
</p>

### 定义数据包结构体

结构体名称必须与 Opcode 名称一致，收包以 `DOWN_` 前缀放入 `PacketStructures/Down/`，发包以 `UP_` 前缀放入 `PacketStructures/Up/`。

```csharp
using System.Runtime.InteropServices;

namespace FFXIVNetworkPacketAnalysisTool.PacketStructures;

[StructLayout(LayoutKind.Explicit, Size = 0x28)]
public unsafe struct DOWN_YourOpcodeName
{
    [FieldOffset(0x00)] public ushort Field1;
    [FieldOffset(0x02)] public ushort Field2;
    [FieldOffset(0x04)] public uint ObjectId;
    [FieldOffset(0x08)] public fixed uint Args[4];
}
```

定义后自动被识别：结构体的 `Size` 用于确定收包长度，无需额外配置。

### 配置选项

| 选项 | 说明 | 默认值 |
|------|------|--------|
| `ShowSendPackets` | 显示发包 | `true` |
| `ShowReceivePackets` | 显示收包 | `true` |
| `ShowOnlyKnownOpcodes` | 仅显示已知 Opcode | `false` |
| `AutoScroll` | 自动滚动 | `true` |
| `CaptureEnabled` | 启用捕获 | `true` |
| `MaxPacketsPerSession` | 单会话最大包数量 | `5000` |


