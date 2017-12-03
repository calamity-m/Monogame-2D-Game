using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

using RC_Framework;
using Microsoft.Xna.Framework.Media;

namespace Assignment1
{
    /// <summary>
    /// Based off of Michael Hoffman's xna tutorial - https://gamedevelopment.tutsplus.com/series/cross-platform-vector-shooter-xna--gamedev-10559
    /// </summary>
    public static class SoundManager
    {
        private static Song music;
        private static Random rand = new Random();
        private static List<SoundEffect> sfShots = new List<SoundEffect>();
        private static List<SoundEffect> sfExplosions = new List<SoundEffect>();
        private static List<SoundEffect> sfEnemySpawn = new List<SoundEffect>();
        private static List<SoundEffect> sfMenuSelect = new List<SoundEffect>();

        public static void LoadContent(ContentManager content)
        {
            string dirBase = "Sounds/Shoot";
            string dir = "";

            music = content.Load<Song>("Sounds/GameMusic");

            for (int i = 0; i < 3; i++)
            {
                dir = dirBase + i;
                SoundEffect sf = content.Load<SoundEffect>(dir);
                sfShots.Add(sf);
            }

            dirBase = "Sounds/Explosion";
            for (int i = 0; i < 3; i++)
            {
                dir = dirBase + i;
                SoundEffect sf = content.Load<SoundEffect>(dir);
                sfExplosions.Add(sf);
            }

            dirBase = "Sounds/Spawn";
            for (int i = 0; i < 3; i++)
            {
                dir = dirBase + i;
                SoundEffect sf = content.Load<SoundEffect>(dir);
                sfEnemySpawn.Add(sf);
            }

            dirBase = "Sounds/MenuSelect";
            for (int i = 0; i < 3; i++)
            {
                dir = dirBase + i;
                SoundEffect sf = content.Load<SoundEffect>(dir);
                sfMenuSelect.Add(sf);
            }

        }

        /// <summary>
        /// Returns a random shot sound effect
        /// </summary>
        /// <returns></returns>
        public static SoundEffect getShot()
        {
            return sfShots[rand.Next(sfShots.Count)];
        }

        /// <summary>
        /// Returns a random explosion sound effect
        /// </summary>
        /// <returns></returns>
        public static SoundEffect getExplosion()
        {
            return sfExplosions[rand.Next(sfExplosions.Count)];
        }

        /// <summary>
        /// Returns a random enemy spawning sound effect
        /// </summary>
        /// <returns></returns>
        public static SoundEffect getEnemySpawn()
        {
            return sfEnemySpawn[rand.Next(sfEnemySpawn.Count)];
        }

        /// <summary>
        /// Returns a random menu select sound effect
        /// </summary>
        /// <returns></returns>
        public static SoundEffect getMenuSelect()
        {
            return sfMenuSelect[rand.Next(sfMenuSelect.Count)];
        }

        /// <summary>
        /// Returns the main music song
        /// </summary>
        /// <returns></returns>
        public static Song getSong()
        {
            return music;
        }


    }

}
