using System.Collections.Generic;

namespace GameCode.Sprites;

public class GrassLoader : ISpriteLoader
{
    public int GetOptionCount() => 3;

    private readonly Dictionary<int, SpriteInfo> spriteOptions = new ()
    {
        { 0, new SpriteInfo("grass0", "Grass") },
        { 1, new SpriteInfo("grass1", "Grass") },
        { 2, new SpriteInfo("grass2", "Grass") },
    };

    public SpriteInfo GetInfo(int option = 0)
    {
        return spriteOptions[option];
    }
}