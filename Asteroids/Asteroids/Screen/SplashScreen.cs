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
        private KeyboardState previousKeyboardState;

        private struct Text
        {
            public String title;
            public Vector2 size;
            public Vector2 center;

            public Text(String title, SpriteFont font)
            {
                this.title  = title;
                this.size   = font.MeasureString(title);
                this.center = new Vector2((AsteroidsGame.config.ScreenWidth / 2) - (size.X / 2), (AsteroidsGame.config.ScreenHeight / 2) - (size.Y / 2));
            }
        }
        private Text gameTitle;
        private Text pressStart;

        public SplashScreen(ContentManager content, EventHandler screenEvent) : base(screenEvent)
        {
            titleFont = content.Load<SpriteFont>("font/Menu");
            textFont  = content.Load<SpriteFont>("font/Segoe");

            asteroidManager = new AsteroidManager(content, AsteroidManager.Mode.TITLE);

            gameTitle  = new Text("XNA Asteroids", titleFont);
            pressStart = new Text("Press ENTER to start", textFont);
            pressStart.center = new Vector2(pressStart.center.X, pressStart.center.Y + gameTitle.size.Y);
        }

        public override void Update(GameTime dt)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Enter) == true && previousKeyboardState.IsKeyDown(Keys.Enter) == false)
            {
                screenEvent.Invoke(this, new EventArgs());
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

    }
}
