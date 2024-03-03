using System.Collections.Generic;

namespace GameCode.Sprites;

public class SandLoader : ISpriteLoader
{
    public int GetOptionCount() => 8;

    private readonly Dictionary<int, SpriteInfo> spriteOptions = new()
    {
        { 0, new SpriteInfo("sand1", "Sand") },
        { 1, new SpriteInfo("sand2", "Sand") },
        { 2, new SpriteInfo("sand3", "Sand") },
        { 3, new SpriteInfo("sand4", "Sand") },
        { 4, new SpriteInfo("sand5", "Sand") },
        { 5, new SpriteInfo("sand6", "Sand") },
        { 6, new SpriteInfo("sand7", "Sand") },
        { 7, new SpriteInfo("sand8", "Sand") },
    };

    public SpriteInfo GetInfo(int option = 0)
    {
        return spriteOptions[option];
    }

}