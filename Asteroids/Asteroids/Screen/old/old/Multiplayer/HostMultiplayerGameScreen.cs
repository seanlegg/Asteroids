using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Asteroids
{
    class HostMultiplayerGameScreen : GameScreen
    {
        private SpriteFont xboxController;
        private SpriteFont segoe;

        private Vector2 backButtonLocation;
        private Vector2 backTextLocation;

        private static String backButton = ")";
        private static String backText = "Back To Title";

        public HostMultiplayerGameScreen(ContentManager content)
        {
            // Load Fonts
            xboxController = content.Load<SpriteFont>("font/xboxControllerSpriteFont");
            segoe          = content.Load<SpriteFont>("font/segoe");

            // Calculate the positions of the strings
            //int w = AsteroidsGame.config.ScreenWidth;
            //int h = AsteroidsGame.config.ScreenHeight;

            //backTextLocation   = new Vector2(w - segoe.MeasureString(backText).X - 20, h - segoe.MeasureString(backText).Y - 25);
            //backButtonLocation = new Vector2(backTextLocation.X - xboxController.MeasureString(backButton).X, h - (xboxController.MeasureString(backButton).Y - 50));
        }

        /*
        public override void onActivate()
        {
            //if (NetworkManager.Instance.IsSignedIn())
            //{
                NetworkManager.Instance.HostGame();

                Console.Write("HOSTING GAME");
            //}
        }

        public override void Update(Microsoft.Xna.Framework.GameTime dt)
        {
            InputManager input = InputManager.Instance;

            if (input.IsKeyPressed(Keys.Escape) || input.IsButtonPressed(Buttons.B))
            {
                EventManager.Instance.Publish(new Event(EventType.NAVIGATE_MULTIPLAYER_MENU));
            }

            NetworkManager.Instance.session.Update();

            base.Update(dt);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            {
                spriteBatch.DrawString(segoe, "Hosting Game", Vector2.Zero, Color.Green);

                spriteBatch.DrawString(xboxController, backButton, backButtonLocation, Color.White);
                spriteBatch.DrawString(segoe, backText, backTextLocation, Color.Green);
            }
            spriteBatch.End();

            base.Draw(spriteBatch);
        }
         * */
    }
}
