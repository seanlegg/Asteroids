using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Asteroids
{
    class GameOverScreen : GameScreen
    {
        
        private KeyboardState previousKeyboardState;

        public GameOverScreen(ContentManager content, EventHandler screenEvent) : base(screenEvent)
        {
            
        }

        public void Init()
        {
            
        }

        public void Shutdown()
        {

        }

        public override void Update(GameTime dt)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Enter) == true && previousKeyboardState.IsKeyDown(Keys.Enter) == false)
            {
                //screenEvent.Invoke(this, new ScreenEvent());
            }
            

            // Keep track of the previous keyboard state
            previousKeyboardState = Keyboard.GetState();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            
        }

        public static void onGameOverEvent(object obj, EventArgs e)
        {

        }
    }
}
