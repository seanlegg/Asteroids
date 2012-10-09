﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Asteroids
{
    class Game : GameScreen
    {
        private AsteroidManager asteroidManager;
        private List<Player> players;

        public Game(ContentManager content)
        {
            players = new List<Player>();
            players.Add(new Player(content));

            asteroidManager = new AsteroidManager(content, AsteroidManager.Mode.GAME);
        }

        public void Init()
        {
            // level = ...            
        }

        public override void Update(GameTime dt)
        {
            // Update players
            players.ForEach(delegate(Player p)
            {
                p.Update(dt);
            });

            // Update Asteroids
            asteroidManager.Update(dt);

            CheckCollisions();

            base.Update(dt);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Render Players
            players.ForEach(delegate(Player p)
            {
                p.Draw(spriteBatch);
            });

            // Render Asteroids
            asteroidManager.Draw(spriteBatch);

            base.Draw(spriteBatch);           
        }

        public void CheckCollisions()
        {
            asteroidManager.Asteroids.ForEach(delegate(Asteroid a)
            {                
                players.ForEach(delegate(Player p)
                {
                    if (Collision.BoundingSphere(a, p) == true)
                    {
                        p.HandleCollision(a);
                    }

                    // Check for collisions with bullets
                    p.Bullets.ForEach(delegate(Bullet b)
                    {
                        if (Collision.BoundingSphere(b, a))
                        {
                            asteroidManager.HandleCollision(a,b);                            
                        }

                        if (Collision.BoundingSphere(b, p))
                        {
                            p.HandleCollision(b);
                        }
                    });
                });                
            });
        }

    }
}
