using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
    class Base
    {
        public virtual void Update (GameTime dt)             { }
        public virtual void Draw   (SpriteBatch spriteBatch) { }

        public virtual void HandleCollision(Asteroid a) { }
        public virtual void HandleCollision(Player p)   { }
        public virtual void HandleCollision(Bullet b)   { }
    }
}
