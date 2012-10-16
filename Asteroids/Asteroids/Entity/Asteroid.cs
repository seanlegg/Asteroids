using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
    public enum AsteroidType
    {
        SMALL,
        MEDIUM,
        LARGE
    };

    class Asteroid : Collidable
    {

        private AsteroidType type;

        private Texture2D texture;

        private Vector2 origin;
        private Vector2 position;
        private Vector2 velocity;

        private float rotation = 0f;
        private float rotationSpeed;
        private float speed = 1.5f;

        public Asteroid(AsteroidType type, Texture2D texture, Vector2 position, Vector2 velocity, float rotation, float rotationSpeed)
        {
            Random rand = new Random();

            this.isActive = true;

            this.type     = type;
            this.texture  = texture;
            this.position = position;
            this.velocity = velocity;
            this.origin   = new Vector2(texture.Width / 2, texture.Height / 2);
            this.rotation = rotation;
            this.rotationSpeed = rotationSpeed;

            //rotation      = rand.Next(0, 359);
            //rotationSpeed = ((float) Math.Sin(rand.Next(0, 1000))) * 0.05f;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime dt)
        {
            if (isActive == false) return;

            position += velocity * speed;

            rotation += rotationSpeed;

            // Wrap the screen
            position = Helper.wrapUniverse(position, texture.Width, texture.Height);

            base.Update(dt);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            if (isActive == false) return;

            spriteBatch.Begin();
            spriteBatch.Draw(texture, position, null, Color.White, rotation, origin, 1.0f, SpriteEffects.None, 0.0f);
            spriteBatch.End();

            base.Draw(spriteBatch);
        }

        public override void HandleCollision(Player p)
        {

        }

        public override void HandleCollision(Bullet b)
        {
            Console.WriteLine("Asteroid was hit by a bullet.");

            isActive = false;
        }

        /**
         * Collision Detection Overrides
         */
        public override Vector3 GetPosition()
        {
            float xOffset = texture.Width  / 2;
            float yOffset = texture.Height / 2;

            return new Vector3(position.X, position.Y, 0.0f);
        }

        public override int GetRadius()
        {
            return (texture.Width > texture.Height ? texture.Width : texture.Height) / 2;
        }

        public AsteroidType Type
        {
            get { return type; }
        }

        public Vector2 Position
        {
            get { return position; }
        }

        public Vector2 Velocity
        {
            get { return velocity; }
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
