using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
    public enum Mode
    {
        TITLE,
        GAME
    }

    class AsteroidManager : Manager
    {
        private List<Asteroid> asteroids;

        private Texture2D texture_small;
        private Texture2D texture_medium;
        private Texture2D texture_large;

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

        public override void Init()
        {
            // Remove any existing asteroids
            asteroids.Clear();

            Random rand = new Random();

            int w = GameBase.config.ScreenWidth;
            int h = GameBase.config.ScreenHeight;
            int n = 1000;

            for (int i = 0; i < 5; i++)
            {
                Vector2 position = new Vector2(rand.Next(0, w), rand.Next(0, h));
                Vector2 velocity = new Vector2((float) Math.Sin(rand.Next(0, n)), (float) Math.Cos(rand.Next(0, n)) );

                asteroids.Add(new Asteroid(AsteroidType.LARGE, texture_large, position, velocity));
            }
        }

        public void InitTitle()
        {
            // Remove any existing asteroids
            asteroids.Clear();

            Random rand = new Random();

            int w = GameBase.config.ScreenWidth;
            int h = GameBase.config.ScreenHeight;
            int n = 1000;

            for (int i = 0; i < 20; i++)
            {
                Vector2 position = new Vector2(rand.Next(0, w), rand.Next(0, h));
                Vector2 velocity = new Vector2((float)Math.Sin(rand.Next(0, n)), (float)Math.Cos(rand.Next(0, n)));

                asteroids.Add(new Asteroid(AsteroidType.LARGE, texture_large, position, velocity));
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

        public void SplitAsteroid(Asteroid parent, AsteroidType type)
        {
            Random rand = new Random();
            Asteroid a = null, b = null;

            if (type == AsteroidType.MEDIUM)
            {
                a = new Asteroid(AsteroidType.MEDIUM, texture_medium, parent.Position, new Vector2(rand.Next(0, 2), rand.Next(0, 2)));
                b = new Asteroid(AsteroidType.MEDIUM, texture_medium, parent.Position, new Vector2(rand.Next(0, 2), rand.Next(0, 2)));
            } 
            else if (type == AsteroidType.SMALL)
            {
                a = new Asteroid(AsteroidType.SMALL, texture_small, parent.Position, new Vector2(rand.Next(0, 2), rand.Next(0, 2)));
                b = new Asteroid(AsteroidType.SMALL, texture_small, parent.Position, new Vector2(rand.Next(0, 2), rand.Next(0, 2)));
            }
            asteroids.Add(a);
            asteroids.Add(b);
        }

        public void HandleCollision(Asteroid a, Player p) 
        {
            // Make sure the player is active
            if (p.isActive == false || p.IsSpawnProtectionActive == true) return;

            // Let the player handle its collision with the asteroid
            p.HandleCollision(a);

            // Handle the asteroid collision
            HandleAsteroidCollision(a);
        }

        public void HandleCollision(Asteroid a, Bullet b) 
        {
            // Make sure the bullet is active
            if (b.isActive == false) return;

            // Let the bullet handle its collision with the asteroid
            b.HandleCollision(a);

            // Handle the asteroid collision
            HandleAsteroidCollision(a);
        }

        public void HandleAsteroidCollision(Asteroid a)
        {
            switch (a.Type)
            {
                case AsteroidType.LARGE:
                    {
                        // Create two medium asteroids
                        SplitAsteroid(a, AsteroidType.MEDIUM);
                    }
                    break;
                case AsteroidType.MEDIUM:
                    {
                        // Create two small asteroids
                        SplitAsteroid(a, AsteroidType.SMALL);
                    }
                    break;
                case AsteroidType.SMALL:
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
