using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace GameCode;

public class MapUI
{
    public void Draw(SpriteBatch sb, float dt)
    {
        var fps = 1f / dt;
        var fpsText = $"FPS: {(int)System.Math.Round(fps)}";
        var font = Settings.Fonts["font_18"];
        var fpsSize = font.MeasureString(fpsText);
        var fpsPos = new Vector2((Settings.MapWindowSize * Settings.TileSize) + 2, Settings.Height - fpsSize.Y - 2);

        sb.DrawString(font, fpsText, fpsPos, Color.White);
    }
}