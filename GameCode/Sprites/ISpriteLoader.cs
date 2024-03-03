using System.Collections.Generic;

namespace GameCode.Sprites;

public interface ISpriteLoader
{
    public SpriteInfo GetInfo(int option = 0);
    public int GetOptionCount ();
}

public struct SpriteInfo
{
    public string TextureName { get; set; }
    public string Name { get; set; } 
    public Dictionary<string, string> Data { get; set; }

    public SpriteInfo(string textureName, string name, Dictionary<string, string> data = null)
    {
        TextureName = textureName;
        Name = name;
        Data = data ?? new Dictionary<string, string>();
    }
}