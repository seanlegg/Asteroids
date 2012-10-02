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

            //asteroids.Add(new Asteroid(texture_small));
            //asteroids.Add(new Asteroid(texture_medium));
            asteroids.Add(new Asteroid(texture_large));
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
