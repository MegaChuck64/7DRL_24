using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace GameCode;

public class Map
{
    public List<Tile> Tiles { get; set; }
    private Rectangle tempRect = new Rectangle(0, 0, Settings.SourceTileSize, Settings.SourceTileSize);


    public void Generate()
    {
        Tiles = new List<Tile>();        
        var rand = new Random();
        for (int x = 0; x < Settings.MapSize; x++)
        {
            for (int y = 0; y < Settings.MapSize; y++)
            {
                var tile = new Tile()
                {
                    Bounds = new Rectangle(x * Settings.TileSize, y * Settings.TileSize, Settings.TileSize, Settings.TileSize),
                    SpriteName = rand.NextDouble() > 0.9f ? "Water" : "Grass"
                };
                Tiles.Add(tile);
            }
        }
    }
    public void Draw(SpriteBatch sb)
    {
        foreach (var tile in Tiles)
        {
            var spr = Settings.Sprites[tile.SpriteName];
            var txt = Settings.Textures[spr.Path];
            tempRect.X = spr.X;
            tempRect.Y = spr.Y;
            sb.Draw(
                texture: txt,
                destinationRectangle: tile.Bounds,
                sourceRectangle: tempRect,
                color: Color.White);
        }
    }
}