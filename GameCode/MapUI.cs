using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace GameCode;

public class MapUI
{
    public void Draw(SpriteBatch sb, float dt, Map map)
    {
        var font = Settings.Fonts["font_18"];

        var fps = 1f / dt;
        var fpsText = $"FPS: {(int)System.Math.Round(fps)}";
        var fpsSize = font.MeasureString(fpsText);
        var fpsPos = new Vector2((Settings.MapWindowSize * Settings.TileSize) + 2, Settings.Height - fpsSize.Y - 2);

        sb.DrawString(font, fpsText, fpsPos, Color.White);


        var health = map.Player.Health;
        var healthText = $"HP:  {health}";
        var healthSize = font.MeasureString(healthText);
        var healthPos = new Vector2((Settings.MapWindowSize * Settings.TileSize) + 2, Settings.Height - fpsSize.Y - 4 - healthSize.Y - 2);

        sb.DrawString(font, healthText, healthPos, Color.White);
    }
}