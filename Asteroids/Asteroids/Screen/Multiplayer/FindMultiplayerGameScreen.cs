using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Asteroids
{
    class FindMultiplayerGameScreen : GameScreen
    {
        private AvailableNetworkSessionCollection availableSessions;

        // Fonts
        private SpriteFont xboxController;
        private SpriteFont segoe;

        private Vector2 backButtonLocation;
        private Vector2 backTextLocation;

        private static String backButton = ")";
        private static String backText = "Back To Title";

        public FindMultiplayerGameScreen(ContentManager content)
        {
            // Load Fonts
            xboxController = content.Load<SpriteFont>("font/xboxControllerSpriteFont");
            segoe = content.Load<SpriteFont>("font/segoe");

            // Calculate the positions of the strings
            int w = GameBase.config.ScreenWidth;
            int h = GameBase.config.ScreenHeight;

            backTextLocation   = new Vector2(w - segoe.MeasureString(backText).X - 20, h - segoe.MeasureString(backText).Y - 25);
            backButtonLocation = new Vector2(backTextLocation.X - xboxController.MeasureString(backButton).X, h - (xboxController.MeasureString(backButton).Y - 50));
        }

        public void FindGames()
        {
            availableSessions = NetworkManager.Instance.FindGames();
        }

        public override void onActivate()
        {
            base.onActivate();
        }

        public override void onDeactivate()
        {
            base.onDeactivate();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime dt)
        {
            InputManager input = InputManager.Instance;

            if (input.IsKeyPressed(Keys.Escape) || input.IsButtonPressed(Buttons.B))
            {
                EventManager.Instance.Publish(new Event(EventType.NAVIGATE_MULTIPLAYER_MENU));
            }
            base.Update(dt);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            if (availableSessions == null || availableSessions.Count == 0)
            {
                Console.WriteLine("No Sessions Found");
            }
            else
            {
                Console.WriteLine("########################");
                for (int i = 0; i < availableSessions.Count; i++)
                {
                    Console.WriteLine("Session Found :: " + availableSessions[i].HostGamertag);
                }
            }

            spriteBatch.Begin();
            {
                spriteBatch.DrawString(xboxController, backButton, backButtonLocation, Color.White);
                spriteBatch.DrawString(segoe, backText, backTextLocation, Color.Green);
            }
            spriteBatch.End();

            base.Draw(spriteBatch);
        }
    }
}
