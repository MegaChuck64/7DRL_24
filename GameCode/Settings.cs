using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace GameCode;

public static class Settings
{
    public static int SourceTileSize = 16;

    public static int TileSize = 16;

    public static int Width = 800;

    public static int Height = 600;

    public static int MapSize = 64;

    public static int MapWindowSize = 48;
    public static string WelcomeMessage = string.Empty;
    public static int MapPixelWidth { get; private set; }

    public static Dictionary<string, Texture2D> Textures = new Dictionary<string, Texture2D>();
    public static Dictionary<string, List<string>> Sprites = new Dictionary<string, List<string>>();
    public static Dictionary<string, SpriteFont> Fonts = new Dictionary<string, SpriteFont>();
    public static Dictionary<string, ItemDescription> Items = new Dictionary<string, ItemDescription>();
    public static void Init()
    {
        var fl = File.ReadAllText("Settings.json");

        var json = JsonNode.Parse(fl, null, new JsonDocumentOptions()
        {
            CommentHandling = JsonCommentHandling.Skip,
        });

        //global settings
        var globalSettings = json["GlobalSettings"];
        SourceTileSize = globalSettings["SourceTileSize"].GetValue<int>();
        TileSize = globalSettings["TileSize"].GetValue<int>();
        Width = globalSettings["Width"].GetValue<int>();
        Height = globalSettings["Height"].GetValue<int>();
        MapSize = globalSettings["MapSize"].GetValue<int>();
        MapWindowSize = globalSettings["MapWindowSize"].GetValue<int>();
        MapPixelWidth = MapWindowSize * TileSize;
    }

    public static void Load(ContentManager content, GraphicsDevice graphicsDevice)
    {
        var fl = File.ReadAllText("Settings.json");

        var json = JsonNode.Parse(fl, null, new JsonDocumentOptions()
        {
            CommentHandling = JsonCommentHandling.Skip,
        });
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

        var items = json["Items"];
        Items = items.Deserialize<Dictionary<string, ItemDescription>>();

        Fonts.Add("font_18", content.Load<SpriteFont>(Path.Combine("Fonts", "font_18")));
        Fonts.Add("font_12", content.Load<SpriteFont>(Path.Combine("Fonts", "font_12")));
        Fonts.Add("font_8", content.Load<SpriteFont>(Path.Combine("Fonts", "font_8")));

        var txt = new Texture2D(graphicsDevice, 1, 1);
        txt.SetData(new Color[] { Color.White });
        Textures["pixel"] = txt;
    }

    public static Item CreateItem(string name, int option, int x = 0, int y = 0)
    {
        var item = Items[name];
        return item.Type switch
        {
            ItemType.Static => new Item
            {
                SpriteName = name,
                Option = option,
                Description = item.Description,
                Collectable = false,
                Collider = item.Collider,
                X = x,
                Y = y,
            },
            ItemType.Collectable => new Item
            {
                SpriteName = name,
                Option = option,
                Description = item.Description,
                Collectable = true,
                Collider = item.Collider,
                X = x,
                Y = y
            },
            ItemType.Weapon => new Weapon
            {
                SpriteName = name,
                Option = option,
                Collectable = true,
                Description = item.Description,
                Collider = item.Collider,
                X = x,
                Y = y
            },
            _ => throw new System.NotImplementedException(),
        };
    }

    public static void PerformAction(ActionType type, Map map, params (string name, object obj) [] args)
    {
        switch (type)
        {
            case ActionType.Chop:
                PerformChop(map, args);
                break;
            case ActionType.BasicAttack:
                PerformBasicAttack(args);
                break;
            default:
                break;
        }
    }


    private static void PerformChop(Map map, params (string name, object obj)[] args)
    {
        map.Items.Remove(map.SelectedTile as Item);
        map.Items.Add(CreateItem("Logs", 0, map.SelectedTile.X, map.SelectedTile.Y));

        map.SelectedTile = null;
    }

    private static void PerformBasicAttack((string name, object obj)[] args)
    {
        var attackerObj = args.FirstOrDefault(t => t.name == "attacker");
        var attackedObj = args.FirstOrDefault(t => t.name == "attacked");

        if (attackerObj.obj is not Actor attacker)
            return;

        if (attackedObj.obj is not Actor attacked)
            return;

        attacked.Health -= attacker.Strength;
    }
}

public class ItemDescription
{
    public string Description { get; set; }
    public ItemType Type { get; set; } = ItemType.Collectable;
    public string DisplayName { get; set; } = string.Empty;
    public bool Collider { get; set; } = false;
    public Dictionary<string, string> Data { get; set; } = new Dictionary<string, string>();
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ItemType
{
    Static,
    Collectable,
    Weapon,
}

public enum ActionType
{
    Chop,
    BasicAttack,
}