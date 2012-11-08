#region File Description
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
using Microsoft.Xna.Framework.Media;
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
        #region Constants

        public enum PacketTypes
        {
            GameData,
            PlayerData,
            PlayerDeath,
            PlayerSpawn,
            GameWon,
        };

        private const int updatesBetweenGameDataPackets = 30;
        private const int updatesBetweenStatusPackets   = 0;        

        #endregion

        #region Fields

        ContentManager content;
        SpriteFont gameFont;

        Random random = new Random();

        float pauseAlpha;

        private List<Player> players;
        private AsteroidManager asteroidManager;

        private StarField starField;

        private Vector2[] scoreRegions;

        private int level;

        #endregion

        #region Sound Effects

        Song backgroundMusic;

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

        #region Networking Data

        private NetworkSession networkSession;

        private PacketReader packetReader = new PacketReader();
        private PacketWriter packetWriter = new PacketWriter();

        private int updatesSinceGameDataSend = 0;
        private int updatesSinceStatusPacket = 0;

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

            // HUD - Score Regions (Positions)
            scoreRegions = new Vector2[4];
            {
                int w = AsteroidsGame.graphics.PreferredBackBufferWidth;
                int h = AsteroidsGame.graphics.PreferredBackBufferHeight;
                int offset = 5;    // offset
                int hWidth  = 115; // hud width
                int hHeight = 45;  // hud height

                scoreRegions[0] = new Vector2(offset, offset);
                scoreRegions[1] = new Vector2(w - hWidth, offset);
                scoreRegions[2] = new Vector2(offset, h - hHeight);
                scoreRegions[3] = new Vector2(w - hWidth, h - hHeight);
            }
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

                // Set HUD properties
                p.ScoreRegion = scoreRegions[0];

                players.Add(p);
            }

            // Load Background Music - (http://freemusicarchive.org/music/Edward_Shallow/World_Head_Law/02_Poisons__Potions)
            backgroundMusic = content.Load<Song>("sound/background");

            // A real game would probably have more content than this sample, so
            // it would take longer to load. We simulate that by delaying for a
            // while, giving you a chance to admire the beautiful loading screen.
            //Thread.Sleep(1000);

            // Create the star field
            starField = new StarField(content);

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
            if (networkSession == null)
            {
                players.ForEach(delegate(Player p)
                {
                    p.Init();
                });
            }
            else
            {
                int hIndex = 0;

                foreach (NetworkGamer gamer in networkSession.AllGamers)
                {
                    Player p = gamer.Tag as Player;

                    // Initialise the player
                    p.Init();

                    // Bind the gameOver event to local players
                    if (gamer.IsLocal)
                    {
                        p.onGameOver += OnGameOver;
                    }

                    // Initialise the HUD
                    p.ScoreRegion = scoreRegions[hIndex];

                    hIndex++;
                }
            }

            // Play the background music
            
            MediaPlayer.Play(backgroundMusic);
            MediaPlayer.Volume = 20;
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

            ScreenManager.AddScreen(gameOverScreen, ControllingPlayer);
        }

        #endregion

        #region Network

        private void UpdateGameData()
        {
            if (packetReader == null)
            {
                throw new ArgumentNullException("packetReader");
            }

            int numberAsteroids = packetReader.ReadInt32();

            // Remove all the asteroids
            asteroidManager.Asteroids.Clear();

            for (int i = 0; i < numberAsteroids; i++)
            {
                asteroidManager.AddAsteroid((AsteroidType)packetReader.ReadInt32(), packetReader.ReadVector2(), packetReader.ReadVector2(), 0, 0);
            }
        }

        private void UpdateShipData(NetworkGamer sender)
        {
            if (sender != null)
            {
                Player player = sender.Tag as Player;

                if (player != null)
                {
                    player.Position    = packetReader.ReadVector2();
                    player.Velocity    = packetReader.ReadVector2();
                    player.Rotation    = packetReader.ReadDouble();
                    player.IsThrusting = packetReader.ReadBoolean();
                    player.isActive    = packetReader.ReadBoolean();
                    player.IsGameOver  = packetReader.ReadBoolean();
                }
            }
        }

        private void SendLocalShipData()
        {
            if ((networkSession != null) && (networkSession.LocalGamers.Count > 0))
            {
                Player player = networkSession.LocalGamers[0].Tag as Player;

                if (player != null)
                {
                    packetWriter.Write((int)PacketTypes.PlayerData);
                    packetWriter.Write(player.Position);
                    packetWriter.Write(player.Velocity);
                    packetWriter.Write(player.Rotation);
                    packetWriter.Write(player.IsThrusting);
                    packetWriter.Write(player.isActive);
                    packetWriter.Write(player.IsGameOver);

                    networkSession.LocalGamers[0].SendData(packetWriter, SendDataOptions.InOrder);
                }
            }
        }

        private void SendLocalShipDeath()
        {
            if ((networkSession != null) && (networkSession.LocalGamers.Count > 0))
            {
                Player player = networkSession.LocalGamers[0].Tag as Player;

                // Player Death Notification
                packetWriter.Write((int)PacketTypes.PlayerDeath);

                networkSession.LocalGamers[0].SendData(packetWriter, SendDataOptions.ReliableInOrder);
            }
        }

        void ReceiveNetworkData()
        {
            if ((networkSession != null) && (networkSession.LocalGamers.Count > 0))
            {
                LocalNetworkGamer gamer = networkSession.LocalGamers[0];

                // Keep reading as long as incoming packets are available.
                while (gamer.IsDataAvailable)
                {
                    NetworkGamer sender;

                    // Read a single packet from the network.
                    gamer.ReceiveData(packetReader, out sender);

                    // Discard packets sent by local gamers: we already know their state!
                    if (sender.IsLocal)
                        continue;

                    // Check the type of the packet
                    PacketTypes packetType = (PacketTypes)packetReader.ReadInt32();

                    switch (packetType)
                    {
                        case PacketTypes.GameData:
                            UpdateGameData();
                            break;
                        case PacketTypes.PlayerData:
                            UpdateShipData(sender);
                            break;
                    }

                    // Bullets
                    /*
                    int activeBullets = packetReader.ReadInt32();

                    for (int i = 0; i < activeBullets; i++)
                    {
                        int id     = packetReader.ReadInt32();
                        double ttl = packetReader.ReadDouble();

                        // Try and retreive the bullet
                        Bullet b = remotePlayer.FindBulletById(id);

                        if (b == null)
                        {
                            remotePlayer.FireBullet(id, packetReader.ReadVector2(), packetReader.ReadVector2());
                        }
                        else
                        {
                            // Update an existing bullet
                            b.isActive   = true;
                            b.TimeToLive = ttl;
                            b.Position   = packetReader.ReadVector2();
                            b.Velocity   = packetReader.ReadVector2();
                        }
                    }
                     * */
                }
            }
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
                starField.Update(gameTime);

                if (networkSession != null)
                {
                    // Process Incoming Packets
                    ReceiveNetworkData();

                    if (networkSession.IsHost)
                    {
                        LocalNetworkGamer gamer = networkSession.Host as LocalNetworkGamer;

                        if (updatesSinceGameDataSend >= updatesBetweenGameDataPackets)
                        {
                            updatesSinceGameDataSend = 0;

                            // Write each of the asteroids
                            packetWriter.Write((int)PacketTypes.GameData);
                            packetWriter.Write(asteroidManager.Asteroids.Count);

                            for (int i = 0; i < asteroidManager.Asteroids.Count; i++)
                            {
                                Asteroid asteroid = asteroidManager.Asteroids[i];

                                packetWriter.Write((int)asteroid.Type);
                                packetWriter.Write(asteroid.Position);
                                packetWriter.Write(asteroid.Velocity);                                
                            }
                            gamer.SendData(packetWriter, SendDataOptions.InOrder);
                        }
                        else
                        {
                            updatesSinceGameDataSend++;
                        }
                    }

                    // Update players
                    foreach (NetworkGamer gamer in networkSession.AllGamers)
                    {
                        Player p = gamer.Tag as Player;

                        if (p != null)
                        {
                            p.Update(gameTime);

                            if (gamer.IsLocal && p.wasKilled)
                            {
                                // Reset the death flag
                                p.wasKilled = false;

                                SendLocalShipDeath();
                            }
                        }
                    }

                    // Send Player Data  
                    if (updatesSinceStatusPacket >= updatesBetweenStatusPackets)
                    {
                        updatesSinceStatusPacket = 0;
                        SendLocalShipData();
                    }
                    else
                    {
                        updatesSinceStatusPacket++;
                    }
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
                if (networkSession == null)
                {
                    CheckCollisions();
                }
                else
                {
                    CheckMultiplayerCollisions();
                }
            }

            // Check for gameOver state
            if (networkSession != null)
            {
                bool shouldReturnToLobby = true;

                foreach (NetworkGamer gamer in networkSession.AllGamers)
                {
                    Player p = gamer.Tag as Player;

                    if (p.IsGameOver == false)
                    {
                        shouldReturnToLobby = false;
                    }
                }

                // If all players are in the gameOver state then return to the lobby
                if (shouldReturnToLobby)
                {
                    if (networkSession.SessionState == NetworkSessionState.Playing)
                    {
                        networkSession.EndGame();
                    }
                }
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
                if ((int)playerIndex >= networkSession.LocalGamers.Count) return true;

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

            // Draw the starfield
            starField.Draw(spriteBatch);

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

        public void CheckMultiplayerCollisions()
        {
            foreach (NetworkGamer gamer in networkSession.AllGamers)
            {
                Player p = gamer.Tag as Player;

                asteroidManager.Asteroids.ForEach(delegate(Asteroid a)
                {
                    // Check for collisions between Asteroids and Players
                    if (Collision.BoundingSphere(a, p) == true)
                    {
                        asteroidManager.HandleCollision(a, p);
                    }

                    // Check for collisions with bullets
                    for (int i = 0; i < p.Bullets.Length; i++)
                    {
                        Bullet b = p.Bullets[i];

                        if (b != null)
                        {
                            // Bullets - Asteroids
                            if (Collision.BoundingSphere(b, a))
                            {
                                asteroidManager.HandleCollision(a, b);
                            }
                        }
                    }
                });
            }
        }

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
                    for (int i = 0; i < p.Bullets.Length; i++)
                    {
                        Bullet b = p.Bullets[i];

                        if (b != null)
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
                        }
                    }
                });
            });
        }

        #endregion
    }
}
