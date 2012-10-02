using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Asteroids
{
    class Player : Base
    {
        private Texture2D ship_texture;
        private Texture2D bullet_texture;
#if DEBUG
        private Texture2D ship_texture_debug;
        private Texture2D bullet_texture_debug;
#endif

        private List<Bullet> bullets;

        private KeyboardState prevKeyboardState;

        private Vector2 origin;
        private Vector2 position;
        private Vector2 velocity;

        private float speed;
        private float rotation;

        private const float drag  = 0.005f;
        private const float brake = 0.025f;
        private const float rotationSpeed = 0.125f;

        public Player(ContentManager content)
        {
            ship_texture   = content.Load<Texture2D>("sprite/ship");
            bullet_texture = content.Load<Texture2D>("sprite/bullet");

#if DEBUG
            ship_texture_debug   = Render.CreateDebugTexture(ship_texture, Color.White);
            bullet_texture_debug = Render.CreateDebugTexture(bullet_texture, Color.White);
#endif

            bullets = new List<Bullet>();

            position = new Vector2((Asteroids.gameConfig.ScreenWidth / 2) - (ship_texture.Width / 2), (Asteroids.gameConfig.ScreenHeight / 2) - (ship_texture.Height / 2));
            velocity = Vector2.Zero;
            origin = new Vector2(ship_texture.Width / 2, ship_texture.Height / 2);

            rotation = 0.0f;
            speed    = 5.0f;
        }

        public override void HandleCollision(Player p)
        {
            Console.WriteLine(1);
        }

        public override void HandleCollision(Asteroid a)
        {
            Console.WriteLine(1);
        }

        public override void HandleCollision(Bullet b)
        {
            Console.WriteLine(1);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            float dt = (float) gameTime.ElapsedGameTime.TotalSeconds;

            // Process user input
            UserInput(dt);   
     
            // Update the position of the ship
            position += velocity;

            // Update the bullets
            bullets.ForEach(delegate(Bullet b)
            {
                b.Update(gameTime);

                if (b.isActive == false)
                {
                    bullets.Remove(b);
                }
            });

            // Wrap the screen
            position = Helper.wrapUniverse(position, ship_texture.Width, ship_texture.Height);

            prevKeyboardState = Keyboard.GetState();

            base.Update(gameTime);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            // Render the player's bullets
            bullets.ForEach(delegate(Bullet b)
            {
                b.Draw(spriteBatch);
            });

            // Render the player's ship
            spriteBatch.Begin();
            spriteBatch.Draw(ship_texture, position, null, Color.White, rotation, origin, 1.0f, SpriteEffects.None, 0.0f);
#if DEBUG
            spriteBatch.Draw(ship_texture_debug, position, null, Color.Red, rotation, origin, 1.0f, SpriteEffects.None, 0.0f);
#endif
            spriteBatch.End();

            base.Draw(spriteBatch);
        }   

        public void UserInput(float dt)
        {
            //if (Keyboard.GetState().IsKeyDown((Keys)Enum.Parse(typeof(Keys), Asteroids.gameConfig.Keyboard.Thrust, true)) == true)
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

            if (Keyboard.GetState().IsKeyDown(Keys.Down) == true)
            {
                velocity.X *= (1.0f - brake);
                velocity.Y *= (1.0f - brake);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Left) == true)
            {
                rotation -= rotationSpeed;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Right) == true)
            {
                rotation += rotationSpeed;
            }

            // if (Keyboard.GetState().IsKeyDown(Keys.Space) == true)
            if (Keyboard.GetState().IsKeyDown(Keys.Space) == true && prevKeyboardState.IsKeyDown(Keys.Space) == false)
            {
                fire();
            }
        }

        public void fire()
        {
#if DEBUG
            bullets.Add(new Bullet(bullet_texture, bullet_texture_debug, this));
#else
            bullets.Add(new Bullet(bullet_texture, this));
#endif
        }

        public List<Bullet> Bullets
        {
            get { return bullets; }
        }

        public Vector2 Position
        {
            get { return position; }
        }

        public float Rotation
        {
            get { return rotation; }
        }

        public int Width
        {
            get { return ship_texture.Width; }
        }

        public int Height
        {
            get { return ship_texture.Height; }
        }
    }
}
