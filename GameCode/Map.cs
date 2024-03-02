using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace GameCode;

public class Map
{
    public List<Tile> Tiles { get; set; }
    private Rectangle _sourceTempRect   = new(0, 0, Settings.SourceTileSize, Settings.SourceTileSize);
    private Rectangle _worldTempRect    = new(0, 0, Settings.TileSize, Settings.TileSize);
    private Rectangle _tileTempRect     = new(0, 0, 1, 1);
    private Rectangle _windowRect       = new(0, 0, Settings.MapWindowSize, Settings.MapWindowSize);

    private MainGame _game;
    private Point _offset = new();

    public Map(MainGame game)
    {
        _game = game;
    }
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
                    X = x,
                    Y = y,
                    SpriteName = rand.NextDouble() > 0.9f ? "Water" : "Grass"
                };
                Tiles.Add(tile);
            }
        }
    }

    public void Update(GameTime gt)
    {
        var mapDif = Settings.MapSize - Settings.MapWindowSize;

        if (_game.Input.WasPressed(Keys.D))
        {
            if (_offset.X > -mapDif)
                _offset.X--;
        }
        if (_game.Input.WasPressed(Keys.A))
        {
            if (_offset.X < 0)
                _offset.X++;
        }
        if (_game.Input.WasPressed(Keys.W))
        {
            if (_offset.Y < 0)
                _offset.Y++;
        }
        if (_game.Input.WasPressed(Keys.S))
        {
            if (_offset.Y > -mapDif)
                _offset.Y--;
        }
    }
    public void Draw(SpriteBatch sb)
    {
        foreach (var tile in Tiles)
        {
            _tileTempRect.X = tile.X + _offset.X;
            _tileTempRect.Y = tile.Y + _offset.Y;
            
            if (_windowRect.Contains(_tileTempRect.X, _tileTempRect.Y))
            {
                var drawBnd = _worldTempRect;
                drawBnd.X = Settings.TileSize * _tileTempRect.X;
                drawBnd.Y = Settings.TileSize * _tileTempRect.Y;
                drawBnd.Width *= _tileTempRect.Width;
                drawBnd.Height *= _tileTempRect.Height;

                var spr = Settings.Sprites[tile.SpriteName];
                var txt = Settings.Textures[spr.Path];
                _sourceTempRect.X = spr.X;
                _sourceTempRect.Y = spr.Y;

                sb.Draw(
                    texture: txt,
                    destinationRectangle: drawBnd,
                    sourceRectangle: _sourceTempRect,
                    color: Color.White);
            }
        }
    }
}