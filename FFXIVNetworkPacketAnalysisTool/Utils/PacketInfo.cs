using System;
using System.Runtime.InteropServices;

namespace FFXIVNetworkPacketAnalysisTool.Utils;

/// <summary>数据包方向。</summary>
public enum PacketDirection
{
    Send, // 发包（客户端→服务器）。
    Receive // 收包（服务器→客户端）。
}

/// <summary>
/// 已捕获的单个网络数据包的完整信息。
/// </summary>
public class PacketInfo
{
    public long SessionId { get; set; } // 所属会话 ID。

    public DateTime Timestamp { get; set; } // 包被捕获的时间戳。

    public PacketDirection Direction { get; set; } // 包方向：发包或收包。

    public ushort Opcode { get; set; } // 包操作码（原始 ushort 值）。

    public string OpcodeName { get; set; } = "Unknown"; // Opcode 对应的可读名称，未知时为 "Unknown"。

    public byte[] RawData { get; set; } = []; // 包原始字节数据（含包头）。

    public uint PacketLength { get; set; } // 包实际总长度（字节）。

    public ushort Priority { get; set; } // 发包优先级（仅发包时有效）。

    public uint TargetID { get; set; } // 目标对象 ID（仅收包时有效）。

    public string TimeString => Timestamp.ToString("HH:mm:ss.fff"); // 用于 UI 显示的时间字符串（HH:mm:ss.fff）。

    public string DirectionString => Direction == PacketDirection.Send ? "发包 ↑" : "收包 ↓"; // 用于 UI 显示的方向字符串（"发包 ↑" 或 "收包 ↓"）。

    public string GetHexDump() // 生成包数据的十六进制转储文本（含偏移、Hex、ASCII 三列）。
    {
        if (RawData == null || RawData.Length == 0)
            return "无数据";

        var sb = new System.Text.StringBuilder();
        for (int i = 0; i < RawData.Length; i += 16)
        {
            // 偏移地址
            sb.AppendFormat("{0:X4}: ", i);

            // 十六进制部分
            for (int j = 0; j < 16; j++)
            {
                if (i + j < RawData.Length)
                    sb.AppendFormat("{0:X2} ", RawData[i + j]);
                else
                    sb.Append("   ");

                if (j == 7) sb.Append(" ");
            }

            sb.Append(" ");

            // ASCII部分
            for (int j = 0; j < 16 && i + j < RawData.Length; j++)
            {
                byte b = RawData[i + j];
                sb.Append(b >= 32 && b < 127 ? (char)b : '.');
            }

            sb.AppendLine();
        }
        return sb.ToString();
    }

    public unsafe static byte[] CloneData(byte* ptr, int length) // 从非托管指针复制指定长度的字节数据为托管数组。
    {
        if (ptr == null || length <= 0)
            return [];

        byte[] data = new byte[length];
        Marshal.Copy((IntPtr)ptr, data, 0, length);
        return data;
    }
}
