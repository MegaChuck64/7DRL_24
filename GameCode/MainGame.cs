using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameCode
{
    public class MainGame : BaseGame
    {

        private SpriteBatch sb;
        private Menu menu;
        private Map map;
        private MapDrawer mapDrawer;
        private MapUI mapUI;
        public Input Input;
        public string Scene = "menu";
        public MainGame () : base(Settings.Width, Settings.Height) 
        {
            menu = new Menu(this);
            mapUI = new MapUI();
            map = new Map();
            mapDrawer = new MapDrawer();

            IsMouseVisible = false;
        }

        protected override void LoadContent()
        {
            Settings.Load(Content);
            sb = new SpriteBatch(GraphicsDevice);
            map.Generate();
            Input = new Input();
        }

        protected override void Update(GameTime gameTime)
        {

            Input.Update();

            if (Input.IsDown(Keys.LeftControl))
            {
                if (Input.WasPressed(Keys.OemPlus))
                {
                    if (Settings.TileSize < 48)
                    {                        
                        Settings.TileSize += 16;
                        Settings.MapWindowSize = Settings.MapPixelWidth / Settings.TileSize;
                    }
                }
                else if (Input.WasPressed(Keys.OemMinus))
                {
                    if (Settings.TileSize > 16)
                    {
                        Settings.TileSize -= 16;
                        Settings.MapWindowSize = Settings.MapPixelWidth / Settings.TileSize;
                    }
                }
            }


            if (Scene == "menu")
                menu.Update(gameTime);
            else
                map.Player.Update(this, map, gameTime);

        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            sb.Begin(
               sortMode: SpriteSortMode.FrontToBack,
               blendState: BlendState.NonPremultiplied,
               samplerState: SamplerState.PointClamp,
               depthStencilState: DepthStencilState.DepthRead,
               rasterizerState: RasterizerState.CullCounterClockwise,
               effect: null,
               transformMatrix: null);

            if (Scene == "menu")
                menu.Draw(sb);
            else
            {
                var dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
                mapDrawer.Draw(this, map, sb);
                mapUI.Draw(this, sb, dt, map);
            }

            sb.End();
        }

    }
}
/*
 Change how loading works.
Instead of manually making different loaders, make only hardcoded options in json, and have a single loader object 
 */