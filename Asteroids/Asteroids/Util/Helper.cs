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
                position.X = AsteroidsGame.config.ScreenWidth;
            }
            else if (position.X > AsteroidsGame.config.ScreenWidth)
            {
                position.X = -textureWidth;
            }

            if (position.Y + textureHeight < 0)
            {
                position.Y = AsteroidsGame.config.ScreenHeight;
            }
            else if (position.Y > AsteroidsGame.config.ScreenHeight)
            {
                position.Y = -textureHeight;
            }
            return position;
        }

    }
}
