using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
    class Helper
    {

        public static Vector2 wrapUniverse(Vector2 position, int textureWidth, int textureHeight)
        {
            if (position.X + textureWidth < 0)
            {
                position.X = Asteroids.gameConfig.ScreenWidth;
            }
            else if (position.X > Asteroids.gameConfig.ScreenWidth)
            {
                position.X = -textureWidth;
            }

            if (position.Y + textureHeight < 0)
            {
                position.Y = Asteroids.gameConfig.ScreenHeight;
            }
            else if (position.Y > Asteroids.gameConfig.ScreenHeight)
            {
                position.Y = -textureHeight;
            }
            return position;
        }

    }
}
