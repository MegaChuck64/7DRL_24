using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameCode
{
    public class MainGame : BaseGame
    {

        private SpriteBatch sb;
        private Menu menu;
        private Map map;
        private MapUI mapUI;
        public Input Input;
        public string Scene = "menu";
        public MainGame () : base(Settings.Width, Settings.Height) 
        {
            menu = new Menu(this);
            mapUI = new MapUI();
            map = new Map(this);
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

            if (Scene == "menu")
                menu.Update(gameTime);
            else
                map.Update(gameTime);

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
                map.Draw(sb);

                mapUI.Draw(sb, dt);
            }

            sb.End();
        }

    }
}
