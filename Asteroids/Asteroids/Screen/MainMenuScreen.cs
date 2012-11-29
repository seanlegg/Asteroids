#region File Description
//-----------------------------------------------------------------------------
// MainMenuScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Media;
using System;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace Asteroids
{
    /// <summary>
    /// The main menu screen is the first thing displayed when the game starts up.
    /// </summary>
    class MainMenuScreen : MenuScreen
    {
        private Texture2D blank;

        #region Initialization

        /// <summary>
        /// Constructor fills in the menu contents.
        /// </summary>
        public MainMenuScreen()
            : base(Resources.MainMenu)
        {
            // Create our menu entries.
            MenuEntry singlePlayerMenuEntry = new MenuEntry(Resources.SinglePlayer);
            MenuEntry systemLinkMenuEntry   = new MenuEntry(Resources.SystemLink);
            MenuEntry settingsMenuEntry     = new MenuEntry(Resources.Settings);
            MenuEntry exitMenuEntry         = new MenuEntry(Resources.Exit);

            // Hook up menu event handlers.
            singlePlayerMenuEntry.Selected += SinglePlayerMenuEntrySelected;
            systemLinkMenuEntry.Selected   += SystemLinkMenuEntrySelected;
            settingsMenuEntry.Selected     += SettingsMenuEntrySelected;
            exitMenuEntry.Selected         += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(singlePlayerMenuEntry);
            MenuEntries.Add(systemLinkMenuEntry);
            MenuEntries.Add(settingsMenuEntry);
            MenuEntries.Add(exitMenuEntry);

            // Stop any background music
            MediaPlayer.Stop();
        }

        public override void LoadContent()
        {
            // Load a blank texture
            blank = new Texture2D(AsteroidsGame.graphics.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            blank.SetData(new[] { Color.White });

            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager screenManager = this.ScreenManager;
            SpriteBatch   spriteBatch   = screenManager.SpriteBatch;

            spriteBatch.Begin();
            {
                // Top
                Helper.DrawLine(
                    spriteBatch,
                    blank,
                    2,
                    Color.White,
                    new Vector2(420,50),
                    new Vector2(850,50)
                );

                Helper.DrawLine(
                    spriteBatch,
                    blank,
                    2,
                    Color.White,
                    new Vector2(420, 50),
                    new Vector2(650, 550)
                );

                Helper.DrawLine(
                    spriteBatch,
                    blank,
                    2,
                    Color.White,
                    new Vector2(650, 550),
                    new Vector2(850, 50)
                );
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }

        #endregion

        #region Handle Input


        /// <summary>
        /// Event handler for when the Single Player menu entry is selected.
        /// </summary>
        void SinglePlayerMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, new GameplayScreen(null));
        }

        /// <summary>
        /// Event handler for when the Settings menu entry is selected.
        /// </summary>
        void SettingsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new SettingsScreen(), e.PlayerIndex);
        }

        /// <summary>
        /// Event handler for when the Live menu entry is selected.
        /// </summary>
        void LiveMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            CreateOrFindSession(NetworkSessionType.PlayerMatch, e.PlayerIndex);
        }


        /// <summary>
        /// Event handler for when the System Link menu entry is selected.
        /// </summary>
        void SystemLinkMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            CreateOrFindSession(NetworkSessionType.SystemLink, e.PlayerIndex);
        }


        /// <summary>
        /// Helper method shared by the Live and System Link menu event handlers.
        /// </summary>
        void CreateOrFindSession(NetworkSessionType sessionType,
                                 PlayerIndex playerIndex)
        {
            // First, we need to make sure a suitable gamer profile is signed in.
            ProfileSignInScreen profileSignIn = new ProfileSignInScreen(sessionType);

            // Hook up an event so once the ProfileSignInScreen is happy,
            // it will activate the CreateOrFindSessionScreen.
            profileSignIn.ProfileSignedIn += delegate
            {
                GameScreen createOrFind = new CreateOrFindSessionScreen(sessionType);

                ScreenManager.AddScreen(createOrFind, playerIndex);
            };

            // Activate the ProfileSignInScreen.
            ScreenManager.AddScreen(profileSignIn, playerIndex);
        }


        /// <summary>
        /// When the user cancels the main menu, ask if they want to exit the sample.
        /// </summary>
        protected override void OnCancel(PlayerIndex playerIndex)
        {
            MessageBoxScreen confirmExitMessageBox =
                                    new MessageBoxScreen(Resources.ConfirmExitSample);

            confirmExitMessageBox.Accepted += ConfirmExitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmExitMessageBox, playerIndex);
        }


        /// <summary>
        /// Event handler for when the user selects ok on the "are you sure
        /// you want to exit" message box.
        /// </summary>
        void ConfirmExitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.Game.Exit();
        }


        #endregion
    }
}
