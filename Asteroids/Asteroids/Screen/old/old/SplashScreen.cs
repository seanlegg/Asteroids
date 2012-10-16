using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Asteroids
{
    class SplashScreen : GameScreen
    {
        private SpriteFont titleFont;
        private SpriteFont textFont;

        private AsteroidManager asteroidManager;

        private Text gameTitle;
        private Text pressStart;

        public SplashScreen(ContentManager content)
        {
            titleFont = content.Load<SpriteFont>("font/Menu");
            textFont  = content.Load<SpriteFont>("font/Segoe");

            asteroidManager = new AsteroidManager(content, Mode.TITLE);

            gameTitle  = new Text("XNA Asteroids", titleFont);
            pressStart = new Text("Press ENTER to start", textFont);
            pressStart.center = new Vector2(pressStart.center.X, pressStart.center.Y + gameTitle.size.Y);
        }

        /*
        public override void Update(GameTime dt)
        {
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);

            if (InputManager.Instance.IsKeyPressed(Keys.Enter) || InputManager.Instance.IsButtonPressed(Buttons.Start))
            {
                EventManager.Instance.Publish(new Event(EventType.NAVIGATE_MAIN_MENU));
            }
            asteroidManager.Update(dt);

            // Keep track of the previous keyboard state
            previousKeyboardState = Keyboard.GetState();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            {
                spriteBatch.DrawString(titleFont, gameTitle.title, gameTitle.center, Color.Green);
                spriteBatch.DrawString(textFont, pressStart.title, pressStart.center, Color.Green);
            }
            spriteBatch.End();

            // Render asteroids
            asteroidManager.Draw(spriteBatch);
        }
         */

    }
}
