using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
    class Bullet : Projectile
    {
        private Texture2D bullet_texture;

#if DEBUG
        private Texture2D bullet_texture_debug;

        public Bullet(Texture2D bullet_texture, Texture2D debug_texture, Player owner)
#else
        public Bullet(Texture2D bullet_texture, Player owner)
#endif
        {
            isActive   = true;
            timeToLive = 1.5f;
            speed      = 8.0f;

            this.bullet_texture = bullet_texture;
#if DEBUG
            this.bullet_texture_debug = debug_texture;
#endif

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
#if DEBUG
            spriteBatch.Draw(bullet_texture_debug, position, Color.Red);
#endif
            spriteBatch.End();

            base.Draw(spriteBatch);
        }
    }
}
