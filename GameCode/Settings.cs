using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    public static Dictionary<string, List<string>> Sprites = new Dictionary<string, List<string>>();
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
        var textures = json["Textures"] as JsonArray;
        foreach (var spr in textures)
        {
            var val = spr.GetValue<string>();
            var splt = val.Split('/', System.StringSplitOptions.TrimEntries | System.StringSplitOptions.RemoveEmptyEntries);
            var name = splt.Last();
            var text = content.Load<Texture2D>(Path.Combine(splt));
            Textures.Add(name, text);
        }

        var sprites = json["Sprites"];
        Sprites = sprites.Deserialize<Dictionary<string, List<string>>>();

        Fonts.Add("font_18", content.Load<SpriteFont>(Path.Combine("Fonts", "font_18")));
    }
}
