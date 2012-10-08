using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
    class Collidable : Base
    {
        public bool isActive;

        // Bounding Sphere
        public virtual Vector3 GetPosition() { return Vector3.Zero; }
        public virtual int     GetRadius()   { return 0; }

        // Collisions
        public virtual void HandleCollision(Asteroid a) { }
        public virtual void HandleCollision(Player p)   { }
        public virtual void HandleCollision(Bullet b)   { }
    }
}
