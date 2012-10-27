﻿#region File Description
//-----------------------------------------------------------------------------
// GameplayScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
#endregion

namespace Asteroids
{
    /// <summary>
    /// This screen implements the actual game logic. It is just a
    /// placeholder to get the idea across: you'll probably want to
    /// put some more interesting gameplay in here!
    /// </summary>
    class GameplayScreen : GameScreen
    {
        #region Fields

        // Networking
        NetworkSession networkSession;
        PacketReader packetReader = new PacketReader();
        PacketWriter packetWriter = new PacketWriter();

        ContentManager content;
        SpriteFont gameFont;

        Random random = new Random();

        float pauseAlpha;

        private List<Player> players;
        private AsteroidManager asteroidManager;

        private int level;

        #endregion

        #region Properties


        /// <summary>
        /// The logic for deciding whether the game is paused depends on whether
        /// this is a networked or single player game. If we are in a network session,
        /// we should go on updating the game even when the user tabs away from us or
        /// brings up the pause menu, because even though the local player is not
        /// responding to input, other remote players may not be paused. In single
        /// player modes, however, we want everything to pause if the game loses focus.
        /// </summary>
        new bool IsActive
        {
            get
            {
                if (IsSinglePlayerGameOver == true)
                {
                    return !IsExiting;
                }
                else if (networkSession == null)
                {
                    // Pause behavior for single player games.
                    return base.IsActive;
                }
                else
                {
                    // Pause behavior for networked games.
                    return !IsExiting;
                }
            }
        }

        bool IsSinglePlayerGameOver
        {
            get 
            {
                if (networkSession != null)
                {
                    return false;
                }
                return players[(int)ControllingPlayer].Lives == 0; 
            }
        }

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public GameplayScreen(NetworkSession networkSession)
        {
            this.networkSession = networkSession;

            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);            

            players = new List<Player>();
        }


        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            gameFont = content.Load<SpriteFont>("font/gamefont");

            asteroidManager = new AsteroidManager(content, Mode.GAME);

            // Create the players ship
            if (networkSession == null)
            {
                Player p = new Player(content, ControllingPlayer);
                p.onGameOver += OnGameOver;

                players.Add(p);
            }
            
            /*
            else
            {

                foreach (NetworkGamer gamer in networkSession.LocalGamers)
                {
                    Player p = new Player(content, PlayerIndex.One);
                    p.onGameOver += OnGameOver;

                    players.Add(p);
                }
                Console.WriteLine("Players.length = " + players.Count);
            }
            */

            // A real game would probably have more content than this sample, so
            // it would take longer to load. We simulate that by delaying for a
            // while, giving you a chance to admire the beautiful loading screen.
            //Thread.Sleep(1000);

            // Initialize the game
            InitGame();

