using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Asteroids
{
    class SplashScreen : Screen
    {
        public SplashScreen(ContentManager cManager, EventHandler screenEvent) : base(screenEvent)
        {

        }

        public override void Update(GameTime dt)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                screenEvent.Invoke(this, new EventArgs());
            }
            base.Update(dt);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

    }
}
