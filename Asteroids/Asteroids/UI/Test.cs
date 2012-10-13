using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
    class Test : Base
    {
        private Texture2D gamerPicture;
        private Texture2D pixel;
        private Rectangle rect;
        private Color color;

        public Test(Rectangle rect, Color color)
        {
            this.rect  = rect;
            this.color = color;

            pixel = new Texture2D(GameBase.graphics.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            pixel.SetData(new[] { Color.White });
        }

        public override void Update(Microsoft.Xna.Framework.GameTime dt)
        {
            base.Update(dt);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            {
               spriteBatch.Draw(pixel, new Rectangle(rect.X, rect.Y, rect.Width, 1), color);
               spriteBatch.Draw(pixel, new Rectangle(rect.X, rect.Y, 1, rect.Height), color);
               spriteBatch.Draw(pixel, new Rectangle(rect.X, rect.Y+rect.Height, rect.Width, 1), color);
               spriteBatch.Draw(pixel, new Rectangle(rect.X+rect.Width, rect.Y, 1, rect.Height), color);
            }
            spriteBatch.End();

            base.Draw(spriteBatch);
        }
    }
}
