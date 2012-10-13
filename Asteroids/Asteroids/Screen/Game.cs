using System;
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

            asteroidManager = new AsteroidManager(content, Mode.GAME);

            // Subscribe to Events
            EventManager eventManager = EventManager.Instance;

            eventManager.Subscribe(EventType.NEW_GAME, this);
        }

        public void InitGame()
        {
            // level = ...

            // Initialize Asteroids
            asteroidManager.Init();

            // Initialize Players
            players.ForEach(delegate(Player p)
            {
                p.Init();
            });

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

        public override void OnEvent(Event e)
        {
            if (e.EventType == EventType.NEW_GAME)
            {
                InitGame();
            }
            base.OnEvent(e);
        }

        public void CheckCollisions()
        {
            asteroidManager.Asteroids.ForEach(delegate(Asteroid a)
            {                
                players.ForEach(delegate(Player p)
                {
                    // Check for collisions between Asteroids and Players
                    if (Collision.BoundingSphere(a, p) == true)
                    {
                        asteroidManager.HandleCollision(a, p); 
                    }

                    // Check for collisions with bullets
                    p.Bullets.ForEach(delegate(Bullet b)
                    {
                        // Bullets - Asteroids
                        if (Collision.BoundingSphere(b, a))
                        {
                            asteroidManager.HandleCollision(a,b);                            
                        }

                        // Bullets - Players
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
