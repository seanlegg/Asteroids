using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Asteroids
{
    class Helper
    {

        public static Vector2 wrapUniverse(Vector2 position, int textureWidth, int textureHeight)
        {
            // TODO: Remove magic numbers
            if (position.X + textureWidth < 0)
            {
                position.X = 1280;
            }
            else if (position.X > 1280)
            {
                position.X = -textureWidth;
            }

            if (position.Y + textureHeight < 0)
            {
                position.Y = 720;
            }
            else if (position.Y > 720)
            {
                position.Y = -textureHeight;
            }
            return position;
        }

        public static void DrawLine(SpriteBatch batch, Texture2D blank, float width, Color color, Vector2 point1, Vector2 point2)
        {
            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            float length = Vector2.Distance(point1, point2);

            batch.Draw(blank, point1, null, color,
                       angle, Vector2.Zero, new Vector2(length, width),
                       SpriteEffects.None, 0);
        }
    }
}
