using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace GameCode;

public class MapUI
{
    public void Draw(MainGame game, SpriteBatch sb, float dt, Map map)
    {
        var font18 = Settings.Fonts["font_18"];
        var font12 = Settings.Fonts["font_12"];
        var font8 = Settings.Fonts["font_8"];

        var fps = 1f / dt;
        var fpsText = $"FPS: {(int)System.Math.Round(fps)}";
        var fpsSize = font8.MeasureString(fpsText);
        var fpsPos = new Vector2((Settings.MapWindowSize * Settings.TileSize) + 2, Settings.Height - fpsSize.Y - 2);

        sb.DrawString(font8, fpsText, fpsPos, Color.White);


        var health = map.Player.Health;
        var healthText = $"HP:  {health}";
        var healthSize = font12.MeasureString(healthText);
        var healthPos = new Vector2((Settings.MapWindowSize * Settings.TileSize) + 2, Settings.Height - fpsSize.Y - 4 - healthSize.Y - 2);

        sb.DrawString(font12, healthText, healthPos, Color.White);


        var tileInfo = map.SelectedTile?.SpriteName ?? string.Empty;
        var tileInfoPos = new Vector2((Settings.MapWindowSize * Settings.TileSize) + 2, 4);
        sb.DrawString(font12, tileInfo, tileInfoPos, Color.Yellow);

        var inventory = map.Player.Inventory;
        var y = (healthSize.Y + 4 + 2) * 2;
        var invPos = new Vector2((Settings.MapWindowSize * Settings.TileSize) + 2, y);
        sb.DrawString(font8, "~ ~ Inventory ~ ~", invPos, Color.White);
        var cnt = 1;
        foreach (var item in inventory)
        {                        
            var invTx = $"{cnt}. {item.SpriteName}";
            y += healthSize.Y + 4 + 2;
            invPos.Y = y;
            sb.DrawString(font8, invTx, invPos, cnt-1 == map.Player.SelectedItem ? Color.Yellow : Color.White);

            cnt++;
        }

        var uiRect = new Rectangle(
            Settings.MapWindowSize * Settings.TileSize, 
            0, 
            Settings.Width -  (Settings.MapWindowSize * Settings.TileSize),
            Settings.Height);

        var mousePos = game.Input.MouseState.Position;
        if (uiRect.Contains(mousePos))
        {
            var cursorText = Settings.Textures["halo"];
            sb.Draw(cursorText, mousePos.ToVector2(), Color.White);
        }


        if (map.SelectedTile?.SpriteName == "Tree" && 
            map.Player.GetSelectedInventoryItem()?.SpriteName == "Axe" && 
            Vector2.Distance(new Vector2(map.SelectedTile.X, map.SelectedTile.Y), new Vector2(map.Player.X, map.Player.Y)) < 2)
        {
            var actionText = $"- Right Click -";
            var actionSize = font12.MeasureString(actionText);
            var actionPos = new Vector2(healthPos.X, healthPos.Y - (actionSize.Y * 2) - 16);
            sb.DrawString(font12, actionText, actionPos, Color.Green);
            var taskText = "Chop Tree";
            actionPos.Y += actionSize.Y + 8;
            sb.DrawString(font12, taskText, actionPos, Color.Green);
        }

    }
}