using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameCode;

public class MapDrawer
{
    private Rectangle _tempRect = new();
    public Point Offset { get; set; }
    public void Draw(Map map, SpriteBatch sb)
    {
        Offset = new Point(-map.Player.X + (Settings.MapWindowSize / 2), -map.Player.Y + (Settings.MapWindowSize / 2));

        var plSpr = Settings.Sprites["Player"];
        var plTxt = Settings.Textures[plSpr.Path];
        
        var winRect = _tempRect;
        winRect.X = 0; winRect.Y = 0;
        winRect.Width = Settings.MapWindowSize;
        winRect.Height = Settings.MapWindowSize;

        var wtrSpr = Settings.Sprites["Water"];
        var wtrTxt = Settings.Textures[wtrSpr.Path];
        var wtrSrc = _tempRect;
        wtrSrc.X = wtrSpr.X;
        wtrSrc.Y = wtrSpr.Y;
        wtrSrc.Width = Settings.SourceTileSize;
        wtrSrc.Height = Settings.SourceTileSize;

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
                  sourceRectangle: wtrSrc,
                  color: Color.White,
                  rotation: 0f,
                  origin: Vector2.Zero,
                  effects: SpriteEffects.None,
                  layerDepth: 0.0f);

            }
        }

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
                var tileTxt = Settings.Textures[tileSpr.Path];

                var src = _tempRect;

                src.X = tileSpr.X;
                src.Y = tileSpr.Y;
                src.Width = Settings.SourceTileSize;
                src.Height = Settings.SourceTileSize;

                sb.Draw(
                  texture: tileTxt,
                  destinationRectangle: dst,
                  sourceRectangle: src,
                  color: Color.White,
                  rotation: 0f,
                  origin: Vector2.Zero,
                  effects: SpriteEffects.None,
                  layerDepth: 0.1f);
            }

            if (tile.X == map.Player.X && tile.Y == map.Player.Y)
            {
                //var src = _tempRect;
                //src.X = plSpr.X;
                //src.Y = plSpr.Y;
                //src.Width = Settings.SourceTileSize;
                //src.Height = Settings.SourceTileSize;
                //sb.Draw(
                //    texture: plTxt,
                //    destinationRectangle: dst,
                //    sourceRectangle: src,
                //    color: Color.White,
                //    rotation: 0f,
                //    origin: Vector2.Zero,
                //    effects: SpriteEffects.None,
                //    layerDepth: 0.2f);
                map.Player.Draw(dst, sb);
            }

        }

    }

}
