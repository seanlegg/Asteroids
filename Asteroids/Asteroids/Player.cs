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

        private Vector2 position;
        private Vector2 direction;

        public Player(ContentManager content)
        {
            ship = content.Load<Texture2D>("sprite/ship");

            position  = new Vector2((Asteroids.gameConfig.ScreenWidth / 2) - (ship.Width / 2), (Asteroids.gameConfig.ScreenHeight / 2) - (ship.Height / 2));
            direction = Vector2.Zero;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {            
            // User Input
            UserInput();        

            // Screen Wrap
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
            base.Update(gameTime);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(ship, new Rectangle((int)position.X, (int)position.Y, ship.Width, ship.Height), Color.White);
            spriteBatch.End();

            base.Draw(spriteBatch);
        }

        public void UserInput()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Up) == true)
            {
                position.Y -= 3.0f;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Down) == true)
            {
                position.Y += 3.0f;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Left) == true)
            {
                position.X -= 3.0f;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Right) == true)
            {
                position.X += 3.0f;
            }
        }
        
    }
}
