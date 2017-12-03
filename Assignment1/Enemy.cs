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
    /// Based off of Michael Hoffman's xna tutorial - https://gamedevelopment.tutsplus.com/series/cross-platform-vector-shooter-xna--gamedev-10559
    /// </summary>
    public class Enemy : SpriteActor
    {
        // Fade in frames
        protected int fadeTicks = 60;

        // Speed/Custom movement floats
        protected float friction = 0.8f;
        protected const float defaultSpeed = 1f;

        // Cooldown ticks (Mostly used for shooting)
        protected int coolDownTicks = 6;
        protected int coolDownRemaining = 0;

        // Damage and Score Rewards
        public int scoreAwarded = 100;
        public int damage = 1;

        // Behaviours
        private Random rand = new Random();
        private List<IEnumerator<int>> behaviours = new List<IEnumerator<int>>();

        public Enemy(Texture2D tex, Vector2 position, Vector2 velocity) : base(tex, position, velocity)
        {
            sprite.setColor(new Color(Color.White, 0.0f));
        }

        public override void Update()
        {
            // Fade our enemy in in terms of colour and applying behaviours/AI
            if (fadeTicks <= 0)
            {
                updateBehaviours();
            }
            else
            {
                fadeTicks--;

                sprite.setColor(Color.White * (1 - fadeTicks / 60f));
            }

            position += velocity;
            // Easy way to ensure ship stays on screen, just clamp the values so we can't exit it
            position = Vector2.Clamp(position, spriteSize / 2, Game1.screenSize - spriteSize / 2);

            // Update our internal sprite position for collision detection
            sprite.setPos(position);

            velocity *= friction;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            sprite.customDraw(spriteBatch, position, sprite.getColor(), orientation, spriteSize);
            if (PlayerInput.keyHeld(Keys.B))
                sprite.drawInfo(spriteBatch, Color.Red, Color.Green);
        }

        /// <summary>
        /// Destroy enemy and add score if wanted
        /// </summary>
        /// <param name="addPoints">score to be added</param>
        public virtual void Destroy(bool addPoints)
        {
            if (!isFinished)
            {
                SoundManager.getExplosion().Play(0.05f * Resources.volume, HelperUtils.RandFloat(rand, -0.2f, 0.2f), 0);
                if (addPoints) Resources.score += scoreAwarded;
                // Create explosion
                if (addPoints) Game1.particleManager.CreateExplosionEnemy(Resources.Particle, 500, 150, position);
            }
            isFinished = true;
        }

        // Push this instance away from colliding enemy
        public void handleCollision(Enemy other)
        {
            Vector2 diff = position - other.position;
            velocity += 10 * (diff / (diff.LengthSquared() + 1));
        }

        // Basic 'factory' create for hunters
        public static Enemy CreateHunter(Vector2 pos)
        {
            Enemy hunter = new Enemy(Resources.Hunter, pos, Vector2.Zero);
            hunter.addBehaviour(hunter.huntPlayer());

            return hunter;
        }

        // Basic 'factory' create for darts
        public static Enemy CreateDart(Vector2 pos)
        {
            Enemy dart = new Enemy(Resources.Dart, pos, Vector2.Zero);
            dart.fadeTicks = 65;
            dart.addBehaviour(dart.huntPlayer(1.5f));
            dart.scoreAwarded = 115;

            return dart;
        }

        // Basic 'factory' create for wanderers
        public static Enemy CreateWanderer(Vector2 pos)
        {
            Enemy wanderer = new Enemy(Resources.Wanderer, pos, Vector2.Zero);
            wanderer.addBehaviour(wanderer.wander());
            wanderer.scoreAwarded = 105;

            return wanderer;
        }

        // Basic 'factory' create for fighters
        public static Enemy CreateFighter(Vector2 pos)
        {
            Enemy fighter = new Enemy(Resources.Fighter, pos, Vector2.Zero);
            fighter.addBehaviour(fighter.fighter());
            fighter.scoreAwarded = 200;

            return fighter;
        }

        /// <summary>
        /// Uses basic hunter algorithm to chase after player
        /// </summary>
        /// <param name="speed">chasing speed</param>
        /// <returns></returns>
        IEnumerable<int> huntPlayer(float speed = defaultSpeed)
        {
            while (true)
            {
                Vector2 dir = Game1.spriteManager.player.position - position;
                if (dir.LengthSquared() > 1)
                    dir.Normalize(); // get our dir, result is a vector with length of 1
                velocity += dir * speed;
                if (velocity != Vector2.Zero)
                {
                    // Get out orientation
                    orientation = HelperUtils.ConvertToAngleAim(velocity);
                }

                yield return 0;
            }
        }

        /// <summary>
        /// Randomly wander around stage
        /// </summary>
        /// <returns></returns>
        IEnumerable<int> wander()
        {
            // Mostly taken from Michael Hoffman's xna tutorial
            float direction = HelperUtils.RandFloat(rand, 0, MathHelper.TwoPi);

            while (true)
            {
                direction += HelperUtils.RandFloat(rand, -0.1f, 0.1f);
                direction = MathHelper.WrapAngle(direction);

                for (int i = 0; i < 6; i++)
                {
                    velocity += HelperUtils.FromPolar(direction, 0.4f);
                    orientation -= 0.05f;

                    Rectangle bound = new Rectangle(0, 0, (int)(Game1.screenSize.X - spriteSize.X), (int)(Game1.screenSize.Y - spriteSize.Y));

                    // If we're outside of the bounds
                    if (!bound.Contains(position.X, position.Y))
                        direction = HelperUtils.ConvertToAngleAim(Game1.screenSize / 2 - position) + HelperUtils.RandFloat(rand, -MathHelper.PiOver2, MathHelper.PiOver2);

                    yield return 0;
                }
            }
        }

        /// <summary>
        /// Fire at player position
        /// </summary>
        /// <returns></returns>
        IEnumerable<int> fighter()
        {
            
            while (true)
            {

                // Work with our cooldown
                if (coolDownRemaining <= 0)
                {
                    coolDownRemaining = coolDownTicks;
                    float aimAngle = HelperUtils.ConvertToAngleAim(Game1.spriteManager.player.position - position);
                    Quaternion aimQuat = Quaternion.CreateFromYawPitchRoll(0, 0, aimAngle);
                    float spread = HelperUtils.RandFloat(rand, -0.05f, 0.05f);
                    Vector2 bulletVelocity = HelperUtils.FromPolar(aimAngle + spread, 11f) * 0.5f;
    
                    fireEnemyBullet(bulletVelocity, aimQuat);
                }

                if (coolDownRemaining > 0)
                    coolDownRemaining--;

                yield return 0;
            }
        }

        // Fire a bullet
        private void fireEnemyBullet(Vector2 vel, Quaternion aimQuat)
        {
            // Enemy fire sound

            // Enemy bullet creation
            Vector2 offset = Vector2.Transform(new Vector2(spriteSize.Y, 0), aimQuat);
            Game1.spriteManager.addSpriteActor(new Bullet(Resources.BulletEnemy, position + offset, vel, this, true));
        }

        protected void addBehaviour(IEnumerable<int> behaviour)
        {
            behaviours.Add(behaviour.GetEnumerator());
        }

        protected void removeBehaviour(IEnumerable<int> behaviour)
        {
            behaviours.Remove(behaviour.GetEnumerator());
        }

        protected void updateBehaviours()
        {
            for (int i = 0; i < behaviours.Count; i++)
            {
                if (!behaviours[i].MoveNext())
                {
                    behaviours.RemoveAt(i--);
                }
            }
        }
        
        
    }
}
