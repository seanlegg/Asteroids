using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Asteroids
{
    class Player : Collidable
    {
        #region Fields

        private PlayerIndex playerIndex;

        private SpriteFont font;

        private Texture2D ship_texture;
        private Texture2D bullet_texture;

        private List<Bullet> bullets;

        private KeyboardState prevKeyboardState;

        private Vector2 origin;
        private Vector2 position;
        private Vector2 velocity;

        private float speed;
        private float rotation;

        private int lives = 3;
        private int score = 0;

        private float timeTillRespawn     = 0f;
        private float spawnProtectionTime = 0f;

        // Constants
        private const float drag            = 0.005f;
        private const float brake           = 0.025f;
        private const float rotationSpeed   = 0.125f;
        private const float gunCooldown     = 0.5f;
        private const float respawnTime     = 1.0f;
        private const float spawnProtection = 2f;

        #endregion

        public Player(ContentManager content, PlayerIndex playerIndex)
        {
            this.playerIndex = playerIndex;

            // Fonts
            font = content.Load<SpriteFont>("font/Segoe");

            // Textures
            ship_texture   = content.Load<Texture2D>("sprite/ship");
            bullet_texture = content.Load<Texture2D>("sprite/bullet");

            bullets = new List<Bullet>();

            position = new Vector2((AsteroidsGame.screenWidth / 2) - (ship_texture.Width / 2), (AsteroidsGame.screenHeight / 2) - (ship_texture.Height / 2));
            velocity = Vector2.Zero;
            origin = new Vector2(ship_texture.Width / 2, ship_texture.Height / 2);

            rotation = 0.0f;
            speed    = 5.0f;
            isActive = true;
        }

        #region Events 

        /// <summary>
        /// Event raised when the menu entry is selected.
        /// </summary>
        public event EventHandler<PlayerIndexEventArgs> onGameOver;

        #endregion

        public override void Init()
        {
            lives = 3;

            base.Init();
        }

        public void Respawn()
        {
            isActive = true;

            spawnProtectionTime = spawnProtection;

            velocity = Vector2.Zero;
            position = new Vector2((AsteroidsGame.screenWidth / 2) - (ship_texture.Width / 2), (AsteroidsGame.screenHeight / 2) - (ship_texture.Height / 2));
            rotation = 0.0f;
        }

        public void DecrementLives()
        {
            isActive = false;
            lives--;

            if (lives <= 0)
            {
                // Game Over
                if (onGameOver != null)
                    onGameOver(this, new PlayerIndexEventArgs(playerIndex));
            }
            else
            {
                timeTillRespawn = respawnTime;                
            }
        }

        public override void HandleCollision(Player p)
        {
            if (IsSpawnProtectionActive == true) return;

            // We have hit a player
            if (isActive)
            {
                DecrementLives();
            }
        }

        public override void HandleCollision(Asteroid a)
        {
            if (IsSpawnProtectionActive == true) return;

            // We have hit an Asteroid
            if (isActive)
            {
                DecrementLives();
            }
        }

        public override void HandleCollision(Bullet b)
        {
            if (IsSpawnProtectionActive == true) return;

            // Immunity to our own bullets
            if (b.Owner.GetHashCode() == this.GetHashCode()) return;

            // We've been hit by a bullet
            if (isActive)
            {
                DecrementLives();
            } 
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            float dt = (float) gameTime.ElapsedGameTime.TotalSeconds;

            if (isActive == false)
            {
                timeTillRespawn -= dt;
                if (timeTillRespawn <= 0)
                {
                    Respawn();
                }
                return;
            }

            // Spawn Protection
            if (spawnProtectionTime > 0)
            {
                spawnProtectionTime -= dt;
            }

            // Update the position of the ship
            position += velocity;

            // Update the bullets
            bullets.ForEach(delegate(Bullet b)
            {
                b.Update(gameTime);

                if (b.isActive == false)
                {
                    bullets.Remove(b);
                }
            });

            // Wrap the screen
            position = Helper.wrapUniverse(position, ship_texture.Width, ship_texture.Height);

            prevKeyboardState = Keyboard.GetState();

            base.Update(gameTime);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            // Render the player's bullets
            bullets.ForEach(delegate(Bullet b)
            {
                b.Draw(spriteBatch);
            });

            spriteBatch.Begin();
            {
                // Render the player's ship
                if (isActive)
                {
                    spriteBatch.Draw(ship_texture, position, null, Color.White, rotation, origin, 1.0f, SpriteEffects.None, 0.0f);
                }

                // Render the HUD
                spriteBatch.DrawString(font, "Lives: " + lives, new Vector2(0, 20), Color.Green);
                spriteBatch.DrawString(font, "Score: " + score, new Vector2(0,  0), Color.Green);
            }
            spriteBatch.End();
          
            base.Draw(spriteBatch);
        }

        public void HandleInput(InputState input, PlayerIndex playerIndex, GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            KeyboardState keyboardState = input.CurrentKeyboardStates[(int)playerIndex];
            GamePadState  gamePadState  = input.CurrentGamePadStates[(int)playerIndex];

            if (keyboardState.IsKeyDown(Keys.Up) || gamePadState.Triggers.Right > 0)
            {
                // Calculate the speed
                float s = gamePadState.Triggers.Right > 0 ? gamePadState.Triggers.Right * speed : speed;

                velocity += new Vector2(
                     (float) Math.Sin(rotation) * s * dt,
                    -(float) Math.Cos(rotation) * s * dt
                );
                velocity.X = MathHelper.Clamp(velocity.X, -speed, speed);
                velocity.Y = MathHelper.Clamp(velocity.Y, -speed, speed);
            } else {
                velocity.X *= (1.0f - drag);
                velocity.Y *= (1.0f - drag);
            }

            if (keyboardState.IsKeyDown(Keys.Down) || gamePadState.Triggers.Left > 0)
            {
                float b = gamePadState.Triggers.Left > 0 ? gamePadState.Triggers.Left * brake : brake;

                velocity.X *= (1.0f - b);
                velocity.Y *= (1.0f - b);
            }

            if (keyboardState.IsKeyDown(Keys.Left) || gamePadState.DPad.Left == ButtonState.Pressed)
            {
                rotation -= rotationSpeed;
            }

            if (keyboardState.IsKeyDown(Keys.Right) || gamePadState.DPad.Right == ButtonState.Pressed)
            {
                rotation += rotationSpeed;
            }

            if (gamePadState.ThumbSticks.Left != Vector2.Zero)
            {
                rotation += gamePadState.ThumbSticks.Left.X * rotationSpeed;
            }

            if (input.IsNewKeyPress(Keys.Space, playerIndex, out playerIndex) || input.IsNewButtonPress(Buttons.A, playerIndex, out playerIndex))
            {
               fire();
            }
        }

        public void fire()
        {
            bullets.Add(new Bullet(bullet_texture, this));
        }

        /**
         * Collision Detection Overrides
         */
        public override Vector3 GetPosition()
        {
            float xOffset = ship_texture.Width / 2;
            float yOffset = ship_texture.Height / 2;

            return new Vector3(position.X, position.Y, 0.0f);
        }

        public override int GetRadius()
        {
            return (ship_texture.Width > ship_texture.Height ? ship_texture.Width : ship_texture.Height) / 2;
        }

        #region Getters & Setters

        public bool IsSpawnProtectionActive
        {
            get { return spawnProtectionTime > 0f; }
        }

        public List<Bullet> Bullets
        {
            get { return bullets; }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public int Lives
        {
            get;
            set;
        }

        public float Rotation
        {
            get { return rotation; }
        }

        public int Width
        {
            get { return ship_texture.Width; }
        }

        public int Height
        {
            get { return ship_texture.Height; }
        }

        #endregion
    }
}
