using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
    class Projectile : Base
    {
        protected Player owner;

        protected Vector2 position = Vector2.Zero;
        protected Vector2 velocity = Vector2.Zero;
        protected float timeToLive;
        protected float speed;

        public bool isActive = true;
    }
}
