using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameCode;

/*
 * todo:
 * add enemies
 * add spells
 *
 */
public class Player : Actor
{
    private readonly Texture2D _texture;

    public Player(int x, int y)
    {
        var rand = new Random();
        var option = rand.Next(Settings.Sprites["Human"].Count);
        var sprite = Settings.Sprites["Human"][option];
        _texture = Settings.Textures[sprite];
        X = x;// Settings.MapSize / 2;
        Y = y;// Settings.MapSize / 2;

        Health = 20;
    }

    public bool TryAddInventoryItem(Item item)
    {
        if (Inventory.Count < 10)
        {
            if (Inventory.ContainsKey(item.SpriteName))
            {
                Inventory[item.SpriteName].Count++;
            }
            else
            {
                Inventory.Add(item.SpriteName, item);
            }
            return true;
        }

        return false;
    }

    public bool TryCraftItem(string itemName)
    {
        if (!CanCraftItem(itemName))
            return false;

        CraftItem(itemName);

        return true;
    }

    private void CraftItem(string itemName)
    {
        var recipe = Settings.Recipes[itemName];
        var removables = new List<string>();
        foreach (var ingredient in recipe)
        {
            Inventory[ingredient.Key].Count -= ingredient.Value;

            if (Inventory[ingredient.Key].Count < 1)
                removables.Add(ingredient.Key);
        }

        foreach (var rem in removables)
        {
            Inventory.Remove(rem);
        }

        TryAddInventoryItem(Settings.CreateItem(itemName, 0));
    }

    public bool CanCraftItem(string itemName)
    {
        if (!Settings.Recipes.ContainsKey(itemName))
            return false;

        var recipe = Settings.Recipes[itemName];
        foreach (var ingredient in recipe)
        {
            if (!Inventory.ContainsKey(ingredient.Key) || Inventory[ingredient.Key].Count < ingredient.Value)
                return false;
        }

        return true;
    }

    public Item GetSelectedInventoryItem()
    {
        if (Inventory.Count > SelectedItem)
        {
            return Inventory.ElementAt(SelectedItem).Value;
        }

        return null;
    }

    public List<string> GetCraftableItems()
    {
        var items = new List<string>();

        foreach (var item in Settings.Recipes)
        {
            if (CanCraftItem(item.Key))
                items.Add(item.Key);
        }

        return items;
    }

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
        if (map.SelectedTile != null && game.Input.WasPressed(Input.MouseButton.Left) && !map.SelectedTile.Collider)
        {
            var path = PathFinder.GetPath(next, new Point(map.SelectedTile.X, map.SelectedTile.Y), Map.GetCollisionMap(map));
            if (path != null && path.Count > 0)
            {
                next = path.First();
            }
        }
        else if (map.SelectedTile != null && game.Input.WasPressed(Input.MouseButton.Right))
        {
            var selectedInventoryItem = GetSelectedInventoryItem();
            if (map.SelectedTile.SpriteName == "Tree" &&
                selectedInventoryItem?.SpriteName == "Axe" &&
                (int)Vector2.Distance(
                    new Vector2(map.SelectedTile.X, map.SelectedTile.Y), 
                    new Vector2(map.Player.X, map.Player.Y)) <=
                    float.Parse(Settings.Items["Axe"].Data["Range"])) 
            {
                map.Items.Remove(map.SelectedTile as Item);
                map.Items.Add(Settings.CreateItem("Logs", 0, map.SelectedTile.X, map.SelectedTile.Y));

                map.SelectedTile = null;
            }
        }

        if (next != new Point(X, Y))
        {
            if (next.X < Settings.MapSize && next.Y < Settings.MapSize && next.X >= 0 && next.Y >= 0)
            {
                if (!map.Tiles[X, Y].Collider &&
                    !map.Items.Any(t => t.X == X && t.Y == Y && t.Collider))
                {
                    X = next.X;
                    Y = next.Y;

                    var collectables = map.Items.Where(t => t.X == X && t.Y == Y && t.Collectable);
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
