using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using RC_Framework;


namespace Assignment1
{
    /// <summary>
    /// Our player class
    /// Based off of Michael Hoffman's xna tutoral - https://gamedevelopment.tutsplus.com/series/cross-platform-vector-shooter-xna--gamedev-10559
    /// </summary>
    public class Player : SpriteActor
    {
        // Cooldown ticks for shooting
        private int coolDownTicks;
        private int coolDownRemaining = 0;

        // Cooldown ticks for power ups
        private int powerUpTicks;
        private int powerUpRemaining = 0;

        // Current player's power-up state
        private PowerupState pState;

        // Sprite Speed
        public float speed = 5.0f;
        private float powerUpSpeed = 10.0f;
        private float defaultSpeed = 5.0f;

        private Random rand = new Random();

        // Powerup state
        public enum PowerupState
        {
            None,
            Weapon,
            Speed
        }

        /// <summary>
        /// Basic player creation.
        /// </summary>
        /// <param name="tex">player texture</param>
        /// <param name="position">initial position</param>
        /// <param name="velocity">initial velocity</param>
        public Player(Texture2D tex, Vector2 position, Vector2 velocity) : base (tex, position, velocity)
        {
            // Note: Make the players hitbox smaller than the actual player
            coolDownTicks = 5;
            powerUpTicks = 330;
            powerUpRemaining = powerUpTicks;
            coolDownRemaining = 0;
            sprite.hitPoints = 5;
            sprite.maxHitPoints = 5;
        }

        /// <summary>
        /// Reset the player's data
        /// </summary>
        public void resetPlayer()
        {
            coolDownTicks = 5;
            powerUpTicks = 330;
            powerUpRemaining = powerUpTicks;
            coolDownRemaining = 0;
            sprite.hitPoints = 5;
            sprite.maxHitPoints = 5;
            position = Game1.screenSize / 2;
            velocity = Vector2.Zero;
        }

        // Handle all movement
        private void handleMovement()
        {
            if (pState == PowerupState.Speed)
                speed = powerUpSpeed;
            else
                speed = defaultSpeed;

            // Calculate our velocity
            velocity = speed * PlayerInput.GetDirection();
            position += velocity;
            // Easy way to ensure ship stays on screen, just clamp the values so we can't exit it
            position = Vector2.Clamp(position, spriteSize / 2, Game1.screenSize - spriteSize / 2);

            // Update our internal sprite position for collision detection
            sprite.setPos(position);

            // Get our aiming direction
            Vector2 aimDir = PlayerInput.getMouseDirection(position);

            // Get our orientation with an offset of 90 degrees (due to source image file)
            if (aimDir.LengthSquared() > 0)
                orientation = HelperUtils.ConvertToAngleAim(aimDir) + (float)(90 * (Math.PI / 180)); //1.5708f;

        }

        // Handle all firing
        private void handleFiring()
        {
            if (!PlayerInput.m1Pressed())
                return;

            Vector2 aimDir = PlayerInput.getMouseDirection(position);
            if (aimDir.LengthSquared() > 0 && coolDownRemaining <= 0)
            {
                // Reset our cooldown
                coolDownRemaining = coolDownTicks;
                // Get our aiming angle from our aiming direction
                float aimAngle = HelperUtils.ConvertToAngleAim(aimDir);
                // Create a quaternion for rotation to be used for spawning offset
                Quaternion aimQuat = Quaternion.CreateFromYawPitchRoll(0, 0, aimAngle);
                // Create a random spread
                float spread = HelperUtils.RandFloat(rand, -0.05f, 0.05f);
                // Convert values into our final vector2 velocity
                Vector2 vel = HelperUtils.FromPolar(aimAngle + spread, 11f);

                // Check which kind of firing we will do
                if (!(pState == PowerupState.Weapon))
                    fireSingle(vel, aimQuat);
                else
                    fireDouble(vel, aimQuat);
            }

            // If we haven't reached 0 reduce our cooldown
            if (coolDownRemaining > 0)
                coolDownRemaining--;
        }

        // Fire a single shot
        private void fireSingle(Vector2 velocity, Quaternion aimQuat)
        {
            // Get a random shot to play and randomize the pitch slightly
            SoundManager.getShot().Play(0.2f * Resources.volume, HelperUtils.RandFloat(rand, -0.2f, 0.2f), 0);

            // Calculate our offset and then create our bullet
            Vector2 offset = Vector2.Transform(new Vector2(spriteSize.Y, 0), aimQuat);
            Game1.spriteManager.addSpriteActor(new Bullet(Resources.Bullet, position + offset, velocity, this, false));

        }

        // Fire two shots
        private void fireDouble(Vector2 velocity, Quaternion aimQuat)
        {

            SoundManager.getShot().Play(0.2f * Resources.volume, HelperUtils.RandFloat(rand, -0.2f, 0.2f), 0);

            Vector2 offset = Vector2.Transform(new Vector2(25, -8), aimQuat);
            Game1.spriteManager.addSpriteActor(new Bullet(Resources.Bullet, position + offset, velocity, this, false));

            offset = Vector2.Transform(new Vector2(25, 8), aimQuat);
            Game1.spriteManager.addSpriteActor(new Bullet(Resources.Bullet, position + offset, velocity, this, false));
        }

        // Handle all powerups
        private void handlePowerUp()
        {
            if (pState != PowerupState.None)
            {
                // restart timedown
                if (powerUpRemaining <= 0)
                {
                    pState = PowerupState.None;
                    powerUpRemaining = powerUpTicks;
                    return;
                }

                if (powerUpRemaining > 0)
                {
                    powerUpRemaining--;
                }
            }
        }

        /// <summary>
        /// Apply damage to player
        /// </summary>
        /// <param name="damage">damage to be applied</param>
        public void PlayerHit(int damage)
        {
            sprite.hitPoints -= damage;

            if (sprite.hitPoints <= 0)
                Kill();
        }

        // Kill player
        public void Kill()
        {
            Game1.levelManager.setLevel(9);
        }

        // Consume powerup
        public void ConsumeHPPack(int life)
        {
            sprite.hitPoints += life;
        }

        // Consume powerup
        public void ConsumeWeaponPack()
        {
            pState = PowerupState.Weapon;
            powerUpRemaining = powerUpTicks;
        }

        // Consume powerup
        public void ConsumeSpeedPack(float speed)
        {
            powerUpSpeed = speed;
            pState = PowerupState.Speed;
            powerUpRemaining = powerUpTicks;
        }

        public override void Update()
        {
            handlePowerUp();
            handleMovement();
            handleFiring();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            sprite.customDraw(spriteBatch, position, color, orientation, spriteSize);
            if (PlayerInput.keyHeld(Keys.B))
                sprite.drawInfo(spriteBatch, Color.Red, Color.Green);
        }
    }
}
