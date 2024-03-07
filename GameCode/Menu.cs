using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameCode;

public class Menu
{
    private MainGame _game;
    public Menu(MainGame game)
    {
        _game = game;
    }

    public void Update(GameTime gameTime)
    {
        if (_game.Input.WasPressed(Keys.Space))
        {
            _game.Scene = "map";
        }
    }

    public void Draw(SpriteBatch sb)
    {
        var font = Settings.Fonts["font_18"];
        var title = "Hollow World";
        var titleSize = font.MeasureString(title);
        var titleX = Settings.Width / 2;
        var titleY = Settings.Height / 4;

        titleX -= (int)(titleSize.X / 2f);
        titleY -= (int)(titleSize.Y / 2f);
        sb.DrawString(font, title, new Vector2(titleX, titleY), Color.White);


        var mousePos = _game.Input.MouseState.Position;
        var txt = Settings.Textures["halo"];
        sb.Draw(txt, mousePos.ToVector2(), Color.White);
    }
}