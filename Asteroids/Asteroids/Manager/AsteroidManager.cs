using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
    class AsteroidManager : Manager
    {
        private List<Asteroid> asteroids;
        private Texture2D texture_small;
        private Texture2D texture_medium;
        private Texture2D texture_large;

        public enum Mode
        {
            TITLE,
            GAME
        }

        public AsteroidManager(ContentManager content, Mode mode)
        {
            asteroids = new List<Asteroid>();

            texture_small  = content.Load<Texture2D>("sprite/asteroid_small");
            texture_medium = content.Load<Texture2D>("sprite/asteroid_medium");
            texture_large  = content.Load<Texture2D>("sprite/asteroid_large");

            if (mode == Mode.TITLE)
            {
                InitTitle();
            }
            else
            {
                Init();
            }
        }

        public void Init()
        {
            Random rand = new Random();

            int w = AsteroidsGame.config.ScreenWidth;
            int h = AsteroidsGame.config.ScreenHeight;
            int n = 1000;

            for (int i = 0; i < 5; i++)
            {
                Vector2 position = new Vector2(rand.Next(0, w), rand.Next(0, h));
                Vector2 velocity = new Vector2((float) Math.Sin(rand.Next(0, n)), (float) Math.Cos(rand.Next(0, n)) );

                asteroids.Add(new Asteroid(Asteroid.AsteroidType.LARGE, texture_large, position, velocity));
            }
        }

        public void InitTitle()
        {
            Random rand = new Random();

            int w = AsteroidsGame.config.ScreenWidth;
            int h = AsteroidsGame.config.ScreenHeight;
            int n = 1000;

            for (int i = 0; i < 20; i++)
            {
                Vector2 position = new Vector2(rand.Next(0, w), rand.Next(0, h));
                Vector2 velocity = new Vector2((float)Math.Sin(rand.Next(0, n)), (float)Math.Cos(rand.Next(0, n)));

                asteroids.Add(new Asteroid(Asteroid.AsteroidType.LARGE, texture_large, position, velocity));
            }
        }

        public override void Update(GameTime dt)
        {
            asteroids.ForEach(delegate(Asteroid a)
            {
                if (a.isActive == false)
                {
                    asteroids.Remove(a);
                }
                else
                {
                    a.Update(dt);
                }
            });
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            asteroids.ForEach(delegate(Asteroid a)
            {
                a.Draw(spriteBatch);
            });
        }

        public void SplitAsteroid(Asteroid parent, Asteroid.AsteroidType type)
        {
            Random rand = new Random();
            Asteroid a = null, b = null;

            if (type == Asteroid.AsteroidType.MEDIUM)
            {
                a = new Asteroid(Asteroid.AsteroidType.MEDIUM, texture_medium, parent.Position, new Vector2(rand.Next(0, 2), rand.Next(0, 2)));
                b = new Asteroid(Asteroid.AsteroidType.MEDIUM, texture_medium, parent.Position, new Vector2(rand.Next(0, 2), rand.Next(0, 2)));
            } 
            else if (type == Asteroid.AsteroidType.SMALL)
            {
                a = new Asteroid(Asteroid.AsteroidType.SMALL, texture_small, parent.Position, new Vector2(rand.Next(0, 2), rand.Next(0, 2)));
                b = new Asteroid(Asteroid.AsteroidType.SMALL, texture_small, parent.Position, new Vector2(rand.Next(0, 2), rand.Next(0, 2)));
            }
            asteroids.Add(a);
            asteroids.Add(b);
        }

        public void HandleCollision(Asteroid a, Player p) 
        {
            p.HandleCollision(a);
        }

        public void HandleCollision(Asteroid a, Bullet b) 
        {
            // Make sure the bullet is active
            if (b.isActive == false) return;

            // Let the bullet handle its collision with the asteroid
            b.HandleCollision(a);

            switch (a.Type)
            {
                case Asteroid.AsteroidType.LARGE:
                {
                    // Create two medium asteroids
                    SplitAsteroid(a, Asteroid.AsteroidType.MEDIUM);
                }
                break;
                case Asteroid.AsteroidType.MEDIUM:
                {
                    // Create two small asteroids
                    SplitAsteroid(a, Asteroid.AsteroidType.SMALL);
                }
                break;
                case Asteroid.AsteroidType.SMALL:
                {
                    // Do Nothing - The Asteroid is now fully destroyed
                }
                break;
            }
            a.isActive = false;
        }

        public List<Asteroid> Asteroids
        {
            get { return asteroids; }
        }
    }
}
