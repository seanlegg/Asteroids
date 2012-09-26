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
        private List<Player> players;
        private KeyboardState previousKeyboardState;

        public Game(ContentManager content, EventHandler screenEvent) : base(screenEvent)
        {
            players = new List<Player>();

            players.Add(new Player(content));
        }

        public override void Update(GameTime dt)
        {
            // Keep track of the previous keyboard state
            previousKeyboardState = Keyboard.GetState();

            // Update players
            players.ForEach(delegate(Player p)
            {
                p.Update(dt);
            });
            base.Update(dt);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Render Players
            players.ForEach(delegate(Player p)
            {
                p.Draw(spriteBatch);
            });
            base.Draw(spriteBatch);
        }
    }
}
