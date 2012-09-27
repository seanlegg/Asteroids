using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
    class Bullet : Projectile
    {
        Texture2D bulletTexture;

        public Bullet()
        {
            isActive   = true;
            timeToLive = 5;
            bulletTexture = new Texture2D(Asteroids.graphics.GraphicsDevice, 1, 1);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (isActive == false) return;

            timeToLive -= (float) gameTime.ElapsedGameTime.TotalSeconds;
            if (timeToLive <= 0)
            {
                isActive = false;
            }
            base.Update(gameTime);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            if (isActive == false) return;

            spriteBatch.Begin();
            spriteBatch.Draw(bulletTexture, new Rectangle(10, 10, 8, 8), Color.White);
            spriteBatch.End();

            base.Draw(spriteBatch);
        }
    }
}
