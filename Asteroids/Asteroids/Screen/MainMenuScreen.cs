using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Asteroids
{
    class MainMenuScreen : GameScreen
    {
        private KeyboardState previousKeyboardState;

        public MainMenuScreen(ContentManager content, EventHandler screenEvent) : base(screenEvent)
        {

        }

        public override void Update(GameTime dt)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Enter) == true && previousKeyboardState.IsKeyDown(Keys.Enter) == false)
            {
                screenEvent.Invoke(this, null);
            }

            // Keep track of the previous keyboard state
            previousKeyboardState = Keyboard.GetState();

            base.Update(dt);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

    }
}
