using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

// Project Mercury Particle System
using ProjectMercury;
using ProjectMercury.Renderers;
using ProjectMercury.Modifiers;
using ProjectMercury.Emitters;

namespace Asteroids
{
    class Player : Collidable
    {
        #region Fields

        private byte Id;
        private PlayerIndex playerIndex;

        private SpriteFont font;

        private Texture2D ship_texture;
        private Texture2D bullet_texture;

        private List<Bullet> bullets;

        private Vector2 origin;
        private Vector2 position;
        private Vector2 velocity;

        private float speed;
        private double rotation;

        private int lives = 3;
        private int score = 0;

        private float timeTillRespawn        = 0f;
        private float spawnProtectionTime    = 0f;
        private float gameOverExplosionTimer = 1f;

        // Constants
        private const float drag            = 0.005f;
        private const float brake           = 0.025f;
        private const float rotationSpeed   = 0.125f;
        private const float gunCooldown     = 0.5f;
        private const float respawnTime     = 1.0f;
        private const float spawnProtection = 2f;

        // Particle System
        Renderer particleRenderer;

        // Particle States
        private bool isThrustEnabled = false;

        // Particle Effects
        ParticleEffect explosionEffect;
        ParticleEffect thrustEffect;

        #endregion

        public Player(ContentManager content, PlayerIndex? playerIndex)
        {
            this.playerIndex = playerIndex.Value;

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

            // Create Particle Renderer
            particleRenderer = new SpriteBatchRenderer
            {
                GraphicsDeviceService = AsteroidsGame.graphics
            };
            particleRenderer.LoadContent(content);

            // Load Particle Effects
            explosionEffect = new ParticleEffect();
            explosionEffect = content.Load<ParticleEffect>("effect/Explosion");
            explosionEffect.LoadContent(content);
            explosionEffect.Initialise();
        }

        /*
        public Player(ContentManager content, byte Id)
        {
            this.Id = Id;
        }
         */

        #region Events 

        /// <summary>
        /// Event raised when the menu entry is selected.
        /// </summary>
        public event EventHandler<PlayerIndexEventArgs> onGameOver;

        #endregion

        public override void Init()
        {
            lives = 3;

            // Reset timers
            gameOverExplosionTimer = 1f;

            // Reset the particle states
            isThrustEnabled = false;

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
                {
                    onGameOver(this, new PlayerIndexEventArgs(playerIndex));
                }
            }
            else
            {
                timeTillRespawn = respawnTime;                
            }
        }

        #region Collision Detection Handlers

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

        #endregion

        public override void Update(GameTime gameTime)
        {
            float dt = (float) gameTime.ElapsedGameTime.TotalSeconds;

            // Update Particles
            explosionEffect.Update(dt);

            if (isActive == false)
            {
                // Only allow the explosion particles to update for a short amount of time in the GameOver state
                if (lives == 0)
                {
                    gameOverExplosionTimer -= dt;
                    if (gameOverExplosionTimer < 0f)
                        return;
                }

                // Trigger an explosion particle effect
                explosionEffect.Trigger(position);
                
                timeTillRespawn -= dt;
                if (lives > 0 && timeTillRespawn <= 0)
                {
                    Respawn();
                }
                return;
            }

            // Trigger Thrust Particles
            if (isThrustEnabled)
            {
                explosionEffect.Trigger(position - velocity);
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

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            int i;

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
                    spriteBatch.Draw(ship_texture, position, null, Color.White, (float)rotation, origin, 1.0f, SpriteEffects.None, 0.0f);
                }

                // Render explosion
                particleRenderer.RenderEffect(explosionEffect);

                // Thrust Particles
                if (isThrustEnabled)
                {
                    // TODO: Render Thrust Particles
                }

                // Render the HUD
                for (i = 0; i < lives; i++)
                {
                    spriteBatch.Draw(ship_texture, new Vector2(ship_texture.Width + (i * ship_texture.Width)+(i*10), ship_texture.Height), Color.White);
                }
                spriteBatch.DrawString(font, "Score: " + score, new Vector2(ship_texture.Width,0), Color.White);
            }
            spriteBatch.End();
        }

        public void HandleInput(InputState input, PlayerIndex playerIndex, GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            KeyboardState keyboardState = input.CurrentKeyboardStates [(int)playerIndex];
            GamePadState  gamePadState  = input.CurrentGamePadStates  [(int)playerIndex];

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

                // Enable the thrust particle state
                isThrustEnabled = true;
            } else {
                velocity.X *= (1.0f - drag);
                velocity.Y *= (1.0f - drag);

                // Disable the thrust particle state
                isThrustEnabled = false;
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
               Fire();
            }
        }

        public void Fire()
        {
            bullets.Add(new Bullet(bullet_texture, this));
        }

        #region Collision Detection

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

        #endregion

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

        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        public int Lives
        {
            get { return lives; }
        }

        public int Score
        {
            get { return score; }
            set { this.score = value; }
        }

        public double Rotation
        {
            get { return rotation; }
            set { rotation = value; }
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