            // once the load has finished, we use ResetElapsedTime to tell the game's
            // timing mechanism that we have just finished a very long frame, and that
            // it should not try to catch up.
            ScreenManager.Game.ResetElapsedTime();
        }

        public void InitGame()
        {
            // Set the starting level
            level = 1;

            // Initialize Asteroids
            asteroidManager.Init();

            // Initialize Players
            players.ForEach(delegate(Player p)
            {
                p.Init();
            });
        }


        /// <summary>
        /// Unload graphics content used by the game.
        /// </summary>
        public override void UnloadContent()
        {
            content.Unload();
        }


        #endregion

        #region Events

        /// <summary>
        /// Event handler for when the game over event
        /// </summary>
        void OnGameOver(object sender, PlayerIndexEventArgs e)
        {
            GameOverScreen gameOverScreen = new GameOverScreen(networkSession);

            if (networkSession == null)
            {
                ScreenManager.AddScreen(gameOverScreen, ControllingPlayer);
            }
            else
            {
                Console.WriteLine("Multiplayer Game Over!");
            }
        }

        #endregion

        #region Network

        void ReceiveNetworkData(LocalNetworkGamer gamer)
        {

            // Keep reading as long as incoming packets are available.
            while (gamer.IsDataAvailable)
            {
                NetworkGamer sender;

                // Read a single packet from the network.
                gamer.ReceiveData(packetReader, out sender);

                // Discard packets sent by local gamers: we already know their state!
                if (sender.IsLocal)
                    continue;

                // Look up the tank associated with whoever sent this packet.
                Player remotePlayer = sender.Tag as Player;

                // Read the state of this tank from the network packet.
                remotePlayer.Position = packetReader.ReadVector2();
                remotePlayer.Velocity = packetReader.ReadVector2();
                remotePlayer.Rotation = packetReader.ReadDouble();

                Console.WriteLine("Player Position = " + remotePlayer.Position.ToString());
            }
        }

        void SendNetworkData(LocalNetworkGamer gamer)
        {
            Player p = gamer.Tag as Player;

            // Write the player data
            packetWriter.Write(p.Position);
            packetWriter.Write(p.Velocity);
            packetWriter.Write(p.Rotation);

            // Send the data to everyone in the session.
            gamer.SendData(packetWriter, SendDataOptions.InOrder);
        }

        #endregion

        #region Update and Draw


        /// <summary>
        /// Updates the state of the game.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
                pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            else
                pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);

            if (IsActive)
            {
                if (networkSession != null)
                {                    
                    foreach (LocalNetworkGamer gamer in networkSession.LocalGamers)
                    {
                        // Send Network Data
                        SendNetworkData(gamer);
                    }

                    // Update the network session object
                    networkSession.Update();

                    foreach (LocalNetworkGamer gamer in networkSession.LocalGamers)
                    {
                        ReceiveNetworkData(gamer);
                    }

                    // Update local players
                    foreach (LocalNetworkGamer gamer in networkSession.LocalGamers)
                    {
                        Player p = gamer.Tag as Player;

                        p.Update(gameTime);
                    }

                    players.ForEach(delegate(Player p)
                    {
                        p.Update(gameTime);
                    });
                }
                else
                {
                    players[(int)ControllingPlayer].Update(gameTime);
                }

                // Update Asteroids
                asteroidManager.Update(gameTime);

                // See if we need to increment the level
                if (asteroidManager.Asteroids.Count == 0)
                {
                    asteroidManager.StartLevel(level++);
                }

                // Handle collision detection
                CheckCollisions();
            }

            // If we are in a network game, check if we should return to the lobby.
            if ((networkSession != null) && !IsExiting)
            {
                if (networkSession.SessionState == NetworkSessionState.Lobby)
                {
                    LoadingScreen.Load(ScreenManager, false, null,
                                       new TitleBackgroundScreen(),
                                       new LobbyScreen(networkSession));
                }
            }
        }


        /// <summary>
        /// Lets the game respond to player input. Unlike the Update method,
        /// this will only be called when the gameplay screen is active.
        /// </summary>
        public override void HandleInput(InputState input, GameTime gameTime)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            if (ControllingPlayer.HasValue)
            {
                // In single player games, handle input for the controlling player.
                HandlePlayerInput(input, ControllingPlayer.Value, gameTime);
            }
            else if (networkSession != null)
            {
                // In network game modes, handle input for all the
                // local players who are participating in the session.
                foreach (LocalNetworkGamer gamer in networkSession.LocalGamers)
                {
                    if (!HandlePlayerInput(input, gamer.SignedInGamer.PlayerIndex, gameTime))
                        break;
                }
            }
        }


        /// <summary>
        /// Handles input for the specified player. In local game modes, this is called
        /// just once for the controlling player. In network modes, it can be called
        /// more than once if there are multiple profiles playing on the local machine.
        /// Returns true if we should continue to handle input for subsequent players,
        /// or false if this player has paused the game.
        /// </summary>
        bool HandlePlayerInput(InputState input, PlayerIndex playerIndex, GameTime gameTime)
        {
            // Look up inputs for the specified player profile.
            KeyboardState keyboardState = input.CurrentKeyboardStates[(int)playerIndex];
            GamePadState gamePadState = input.CurrentGamePadStates[(int)playerIndex];

            // The game pauses either if the user presses the pause button, or if
            // they unplug the active gamepad. This requires us to keep track of
            // whether a gamepad was ever plugged in, because we don't want to pause
            // on PC if they are playing with a keyboard and have no gamepad at all!
            bool gamePadDisconnected = !gamePadState.IsConnected &&
                                       input.GamePadWasConnected[(int)playerIndex];

            if (input.IsPauseGame(playerIndex) || gamePadDisconnected)
            {
                ScreenManager.AddScreen(new PauseMenuScreen(networkSession), playerIndex);
                return false;
            }

            // Handle Player Input
            if (networkSession == null)
            {
                players[(int)playerIndex].HandleInput(input, playerIndex, gameTime);
            }
            else
            {
                Player p = networkSession.LocalGamers[(int)playerIndex].Tag as Player;

                p.HandleInput(input, playerIndex, gameTime);
            }
            return true;
        }


        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.Black, 0, 0);

            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            // Render Players
            if (networkSession == null)
            {
                players.ForEach(delegate(Player p)
                {
                    p.Draw(spriteBatch);
                });
            }
            else
            {
                foreach (NetworkGamer gamer in networkSession.AllGamers)
                {
                    Player p = gamer.Tag as Player;
                    p.Draw(spriteBatch);
                }                
            }

            // Render Asteroids
            asteroidManager.Draw(spriteBatch);   

            spriteBatch.Begin();

            if (networkSession != null)
            {
                string message = "Players: " + networkSession.AllGamers.Count;
                Vector2 messagePosition = new Vector2(100, 480);
                spriteBatch.DrawString(gameFont, message, messagePosition, Color.White);
            }

            spriteBatch.End();

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }

        #endregion

        #region Collision Detection

        public void CheckCollisions()
        {
            asteroidManager.Asteroids.ForEach(delegate(Asteroid a)
            {
                players.ForEach(delegate(Player p)
                {
                    // Check for collisions between Asteroids and Players
                    if (Collision.BoundingSphere(a, p) == true)
                    {
                        asteroidManager.HandleCollision(a, p);
                    }

                    // Check for collisions with bullets
                    p.Bullets.ForEach(delegate(Bullet b)
                    {
                        // Bullets - Asteroids
                        if (Collision.BoundingSphere(b, a))
                        {
                            asteroidManager.HandleCollision(a, b);
                        }

                        // Bullets - Players
                        if (Collision.BoundingSphere(b, p))
                        {
                            p.HandleCollision(b);
                        }
                    });
                });
            });
        }

        #endregion
    }
}
