using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
    /*
     * http://www.xnadevelopment.com/tutorials/thestateofthings/thestateofthings.shtml
     */
    class GameScreen
    {
        protected EventHandler screenEvent;

        public GameScreen(EventHandler screenEvent)
        {
            this.screenEvent = screenEvent;
        }

        public virtual void Update (GameTime dt)             { }
        public virtual void Draw   (SpriteBatch spriteBatch) { }
    }
}
