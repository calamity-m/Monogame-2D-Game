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
    public class PickupSpawner
    {
        private Random rand = new Random();

        // Basic variables needed for spawning
        public float spawnChance = 1500;
        public float maxSpawnChance = 1000;
        public float maxSprites = 235;

        // What types of powerups should be spawned, in-efficent but simple
        public bool spawnHPPack;
        public bool spawnWeaponPack;
        public bool spawnSpeedPack;

        /// <summary>
        /// Create a pickup spawner of default spawn chances
        /// </summary>
        /// <param name="spawnHPPack">spawn hp packs?</param>
        /// <param name="spawnWeaponPack">spawn weapon packs?</param>
        /// <param name="spawnSpeedPack">spawn speed packs?</param>
        public PickupSpawner(bool spawnHPPack, bool spawnWeaponPack, bool spawnSpeedPack)
        {
            this.spawnHPPack = spawnHPPack;
            this.spawnWeaponPack = spawnWeaponPack;
            this.spawnSpeedPack = spawnSpeedPack;
        }

        /// <summary>
        /// Create a pickup spawner of custom spawn chances
        /// </summary>
        /// <param name="maxSpawnChance"></param>
        /// <param name="spawnChance"></param>
        /// <param name="spawnHPPack">spawn hp packs?</param>
        /// <param name="spawnWeaponPack">spawn weapon packs?</param>
        /// <param name="spawnSpeedPack">spawn speed packs?</param>
        public PickupSpawner(float maxSpawnChance, float spawnChance, bool spawnHPPack, bool spawnWeaponPack, bool spawnSpeedPack)
        {
            this.maxSpawnChance = maxSpawnChance;
            this.spawnChance = spawnChance;

            this.spawnHPPack = spawnHPPack;
            this.spawnWeaponPack = spawnWeaponPack;
            this.spawnSpeedPack = spawnSpeedPack;
        } 

        /// <summary>
        /// Reset spawn chance
        /// </summary>
        public void reset()
        {
            spawnChance = 60;
        }

        public void Update(int spriteCount)
        {
            if (spriteCount < maxSprites)
            {
                if (spawnHPPack)
                {
                    if (rand.Next((int)spawnChance) == 0)
                    {
                        // Spawn HP Pack
                        Game1.spriteManager.addSpriteActor(new Pickup(Resources.Pickups, getRandomSpawnPosition(), Vector2.Zero, PickUpType.HealthPack));
                    }
                }

                if (spawnWeaponPack)
                {
                    if (rand.Next((int)(spawnChance*1.5f)) == 0)
                    {
                        // Spawn Weapon Pack
                        Game1.spriteManager.addSpriteActor(new Pickup(Resources.Pickups, getRandomSpawnPosition(), Vector2.Zero, PickUpType.WeaponPowerUp));
                    }
                }

                if (spawnSpeedPack)
                {
                    if (rand.Next((int)(spawnChance*2f)) == 0)
                    {
                        // Spawn Speed Pack
                        Game1.spriteManager.addSpriteActor(new Pickup(Resources.Pickups, getRandomSpawnPosition(), Vector2.Zero, PickUpType.SpeedPowerUp));
                    }
                }
            }

            if (spawnChance > maxSpawnChance)
            {
                spawnChance -= 0.005f;
            }

        }

        // Get a random spawn position, taken from enemyspawner (same source)
        private Vector2 getRandomSpawnPosition()
        {
            Vector2 spawnPosition = Vector2.Zero;
            do
            {
                spawnPosition = new Vector2(rand.Next((int)Game1.screenSize.X), rand.Next((int)Game1.screenSize.Y));
            } while (Vector2.DistanceSquared(spawnPosition, Game1.spriteManager.player.position) < 250 * 250);

            return spawnPosition;
        }
    }
}
