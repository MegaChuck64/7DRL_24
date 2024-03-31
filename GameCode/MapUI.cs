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

        if (map.SelectedTile != null)
        {
            var tileInfoPos = new Vector2((Settings.MapWindowSize * Settings.TileSize) + 2, 4);
            var displayName = map.SelectedTile.SpriteName;
            if (map.SelectedTile is Item && !string.IsNullOrEmpty(Settings.Items[map.SelectedTile.SpriteName].DisplayName))
            {
                displayName = Settings.Items[map.SelectedTile.SpriteName].DisplayName;
            }

            sb.DrawString(font12, displayName, tileInfoPos, Color.Yellow);

        }

        var inventory = map.Player.Inventory;
        var y = (healthSize.Y + 4 + 2) * 2;
        var invPos = new Vector2((Settings.MapWindowSize * Settings.TileSize) + 2, y);
        sb.DrawString(font8, "~ ~ Inventory (i)~ ~", invPos, Color.White);
        var cnt = 1;
        foreach (var item in inventory)
        {                        
            var invTx = $"{cnt}. x{item.Value.Count} - {item.Key}";
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


        var selectedInventoryItem = map.Player.GetSelectedInventoryItem();
        if (selectedInventoryItem != null && map.SelectedTile != null)
        {
            if (selectedInventoryItem.SpriteName == "Axe" && map.SelectedTile.SpriteName == "Tree")
            {
                if ((int)Vector2.Distance(
                    new Vector2(map.SelectedTile.X, map.SelectedTile.Y),
                    new Vector2(map.Player.X, map.Player.Y)) <=
                    float.Parse(Settings.Items["Axe"].Data["Range"]))
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

        if (map.Player.GetCraftableItems().Count > 0)
        {
            var craftText = "- Craft (c) -";
            var craftSize = font12.MeasureString(craftText);
            var craftPos = new Vector2(healthPos.X, healthPos.Y - (((craftSize.Y * 2) - 16 ) * 8));
            sb.DrawString(font12, craftText, craftPos, Color.Green);
        }

    }
}