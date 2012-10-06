using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
    class Collidable : Base
    {
        protected float boundingSphereRadius = 0.0f;

        public virtual Vector3   GetPosition()  { return Vector3.Zero; }
        public virtual int       GetRadius()    { return 0; }

        public virtual void HandleCollision(Asteroid a) { }
        public virtual void HandleCollision(Player p)   { }
        public virtual void HandleCollision(Bullet b)   { }
    }
}
