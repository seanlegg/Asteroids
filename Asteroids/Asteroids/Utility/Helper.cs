using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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

    }
}
