using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
    class Projectile
    {
        protected Vector2 position   = Vector2.Zero;
        protected Vector2 velocity   = Vector2.Zero;
        protected float   timeToLive = 0;
        protected bool    isActive   = true;

        public virtual void Update(GameTime gameTime)
        {
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
        }
    }
}
