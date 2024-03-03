using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Linq;

namespace GameCode;

public class Player : Actor
{
    private Sprite _sprite = Settings.Sprites["Player"];
    private Texture2D _texture;
    private Rectangle _sourceRect;

    public Player()
    {
        _texture = Settings.Textures[_sprite.Path];
        _sourceRect = new Rectangle(_sprite.X, _sprite.Y, Settings.SourceTileSize, Settings.SourceTileSize);
        X = Settings.MapSize / 2;
        Y = Settings.MapSize / 2;
    }

    public void Draw(Rectangle dst, SpriteBatch sb)
    {
        sb.Draw(
            texture: _texture,
            destinationRectangle: dst,
            sourceRectangle: _sourceRect,
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
        if (game.Input.WasPressed(Keys.A))
        {
            if (next.X > 0)
                next.X--;
        }
        if (game.Input.WasPressed(Keys.W))
        {
            if (next.Y > 0)
                next.Y--;
        }
        if (game.Input.WasPressed(Keys.S))
        {
            if (next.Y < Settings.MapSize - 1)
                next.Y++;
        }

        if (next != new Point(X, Y))
        {
            if (!map.Tiles.Any(t => t.X == next.X && t.Y == next.Y && t.SpriteName == "Water"))
            {
                X = next.X;
                Y = next.Y;
            }
        }

    }
}
