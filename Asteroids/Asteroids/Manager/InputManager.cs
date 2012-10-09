using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Asteroids
{
    class InputManager : Manager
    {
        private static InputManager instance;

        private KeyboardState prevKeyboardState;
        private GamePadState  prevGamePadState;

        private InputManager()
        {
            prevKeyboardState = Keyboard.GetState();
            prevGamePadState  = GamePad.GetState(PlayerIndex.One);
        }

        public override void Update(GameTime dt)
        {
            prevKeyboardState = Keyboard.GetState();
            prevGamePadState  = GamePad.GetState(PlayerIndex.One);
        }

        #region GamePad

        public bool IsButtonPressed(Buttons button)
        {
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);

            return gamePadState.IsButtonDown(button) && prevGamePadState.IsButtonUp(button);
        }

        public bool IsButtonHeld(Buttons button)
        {
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);

            return gamePadState.IsButtonDown(button) && prevGamePadState.IsButtonDown(button);
        }

        #endregion

        #region Keyboard

        public bool IsKeyPressed(Keys key)
        {
            return Keyboard.GetState().IsKeyDown(key) && prevKeyboardState.IsKeyUp(key);
        }

        public bool IsKeyHeld(Keys key)
        {
            return Keyboard.GetState().IsKeyDown(key) && prevKeyboardState.IsKeyDown(key);
        }

        #endregion

        #region singleton

        public static InputManager Instance
        {
            get 
            {
                if (instance == null)
                {
                    instance = new InputManager();
                }
                return instance; 
            }
        }

        #endregion
    }
}
