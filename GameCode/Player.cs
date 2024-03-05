using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameCode;

public class Player : Actor
{
    private readonly Texture2D _texture;
    private List<string> inventory;
    public Player()
    {
        var rand = new Random();
        var option = rand.Next(Settings.Sprites["Human"].Count);
        var sprite = Settings.Sprites["Human"][option];
        _texture = Settings.Textures[sprite];
        X = Settings.MapSize / 2;
        Y = Settings.MapSize / 2;

        inventory = new List<string>();
        Health = 20;
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
        //if (game.Input.WasPressed(Keys.D))
        //{
        //    if (next.X < Settings.MapSize - 1)
        //        next.X++;
        //}
        //else if (game.Input.WasPressed(Keys.A))
        //{
        //    if (next.X > 0)
        //        next.X--;
        //}
        //else if (game.Input.WasPressed(Keys.W))
        //{
        //    if (next.Y > 0)
        //        next.Y--;
        //}
        //else if (game.Input.WasPressed(Keys.S))
        //{
        //    if (next.Y < Settings.MapSize - 1)
        //        next.Y++;
        //}

        if (map.SelectedTile != null && game.Input.WasPressed(Input.MouseButton.Left) && !map.SelectedTile.Data.Contains("Collider"))
        {
            var path = PathFinder.GetPath(next, new Point(map.SelectedTile.X, map.SelectedTile.Y), Map.GetCollisionMap(map));
            if (path != null && path.Count > 0)
            {
                next = path.First();
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
                    if (collectables.Any())
                    {
                        inventory.AddRange(collectables.Select(t => t.SpriteName));
                        map.Items.RemoveAll(r => collectables.Contains(r));
                    }
                }
                

                 
            }
        }

    }

}
