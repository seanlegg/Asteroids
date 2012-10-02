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
            pixel = new Texture2D(AsteroidsGame.graphics.GraphicsDevice, 1, 1);
            pixel.SetData(new Color[] { Color.White });

            transparent = new Texture2D(AsteroidsGame.graphics.GraphicsDevice, 1, 1);
            transparent.SetData(new Color[] { Color.Transparent });
        }

        public static Texture2D CreateDebugTexture(Texture2D texture, Color c)
        {
            int w = texture.Width;
            int h = texture.Height;

            Texture2D result = new Texture2D(AsteroidsGame.graphics.GraphicsDevice, w, h);
            Color[] tex = new Color[w * h];

            for (int i = 0; i < texture.Width; i++)
            {
                tex[i] = c;
                tex[(w * (h - 1)) + i] = c;
            }

            for (int j = 0; j < texture.Height; j++)
            {
                tex[(j * w)] = c;
                tex[(j * w) + (w - 1)] = c;
            }
            result.SetData(tex);

            return result;
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
