using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
    class Asteroid : Base
    {
        private Texture2D texture;

        private Vector2 position;
        private Vector2 velocity;

        public Asteroid(Texture2D texture)
        {
            this.texture  = texture;
            this.position = Vector2.Zero;
            this.velocity = Vector2.Zero;            
        }

        public override void Update(Microsoft.Xna.Framework.GameTime dt)
        {
            position += velocity;

            base.Update(dt);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(texture, position, Color.White);
            spriteBatch.End();

            base.Draw(spriteBatch);
        }

        public Vector2 Position
        {
            get { return position; }
        }

        public int Width
        {
            get { return texture.Width; }
        }

        public int Height
        {
            get { return texture.Height; }
        }
    }
}
