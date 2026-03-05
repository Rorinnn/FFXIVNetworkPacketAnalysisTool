using Dalamud.Interface.Textures.TextureWraps;
using Dalamud.Interface.Textures;

namespace FFXIVNetworkPacketAnalysisTool.Utils;

/// <summary>
/// 游戏纹理加载工具类。
/// </summary>
public class TexturesHelper
{
    public static IDalamudTextureWrap? GetTextureFromIconId(uint iconId, uint stackCount = 0, bool hdIcon = true) // 根据游戏图标 ID 获取纹理。
    {
        GameIconLookup lookup = new GameIconLookup(iconId + stackCount, false, hdIcon);
        return Plugin.TextureProvider.GetFromGameIcon(lookup).GetWrapOrDefault();
    }

    public static IDalamudTextureWrap? GetTextureFromPath(string path) // 根据游戏内路径获取纹理。
    {
        return Plugin.TextureProvider.GetFromGame(path).GetWrapOrDefault();
    }
}
