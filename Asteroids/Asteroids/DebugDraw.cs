using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
    class DebugDraw
    {
        private static DebugDraw instance;

        private Texture2D pixel;
        private Texture2D transparent;

        private DebugDraw()
        {
            pixel = new Texture2D(Asteroids.graphics.GraphicsDevice, 1, 1);
            pixel.SetData(new Color[] { Color.White });

            transparent = new Texture2D(Asteroids.graphics.GraphicsDevice, 1, 1);
            transparent.SetData(new Color[] { Color.Transparent });
        }

        public void drawBoundingRect(SpriteBatch spriteBatch, Rectangle rect, float angle, Color color)
        {
            spriteBatch.Begin();
            //spriteBatch.Draw(pixel, rect, color);
            //spriteBatch.Draw(transparent, new Rectangle(rect.X+1, rect.Y+1, rect.X+rect.Width-1, rect.Y+rect.Height-1), Color.Red);
            spriteBatch.End();

        }

        public static DebugDraw Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DebugDraw();
                }
                return instance;
            }
        }
    }
}
