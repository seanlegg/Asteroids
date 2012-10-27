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
        #region Fields

        public static GraphicsDeviceManager graphics;
        public static bool isRunning = true;

        private SpriteBatch spriteBatch;

        private ScreenManager screenManager;

        public static int screenWidth  = 1280;
        public static int screenHeight = 720;

        #endregion

        public AsteroidsGame()
        {
            Content.RootDirectory = "Content";

            graphics = new GraphicsDeviceManager(this);
                        
            // Add Components
            screenManager = new ScreenManager(this);

            Components.Add(screenManager);
            Components.Add(new MessageDisplayComponent(this));
            Components.Add(new GamerServicesComponent(this));

            // Activate the first screens.
            screenManager.AddScreen(new TitleBackgroundScreen(), null);
            screenManager.AddScreen(new MainMenuScreen(), null);

            // Listen for invite notification events.
            // NetworkSession.InviteAccepted += (sender, e) => NetworkSessionComponent.InviteAccepted(screenManager, e);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {            
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferMultiSampling = true;

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
            if (isRunning == false)
                this.Exit();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            base.Draw(gameTime);
        }

    }

    #region Entry Point

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    static class Program
    {
        static void Main()
        {
            using (AsteroidsGame game = new AsteroidsGame())
            {
                game.Run();
            }
        }
    }

    #endregion
}
