using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
    class Projectile
    {
        protected Player owner;

        protected Vector2 position = Vector2.Zero;
        protected Vector2 velocity = Vector2.Zero;
        protected float timeToLive;
        protected float speed;

        public bool isActive = true;

        public virtual void Update (GameTime gameTime)       { }
        public virtual void Draw   (SpriteBatch spriteBatch) { }
    }
}
