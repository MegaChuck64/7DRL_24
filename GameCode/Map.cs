using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

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
        var riverCount = rand.Next(4,10);
        var rivers = new List<List<Point>>();
        for (int i = 0; i < riverCount; i++)
        {
            var river = GenerateRiver(rand);
            rivers.Add(river);
        }
        var radDiv = 2;
        var radius = Settings.MapSize / radDiv;
        var shorOffset = 0;

        for (int x = 0; x < Settings.MapSize; x++)
        {
            for (int y = 0; y < Settings.MapSize; y++)
            {
                var spriteName = "Grass";
                var dist = Vector2.Distance(new Vector2(x,y), new Vector2(Settings.MapSize/2, Settings.MapSize/2));
                var tempRad = radius;
                if (dist > radius)
                {
                    var shrOff = rand.Next(-1, 2);
                    tempRad += shrOff;
                }

                if (dist > tempRad || rivers.Any(r => r.Contains(new Point(x, y))))
                    spriteName = "Water";

                var tile = new Tile()
                {
                    X = x,
                    Y = y,
                    SpriteName = spriteName
                };
                Tiles.Add(tile);
            }
        }

        _offset = new Point(-(Settings.MapWindowSize / (radDiv * 2)), -(Settings.MapWindowSize / (radDiv * 2)));
    }

    private static List<Point> GenerateRiver(Random rand)
    {
        var river = new List<Point>();
        var _riverDir = (Direction)rand.Next(4);
        river.Add(new Point(rand.Next(Settings.MapSize/4, (Settings.MapSize/4) * 3), rand.Next(Settings.MapSize / 4, (Settings.MapSize / 4) * 3)));

        //var wall = rand.Next(4);
        //if (wall == 0)
        //{
        //    river.Add(new Point(rand.Next(Settings.MapSize), 0));
        //    _riverDir = Direction.South;
        //}
        //else if (wall == 1)
        //{
        //    river.Add(new Point(Settings.MapSize - 1, rand.Next(Settings.MapSize)));
        //    _riverDir = Direction.West;
        //}
        //else if (wall == 2)
        //{
        //    river.Add(new Point(rand.Next(Settings.MapSize), Settings.MapSize - 1));
        //    _riverDir = Direction.North;
        //}
        //else if (wall == 3)
        //{
        //    river.Add(new Point(0, rand.Next(Settings.MapSize)));
        //    _riverDir = Direction.East;
        //}

        var steps = 1000;
        for (int i = 0; i < steps; i++)
        {
            var last = river.Last();
            var count = 0;
            do
            {
                var nextdir = rand.Next(3);                
                var next = NextRiverLoc(_riverDir, nextdir);
                next += last;
                if (next.X >= 0 && next.Y >= 0 && next.X < Settings.MapSize - 1 && next.Y < Settings.MapSize && !river.Contains(next))
                {
                    river.Add(next);
                    break;
                }

                if (count++ > 1000)
                    break;

            } while (true);
        }

        return river;
    }

    private static Point NextRiverLoc(Direction dir, int choice)
    {
        if (dir == Direction.North)
        {
            if (choice == 0)
                return new Point(0, -1);
            else if (choice == 1)
                return new Point(1, 0);
            else if (choice == 2)
                return new Point(-1, 0);
        }
        else if (dir == Direction.East)
        {
            if (choice == 0)
                return new Point(1, 0);
            else if (choice == 1)
                return new Point(0, 1);
            else if (choice == 2)
                return new Point(0, -1);
        }
        else if (dir == Direction.South)
        {
            if (choice == 0)
                return new Point(0, 1);
            else if (choice == 1)
                return new Point(-1, 0);
            else if (choice == 2)
                return new Point(1, 0);
        }
        else if (dir == Direction.West)
        {
            if (choice == 0)
                return new Point(-1, 0);
            else if (choice == 1)
                return new Point(0, -1);
            else if (choice == 2)
                return new Point(0, 1);
        }

        
        return Point.Zero;
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

    public enum Direction
    {
        North,
        East,
        South,
        West

    }
}