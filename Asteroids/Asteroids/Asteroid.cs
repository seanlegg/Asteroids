using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
    class Asteroid : Base
    {
        public static float MAX_SPEED = 2.0f;

        private Texture2D texture;

        private Vector2 position;
        private Vector2 velocity;
        private float speed = 1.5f;

        public Asteroid(Texture2D texture, Vector2 position, Vector2 velocity)
        {
            this.texture  = texture;
            this.position = position;
            this.velocity = velocity;            
        }

        public override void Update(Microsoft.Xna.Framework.GameTime dt)
        {
            position += velocity * speed;

            // Wrap the screen
            position = Helper.wrapUniverse(position, texture.Width, texture.Height);

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
