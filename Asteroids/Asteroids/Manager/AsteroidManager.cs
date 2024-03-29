﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using ProjectMercury;
using ProjectMercury.Renderers;

namespace Asteroids
{
    public enum Mode
    {
        TITLE,
        GAME
    }

    class AsteroidManager : Manager
    {

        #region Constants

        private const int max_asteroids_title = 15;

        #endregion

        #region Sound Effects

        SoundEffect asteroid_hit;
        SoundEffect asteroid_hit_bullet;

        #endregion

        #region Particle Effects

        // Particle System
        Renderer particleRenderer;

        // Particle States
        private bool isThrustEnabled = false;

        // Particle Effects
        ParticleEffect explosionEffect;

        #endregion

        #region Properties

        private List<Asteroid> asteroids;

        private int currentId;

        private Texture2D texture_small;
        private Texture2D texture_medium;
        private Texture2D texture_large;
        
        private Random rand;

        #endregion

        public AsteroidManager(ContentManager content, Mode mode)
        {
            rand = new Random();

            asteroids = new List<Asteroid>();

            // Textures
            texture_small  = content.Load<Texture2D>("sprite/asteroid_small");
            texture_medium = content.Load<Texture2D>("sprite/asteroid_medium");
            texture_large  = content.Load<Texture2D>("sprite/asteroid_large");

            // Sounds
            asteroid_hit = content.Load<SoundEffect>("sound/asteroid_impact");
            asteroid_hit_bullet = content.Load<SoundEffect>("sound/asteroid_impact_bullet");

            if (mode == Mode.TITLE)
            {
                InitTitle();
            }
            else
            {
                Init();
            }

            // Create Particle Renderer
            particleRenderer = new SpriteBatchRenderer
            {
                GraphicsDeviceService = AsteroidsGame.graphics
            };
            particleRenderer.LoadContent(content);

            // Load Particle Effects
            explosionEffect = new ParticleEffect();
            explosionEffect = content.Load<ParticleEffect>("effect/Explosion");
            explosionEffect.LoadContent(content);
            explosionEffect.Initialise();
        }

        public override void Init()
        {
            // Remove any existing asteroids
            asteroids.Clear();

            // TODO: Remove Magic Numbers
            int w = AsteroidsGame.graphics.PreferredBackBufferWidth;
            int h = AsteroidsGame.graphics.PreferredBackBufferHeight;
            int n = 1000;

            // Reset the Id
            currentId = 0;

            for (int i = 0; i < 20; i++)
            {
                Vector2 position = new Vector2(rand.Next(0, w), rand.Next(0, h));
                Vector2 velocity = new Vector2((float) Math.Sin(rand.Next(0, n)), (float) Math.Cos(rand.Next(0, n)) );

                float rotation = rand.Next(0, 359);
                float rotationSpeed = ((float)Math.Sin(rand.Next(0, 1000))) * 0.05f;

                Asteroid asteroid = new Asteroid(currentId++, AsteroidType.LARGE, texture_large, position, velocity, rotation, rotationSpeed);

                asteroids.Add(asteroid);
            }
        }

        public void StartLevel(int level)
        {
            // Remove any existing asteroids
            asteroids.Clear();

            // TODO: Remove Magic Numbers
            int w = AsteroidsGame.graphics.PreferredBackBufferWidth;
            int h = AsteroidsGame.graphics.PreferredBackBufferHeight;

            // Reset the Id
            currentId = 0;

            for (int i = 0; i < (level*5); i++)
            //for (int i = 0; i < 20; i++)
            {
                Vector2 position = new Vector2(rand.Next(0, w), rand.Next(0, h));     
                Vector2 velocity = new Vector2((float)Math.Sin(rand.Next(0, level)), (float)Math.Cos(rand.Next(0, level)));

                float rotation      = rand.Next(0, 359);
                float rotationSpeed = ((float)Math.Sin(rand.Next(0, 1000))) * 0.05f;

                asteroids.Add(new Asteroid(currentId++, AsteroidType.LARGE, texture_large, position, velocity, rotation, rotationSpeed));
            }
        }

