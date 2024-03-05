using Engine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace GameCode;

public class Map
{
    public Tile[,] Tiles { get; set; }
    public List<Tile> Items { get; set; }
    public Player Player { get; set; }

    private Random _rand;

    public Tile SelectedTile { get; set; }
    public Map()
    {
    }
    public void Generate()
    {
        Tiles = new Tile[Settings.MapSize, Settings.MapSize];
        Items = new List<Tile>();

        _rand = new Random();
        var riverCount = _rand.Next(4, 10);
        var rivers = new List<List<Point>>();
        for (int i = 0; i < riverCount; i++)
        {
            var river = GenerateRiver(_rand);
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
                var option = 0;

                option = GetRandomSpriteOption("Grass");

                var dist = Vector2.Distance(new Vector2(x, y), new Vector2(Settings.MapSize / 2, Settings.MapSize / 2));
                var tempRad = radius;
                var data = new List<string>();
                if (dist > radius)
                {
                    shorOffset += _rand.Next(-2, 3);
                    tempRad += shorOffset;
                    if (tempRad > radius)
                        tempRad = radius;
                }

                if (dist > tempRad || rivers.Any(r => r.Contains(new Point(x, y))))
                {
                    spriteName = "Water";
                    option = GetRandomSpriteOption(spriteName);
                    data.Add("Collider");
                }
                else if (dist > tempRad * .85f)
                {
                    spriteName = "Sand";
                    option = GetRandomSpriteOption(spriteName);
                }
                else if (_rand.NextDouble() > 0.95f)
                {
                    var treeTile = new Tile()
                    {
                        X = x,
                        Y = y,
                        SpriteName = "Tree",
                        Option = GetRandomSpriteOption("Tree"),
                        Data = new List<string> { "Object", "Collider"}
                    };
                    Items.Add(treeTile);
                }

                var tile = new Tile()
                {
                    X = x,
                    Y = y,
                    SpriteName = spriteName,
                    Option = option,
                    Data = data,
                };
                Tiles[x,y] = tile;
            }
        }

        var startTile = GetRandomEmptyTileInRange(10, Settings.MapSize / 2, Settings.MapSize / 2);
        Player = new Player(startTile.X, startTile.Y);

        PlaceWelcomeScroll();
    }

    private void PlaceWelcomeScroll()
    {
        var choice = GetRandomEmptyTileInRange(5, Player.X, Player.Y);

        var path = PathFinder.GetPath(new Point(Player.X, Player.Y), new Point(choice.X, choice.Y), GetCollisionMap(this));
        if (path == null || path.Count == 0)
        {
            throw new Exception("Todo: can't reach chosen grass");
        }

        var scrollTile = new Tile()
        {
            X = choice.X,
            Y = choice.X,
            SpriteName = "Scroll",
            Option = GetRandomSpriteOption("Scroll"),
            Data = new List<string> { "Object", "Collectable"}
        };
        Items.Add(scrollTile);
    }

    private Tile GetRandomEmptyTileInRange(int range, int x, int y)
    {
        var empty = GetEmptyTilesInRange(range, x, y);
        var choice = _rand.Next(empty.Count);
        var tile = empty.ElementAt(choice);
        return Tiles[tile.X, tile.Y];
    }
    private List<Tile> GetEmptyTilesInRange(int range, int x, int y)
    {
        var tiles = new List<Tile>();
        var rect = new Rectangle(
           x - range,
           y - range,
           range * 2,
           range * 2);

        var nearby = GetTilesInRect(rect);
        var empty = nearby.Where(t => !Items.Any(i => i.X == t.X && i.Y == t.Y) && !t.Data.Contains("Collider"));
        tiles.AddRange(empty);

        return tiles;
    }

    private List<Tile> GetTilesInRect(Rectangle rect)
    {
        var tiles = new List<Tile>();

        var startX = rect.X;
        var endX = rect.X + rect.Width;
        var startY = rect.Y;
        var endY = rect.Y + rect.Height;

        for (int x = startX; x < endX; x++)
        {
            for (int y = startY; y < endY; y++)
            {
                tiles.Add(Tiles[x,y]);
            }
        }

        return tiles;
    }


    private int GetRandomSpriteOption(string spriteName)
    {
        return _rand.Next(Settings.Sprites[spriteName].Count);
    }

    public static bool[,] GetCollisionMap(Map map)
    {
        var colM = new bool[Settings.MapSize, Settings.MapSize];
        for (int x = 0; x < Settings.MapSize; x++)
        {
            for (int y = 0; y < Settings.MapSize; y++)
            {      
                colM[x, y] = true;
                if (map.Tiles[x,y].Data.Contains("Collider"))
                {
                    colM[x, y] = false;
                }
            }
        }

        foreach (var tile in map.Items)
        {            
            if (tile.Data.Contains("Collider"))
            {
                colM[tile.X, tile.Y] = false;
            }
        }
        return colM;
    }

    private static List<Point> GenerateRiver(Random rand)
    {
        var river = new List<Point>();
        var _riverDir = (Direction)rand.Next(4);
        river.Add(new Point(rand.Next(Settings.MapSize/4, (Settings.MapSize/4) * 3), rand.Next(Settings.MapSize / 4, (Settings.MapSize / 4) * 3)));

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


    

    public enum Direction
    {
        North,
        East,
        South,
        West

    }
}
