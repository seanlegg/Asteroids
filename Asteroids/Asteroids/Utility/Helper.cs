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
                position.X = GameBase.config.ScreenWidth;
            }
            else if (position.X > GameBase.config.ScreenWidth)
            {
                position.X = -textureWidth;
            }

            if (position.Y + textureHeight < 0)
            {
                position.Y = GameBase.config.ScreenHeight;
            }
            else if (position.Y > GameBase.config.ScreenHeight)
            {
                position.Y = -textureHeight;
            }
            return position;
        }

    }
}
