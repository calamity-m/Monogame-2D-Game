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
    /// Slightly in-efficient boss class, uses two forms of input (Sprite3 and custom) and only works for bosses
    /// that follow a certain mold, but as we only have one boss, this class extension will suffice and is more than
    /// enough for the needs of the game.
    /// </summary>
    public class Boss : Enemy
    {
        // Boss State
        public BossState bState;
        // Boss Health
        public int health = 300;
        // Boss WaypointList
        private WayPointList wl;
        private Random rand = new Random();
        // Bool check on whether phase3 has started or not
        private bool phase3Initiated = false;
        /// <summary>
        /// 3 Phase boss state including cutscene and finished states
        /// </summary>
        public enum BossState
        {
            Cutscene,
            Phase1,
            Phase2,
            Phase3,
            Finished
        }

        /// <summary>
        /// Create a basic boss
        /// </summary>
        /// <param name="tex">Boss textures</param>
        /// <param name="position">inital position</param>
        /// <param name="velocity">inital velocity</param>
        public Boss(Texture2D tex, Vector2 position, Vector2 velocity) : base(tex, position, velocity)
        {
            // Initialize our boss
            bState = BossState.Phase1;
            addBehaviour(BossShoot());
            addBehaviour(BossMove());

            // Initialize our waypoint list used for phase 2
            wl = new WayPointList();

            // Make waypoint list
            wl.makePathCircle(Game1.screenSize / 2, 225, -Util.degToRad(90), Util.degToRad(20), 18, 2, 1);
            sprite.wayList = wl;
        }

        public override void Update()
        {
            // Minor state management, in-efficent way to do so
            if (health >= 200)
                bState = BossState.Phase1;
            else if (health >= 100)
                bState = BossState.Phase2;
            else if (health >= 1)
                bState = BossState.Phase3;
            else
                bState = BossState.Finished;

            // Fade-in for boss behaviours/ai
            if (fadeTicks <= 0)
                updateBehaviours();
            else
            {
                fadeTicks--;
                sprite.setColor(Color.White * (1 - fadeTicks / 60f));
            }

            // Get out orientation with a 90 degree offset (due to source image files)
            orientation = HelperUtils.ConvertToAngleAim(Game1.spriteManager.player.position - position) + (float)(90 * (Math.PI / 180));

            position += velocity;
            // Easy way to ensure enemy stays on screen, just clamp the values so we can't exit it
            position = Vector2.Clamp(position, spriteSize / 2, Game1.screenSize - spriteSize / 2);

            // Update our internal sprite position for collision detection
            if (bState == BossState.Phase1 || bState == BossState.Phase3)
                sprite.setPos(position);

            velocity *= friction;
        }

        /// <summary>
        /// Damage boss and create explosion if less than 0 hp
        /// </summary>
        /// <param name="addPoints"></param>
        public override void Destroy(bool addPoints)
        {
            health -= 1;
            if (health < 0)
            {
                health = 0;
                Game1.particleManager.CreateExplosionBoss(Resources.Particle, 500, 200, position);
                SoundManager.getExplosion().Play(0.2f * Resources.volume, HelperUtils.RandFloat(rand, -0.2f, 0.2f), 0);
            }
        }

        /// <summary>
        /// Kill boss
        /// </summary>
        /// <param name="addPoints">should points be added</param>
        public void Kill(bool addPoints)
        {
            if (!isFinished && addPoints)
                Resources.score += 15000;

            isFinished = true;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (bState == BossState.Phase2)
                sprite.customDraw(spriteBatch, sprite.getPos(), sprite.getColor(), orientation, spriteSize);
            else
                sprite.customDraw(spriteBatch, position, sprite.getColor(), orientation, spriteSize);

            if (PlayerInput.keyHeld(Keys.B))
            {
                sprite.drawInfo(spriteBatch, Color.Red, Color.Green);
                if (bState == BossState.Phase2)
                    wl.Draw(spriteBatch, Color.Red, Color.White);
            }
        }

        /// <summary>
        /// Movement for boss phases, phase1 = no movement, phase2 = waypoint movement, phase3 = hunt movement
        /// </summary>
        /// <returns></returns>
        IEnumerable<int> BossMove()
        {
            while (true)
            {
                switch (bState)
                {
                    case BossState.Phase1:
                        // If we're in phase 1, don't move
                        break;
                    case BossState.Phase2:
                        // If we're in phase 2, move from waypoint to waypoint
                        sprite.moveWayPointList(false);
                        break;
                    case BossState.Phase3:
                        // If we're in phase 3, start hunting player
                        if (!phase3Initiated)
                        {
                            position = sprite.getPos();
                            phase3Initiated = true;
                        }

                        hunt(0.5f);
                        break;
                }
                yield return 0;
            }
        }

        /// <summary>
        /// Shooting for boss phases, phase1 = normal shooting, phase2/3 = hyper shooting
        /// </summary>
        /// <returns></returns>
        IEnumerable<int> BossShoot()
        {
            while (true)
            {

                // Work with our cooldown
                if (coolDownRemaining <= 0)
                {
                    coolDownRemaining = coolDownTicks;
                    float aimAngle = 0;
                    if (bState == BossState.Phase2)
                        aimAngle = HelperUtils.ConvertToAngleAim(Game1.spriteManager.player.position - sprite.getPos());
                    else
                        aimAngle = HelperUtils.ConvertToAngleAim(Game1.spriteManager.player.position - position);

                    Quaternion aimQuat = Quaternion.CreateFromYawPitchRoll(0, 0, aimAngle);
                    float spread = HelperUtils.RandFloat(rand, -0.05f, 0.05f);
                    Vector2 bulletVelocity = HelperUtils.FromPolar(aimAngle + spread, 11f) * 0.5f;

                    switch (bState) {
                        case BossState.Phase1:
                            fireBullet(bulletVelocity, aimQuat);
                            break;
                        case BossState.Phase2:
                            fireBulletP2(bulletVelocity, aimQuat);
                            break;
                        case BossState.Phase3:
                            fireBulletP2(bulletVelocity, aimQuat);
                            break;
                    }
                }

                if (coolDownRemaining > 0)
                    coolDownRemaining--;
                yield return 0;
            }
        }

        /// <summary>
        /// Hunt the player
        /// </summary>
        /// <param name="speed"></param>
        private void hunt(float speed)
        {
            Vector2 dir = Game1.spriteManager.player.position - position;
            if (dir.LengthSquared() > 1)
                dir.Normalize(); // get our dir, result is a vector with l of 1
            velocity += dir * speed;
        }

        /// <summary>
        /// Fire a bullet for Phase 1
        /// </summary>
        /// <param name="vel"></param>
        /// <param name="aimQuat"></param>
        private void fireBullet(Vector2 vel, Quaternion aimQuat)
        {
            // Enemy bullet creation
            createBullet(Vector2.Transform(new Vector2(spriteSize.Y - 50, -10), aimQuat), vel);
            createBullet(Vector2.Transform(new Vector2(spriteSize.Y - 50, 10), aimQuat), vel);
        }

        /// <summary>
        /// Fire a bullet for phase 2 and Phase 3
        /// </summary>
        /// <param name="vel"></param>
        /// <param name="aimQuat"></param>
        private void fireBulletP2(Vector2 vel, Quaternion aimQuat)
        {
            // Enemy bullet creation
            createBullet(Vector2.Transform(new Vector2(spriteSize.Y - 50, -10), aimQuat), vel);
            createBullet(Vector2.Transform(new Vector2(spriteSize.Y - 50, 10), aimQuat), vel);

            // Second enemy bullet creation
            createBullet(Vector2.Transform(new Vector2(0, 55), aimQuat), vel);
            createBullet(Vector2.Transform(new Vector2(0, -55), aimQuat), vel);
        }

        /// <summary>
        /// Actually create bullet
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="vel"></param>
        private void createBullet(Vector2 offset, Vector2 vel)
        {
            Game1.spriteManager.addSpriteActor(new Bullet(Resources.BulletEnemy, sprite.getPos() + offset, vel, this, true));
        }
    }
}
