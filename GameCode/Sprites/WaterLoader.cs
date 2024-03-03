using System.Collections.Generic;

namespace GameCode.Sprites;

public class WaterLoader : ISpriteLoader
{
    private readonly Dictionary<int, SpriteInfo> options = new()
    {
        { 0, new SpriteInfo("shallow_water", "Water") },
        { 1, new SpriteInfo("shallow_water2", "Water") },
    };

    public int GetOptionCount() => 2;

    public SpriteInfo GetInfo(int option = 0)
    {
        return options[option];
    }
}