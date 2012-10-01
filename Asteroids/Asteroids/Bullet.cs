using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
    class Bullet : Projectile
    {
        Texture2D bulletTexture;

        public Bullet(ContentManager content, Vector2 position, float rotation)
        {
            isActive      = true;
            timeToLive    = 1.5f;
            speed = 8;
            bulletTexture = content.Load<Texture2D>("sprite/bullet");

            this.position = position;
            this.velocity = new Vector2(
                 (float)Math.Sin(rotation) * speed,
                -(float)Math.Cos(rotation) * speed   
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

            position = Game.wrapUniverse(position, bulletTexture.Width, bulletTexture.Height);

            base.Update(gameTime);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            if (isActive == false) return;

            spriteBatch.Begin();
            spriteBatch.Draw(bulletTexture, position, Color.White);
            spriteBatch.End();

            base.Draw(spriteBatch);
        }
    }
}
