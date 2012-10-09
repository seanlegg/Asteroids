using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
    class MenuOption
    {
        public EventType type;
        public String    title;
        public Vector2   size;
        public Vector2   center;

        public MenuOption(String title, EventType type, SpriteFont font)
        {
            this.title  = title;
            this.type   = type;
            this.size   = font.MeasureString(title);
            this.center = new Vector2((AsteroidsGame.config.ScreenWidth / 2) - (size.X / 2), (AsteroidsGame.config.ScreenHeight / 2) - (size.Y / 2));
        }
    }
}
