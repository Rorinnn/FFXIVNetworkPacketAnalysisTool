using Dalamud.Interface.Windowing;
using Dalamud.Bindings.ImGui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using FFXIVNetworkPacketAnalysisTool.Utils;
using System.Reflection;
using System.Runtime.InteropServices;

namespace FFXIVNetworkPacketAnalysisTool.Windows;

/// <summary>
/// 坐标转换工具类，将 Web 坐标系与游戏内坐标系互相转换。
/// </summary>
public static class CoordinateConverter
{
    public static float PosWebToRaw(ushort webCoord) // 将 Web 坐标系（ushort）转换为游戏内原始坐标（float），参考 Python pos_web_to_raw 函数。
    {
        return webCoord / 1000f;
    }

    public static Vector3 CreatePosFromWebCoords(ushort[] webCoords) // 从三个 ushort Web 坐标创建 Vector3。
    {
        if (webCoords == null || webCoords.Length < 3)
            return Vector3.Zero;

        return new Vector3(
            PosWebToRaw(webCoords[0]),
            PosWebToRaw(webCoords[1]),
            PosWebToRaw(webCoords[2])
        );
    }
}

/// <summary>
/// 插件主窗口，包含包列表（时间轴）、详情面板和会话管理。
/// </summary>
public class MainWindow : Window, IDisposable
{
    private Plugin plugin;

    private List<PacketSession> sessions = new List<PacketSession>(); // 会话管理
    private PacketSession currentSession;
    private int sessionCounter = 1;
    private long pendingSessionSwitch = 0; // 待切换的会话ID

    private bool isPaused = false; // 控制状态

    private PacketInfo? selectedPacket = null; // 选中的包 - 支持多选
    private int selectedIndex = -1;
    private HashSet<int> selectedIndices = new HashSet<int>(); // 多选支持
    private int lastClickedIndex = -1; // Shift多选的起点

    private float leftPanelWidth = 600f; // UI 布局
    private float splitterWidth = 8f;

    private string filterText = ""; // 过滤器

    private int MaxPacketsInSession => plugin.Configuration.MaxPacketsPerSession;

    private Dictionary<string, Type?> structTypeCache = new Dictionary<string, Type?>(); // 包体结构体解析缓存

    public MainWindow(Plugin plugin, string goatImagePath) // 初始化主窗口，创建默认会话。
        : base("FFXIV NPATool###FFXIVNetworkPacketAnalysisTool")
    {
        SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(900, 500),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };

        this.plugin = plugin;

        currentSession = new PacketSession(DateTime.Now.Ticks, $"会话 {sessionCounter}");
        sessions.Add(currentSession);

