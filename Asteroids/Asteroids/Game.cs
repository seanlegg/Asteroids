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

        public static Vector2 wrapUniverse(Vector2 position, int textureWidth, int textureHeight)
        {
            if (position.X + textureWidth < 0)
            {
                position.X = Asteroids.gameConfig.ScreenWidth;
            }
            else if (position.X > Asteroids.gameConfig.ScreenWidth)
            {
                position.X = -textureWidth;
            }

            if (position.Y + textureHeight < 0)
            {
                position.Y = Asteroids.gameConfig.ScreenHeight;
            }
            else if (position.Y > Asteroids.gameConfig.ScreenHeight)
            {
                position.Y = -textureHeight;
            }
            return position;
        }
    }
}
