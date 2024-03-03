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

    public SpriteInfo(string textureName, string name)
    {
        TextureName = textureName;
        Name = name;
    }
}