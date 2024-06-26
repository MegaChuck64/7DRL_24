﻿using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;
namespace GameCode;

public class CraftMenu
{
    private Rectangle GetLeftPaneRect =>
    new(0, 0, Settings.Width / 2, Settings.Height);
    private Rectangle GetRightPaneRect =>
        new(Settings.Width / 2, 0, Settings.Width / 2, Settings.Height);

    private int selectedItem = -1;


    public void Update(Input input, Map map)
    {
        var pressedKeys = input.KeyState.GetPressedKeys();
        foreach (var key in pressedKeys.Where(k => (int)k >= 48 && (int)k <= 57))
        {
            var val = (int)key - 48;
            if (val == 0)
                val = 10;

            val--;
            //a b c
            //  1

            if (map.Player.GetCraftableItems().Count > val)
            {
                selectedItem = val;
            }
        }

        if (selectedItem >= 0 && input.WasPressed(Keys.Enter))
        {
            var craftables = map.Player.GetCraftableItems();
            if (craftables.Count > selectedItem)
            {
                var item = craftables[selectedItem];
                if (map.Player.TryCraftItem(item))
                {
                    selectedItem = -1;
                }
            }
            else if (craftables.Count == 0)
            {
                selectedItem = -1;
            }
            else
            {
                selectedItem = 0;
            }
        }
    }

    public void Draw(SpriteBatch sb, Map map, Input input)
    {
        var font12 = Settings.Fonts["font_12"];
        var font8 = Settings.Fonts["font_8"];

        var escText = "esc";
        var escSize = font8.MeasureString(escText);
        var escPos = new Vector2(8, Settings.Height - escSize.Y - 4);
        sb.DrawString(font8, escText, escPos, Color.LightGray);

        var titleText = "~ ~ Craftable ~ ~";
        var titleSize = font12.MeasureString(titleText);
        var x = GetLeftPaneRect.Center.X - (titleSize.X / 2);
        var y = 48;

        sb.DrawString(font12, titleText, new Vector2(x, y), Color.White);

        var craftableItems = map.Player.GetCraftableItems();
        var i = 1;
        x = 48;
        foreach (var item in craftableItems.Take(10))
        {
            var selected = selectedItem == i - 1;
            if (i == 10)
                i = 0;

            var txt = $"{i}. {item}";
            var txtSize = font8.MeasureString(txt);
            y += (int)titleSize.Y + 8;
            sb.DrawString(font8, txt, new Vector2(x, y), selected ? Color.Yellow : Color.White);
            i++;
        }

        //------------------------ right panel
        if (selectedItem >= 0)
        {
            var item = craftableItems[selectedItem];
            var nameText = $"- {item} -";
            var nameSize = font12.MeasureString(nameText);
            x = GetRightPaneRect.Center.X - (nameSize.X / 2);
            y = 48;

            sb.DrawString(font12, nameText, new Vector2(x, y), Color.White);

            var itemInfo = Settings.Items[item];            
            var lines = itemInfo.Description.Split('\n', 
                StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                y += (int)nameSize.Y + 8;
                x = GetRightPaneRect.X + 48;
                sb.DrawString(font8, line, new Vector2(x, y), Color.LightGray);
            }
            
        }



        var mousePos = input.MouseState.Position;
        var mouseTxt = Settings.Textures["halo"];
        sb.Draw(mouseTxt, mousePos.ToVector2(), Color.White);
    }
}
