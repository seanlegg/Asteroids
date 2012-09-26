using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Asteroids
{
    class Player : Renderable
    {
        private Texture2D ship;
        
        private Vector2 origin;
        private Vector2 position;
        private Vector2 velocity;

        private float   speed;
        private float   rotation;

        private const float drag = 0.005f;
        private const float rotationSpeed = 0.125f;

        public Player(ContentManager content)
        {
            ship = content.Load<Texture2D>("sprite/ship");

            position = new Vector2((Asteroids.gameConfig.ScreenWidth / 2) - (ship.Width / 2), (Asteroids.gameConfig.ScreenHeight / 2) - (ship.Height / 2));
            velocity = Vector2.Zero;
            origin   = new Vector2(ship.Width / 2, ship.Height / 2);

            rotation = 0.0f;
            speed    = 5.0f;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            float dt = (float) gameTime.ElapsedGameTime.TotalSeconds;

            // User Input
            UserInput(dt);   
     
            // Update Position
            position += velocity;

            // Screen Wrap
            WrapUniverse();

            base.Update(gameTime);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(ship, position, null, Color.White, rotation, origin, 1.0f, SpriteEffects.None, 0.0f);
            spriteBatch.End();

            base.Draw(spriteBatch);
        }

        public void WrapUniverse()
        {
            if (position.X + ship.Width < 0)
            {
                position.X = Asteroids.gameConfig.ScreenWidth;
            }
            else if (position.X > Asteroids.gameConfig.ScreenWidth)
            {
                position.X = -ship.Width;
            }

            if (position.Y + ship.Height < 0)
            {
                position.Y = Asteroids.gameConfig.ScreenHeight;
            }
            else if (position.Y > Asteroids.gameConfig.ScreenHeight)
            {
                position.Y = -ship.Height;
            }
        }

        public void UserInput(float dt)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Up) == true)
            {
                velocity += new Vector2(
                     (float) Math.Sin(rotation) * speed * dt,
                    -(float) Math.Cos(rotation) * speed * dt
                );
                velocity.X = MathHelper.Clamp(velocity.X, -speed, speed);
                velocity.Y = MathHelper.Clamp(velocity.Y, -speed, speed);
            } else {
                velocity.X *= (1.0f - drag);
                velocity.Y *= (1.0f - drag);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Left) == true)
            {
                rotation -= rotationSpeed;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Right) == true)
            {
                rotation += rotationSpeed;
            }
        }
        
    }
}
