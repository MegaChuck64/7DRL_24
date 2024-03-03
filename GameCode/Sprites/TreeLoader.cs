using System.Collections.Generic;

namespace GameCode.Sprites;

public class TreeLoader : ISpriteLoader
{

    public int GetOptionCount() => 3;

    private readonly Dictionary<int, SpriteInfo> options = new()
    {
        { 0, new SpriteInfo("mangrove1", "Tree") },
        { 1, new SpriteInfo("mangrove2", "Tree") },
        { 2, new SpriteInfo("mangrove3", "Tree") },
    };

    public SpriteInfo GetInfo(int option = 0)
    {
        return options[option];
    }

}