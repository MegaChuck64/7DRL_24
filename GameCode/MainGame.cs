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
        public string Scene = "menu";
        public MainGame () : base(Settings.Width, Settings.Height) 
        {
            menu = new Menu(this);
            map = new Map();
        }

        protected override void LoadContent()
        {
            Settings.Load(Content);
            sb = new SpriteBatch(GraphicsDevice);
            menu.Load(Content);
            map.Generate();
        }

        protected override void Update(GameTime gameTime)
        {

            if (Scene == "menu")
                menu.Update(gameTime);
            

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
                map.Draw(sb);

            sb.End();
        }

    }
}
