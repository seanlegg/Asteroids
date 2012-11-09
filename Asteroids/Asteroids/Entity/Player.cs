using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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

        private PlayerIndex playerIndex;

        private SpriteFont font;

        private Texture2D ship_texture;
        private Texture2D bullet_texture;

        private Bullet[] bullets;

        private Vector2 origin;
        private Vector2 position;
        private Vector2 velocity;

        public  bool wasKilled;
        private bool isGameOver;

        private float speed;
        private double rotation;

        private int lives = 3;
        private int score = 0;

        private float timeTillRespawn        = 0f;
        private float spawnProtectionTime    = 0f;
        private float gameOverExplosionTimer = 1f;

        // Heads Up Display (HUD)
        Vector2 scoreRegion;
        
        #endregion

        #region Constants

        private const float drag            = 0.005f;
        private const float brake           = 0.025f;
        private const float rotationSpeed   = 0.125f;
        private const float gunCooldown     = 0.5f;
        private const float respawnTime     = 1.0f;
        private const float spawnProtection = 2f;
        private const int   maxBullets      = 20;

        #endregion

        #region Particle Effects

        // Particle System
        Renderer particleRenderer;

        // Particle States
        private bool isThrustEnabled = false;

        // Particle Effects
        ParticleEffect explosionEffect;
        ParticleEffect thrustEffect;

        #endregion

        #region Sound Effects

        SoundEffect bullet_fire;
        SoundEffect thrust_sound;

        float trust_sound_timer;

        #endregion

        public Player(ContentManager content, PlayerIndex? playerIndex)
        {
            this.playerIndex = playerIndex.Value;

            // Fonts
            font = content.Load<SpriteFont>("font/Segoe");

            // Textures
            ship_texture   = content.Load<Texture2D>("sprite/ship");
            bullet_texture = content.Load<Texture2D>("sprite/bullet");

            // Sounds
            bullet_fire  = content.Load<SoundEffect>("sound/asteroids-ship-fire");
            thrust_sound = content.Load<SoundEffect>("sound/thrust");

            trust_sound_timer = (float)thrust_sound.Duration.TotalSeconds;

            // Bullets
            bullets = new Bullet[maxBullets];

            for (int i = 0; i < maxBullets; i++)
            {
                bullets[i] = new Bullet(bullet_texture, this, i);
            }

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

            // Thrust Effect
            thrustEffect = new ParticleEffect();
            thrustEffect = content.Load<ParticleEffect>("effect/Thrust");
            thrustEffect.LoadContent(content);
            thrustEffect.Initialise();
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

            // Reset timers
            gameOverExplosionTimer = 1f;
            spawnProtectionTime    = spawnProtection;

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
            isActive  = false;
            wasKilled = true;
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
            thrustEffect.Update(dt);

            // Update the bullets
            for (int i = 0; i < bullets.Length; i++)
            {
                Bullet b = bullets[i];

                if (b != null)
                {
                    b.Update(gameTime);
                }
            }

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
                // Play Sound
                // thrust_sound.Play();

                /*
                if (trust_sound_timer < (float)thrust_sound.Duration.TotalSeconds/2)
                {
                    trust_sound_timer = (float)thrust_sound.Duration.TotalSeconds;

                    thrust_sound.Play();
                }
                trust_sound_timer -= dt;
                 * */

                // Update Particles
                Vector2 p = new Vector2(
                     (float)Math.Sin(rotation),
                    -(float)Math.Cos(rotation)
                );
                thrustEffect.Trigger((position-(p*ship_texture.Height/2)) - velocity);
            }

            // Spawn Protection
            if (spawnProtectionTime > 0)
            {
                spawnProtectionTime -= dt;
            }

            // Update the position of the ship
            position += velocity;

            // Wrap the screen
            position = Helper.wrapUniverse(position, ship_texture.Width, ship_texture.Height);

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            int i;

            // Render the player's bullets
            for (i = 0; i < bullets.Length; i++)
            {
                Bullet b = bullets[i];

                if (b != null && b.isActive)
                {
                    b.Draw(spriteBatch);
                }
            }

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
                particleRenderer.RenderEffect(thrustEffect);

                // Render the HUD
                for (i = 0; i < lives; i++)
                {
                    spriteBatch.Draw(ship_texture, new Vector2(scoreRegion.X + (i * ship_texture.Width/2) + (i * 10), scoreRegion.Y), null, Color.White, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0.0f);                    
                }
                spriteBatch.DrawString(font, "Score: " + score, new Vector2(scoreRegion.X, scoreRegion.Y+ship_texture.Height / 2), Color.White);
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
                Vector2 bulletVelocity = new Vector2(
                     (float)Math.Sin(rotation) * Bullet.constant_speed,
                    -(float)Math.Cos(rotation) * Bullet.constant_speed
                );
                FireBullet(0, position, bulletVelocity);
            }
        }

        public void FireBullet(int id, Vector2 position, Vector2 velocity)
        {
            bool fired = false;

            // Find the next free position
            for (int i = 0; i < bullets.Length && fired == false; i++)
            {
                Bullet b = bullets[i];

                if (b != null)
                {
                    if (b.isActive == false)
                    {
                        b.Position   = position;
                        b.Velocity   = velocity;
                        b.TimeToLive = Bullet.constant_ttl;
                        if (id != 0)
                        {
                            b.Id = id;
                        }
                        b.isActive   = true;
                        
                        // We have fired the bullet
                        fired = true;

                        // Play a sound - (http://www.freesound.org/people/CGEffex/sounds/96692/)
                        bullet_fire.Play();
                    }
                }
            }
        }

        public Bullet FindBulletById(int id)
        {
            return bullets[id];
        }

        public void UpdateBulletById(int index, float timeToLive, Vector2 position, Vector2 velocity)
        {
            bullets[index].isActive   = true;
            bullets[index].TimeToLive = timeToLive;
            bullets[index].Position   = position;
            bullets[index].Velocity   = velocity;
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

        #region HUD

        public Vector2 ScoreRegion
        {
            get { return scoreRegion; }
            set { scoreRegion = value; }
        }

        #endregion

        #region Getters & Setters

        public bool IsSpawnProtectionActive
        {
            get { return spawnProtectionTime > 0f; }
        }

        public bool IsGameOver
        {
            get { return lives <= 0; }
            set { isGameOver = true; }
        }

        public bool IsThrusting
        {
            get { return isThrustEnabled; }
            set { isThrustEnabled = value; }
        }

        public Bullet[] Bullets
        {
            get { return bullets; }
        }

        public int NumActiveBullets
        {
            get
            {
                int count = 0;

                for (int i = 0; i < bullets.Length; i++)
                {
                    Bullet b = bullets[i];

                    if (b.isActive == true)
                    {
                        count++;
                    }
                }
                return count;
            }
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
