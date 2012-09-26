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
        private Vector2 acceleration;

        private const float drag = 0.05f;

        public Player(ContentManager content)
        {
            ship = content.Load<Texture2D>("sprite/ship");

            position     = Vector2.Zero;
            direction    = Vector2.Zero;
            acceleration = Vector2.Zero;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            Console.WriteLine("[Player] dt = " + gameTime.TotalGameTime);

            position.X += 0.5f;
            position.Y += 0.5f;

            base.Update(gameTime);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(ship, new Rectangle((int)position.X, (int)position.Y, 32, 32), Color.White);
            spriteBatch.End();

            base.Draw(spriteBatch);
        }
        
    }
}
