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
    class GameOverScreen : GameScreen
    {
        #region Fields

        NetworkSession networkSession;

        #endregion

        #region Initialization

        /// <summary>
        /// Constructor.
        /// </summary>
        public GameOverScreen(NetworkSession networkSession)
        {
            this.networkSession = networkSession;
        }


        #endregion

        #region Update & Draw

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        #endregion

        #region Handle Input


        #endregion
    }
}
