﻿using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameCode;

/*
 * todo:
 * refactor crafting system to generate from settings file
 * add enemies
 * add spells/range
 *
 */
public class Player : Actor
{
    private readonly Texture2D _texture;
    public List<Tile> Inventory { get; private set; }

    public int SelectedItem { get; private set; }


    public Player(int x, int y)
    {
        var rand = new Random();
        var option = rand.Next(Settings.Sprites["Human"].Count);
        var sprite = Settings.Sprites["Human"][option];
        _texture = Settings.Textures[sprite];
        X = x;// Settings.MapSize / 2;
        Y = y;// Settings.MapSize / 2;

        Inventory = new List<Tile>();        
        Health = 20;
    }

    public bool TryAddInventoryItem(Tile item)
    {
        if (Inventory.Count < 10)
        {
            Inventory.Add(item);
            return true;
        }

        return false;
    }

    public Tile GetSelectedInventoryItem()
    {
        if (Inventory.Count > SelectedItem)
        {
            return Inventory[SelectedItem];
        }

        return null;
    }

    public List<Tile> GetCraftableItems()
    {
        var items = new List<Tile>();

        if (CanCraftAxe())
        {
            items.Add(new Tile
            {
                SpriteName = "Axe",
                Option = 0,
                Data = new List<string>()
                {
                    "Collectable",
                    "Object",
                    "Weapon"
                },
            });
        }

        if (CanCraftSword())
        {
            items.Add(new Tile
            {
                SpriteName = "Sword",
                Option = 0,
                Data = new List<string>()
                {
                    "Collectable",
                    "Object",
                    "Weapon"
                },
            });
        }

        return items;
    }

    public bool CanCraftAxe() => 
        Inventory.Count(t => t.SpriteName == "Logs") >= 2;

    public bool CanCraftSword() =>
        Inventory.Count(t => t.SpriteName == "Logs") >= 3;
    
    
    public void Draw(Rectangle dst, SpriteBatch sb)
    {
        sb.Draw(
            texture: _texture,
            destinationRectangle: dst,
            sourceRectangle: null,
            color: Color.White,
            rotation: 0f,
            origin: Vector2.Zero,
            effects: SpriteEffects.None,
            layerDepth: 0.2f);
    }


    public void Update(MainGame game, Map map, GameTime gt)
    {

        var next = new Point(X, Y);
        if (map.SelectedTile != null && game.Input.WasPressed(Input.MouseButton.Left) && !map.SelectedTile.Data.Contains("Collider"))
        {
            var path = PathFinder.GetPath(next, new Point(map.SelectedTile.X, map.SelectedTile.Y), Map.GetCollisionMap(map));
            if (path != null && path.Count > 0)
            {
                next = path.First();
            }
        }
        else if (map.SelectedTile != null && game.Input.WasPressed(Input.MouseButton.Right))
        {
            if (map.SelectedTile.SpriteName == "Tree" && GetSelectedInventoryItem()?.SpriteName == "Axe")
            {
                map.Items.Remove(map.SelectedTile);
                map.Items.Add(new Tile
                {
                    X = map.SelectedTile.X,
                    Y = map.SelectedTile.Y,
                    Data = new List<string>
                    {
                        "Collectable",
                        "Object",
                        "Description-Wood. Used to build stuff."
                    },
                    Option = 0,
                    SpriteName = "Logs"                    
                });

                map.SelectedTile = null;
            }
        }

        if (next != new Point(X, Y))
        {
            if (next.X < Settings.MapSize && next.Y < Settings.MapSize && next.X >= 0 && next.Y >= 0)
            {
                if (!map.Tiles[X, Y].Data.Contains("Collider") &&
                    !map.Items.Any(t => t.X == X && t.Y == Y && t.Data.Contains("Collider")))
                {
                    X = next.X;
                    Y = next.Y;

                    var collectables = map.Items.Where(t => t.X == X && t.Y == Y && t.Data.Contains("Collectable"));
                    var removables = new List<Tile>();
                    if (collectables.Any())
                    {
                        foreach (var item in collectables)
                        {
                            if (TryAddInventoryItem(item))
                            {
                                removables.Add(item);
                            }
                        }

                        map.Items.RemoveAll(c=>removables.Contains(c));
                    }
                }
                

                 
            }
        }

        var pressedKeys = game.Input.KeyState.GetPressedKeys();
        foreach (var key in pressedKeys.Where(k => (int)k >= 48 && (int)k <= 57))
        {
            var val = (int)key - 48;
            if (val == 0)
                val = 10;

            val--;
            //a b c
            //  1

            if (map.Player.Inventory.Count > val)
            {
                SelectedItem = val;
            }
        }

    }

}
