using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Asteroids
{
    class SplashScreen : Screen
    {
        private Texture2D titleBackground;
        private KeyboardState previousKeyboardState;

        public SplashScreen(ContentManager content, EventHandler screenEvent) : base(screenEvent)
        {
            titleBackground = content.Load<Texture2D>("background/title");
        }

        public override void Update(GameTime dt)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Enter) == true && previousKeyboardState.IsKeyDown(Keys.Enter) == false)
            {
                screenEvent.Invoke(this, new ScreenEvent());
            }

            // Keep track of the previous keyboard state
            previousKeyboardState = Keyboard.GetState();

            base.Update(dt);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(titleBackground, new Rectangle(0, 0, 800, 480), Color.White);
            spriteBatch.End();

            base.Draw(spriteBatch);
        }

    }
}
