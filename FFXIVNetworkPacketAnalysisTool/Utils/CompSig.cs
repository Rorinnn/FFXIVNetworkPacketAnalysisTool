using System;
using System.Runtime.InteropServices;
using Dalamud.Game;
using Dalamud.Hooking;

namespace FFXIVNetworkPacketAnalysisTool.Utils;

/// <summary>
/// 复合签名（支持国服/国际服不同签名）
/// </summary>
public record CompSig(string Signature, string? SignatureCN = null)
{
    public static bool IsClientCN => Plugin.ClientState.ClientLanguage == (ClientLanguage)4; // 当前客户端是否为国服。

    public string? Get() => TryGet(out var sig) ? sig : null; // 根据客户端区域返回对应的签名字符串，失败返回 null。

    public bool TryGet(out string? signature) // 尝试获取当前区域的签名字符串。
    {
        signature = IsClientCN && !string.IsNullOrWhiteSpace(SignatureCN) ? SignatureCN : Signature;
        return !string.IsNullOrWhiteSpace(signature);
    }

    private bool TryGetValidSignature(out string sig)
        => TryGet(out sig!) && !string.IsNullOrWhiteSpace(sig);
    public nint ScanText() // 在游戏内存中搜索文本段签名，返回匹配地址；未找到返回 nint.Zero。
    {
        if (!TryGetValidSignature(out var sig)) return nint.Zero;
        try { return Plugin.SigScanner.ScanText(sig); }
        catch (Exception ex)
        {
            Serilog.Log.Warning($"[CompSig] ScanText 未找到签名 \"{sig}\": {ex.Message}");
            return nint.Zero;
        }
    }
    public unsafe T* ScanText<T>() where T : unmanaged // 在游戏内存中搜索文本段签名，返回类型化指针。
        => TryGetValidSignature(out var sig) ? (T*)Plugin.SigScanner.ScanText(sig) : null;
    public nint GetStatic(int offset = 0) // 从签名中获取静态地址。
        => TryGetValidSignature(out var sig) ? Plugin.SigScanner.GetStaticAddressFromSig(sig, offset) : nint.Zero;

    public unsafe T* GetStatic<T>(int offset = 0) where T : unmanaged // 从签名中获取静态地址的类型化指针。
        => TryGetValidSignature(out var sig) ? (T*)Plugin.SigScanner.GetStaticAddressFromSig(sig, offset) : null;

    public T GetDelegate<T>() where T : Delegate // 从签名扫描结果创建委托。
        => Marshal.GetDelegateForFunctionPointer<T>(ScanText());
    public Hook<T> GetHook<T>(T detour) where T : Delegate // 从签名创建 Dalamud Hook。
        => Plugin.Hook.HookFromSignature(Get() ?? string.Empty, detour);
}
