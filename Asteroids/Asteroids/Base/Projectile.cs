﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
    class Projectile : Collidable
    {
        protected Player owner;

        protected Vector2 position = Vector2.Zero;
        protected Vector2 velocity = Vector2.Zero;

        protected double timeToLive;
        protected float speed;
    }
}
