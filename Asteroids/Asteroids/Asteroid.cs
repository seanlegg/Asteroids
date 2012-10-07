using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
    class Asteroid : Collidable
    {
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
            //DebugDraw circle = new DebugDraw(AsteroidsGame.graphics.GraphicsDevice);
            //circle.CreateCircle(GetRadius(), 100);
            //circle.Position = new Vector2(GetPosition().X, GetPosition().Y);
            //circle.Colour = Color.Red;

            spriteBatch.Begin();
            spriteBatch.Draw(texture, position, Color.White);
            //circle.Render(spriteBatch);
            spriteBatch.End();

            base.Draw(spriteBatch);
        }

        /**
         * Collision Detection Overrides
         */
        public override Vector3 GetPosition()
        {
            float xOffset = texture.Width  / 2;
            float yOffset = texture.Height / 2;

            return new Vector3(position.X + xOffset, position.Y + yOffset, 0.0f);
        }

        public override int GetRadius()
        {
            return (texture.Width > texture.Height ? texture.Width : texture.Height) / 2;
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
