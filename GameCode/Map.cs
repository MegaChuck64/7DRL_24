using GameCode;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameCode;

public class Map
{
    public List<Tile> Tiles { get; set; }

    private MainGame _game;
    public Point Player = new(Settings.MapSize/2, Settings.MapSize/2);
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
                    shorOffset += rand.Next(-1, 2);                    
                    tempRad += shorOffset;
                    if (tempRad > radius)
                        tempRad = radius;
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

    public void Update(GameTime gt)
    {

        var next = Player;
        if (_game.Input.WasPressed(Keys.D))
        {   
            if (next.X < Settings.MapSize - 1)
                next.X++;
        }
        if (_game.Input.WasPressed(Keys.A))
        {   
            if (next.X > 0)
                next.X--;
        }
        if (_game.Input.WasPressed(Keys.W))
        {
            if (next.Y > 0)
                next.Y--;
        }
        if (_game.Input.WasPressed(Keys.S))
        {      
            if (next.Y < Settings.MapSize - 1)
                next.Y++;
        }

        if (next != Player)
        {
            if (!Tiles.Any(t => t.X == next.X && t.Y == next.Y && t.SpriteName == "Water"))
                Player = next;
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
