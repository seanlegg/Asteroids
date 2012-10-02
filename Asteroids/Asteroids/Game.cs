using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Asteroids
{
    class Game : Screen
    {
        private AsteroidManager asteroidManager;
        private List<Player> players;
        private List<Asteroid> asteroids;

        public Game(ContentManager content, EventHandler screenEvent) : base(screenEvent)
        {
            players = new List<Player>();
            players.Add(new Player(content));

            asteroidManager = new AsteroidManager(content);
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

        // Bounding Box
        public void CheckCollisions()
        {
            asteroidManager.Asteroids.ForEach(delegate(Asteroid a)
            {                
                players.ForEach(delegate(Player p)
                {
                    Vector2 p1 = a.Position;
                    Vector2 p2 = p.Position;

                    // Check for collisions with players
                    if ( (p1.X <= p2.X + p.Width && p1.X + a.Width >= p2.X) || (p2.X + a.Width >= p1.X && p1.X <= p2.X + p.Width) )
                    {
                        if (p1.Y <= p2.Y + p.Height && p1.Y + a.Height >= p2.Y)
                        {
                            a.HandleCollision(p); // Let the asteroids handle collisions with players
                            p.HandleCollision(a); // Let the Players handle collisions with asteroids
                        }
                    }

                    // Check for collisions with bullets
                    p.Bullets.ForEach(delegate(Bullet b)
                    {

                    });
                });                
            });
        }

    }
}
