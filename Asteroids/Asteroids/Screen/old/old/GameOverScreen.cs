using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Asteroids
{
    class GameOverScreen : GameScreen
    {
        private SpriteFont textFont;
        private Text gameOverMessage;

        public GameOverScreen(ContentManager content)
        {
            textFont = content.Load<SpriteFont>("font/Menu");

            gameOverMessage = new Text("Game Over", textFont);
        }

        /*
        public override void Init()
        {
            
        }

        public void Shutdown()
        {

        }

        public override void Update(GameTime dt)
        {
            InputManager iManager = InputManager.Instance;

            if (iManager.IsKeyPressed(Keys.Enter) || iManager.IsButtonPressed(Buttons.Start))
            {
                EventManager.Instance.Publish(new Event(EventType.NAVIGATE_MAIN_MENU));
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            {
                spriteBatch.DrawString(textFont, gameOverMessage.title, gameOverMessage.center, Color.Green);
            }
            spriteBatch.End();
        }
         * */
    }
}
