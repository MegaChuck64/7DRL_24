
using Microsoft.Xna.Framework.Input;

namespace Engine
{
    public class Input
    {
        public KeyboardState KeyState { get; private set; }
        public KeyboardState LastKeyState { get; private set; }

        public MouseState MouseState { get; private set; }
        public MouseState LastMouseState { get; private set; }


        public void Update()
        {
            LastKeyState = KeyState;
            LastMouseState = MouseState;

            KeyState = Keyboard.GetState();
            MouseState = Mouse.GetState();
        }

        public bool IsDown(Keys key) => KeyState.IsKeyDown(key);
        public bool IsDown(MouseButton mb) => mb switch
        {
            MouseButton.Left => MouseState.LeftButton == ButtonState.Pressed,
            MouseButton.Middle => MouseState.MiddleButton == ButtonState.Pressed,
            MouseButton.Right => MouseState.RightButton == ButtonState.Pressed,
            _ => false
        };
        
        public bool IsUp(Keys key) => KeyState.IsKeyUp(key);

        public bool IsUp(MouseButton mb) => mb switch
        {
            MouseButton.Left => MouseState.LeftButton == ButtonState.Released,
            MouseButton.Middle => MouseState.MiddleButton == ButtonState.Released,
            MouseButton.Right => MouseState.RightButton == ButtonState.Released,
            _ => false
        };

        public bool WasPressed(Keys key) => KeyState.IsKeyDown(key) && LastKeyState.IsKeyUp(key);
        public bool WasPressed(MouseButton mb) => mb switch
        {
            MouseButton.Left => MouseState.LeftButton == ButtonState.Pressed && LastMouseState.LeftButton == ButtonState.Released,
            MouseButton.Middle => MouseState.MiddleButton == ButtonState.Pressed && LastMouseState.MiddleButton == ButtonState.Released,
            MouseButton.Right => MouseState.RightButton == ButtonState.Pressed && LastMouseState.RightButton == ButtonState.Released,
            _ => false,
        };

        public bool WasReleased(Keys key) => KeyState.IsKeyUp(key) && LastKeyState.IsKeyDown(key);
        public bool WasReleased(MouseButton mb) => mb switch
        {
            MouseButton.Left => MouseState.LeftButton == ButtonState.Released && LastMouseState.LeftButton == ButtonState.Pressed,
            MouseButton.Middle => MouseState.MiddleButton == ButtonState.Released && LastMouseState.MiddleButton == ButtonState.Pressed,
            MouseButton.Right => MouseState.RightButton == ButtonState.Released && LastMouseState.RightButton == ButtonState.Pressed,
            _ => false,
        };


        public enum MouseButton
        {
            Left,
            Middle,
            Right
        }
    }
}