        // 注意：不在构造函数中加载配置，因为 NetRe 可能还未初始化
        // 配置会在第一次 Draw() 时加载
    }

    public void Dispose() // 保存设置并释放所有会话资源。
    {
        SaveSettingsToConfig();

        foreach (var session in sessions)
        {
            session.Dispose();
        }
        sessions.Clear();
    }

    private bool settingsLoaded = false;

    private void LoadSettingsFromConfig()
    {
        if (settingsLoaded || plugin.MyNetRe == null)
            return;

        var config = plugin.Configuration;
        plugin.MyNetRe.CaptureEnabled = config.CaptureEnabled;
        settingsLoaded = true;
    }

    private void SaveSettingsToConfig()
    {
        if (plugin.MyNetRe == null)
            return;

        // 配置已经在使用时实时保存，这里只需要确保最终保存
        plugin.Configuration.CaptureEnabled = plugin.MyNetRe.CaptureEnabled;
        plugin.Configuration.Save();
    }

    public override void Draw() // 绘制主窗口全部 UI（会话标签 + 控制面板 + 分栏视图）。
    {
        LoadSettingsFromConfig();

        if (!isPaused && currentSession.IsActive)
        {
            ProcessIncomingPackets();
        }

        DrawSessionTabs();
        ImGui.Separator();

        DrawControlPanel();
        ImGui.Separator();

        DrawSplitView();
    }

    private void ProcessIncomingPackets()
    {
        if (plugin.MyNetRe == null)
            return;

        int processedCount = 0;
        while (plugin.MyNetRe.PacketQueue.TryDequeue(out var packet) && processedCount < 100) // 每帧最多处理100个包
        {
            packet.SessionId = currentSession.SessionId;
            currentSession.Packets.Add(packet);
            processedCount++;

            // 限制缓存大小
            if (currentSession.Packets.Count > MaxPacketsInSession)
            {
                currentSession.Packets.RemoveAt(0);
                if (selectedIndex >= 0)
                    selectedIndex--;
            }
        }
    }

    private void DrawSessionTabs()
    {
        ImGui.Text("会话标签:");
        ImGui.SameLine();

        // 使用 AutoSelectTabToFitWidth 让标签页自动调整宽度
        if (ImGui.BeginTabBar("SessionTabs", ImGuiTabBarFlags.Reorderable | ImGuiTabBarFlags.FittingPolicyScroll | ImGuiTabBarFlags.AutoSelectNewTabs))
        {
            for (int i = 0; i < sessions.Count; i++)
            {
                var session = sessions[i];

                // 活跃会话不显示关闭按钮
                bool isOpen = !session.IsActive;

                var tabFlags = ImGuiTabItemFlags.None;
                if (session.IsActive)
                    tabFlags |= ImGuiTabItemFlags.UnsavedDocument; // 活跃会话显示小点

                // 如果需要切换到这个会话，设置 SetSelected 标志
                if (pendingSessionSwitch == session.SessionId)
                {
                    tabFlags |= ImGuiTabItemFlags.SetSelected;
                    pendingSessionSwitch = 0; // 清除待切换标志
                }

                // 活跃会话使用 NoCloseWithMiddleMouseButton 防止中键关闭
                if (session.IsActive)
                    tabFlags |= ImGuiTabItemFlags.NoCloseWithMiddleMouseButton;

                bool hasCloseButton = !session.IsActive;

                if (hasCloseButton)
                {
                    if (ImGui.BeginTabItem($"{session.GetDisplayName()}###Session{session.SessionId}", ref isOpen, tabFlags))
                    {
                        // 切换到这个会话
                        if (currentSession != session)
                        {
                            currentSession = session;
                            selectedPacket = null;
                            selectedIndex = -1;
                            selectedIndices.Clear();
                            lastClickedIndex = -1;
                        }

                        ImGui.EndTabItem();
                    }
                }
                else
                {
                    // 活跃会话不带关闭按钮
                    if (ImGui.BeginTabItem($"{session.GetDisplayName()}###Session{session.SessionId}", tabFlags))
                    {
                        // 切换到这个会话
                        if (currentSession != session)
                        {
                            currentSession = session;
                            selectedPacket = null;
                            selectedIndex = -1;
                            selectedIndices.Clear();
                            lastClickedIndex = -1;
                        }

                        ImGui.EndTabItem();
                    }
                }

                // 用户点击关闭按钮（只有非活跃会话才会触发）
                if (!isOpen && !session.IsActive)
                {
                    // 如果只剩一个会话，不允许关闭
                    if (sessions.Count > 1)
                    {
                        // 如果关闭的是当前会话，切换到其他会话
                        if (currentSession == session)
                        {
                            int nextIndex = i > 0 ? i - 1 : 1;
                            currentSession = sessions[nextIndex];
                            selectedPacket = null;
                            selectedIndex = -1;
                            selectedIndices.Clear();
                            lastClickedIndex = -1;
                        }

                        // 清理并删除会话
                        session.Dispose();
                        sessions.RemoveAt(i);
                        break; // 退出循环，因为列表已被修改
                    }
                }
            }

            ImGui.EndTabBar();
        }
    }

    private void DrawControlPanel()
    {
        ImGui.Text("控制面板");
        ImGui.SameLine();

        if (ImGui.Button(isPaused ? "继续" : "暂停"))
        {
            isPaused = !isPaused;
        }

        ImGui.SameLine();

        if (ImGui.Button("清空日志"))
        {
            currentSession.Packets.Clear();
            selectedPacket = null;
            selectedIndex = -1;
            selectedIndices.Clear();
            lastClickedIndex = -1;
        }

        ImGui.SameLine();

        if (ImGui.Button("新建会话"))
        {
            currentSession.IsActive = false;

            sessionCounter++;
            var newSession = new PacketSession(DateTime.Now.Ticks, $"会话 {sessionCounter}");
            sessions.Add(newSession);
            currentSession = newSession;

            // 标记需要切换到新会话
            pendingSessionSwitch = newSession.SessionId;

            selectedPacket = null;
            selectedIndex = -1;
            selectedIndices.Clear();
            lastClickedIndex = -1;
        }

        ImGui.SameLine();

        if (ImGui.Button("删除选中"))
        {
            DeleteSelectedPackets();
        }

        ImGui.SameLine();

        bool autoScroll = plugin.Configuration.AutoScroll;
        if (ImGui.Checkbox("自动滚动", ref autoScroll))
        {
            plugin.Configuration.AutoScroll = autoScroll;
            plugin.Configuration.Save();
        }

        ImGui.SameLine();

        bool captureEnabled = plugin.Configuration.CaptureEnabled;
        if (ImGui.Checkbox("启用捕获", ref captureEnabled))
        {
            plugin.Configuration.CaptureEnabled = captureEnabled;
            if (plugin.MyNetRe != null)
            {
                plugin.MyNetRe.CaptureEnabled = captureEnabled;
            }
            plugin.Configuration.Save();
        }

        ImGui.SameLine();
        ImGui.Text($"| 总计: {currentSession.Packets.Count} 包");
        if (selectedIndices.Count > 0)
        {
            ImGui.SameLine();
            ImGui.TextColored(new Vector4(1f, 0.5f, 0.2f, 1f), $"| 已选: {selectedIndices.Count}");
        }

        ImGui.SetNextItemWidth(200);
        ImGui.InputTextWithHint("##filter", "搜索 Opcode...", ref filterText, 128);

        ImGui.SameLine();

        bool showSendPackets = plugin.Configuration.ShowSendPackets;
        if (ImGui.Checkbox("发包", ref showSendPackets))
        {
            plugin.Configuration.ShowSendPackets = showSendPackets;
            plugin.Configuration.Save();
        }

        ImGui.SameLine();

        bool showReceivePackets = plugin.Configuration.ShowReceivePackets;
        if (ImGui.Checkbox("收包", ref showReceivePackets))
        {
            plugin.Configuration.ShowReceivePackets = showReceivePackets;
            plugin.Configuration.Save();
        }

        ImGui.SameLine();

        bool showOnlyKnownOpcodes = plugin.Configuration.ShowOnlyKnownOpcodes;
        if (ImGui.Checkbox("仅已知Opcode", ref showOnlyKnownOpcodes))
        {
            plugin.Configuration.ShowOnlyKnownOpcodes = showOnlyKnownOpcodes;
            plugin.Configuration.Save();
        }

        ImGui.SameLine();
        ImGui.SetNextItemWidth(120);
        int maxPackets = plugin.Configuration.MaxPacketsPerSession;
        if (ImGui.DragInt("最大包数", ref maxPackets, 100, 100, 50000))
        {
            plugin.Configuration.MaxPacketsPerSession = maxPackets;
            plugin.Configuration.Save();
        }
    }

    private void DeleteSelectedPackets()
    {
        if (selectedIndices.Count == 0)
            return;

        var filteredPackets = GetFilteredPackets();

        var packetsToDelete = new List<PacketInfo>();
        foreach (var index in selectedIndices.OrderBy(x => x))
        {
            if (index >= 0 && index < filteredPackets.Count)
            {
                packetsToDelete.Add(filteredPackets[index]);
            }
        }

        foreach (var packet in packetsToDelete)
        {
            currentSession.Packets.Remove(packet);
        }

        selectedPacket = null;
        selectedIndex = -1;
        selectedIndices.Clear();
        lastClickedIndex = -1;
    }

    private void DrawSplitView()
    {
        var availableRegion = ImGui.GetContentRegionAvail();

        ImGui.BeginChild("PacketListPanel", new Vector2(leftPanelWidth, availableRegion.Y), true);
        DrawPacketList();
        ImGui.EndChild();

        ImGui.SameLine();

        DrawSplitter(availableRegion.Y);

        ImGui.SameLine();

        var rightPanelWidth = availableRegion.X - leftPanelWidth - splitterWidth;
        ImGui.BeginChild("PacketDetailPanel", new Vector2(rightPanelWidth, availableRegion.Y), true);
        DrawPacketDetail();
        ImGui.EndChild();
    }

    private void DrawSplitter(float height)
    {
        var splitterPos = ImGui.GetCursorScreenPos();
        var splitterSize = new Vector2(splitterWidth, height);

        ImGui.InvisibleButton("Splitter", splitterSize);

        if (ImGui.IsItemHovered())
        {
            ImGui.SetMouseCursor(ImGuiMouseCursor.ResizeEw);
        }

        if (ImGui.IsItemActive())
        {
            if (ImGui.IsMouseDragging(ImGuiMouseButton.Left))
            {
                leftPanelWidth += ImGui.GetIO().MouseDelta.X;
                leftPanelWidth = Math.Clamp(leftPanelWidth, 300f, ImGui.GetContentRegionAvail().X - 300f);
            }
        }

        var drawList = ImGui.GetWindowDrawList();
        drawList.AddRectFilled(
            splitterPos,
            splitterPos + splitterSize,
            ImGui.GetColorU32(ImGui.IsItemHovered() ? ImGuiCol.ButtonHovered : ImGuiCol.Border)
        );
    }

    private void DrawPacketList()
    {
        ImGui.Text("包列表 (时间轴)");
        ImGui.Separator();

        if (ImGui.BeginTable("PacketTable", 5, ImGuiTableFlags.Borders | ImGuiTableFlags.RowBg | ImGuiTableFlags.ScrollY | ImGuiTableFlags.Resizable))
        {
            ImGui.TableSetupColumn("时间", ImGuiTableColumnFlags.WidthFixed, 100f);
            ImGui.TableSetupColumn("方向", ImGuiTableColumnFlags.WidthFixed, 60f);
            ImGui.TableSetupColumn("Opcode", ImGuiTableColumnFlags.WidthFixed, 70f);
            ImGui.TableSetupColumn("名称", ImGuiTableColumnFlags.WidthStretch);
            ImGui.TableSetupColumn("长度(以结构体大小为准)", ImGuiTableColumnFlags.WidthFixed, 60f);
            ImGui.TableSetupScrollFreeze(0, 1);
            ImGui.TableHeadersRow();

            // 过滤后的包列表
            var filteredPackets = GetFilteredPackets();

            for (int i = 0; i < filteredPackets.Count; i++)
            {
                var packet = filteredPackets[i];

                ImGui.TableNextRow();
                ImGui.TableNextColumn();

                // 可选择的行 - 支持多选
                bool isSelected = selectedIndices.Contains(i);
                var rowColor = packet.Direction == PacketDirection.Send
                    ? new Vector4(0.2f, 0.3f, 0.5f, 0.3f)  // 发包：蓝色
                    : new Vector4(0.3f, 0.5f, 0.2f, 0.3f); // 收包：绿色

                // 设置行背景色
                if (isSelected)
                {
                    // 选中项：鲜红色背景
                    ImGui.TableSetBgColor(ImGuiTableBgTarget.RowBg0, ImGui.GetColorU32(new Vector4(0.85f, 0.1f, 0.1f, 1.0f)));
                    ImGui.TableSetBgColor(ImGuiTableBgTarget.RowBg1, ImGui.GetColorU32(new Vector4(0.85f, 0.1f, 0.1f, 1.0f)));

                    // 推送选中样式，使用更鲜艳的红色
                    ImGui.PushStyleColor(ImGuiCol.Header, new Vector4(0.9f, 0.1f, 0.1f, 1.0f));
                    ImGui.PushStyleColor(ImGuiCol.HeaderHovered, new Vector4(1.0f, 0.2f, 0.2f, 1.0f));
                    ImGui.PushStyleColor(ImGuiCol.HeaderActive, new Vector4(0.95f, 0.15f, 0.15f, 1.0f));
                }
                else
                {
                    ImGui.TableSetBgColor(ImGuiTableBgTarget.RowBg0, ImGui.GetColorU32(rowColor));
                }

                if (ImGui.Selectable($"##{i}", isSelected, ImGuiSelectableFlags.SpanAllColumns))
                {
                    var io = ImGui.GetIO();

                    if (io.KeyShift && lastClickedIndex >= 0)
                    {
                        // Shift多选：选择范围
                        int start = Math.Min(lastClickedIndex, i);
                        int end = Math.Max(lastClickedIndex, i);

                        for (int j = start; j <= end; j++)
                        {
                            selectedIndices.Add(j);
                        }
                    }
                    else if (io.KeyCtrl)
                    {
                        // Ctrl多选：切换选择状态
                        if (selectedIndices.Contains(i))
                            selectedIndices.Remove(i);
                        else
                            selectedIndices.Add(i);
                    }
                    else
                    {
                        // 单选：清空其他选择
                        selectedIndices.Clear();
                        selectedIndices.Add(i);
                    }

                    lastClickedIndex = i;
                    selectedPacket = packet;
                    selectedIndex = i;
                }

                // 右键菜单：删除单个包
                if (ImGui.IsItemClicked(ImGuiMouseButton.Right))
                {
                    ImGui.OpenPopup($"PacketContextMenu{i}");
                }

                if (ImGui.BeginPopup($"PacketContextMenu{i}"))
                {
                    if (ImGui.MenuItem("重发此包"))
                    {
                        OpenResendWindow(packet);
                    }

                    if (ImGui.MenuItem("删除此包"))
                    {
                        currentSession.Packets.Remove(packet);
                        selectedIndices.Remove(i);
                        if (selectedPacket == packet)
                        {
                            selectedPacket = null;
                            selectedIndex = -1;
                        }
                    }

                    ImGui.EndPopup();
                }

                ImGui.SameLine();
                ImGui.Text(packet.TimeString);

                ImGui.TableNextColumn();
                ImGui.Text(packet.DirectionString);

                ImGui.TableNextColumn();
                ImGui.Text($"0x{packet.Opcode:X4}");

                ImGui.TableNextColumn();
                ImGui.Text(packet.OpcodeName);

                ImGui.TableNextColumn();
                ImGui.Text($"{packet.PacketLength}");

                // 如果是选中项，弹出之前推送的样式
                if (isSelected)
                {
                    ImGui.PopStyleColor(3);
                }
            }

            // 自动滚动到底部
            if (plugin.Configuration.AutoScroll && filteredPackets.Count > 0)
            {
                ImGui.SetScrollHereY(1.0f);
            }

            ImGui.EndTable();
        }
    }

    private List<PacketInfo> GetFilteredPackets()
    {
        return currentSession.Packets.Where(p =>
        {
            if (p.Direction == PacketDirection.Send && !plugin.Configuration.ShowSendPackets)
                return false;
            if (p.Direction == PacketDirection.Receive && !plugin.Configuration.ShowReceivePackets)
                return false;

            if (plugin.Configuration.ShowOnlyKnownOpcodes && p.OpcodeName == "Unknown")
                return false;

            if (!string.IsNullOrWhiteSpace(filterText))
            {
                return p.OpcodeName.Contains(filterText, StringComparison.OrdinalIgnoreCase) ||
                       p.Opcode.ToString("X4").Contains(filterText, StringComparison.OrdinalIgnoreCase);
            }

            return true;
        }).ToList();
    }

    private void DrawPacketDetail()
    {
        if (selectedPacket == null)
        {
            ImGui.TextColored(new Vector4(0.5f, 0.5f, 0.5f, 1f), "请从左侧列表选择一个包以查看详细信息");
            return;
        }

        var packet = selectedPacket;

        ImGui.Text($"包详细信息 - {packet.OpcodeName}");
        ImGui.Separator();

        ImGui.Text($"时间: {packet.Timestamp:yyyy-MM-dd HH:mm:ss.fff}");
        ImGui.Text($"方向: {packet.DirectionString}");
        ImGui.Text($"Opcode: 0x{packet.Opcode:X4} ({packet.Opcode})");
        ImGui.Text($"名称: {packet.OpcodeName}");
        ImGui.Text($"长度: {packet.PacketLength} 字节");

        if (packet.Direction == PacketDirection.Send)
        {
            ImGui.Text($"优先级: {packet.Priority}");
        }
        else
        {
            ImGui.Text($"目标ID: 0x{packet.TargetID:X8}");
        }

        ImGui.Separator();

        if (ImGui.BeginTabBar("PacketDetailTabs"))
        {
            if (ImGui.BeginTabItem("十六进制数据"))
            {
                DrawHexView(packet);
                ImGui.EndTabItem();
            }

            if (ImGui.BeginTabItem("结构体解析"))
            {
                DrawStructView(packet);
                ImGui.EndTabItem();
            }

            ImGui.EndTabBar();
        }
    }

    private void DrawHexView(PacketInfo packet)
    {
        var hexDump = packet.GetHexDump();

        ImGui.BeginChild("HexDumpChild", new Vector2(0, -30), true, ImGuiWindowFlags.HorizontalScrollbar);
        ImGui.PushFont(ImGui.GetIO().Fonts.Fonts[0]); // 使用等宽字体
        ImGui.TextUnformatted(hexDump);
        ImGui.PopFont();
        ImGui.EndChild();

        if (ImGui.Button("复制十六进制数据"))
        {
            ImGui.SetClipboardText(hexDump);
        }

        ImGui.SameLine();

        if (ImGui.Button("复制原始字节 (C# byte[])"))
        {
            var byteArrayStr = "byte[] data = new byte[] { " +
                string.Join(", ", packet.RawData.Select(b => $"0x{b:X2}")) + " };";
            ImGui.SetClipboardText(byteArrayStr);
        }
    }

    private void DrawStructView(PacketInfo packet)
    {
        var structType = FindStructType(packet.OpcodeName);

        if (structType == null)
        {
            ImGui.TextColored(new Vector4(1f, 0.7f, 0.3f, 1f), $"未找到对应的结构体定义: {packet.OpcodeName}");
            ImGui.Text("请确保已经定义了对应的结构体，例如：");
            ImGui.TextWrapped($"[StructLayout(LayoutKind.Explicit, Size = 0x...)]\npublic unsafe struct {packet.OpcodeName}\n{{\n    [FieldOffset(0x00)] public ushort Field1;\n    // ...\n}}");
            return;
        }

        const int PacketHeaderSize = 0x20; // 收包和发包数据前均有 0x20 (32) 字节的包头，需要跳过
        int headerOffset = PacketHeaderSize;

        ImGui.TextColored(new Vector4(0.4f, 0.8f, 0.4f, 1f), $"✓ 找到结构体: {structType.Name}");
        ImGui.SameLine();
        ImGui.TextColored(new Vector4(1f, 0.7f, 0.3f, 1f), $"收包结构体不能保证正确性，如有差异请手动调整结构体。");
        ImGui.Text($"结构体大小: {Marshal.SizeOf(structType)} 字节");
        if (headerOffset > 0)
        {
            string directionStr = packet.Direction == PacketDirection.Receive ? "收包" : "发包";
            ImGui.TextColored(new Vector4(0.7f, 0.7f, 1f, 1f), $"{directionStr}包头偏移: +0x{headerOffset:X2} ({headerOffset}) 字节");
        }

        ImGui.Spacing();
        ImGui.Separator();
        ImGui.Spacing();

        var fields = structType.GetFields(BindingFlags.Public | BindingFlags.Instance)
            .Where(f => f.GetCustomAttribute<FieldOffsetAttribute>() != null)
            .OrderBy(f => f.GetCustomAttribute<FieldOffsetAttribute>()!.Value)
            .ToList();

        if (fields.Count == 0)
        {
            ImGui.TextColored(new Vector4(1f, 0.7f, 0.3f, 1f), "结构体中没有定义字段");
            return;
        }

        ImGui.Text($"字段总数: {fields.Count}");
        ImGui.Spacing();

        ImGui.BeginChild("StructFieldsChild", new Vector2(0, 0), true);

        // 推送表格样式，增加行间距和内边距
        ImGui.PushStyleVar(ImGuiStyleVar.CellPadding, new Vector2(8f, 6f));
        ImGui.PushStyleVar(ImGuiStyleVar.ItemSpacing, new Vector2(8f, 4f));

        if (ImGui.BeginTable("StructFields", 4, ImGuiTableFlags.Borders | ImGuiTableFlags.RowBg | ImGuiTableFlags.ScrollY | ImGuiTableFlags.Resizable))
        {
            ImGui.TableSetupColumn("偏移", ImGuiTableColumnFlags.WidthFixed, 80f);
            ImGui.TableSetupColumn("字段名", ImGuiTableColumnFlags.WidthStretch);
            ImGui.TableSetupColumn("类型", ImGuiTableColumnFlags.WidthFixed, 120f);
            ImGui.TableSetupColumn("值", ImGuiTableColumnFlags.WidthFixed, 180f);
            ImGui.TableHeadersRow();

            foreach (var field in fields)
            {
                var offsetAttr = field.GetCustomAttribute<FieldOffsetAttribute>()!;
                var offset = offsetAttr.Value;

                ImGui.TableNextRow();
                ImGui.TableNextColumn();
                ImGui.Text($"0x{offset:X2}");

                ImGui.TableNextColumn();
                ImGui.Text(field.Name);

                ImGui.TableNextColumn();
                ImGui.Text(field.FieldType.Name);

                ImGui.TableNextColumn();

                try // 尝试读取值（收包需要加上包头偏移）
                {
                    var k = ReadFieldValue(field.FieldType, packet.RawData, offset + headerOffset);
                    // 一般来说只有枚举需要原始数据
                    if (field.FieldType.IsEnum)
                    {
                        var name = Enum.GetName(field.FieldType, k) ?? k.ToString();
                        var underlying = Enum.GetUnderlyingType(field.FieldType);
                        var numeric = Convert.ChangeType(k, underlying);
                        ImGui.Text($"{name} ({numeric})");
                    }
                    else
                    {
                        ImGui.Text(k.ToString());
                    }
                }
                catch (Exception e)
                {
                    ImGui.TextColored(new Vector4(1f, 0f, 0f, 1f), $"{e.Message}");
                }
            }

            ImGui.EndTable();
        }

        ImGui.PopStyleVar(2);

        ImGui.EndChild();
    }


    public static object ReadFieldValue(Type type, byte[] data, int offset) // 从字节数组的指定偏移处读取指定类型的字段值。
    {
        if (offset >= data.Length) return "超出范围";
        var method = typeof(StructConverter)
                     .GetMethod(nameof(StructConverter.FromBytesGeneric), BindingFlags.Static | BindingFlags.Public)
                     ?.MakeGenericMethod(type);

        return method?.Invoke(null, [data, offset]) ?? "反射失败";
    }

    public static class StructConverter // 结构体与字节数组之间的泛型转换工具。
    {
        public static T FromBytesGeneric<T>(byte[] data, int offset) where T : struct // 将字节数组指定偏移处的数据转换为目标结构体。
        {
            return MemoryMarshal.Cast<byte, T>(data.AsSpan(offset))[0];
        }
    }


    private Type? FindStructType(string opcodeName)
    {
        if (structTypeCache.TryGetValue(opcodeName, out var cachedType))
            return cachedType;

        try
        {
            // 在当前程序集中查找
            var assembly = Assembly.GetExecutingAssembly();
            var types = assembly.GetTypes();

            Plugin.Log.Debug($"[结构体查找] 开始查找: {opcodeName}, 程序集中共有 {types.Length} 个类型");

            foreach (var type in types)
            {
                // 详细记录匹配过程
                if (type.Name == opcodeName)
                {
                    Plugin.Log.Debug($"[结构体查找] 找到名称匹配: {type.FullName}");
                    Plugin.Log.Debug($"[结构体查找]   IsValueType: {type.IsValueType}");

                    // 尝试多种方式获取 StructLayout 特性
                    var structLayoutAttr = type.GetCustomAttribute<StructLayoutAttribute>(inherit: false);

                    // 如果第一种方式失败，尝试直接从 CustomAttributes 查找
                    if (structLayoutAttr == null)
                    {
                        var attrs = type.GetCustomAttributesData();
                        foreach (var attr in attrs)
                        {
                            if (attr.AttributeType == typeof(StructLayoutAttribute))
                            {
                                structLayoutAttr = Attribute.GetCustomAttribute(type, typeof(StructLayoutAttribute)) as StructLayoutAttribute;
                                break;
                            }
                        }
                    }

                    Plugin.Log.Debug($"[结构体查找]   StructLayout: {structLayoutAttr != null}");

                    // 对于 unsafe struct，只要是 ValueType 就认为是有效的
                    if (type.IsValueType)
                    {
                        structTypeCache[opcodeName] = type;
                        Plugin.Log.Info($"[结构体查找] ✓ 成功找到结构体: {type.FullName} (Size: {Marshal.SizeOf(type)} 字节)");
                        return type;
                    }
                }
            }

            Plugin.Log.Warning($"[结构体查找] ✗ 未找到结构体: {opcodeName}");
        }
        catch (Exception ex)
        {
            Plugin.Log.Error($"[结构体查找] 查找时出错: {ex.Message}");
        }

        structTypeCache[opcodeName] = null;
        return null;
    }

    private void OpenResendWindow(PacketInfo packet)
    {
        // 查找对应的结构体
        var structType = FindStructType(packet.OpcodeName);

        // 生成唯一的窗口ID
        var windowId = $"ResendPacket{packet.Timestamp.Ticks}";

        // 检查是否已经存在相同的窗口
        var existingWindow = plugin.WindowSystem.Windows.FirstOrDefault(w => w.WindowName.Contains(windowId));
        if (existingWindow != null)
        {
            // 如果窗口已存在，直接打开它
            existingWindow.IsOpen = true;
            return;
        }

        // 创建重发窗口
        var resendWindow = new PacketResendWindow(plugin, packet, structType);

        // 添加到窗口系统
        plugin.WindowSystem.AddWindow(resendWindow);
    }
}
