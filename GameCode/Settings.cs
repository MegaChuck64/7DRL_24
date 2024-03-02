using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace GameCode;

public static class Settings
{
    public static int SourceTileSize = 16;

    public static int TileSize = 16;

    public static int Width = 800;

    public static int Height = 600;

    public static int MapSize = 64;

    public static int MapWindowSize = 48;

    public static Dictionary<string, Texture2D> Textures = new Dictionary<string, Texture2D>();
    public static Dictionary<string, Sprite> Sprites = new Dictionary<string, Sprite>();
    public static Dictionary<string, SpriteFont> Fonts = new Dictionary<string, SpriteFont>();
    public static void Init()
    {
        var fl = File.ReadAllText("Settings.json");

        var json = JsonNode.Parse(fl);

        //global settings
        var globalSettings = json["GlobalSettings"];
        SourceTileSize = globalSettings["SourceTileSize"].GetValue<int>();
        TileSize = globalSettings["TileSize"].GetValue<int>();
        Width = globalSettings["Width"].GetValue<int>();
        Height = globalSettings["Height"].GetValue<int>();
        MapSize = globalSettings["MapSize"].GetValue<int>();
        MapWindowSize = globalSettings["MapWindowSize"].GetValue<int>();
    }

    public static void Load(ContentManager content)
    {
        var fl = File.ReadAllText("Settings.json");

        var json = JsonNode.Parse(fl);

        //textures
        Textures.Add("Objects/Floor", content.Load<Texture2D>(Path.Combine("Sprites", "DawnLike", "Objects", "Floor")));
        Textures.Add("Objects/Pit0", content.Load<Texture2D>(Path.Combine("Sprites", "DawnLike", "Objects", "Pit0")));

        var sprites = json["Sprites"];
        Sprites = sprites.Deserialize<Dictionary<string, Sprite>>();

        Fonts.Add("font_18", content.Load<SpriteFont>(Path.Combine("Fonts", "font_18")));
    }

}

public class Sprite
{
    public string Path { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
}