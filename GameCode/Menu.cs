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
    public void Load(ContentManager content)
    {
    }

    public void Update(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Space))
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
    }
}