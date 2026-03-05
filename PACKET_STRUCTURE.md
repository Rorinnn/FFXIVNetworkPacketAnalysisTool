# FFXIV 网络包结构说明

## 包头结构

### 收包 (DOWN)

```
偏移   大小   类型     说明
--------------------------------------
0x00   4     uint32   包总长度 (含包头)
0x04   4     uint32   未知/保留
0x08   8     uint64   时间戳
0x10   2     ushort   未知
0x12   2     ushort   Opcode
0x14   2     ushort   未知
0x16   2     ushort   ServerID
0x18   ...   bytes    包体数据
```

### 发包 (UP)

```
偏移   大小   类型     说明
--------------------------------------
0x00   2     ushort   Opcode
0x02   2     ushort   未知
0x04   4     uint32   未知
0x08   4     uint32   包长度
0x0C   ...   bytes    包体数据
```

## 包长度获取

### 收包

`NetManager.GetPacketLengthFromStruct(opcodeName)` 根据 Opcode 名称在程序集中查找同名结构体，读取 `[StructLayout(Size = ...)]` 作为包体长度。未找到结构体或 Opcode 为 `Unknown` 时返回默认值 512 字节。查询结果由 `packetLengthCache`（`ConcurrentDictionary`）缓存。

### 发包

直接从包数据偏移 `0x08` 处读取：`*(uint*)(packet + 8)`。

## 定义数据包结构体

收包结构体放入 `PacketStructures/Down/`，发包放入 `PacketStructures/Up/`：

```csharp
using System.Runtime.InteropServices;

namespace FFXIVNetworkPacketAnalysisTool.PacketStructures;

// Size 会被 GetPacketLengthFromStruct 用作收包长度
[StructLayout(LayoutKind.Explicit, Size = 0x290)]
public unsafe struct DOWN_NpcSpawn
{
    [FieldOffset(0x10)] public ulong MainTarget;
    [FieldOffset(0x40)] public uint DataID;
    [FieldOffset(0x44)] public uint OwnerID;
}
```

**规则**：
1. 结构体名称必须与 Opcode 名称完全匹配（如 `DOWN_NpcSpawn`、`UP_ClientTrigger`）
2. 必须标注 `[StructLayout(LayoutKind.Explicit, Size = ...)]`
3. 定义后自动被识别，无需额外配置

## 实现细节

- **收包指针回退**：`ReceivePacketInternalDetour` 中 `packet -= 16` 是因为原始指针指向包体偏移处，需回退到包头起始位置
- **包头跳过**：结构体解析时，收包和发包数据前均有 0x20（32）字节的包头，解析器自动跳过（`PacketHeaderSize = 0x20`）
- **捕获大小限制**：`CapturePacket` 限制单包最大复制 4KB，防止超大包占用过多内存

## 调试技巧

在 `ReceivePacketInternalDetour` 中添加日志以确认包长度字段位置：

```csharp
Plugin.Log.Debug($"包长度: offset0={Marshal.ReadInt32((nint)packet, 0)}, "
               + $"offset24={Marshal.ReadInt32((nint)packet, 24)}, "
               + $"offset8={Marshal.ReadInt32((nint)packet, 8)}");
```