        public void InitTitle()
        {
            // Remove any existing asteroids
            asteroids.Clear();

            // TODO: Remove Magic Numbers
            int w = AsteroidsGame.graphics.PreferredBackBufferWidth;
            int h = AsteroidsGame.graphics.PreferredBackBufferHeight;
            int n = 1000;

            // Reset the Id
            currentId = 0;

            for (int i = 0; i < max_asteroids_title; i++)
            {
                Vector2 position = new Vector2(rand.Next(0, w), rand.Next(0, h));
                Vector2 velocity = new Vector2((float)Math.Sin(rand.Next(0, n)), (float)Math.Cos(rand.Next(0, n)));

                float rotation      = rand.Next(0, 359);
                float rotationSpeed = ((float)Math.Sin(rand.Next(0, 1000))) * 0.05f;

                asteroids.Add(new Asteroid(currentId++, AsteroidType.LARGE, texture_large, position, velocity, rotation, rotationSpeed));
            }
        }

        public override void Update(GameTime gameTime)
        {
            float dt = (float) gameTime.ElapsedGameTime.TotalSeconds;

            // Update Particles
            explosionEffect.Update(dt);

            asteroids.ForEach(delegate(Asteroid a)
            {
                if (a.isActive == false)
                {
                    asteroids.Remove(a);
                }
                else
                {
                    a.Update(gameTime);
                }
            });
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Render explosion
            spriteBatch.Begin();
            {
                particleRenderer.RenderEffect(explosionEffect);
            }
            spriteBatch.End();

            asteroids.ForEach(delegate(Asteroid a)
            {
                a.Draw(spriteBatch);
            });
        }

        public void AddAsteroid(AsteroidType type, Vector2 position, Vector2 velocity, float rotation, float rotationSpeed)
        {
            Texture2D texture = (type == AsteroidType.LARGE ? texture_large : (type == AsteroidType.MEDIUM ? texture_medium : texture_small));

            asteroids.Add(new Asteroid(currentId++, AsteroidType.LARGE, texture, position, velocity, rotation, rotationSpeed));
        }

        public void SplitAsteroid(Asteroid parent, AsteroidType type)
        {
            Asteroid a = null, b = null;

            Vector2 v1 = new Vector2((float)Math.Sin(rand.Next() % 1000), (float)Math.Cos(rand.Next() % 1000));
            Vector2 v2 = new Vector2((float)Math.Sin(rand.Next() % 1000), (float)Math.Cos(rand.Next() % 1000));

            // Rotations
            float r1 = rand.Next(0, 359);
            float r2 = rand.Next(0, 359);

            // Rotation Speed
            float s1 = ((float)Math.Sin(rand.Next(0, 1000))) * 0.05f;
            float s2 = ((float)Math.Sin(rand.Next(0, 1000))) * 0.05f;

            if (type == AsteroidType.MEDIUM)
            {
                a = new Asteroid(currentId++, AsteroidType.MEDIUM, texture_medium, parent.Position, v1, r1, s1);
                b = new Asteroid(currentId++, AsteroidType.MEDIUM, texture_medium, parent.Position, v2, r2, s2);
            } 
            else if (type == AsteroidType.SMALL)
            {
                a = new Asteroid(currentId++, AsteroidType.SMALL, texture_small, parent.Position, v1, r1, s1);
                b = new Asteroid(currentId++, AsteroidType.SMALL, texture_small, parent.Position, v2, r2, s2);
            }
            asteroids.Add(a);
            asteroids.Add(b);
        }

        public void HandleCollision(Asteroid a, Player p) 
        {
            // Make sure the asteroid is active
            if (a.isActive == false) return;

            // Make sure the player is active
            if (p.isActive == false || p.IsSpawnProtectionActive == true) return;

            // Let the player handle its collision with the asteroid
            p.HandleCollision(a);

            // Trigger an explosion particle effect
            explosionEffect.Trigger(a.Position);

            // Play a sound - (http://www.freesound.org/people/m_O_m/sounds/109073/)
            asteroid_hit.Play();

            // Handle the asteroid collision
            HandleAsteroidCollision(a);
        }

        public void HandleCollision(Asteroid a, Bullet b) 
        {
            // Make sure the asteroid is active
            if (a.isActive == false) return;

            // Make sure the bullet is active
            if (b.isActive == false) return;

            // Let the bullet handle its collision with the asteroid
            b.HandleCollision(a);

            // Trigger an explosion particle effect
            explosionEffect.Trigger(a.Position);

            // Play a sound - (http://www.freesound.org/people/m_O_m/sounds/117771/)
            asteroid_hit_bullet.Play();

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
