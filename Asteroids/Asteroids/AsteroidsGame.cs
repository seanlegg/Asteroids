using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Asteroids
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class AsteroidsGame : Microsoft.Xna.Framework.Game
    {
        // Game Configuration
        public static Config config;
        public static GraphicsDeviceManager graphics;

        SpriteBatch spriteBatch;

        // Screens
        GameScreen currentScreen;

        Game game;
        SplashScreen   splashScreen;
        MainMenuScreen menuScreen;
        GameOverScreen gameOverScreen;

        public AsteroidsGame()
        {
            graphics = new GraphicsDeviceManager(this);
                        
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {            
            // Load Game Configuration
            config = Content.Load<Config>("config");

            graphics.IsFullScreen = config.IsFullScreen;
            graphics.PreferredBackBufferWidth = config.ScreenWidth;
            graphics.PreferredBackBufferHeight = config.ScreenHeight;
            graphics.PreferMultiSampling = config.PreferMultiSampling;

            graphics.ApplyChanges();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Screens
            splashScreen   = new SplashScreen(this.Content, onSplashScreenEvent);
            menuScreen     = new MainMenuScreen(this.Content, onMainMenuEvent);
            gameOverScreen = new GameOverScreen (this.Content, new EventHandler(GameOverScreen.onGameOverEvent));

            game = new Game(this.Content, null);

            // Set the currently active screen
            //currentScreen = game;
            currentScreen = splashScreen;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            // Update the current screen
            currentScreen.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            currentScreen.Draw(spriteBatch);

            base.Draw(gameTime);
        }

        public void onSplashScreenEvent(object obj, EventArgs e)
        {
            currentScreen = menuScreen;
        }

        public void onMainMenuEvent(object obj, EventArgs e)
        {
            currentScreen = game;
        }
    }
}
