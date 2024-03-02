using Microsoft.Xna.Framework;

namespace Engine
{
    public abstract class BaseGame : Game
    {
        private GraphicsDeviceManager _graphics;
        public int Width { get; private set; }
        public int Height { get; private set; }
        public BaseGame(int width = 800, int height = 600)
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            Width = width;
            Height = height;

            _graphics.PreferredBackBufferWidth = Width;
            _graphics.PreferredBackBufferHeight = Height;
            Window.AllowUserResizing = false;
            _graphics.ApplyChanges();

        }

        protected override void Initialize()
        {
        
            base.Initialize();
        }

        protected abstract override void LoadContent();

        protected abstract override void Update(GameTime gameTime);


        protected abstract override void Draw(GameTime gameTime);

    }
}
