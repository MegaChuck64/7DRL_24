using System.Collections.Generic;

namespace GameCode.Sprites;

public class PlayerLoader : ISpriteLoader
{

    public int GetOptionCount() => 2;
    private Dictionary<int, SpriteInfo> spriteOptions = new()
    {
        { 0, new SpriteInfo("human_m", "Human") },
        { 1, new SpriteInfo("human_f", "Human") },
    };
    public SpriteInfo GetInfo(int option = 0)
    {
        return spriteOptions[option];
    }


}