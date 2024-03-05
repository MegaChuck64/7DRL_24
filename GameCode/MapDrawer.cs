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
        //mouseTilePos.X += Offset.X;
        //mouseTilePos.Y += Offset.Y;

        var wtrSpr = Settings.Sprites["Water"];
        var wtrTxt = Settings.Textures[wtrSpr[0]];

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
                if (tile?.Data.TryGetValue("Layer", out string layerVal) ?? false)
                {
                    layer = layerVal == "Object" ? 0.2f : 0.1f;
                }
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

                    if (map.SelectedTile == null || (tile.Data.ContainsKey("Layer") && tile.Data["Layer"] == "Object"))
                    {
                        map.SelectedTile = tile;
                    }
                }

            }

            if (tile.X == map.Player.X && tile.Y == map.Player.Y)
            {
                map.Player.Draw(dst, sb);
            }

        }

    }
}
