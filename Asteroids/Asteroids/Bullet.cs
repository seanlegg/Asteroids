using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
    class Bullet : Projectile
    {
        private Texture2D bullet_texture;

        public Bullet(Texture2D bullet_texture, Player owner)
        {
            isActive   = true;
            timeToLive = 1.5f;
            speed      = 8.0f;

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
            spriteBatch.Draw(bullet_texture, position, Color.White);
            spriteBatch.End();

            base.Draw(spriteBatch);
        }

        public Player Owner
        {
            get { return this.owner; }
        }
    }
}
