using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;

namespace GameCode;

public class Player : Actor
{
    private readonly Texture2D _texture;

    public Player()
    {
        var rand = new Random();
        var option = rand.Next(Settings.Sprites["Human"].Count);
        var sprite = Settings.Sprites["Human"][option];
        _texture = Settings.Textures[sprite];
        X = Settings.MapSize / 2;
        Y = Settings.MapSize / 2;

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
