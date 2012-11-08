using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
    class StarField : Base
    {

        #region Constants

        private const int num_stars = 100;

        #endregion

        #region Fields

        // Textures
        private Texture2D star_texture;

        private Random rand;

        private Vector2[] position;
        private float[] scale;

        #endregion

        public StarField(ContentManager content)
        {
            star_texture = content.Load<Texture2D>("sprite/star");

            position = new Vector2[num_stars];
            scale    = new float[num_stars];

            rand = new Random();

            int w = AsteroidsGame.graphics.PreferredBackBufferWidth;
            int h = AsteroidsGame.graphics.PreferredBackBufferHeight;

            for (int i = 0; i < num_stars; i++)
            {
                // Position
                position[i] = new Vector2(rand.Next(0,w), rand.Next(0,h));

                // Scale
                scale[i] = (float) rand.NextDouble();
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            {
                for (int i = 0; i < num_stars; i++)
                {
                    spriteBatch.Draw(star_texture, position[i], null, Color.White, 0.0f, Vector2.Zero, scale[i], SpriteEffects.None, 0.0f);
                }
            }
            spriteBatch.End();
        }
    }
}
