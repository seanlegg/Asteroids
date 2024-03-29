﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
    class Bullet : Projectile
    {
        private Texture2D bullet_texture;

        private int id;

        public static float constant_ttl   = 1.5f;
        public static float constant_speed = 8.0f;

        public Bullet(Texture2D bullet_texture, Player owner, int index)
        {
            isActive   = false;

            // Constants
            speed      = constant_speed;
            timeToLive = constant_ttl;

            // Generate Id
            id = index;

            // Texture
            this.bullet_texture = bullet_texture;

            this.owner    = owner;
            this.position = owner.Position;
            this.velocity = new Vector2(
                 (float)Math.Sin(owner.Rotation) * speed,
                -(float)Math.Cos(owner.Rotation) * speed   
            );
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (isActive == false) return;            

            timeToLive -= (float) gameTime.ElapsedGameTime.TotalSeconds;
            if (timeToLive <= 0)
            {
                isActive = false;
            }
            position += velocity;
            position = Helper.wrapUniverse(position, bullet_texture.Width, bullet_texture.Height);

            base.Update(gameTime);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            if (isActive == false) return;

            spriteBatch.Begin();
            spriteBatch.Draw(bullet_texture, position, Color.Yellow);
            spriteBatch.End();

            base.Draw(spriteBatch);
        }

        public override void HandleCollision(Asteroid a)
        {
            if (isActive)
            {
                // Increment the players score
                if (owner != null)
                {
                    switch (a.Type)
                    {
                        case AsteroidType.SMALL:
                            owner.Score += 100;
                            break;
                        case AsteroidType.MEDIUM:
                            owner.Score += 75;
                            break;
                        case AsteroidType.LARGE:
                            owner.Score += 50;                            
                            break;
                    }
                }
            }
            isActive = false;
        }

        /**
         * Collision Detection Overrides
         */
        public override Vector3 GetPosition()
        {
            float xOffset = bullet_texture.Width  / 2;
            float yOffset = bullet_texture.Height / 2;

            return new Vector3(position.X + xOffset, position.Y + yOffset, 0.0f);
        }

        public override int GetRadius()
        {
            return (bullet_texture.Width > bullet_texture.Height ? bullet_texture.Width : bullet_texture.Height);
        }

        public Player Owner
        {
            get { return this.owner; }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        public int Id
        {
            get { return 0; }
            set { id = value; }
        }

        public float Speed
        {
            get { return speed; }
        }

        public double TimeToLive
        {
            get { return timeToLive; }
            set { timeToLive = value; }
        }
    }
}
