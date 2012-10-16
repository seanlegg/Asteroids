#region File Description
//-----------------------------------------------------------------------------
// PauseMenuScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Net;
#endregion

namespace Asteroids
{
    /// <summary>
    /// The pause menu comes up over the top of the game,
    /// giving the player options to resume or quit.
    /// </summary>
    class GameOverScreen : MenuScreen
    {
        #region Fields

        NetworkSession networkSession;

        #endregion

        #region Initialization

        /// <summary>
        /// Constructor.
        /// </summary>
        public GameOverScreen(NetworkSession networkSession)
            : base(Resources.GameOver)
        {
            this.networkSession = networkSession;

            // If this is a single player game then allow the player to return to the menu
            if (networkSession == null)
            {
                MenuEntry returnToTitleEntry = new MenuEntry(Resources.ReturnToTitle);
                returnToTitleEntry.Selected += ReturnToTitleSelected;
                MenuEntries.Add(returnToTitleEntry);
            }
        }


        #endregion

        #region Handle Input

        /// <summary>
        /// Event handler for when the return to title entry is selected.
        /// </summary>
        void ReturnToTitleSelected(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, false, null, new TitleBackgroundScreen(), new MainMenuScreen());
        }

        #endregion
    }
}
