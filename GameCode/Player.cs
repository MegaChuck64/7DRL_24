using GameCode.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;

namespace GameCode;

public class Player : Actor
{
    private Texture2D _texture;

    public Player()
    {
        var rand = new Random();
        var playerLoader = new PlayerLoader();
        var options = playerLoader.GetOptionCount();
        var option = rand.Next(options);
        var spriteInfo = playerLoader.GetInfo(option);
        _texture = Settings.Textures[spriteInfo.TextureName];
        X = Settings.MapSize / 2;
        Y = Settings.MapSize / 2;
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
        if (game.Input.WasPressed(Keys.D))
        {
            if (next.X < Settings.MapSize - 1)
                next.X++;
        }
        else if (game.Input.WasPressed(Keys.A))
        {
            if (next.X > 0)
                next.X--;
        }
        else if (game.Input.WasPressed(Keys.W))
        {
            if (next.Y > 0)
                next.Y--;
        }
        else if (game.Input.WasPressed(Keys.S))
        {
            if (next.Y < Settings.MapSize - 1)
                next.Y++;
        }

        if (next != new Point(X, Y))
        {
            if (!map.Tiles.Any(t => t.X == next.X && t.Y == next.Y && t.Data.ContainsKey("Collider") && t.Data["Collider"] == "True"))
            {
                X = next.X;
                Y = next.Y;
            }
        }

    }
}
