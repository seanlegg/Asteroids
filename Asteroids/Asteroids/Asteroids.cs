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
    public class Asteroids : Microsoft.Xna.Framework.Game
    {
        // Game Configuration
        public static Config gameConfig;
        
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // Screens
        Screen currentScreen;

        Game game;
        SplashScreen splashScreen;
        MainMenuScreen menuScreen;


        public Asteroids()
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
            gameConfig = Content.Load<Config>("config");

            graphics.IsFullScreen = gameConfig.IsFullScreen;
            graphics.PreferredBackBufferWidth  = gameConfig.ScreenWidth;
            graphics.PreferredBackBufferHeight = gameConfig.ScreenHeight;
            graphics.PreferMultiSampling = gameConfig.PreferMultiSampling;

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
            EventHandler eventHandler = new EventHandler(ScreenEvent);

            splashScreen = new SplashScreen(this.Content, eventHandler);
            menuScreen   = new MainMenuScreen(this.Content, eventHandler);
            game = new Game(this.Content, eventHandler);

            // Set the currently active screen
            currentScreen = game;
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

        public void ScreenEvent(object obj, EventArgs e)
        {
            Console.WriteLine("ScreenEvent Triggered");

            // Change the currently active screen
            currentScreen = menuScreen;
        }
    }
}
