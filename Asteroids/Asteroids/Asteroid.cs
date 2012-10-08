﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
    class Asteroid : Collidable
    {
        private Texture2D texture;

        private Vector2 origin;
        private Vector2 position;
        private Vector2 velocity;

        private float rotation = 0f;
        private float rotationSpeed;
        private float speed = 1.5f;

        public Asteroid(Texture2D texture, Vector2 position, Vector2 velocity)
        {
            Random rand = new Random();

            this.isActive = true;

            this.texture  = texture;
            this.position = position;
            this.velocity = velocity;
            this.origin   = new Vector2(texture.Width / 2, texture.Height / 2);

            rotation = rand.Next(0, 359);
            rotationSpeed = (float) rand.NextDouble() / 20;
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
            
            //DebugDraw circle = new DebugDraw(AsteroidsGame.graphics.GraphicsDevice);
            //circle.CreateCircle(GetRadius(), 100);
            //circle.Position = new Vector2(GetPosition().X, GetPosition().Y);
            //circle.Colour = Color.Red;

            spriteBatch.Begin();
            spriteBatch.Draw(texture, position, null, Color.White, rotation, origin, 1.0f, SpriteEffects.None, 0.0f);
            //circle.Render(spriteBatch);
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