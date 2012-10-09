using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
    struct Text
    {
        public String title;
        public Vector2 size;
        public Vector2 center;

        public Text(String title, SpriteFont font)
        {
            this.title = title;
            this.size = font.MeasureString(title);
            this.center = new Vector2((AsteroidsGame.config.ScreenWidth / 2) - (size.X / 2), (AsteroidsGame.config.ScreenHeight / 2) - (size.Y / 2));
        }
    }
}
