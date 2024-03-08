using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameCode;

public class MapDrawer
{
    private Rectangle _tempRect = new();
    public Point Offset { get; set; }

    public void Draw(MainGame game, Map map, SpriteBatch sb)
    {
        Offset = new Point(-map.Player.X + (Settings.MapWindowSize / 2), -map.Player.Y + (Settings.MapWindowSize / 2));
        var mousePos = game.Input.MouseState.Position;
        var mouseTilePos = new Point(mousePos.X / Settings.TileSize, mousePos.Y / Settings.TileSize);

        var wtrSpr = Settings.Sprites["Water"];
        var wtrTxt = Settings.Textures[wtrSpr[0]];

        //make off map just water
        for (int x = 0; x < Settings.MapWindowSize; x++)
        {
            for (int y = 0; y < Settings.MapWindowSize; y++)
            {
                var dst = _tempRect;

                dst.X = x * Settings.TileSize;
                dst.Y = y * Settings.TileSize;
                dst.Width = Settings.TileSize;
                dst.Height = Settings.TileSize;

                sb.Draw(
                  texture: wtrTxt,
                  destinationRectangle: dst,
                  sourceRectangle: null,
                  color: Color.White,
                  rotation: 0f,
                  origin: Vector2.Zero,
                  effects: SpriteEffects.None,
                  layerDepth: 0.0f);

            }
        }


        var winRect = _tempRect;
        winRect.X = 0; winRect.Y = 0;
        winRect.Width = Settings.MapWindowSize;
        winRect.Height = Settings.MapWindowSize;

        map.SelectedTile = null;

        foreach (var tile in map.Tiles)
        {
            var dst = _tempRect;

            dst.X = (tile.X + Offset.X) * Settings.TileSize;
            dst.Y = (tile.Y + Offset.Y) * Settings.TileSize;
            dst.Width = Settings.TileSize;
            dst.Height = Settings.TileSize;

            if (winRect.Contains(tile.X + Offset.X, tile.Y + Offset.Y))
            {
                var tileSpr = Settings.Sprites[tile.SpriteName];
                var layer = 0.1f;
                
                //tiles
                if (tileSpr != null)
                {
                    var tileTxt = Settings.Textures[tileSpr[tile.Option]];
                    sb.Draw(
                      texture: tileTxt,
                      destinationRectangle: dst,
                      sourceRectangle: null,
                      color: Color.White,
                      rotation: 0f,
                      origin: Vector2.Zero,
                      effects: SpriteEffects.None,
                      layerDepth: layer);
                }


                //cursor
                if (tile.X + Offset.X == mouseTilePos.X && tile.Y + Offset.Y == mouseTilePos.Y)
                {
                    var cursorTxt = Settings.Textures["cursor"];
                    sb.Draw(
                      texture: cursorTxt,
                      destinationRectangle: dst,
                      sourceRectangle: null,
                      color: Color.White,
                      rotation: 0f,
                      origin: Vector2.Zero,
                      effects: SpriteEffects.None,
                      layerDepth: layer + 0.1f);

                    
                    map.SelectedTile = tile;                    
                }

            }

            if (tile.X == map.Player.X && tile.Y == map.Player.Y)
            {
                map.Player.Draw(dst, sb);
            }

        }

        foreach (var item in map.Items)
        {
            if (winRect.Contains(item.X + Offset.X, item.Y + Offset.Y))
            {
                var dst = _tempRect;

                dst.X = (item.X + Offset.X) * Settings.TileSize;
                dst.Y = (item.Y + Offset.Y) * Settings.TileSize;
                dst.Width = Settings.TileSize;
                dst.Height = Settings.TileSize;

                var itmSpr = Settings.Sprites[item.SpriteName];
                var itmTxt = Settings.Textures[itmSpr[item.Option]];
                var layer = 0.2f;

                sb.Draw(
                  texture: itmTxt,
                  destinationRectangle: dst,
                  sourceRectangle: null,
                  color: Color.White,
                  rotation: 0f,
                  origin: Vector2.Zero,
                  effects: SpriteEffects.None,
                  layerDepth: layer);


                if (map.SelectedTile != null && map.SelectedTile.X == item.X && map.SelectedTile.Y == item.Y)
                    map.SelectedTile = item;
            }
        }

    }
}
