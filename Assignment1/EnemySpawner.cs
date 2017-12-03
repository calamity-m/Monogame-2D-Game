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
    public class EnemySpawner
    {
        private Random rand = new Random();

        /// <summary>
        /// Lower = faster spawn rate
        /// </summary>
        public float spawnChance = 60;
        public float maxSpawnChance = 20;
        public float maxSprites = 235;

        // Unscalable but workable/efficient for small sized games
        public bool spawnHunter;
        public bool spawnWanderer = false;
        public bool spawnFighter = false;
        public bool spawnDart = false;

        /// <summary>
        /// Create enemy spawner with default spawn chances
        /// </summary>
        /// <param name="spawnHunter"></param>
        /// <param name="spawnWanderer"></param>
        /// <param name="spawnFighter"></param>
        /// <param name="spawnDart"></param>
        public EnemySpawner(bool spawnHunter, bool spawnWanderer, bool spawnFighter, bool spawnDart)
        {
            this.spawnHunter = spawnHunter;
            this.spawnWanderer = spawnWanderer;
            this.spawnFighter = spawnFighter;
            this.spawnDart = spawnDart;
        }

        /// <summary>
        /// Create enemy spawner with custom spawn chances
        /// </summary>
        /// <param name="maxSpawnChance"></param>
        /// <param name="spawnChance"></param>
        /// <param name="spawnHunter"></param>
        /// <param name="spawnWanderer"></param>
        /// <param name="spawnFighter"></param>
        /// <param name="spawnDart"></param>
        public EnemySpawner(float maxSpawnChance, float spawnChance, bool spawnHunter, bool spawnWanderer, bool spawnFighter, bool spawnDart)
        {
            this.maxSpawnChance = maxSpawnChance;
            this.spawnChance = spawnChance;

            this.spawnHunter = spawnHunter;
            this.spawnWanderer = spawnWanderer;
            this.spawnFighter = spawnFighter;
            this.spawnDart = spawnDart;
        }

        public void Update(int spriteCount)
        {

            // GOOD IDEA FOR DIFFICULTY:
            // GIVE ENEMIES 2HPS, WHEN THEY GET HIT MAKE THEM SUPER RED AND/OR BLUE OR A DIFFERENT COLOUR, THEN DELETE THEM AFTER NEXT HIT

            // Add check to see if player alive
            if (spriteCount < maxSprites)
            {
                // If enabled, spawn hunter
                if (spawnHunter)
                {
                    // Create Hunter
                    if (rand.Next((int)spawnChance) == 0)
                    {
                        // Play spawn sound
                        SoundManager.getEnemySpawn().Play(0.2f * Resources.volume, HelperUtils.RandFloat(rand, -0.2f, 0.2f), 0);
                        Game1.spriteManager.addSpriteActor(Enemy.CreateHunter(getRandomSpawnPosition()));
                    }
                }

                // If enabled, spawn dart
                if (spawnDart)
                {
                    float dartChance = spawnChance * 1f;
                    if (rand.Next((int)dartChance) == 0)
                    {
                        SoundManager.getEnemySpawn().Play(0.2f * Resources.volume, HelperUtils.RandFloat(rand, -0.2f, 0.2f), 0);
                        Game1.spriteManager.addSpriteActor(Enemy.CreateDart(getRandomSpawnPosition()));
                    }
                }

                // if enabled, spawn wanderer
                if (spawnWanderer)
                {
                    if (rand.Next((int)spawnChance) == 0)
                    {
                        SoundManager.getEnemySpawn().Play(0.2f * Resources.volume, HelperUtils.RandFloat(rand, -0.2f, 0.2f), 0);
                        Game1.spriteManager.addSpriteActor(Enemy.CreateWanderer(getRandomSpawnPosition()));
                    }
                }

                // if enabled, spawn fighter
                if (spawnFighter)
                {
                    float fighterChance = spawnChance;
                    if (rand.Next((int)fighterChance) == 0)
                    {
                        SoundManager.getEnemySpawn().Play(0.2f * Resources.volume, HelperUtils.RandFloat(rand, -0.2f, 0.2f), 0);
                        Game1.spriteManager.addSpriteActor(Enemy.CreateFighter(getRandomSpawnPosition()));
                    }
                }
            }

            // Increase our spawn chance (lower spawnchance = higher chance of spawning)
            if (spawnChance > maxSpawnChance)
            {
                spawnChance -= 0.005f;
            }
        }

        /// <summary>
        /// Reset spawn chance
        /// </summary>
        public void reset()
        {
            spawnChance = 60;
        }

        // Get random position, taken from Michael Hoffman's xna tutorial, repeats a loop until finding a valid spawn position outside of 250x and 250y pixels of player
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
