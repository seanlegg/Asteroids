using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
    class AsteroidManager : Base
    {
        private List<Asteroid> asteroids;
        private Texture2D texture_small;
        private Texture2D texture_medium;
        private Texture2D texture_large;

        public AsteroidManager(ContentManager content)
        {
            asteroids = new List<Asteroid>();

            texture_small  = content.Load<Texture2D>("sprite/asteroid_small");
            texture_medium = content.Load<Texture2D>("sprite/asteroid_medium");
            texture_large  = content.Load<Texture2D>("sprite/asteroid_large");

            Init();
        }

        public void Init()
        {
            Random rand = new Random();

            int w = AsteroidsGame.config.ScreenWidth;
            int h = AsteroidsGame.config.ScreenHeight;
            int n = 1000;

            //asteroids.Add(new Asteroid(texture_small));
            //asteroids.Add(new Asteroid(texture_medium));
            //asteroids.Add(new Asteroid(texture_large));

            for (int i = 0; i < 5; i++)
            {
                Vector2 position = new Vector2(rand.Next(0, w), rand.Next(0, h));
                Vector2 velocity = new Vector2((float) Math.Sin(rand.Next(0, n)), (float) Math.Cos(rand.Next(0, n)) );

                asteroids.Add(new Asteroid(texture_large, position, velocity));
            }
        }

        public override void Update(GameTime dt)
        {
            asteroids.ForEach(delegate(Asteroid a)
            {
                a.Update(dt);
            });
            base.Update(dt);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            asteroids.ForEach(delegate(Asteroid a)
            {
                a.Draw(spriteBatch);
            });
            base.Draw(spriteBatch);
        }

        public List<Asteroid> Asteroids
        {
            get { return asteroids; }
        }
    }
}
