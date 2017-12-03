using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Assignment1
{
    /// <summary>
    /// Based off of Michael Hoffman's xna tutorial - https://gamedevelopment.tutsplus.com/series/cross-platform-vector-shooter-xna--gamedev-10559
    /// </summary>
    public static class Resources
    {
        // Textures that will be needed by the majority, so just load them in for all states
        public static Texture2D Bullet;
        public static Texture2D BulletEnemy;
        public static Texture2D Player;
        public static Texture2D Hunter;
        public static Texture2D Dart;
        public static Texture2D Wanderer;
        public static Texture2D Fighter;
        public static Texture2D Pickups;
        public static Texture2D Pointer;
        public static Texture2D Crosshair;
        public static Texture2D Boss;
        public static Texture2D Particle;
        // Fonts used
        public static SpriteFont FontMain;
        public static SpriteFont FontMainTitle;
        // Colours kept ordered for coherence
        public static Color ColorHighLight;
        public static Color ColorHud;
        public static Color ColorPause;
        public static Color ColorBg;
        public static Color ColorLose;
        // Important strings
        public static string Title;
        public static string SubTitle;
        // Needed Score for each level
        public static int levelOneNeeded = 5000;
        public static int levelTwoNeeded = 20000;
        public static int levelThreeNeeded = 30000;
        public static int levelFourNeeded = 40000;

        // H A C K
        public static int score = 0;
        public static float volume = 0.1f;
        public static bool cheatingFlag = false;
        public static int currPlayLevel = 0;

        /// <summary>
        /// 0 = Low, 1 = Medium, 2 = High
        /// </summary>
        public static int graphicsQuality = 1;

        public static void LoadContent(ContentManager content)
        {
            // Load Textures
            Bullet = content.Load<Texture2D>("Textures/BulletYellow");
            BulletEnemy = content.Load<Texture2D>("Textures/BulletEnemy");
            Player = content.Load<Texture2D>("Textures/Player");
            Pointer = content.Load<Texture2D>("Textures/Pointer0");
            Hunter = content.Load<Texture2D>("Textures/Hunter");
            Dart = content.Load<Texture2D>("Textures/Dart");
            Wanderer = content.Load<Texture2D>("Textures/Wanderer");
            Fighter = content.Load<Texture2D>("Textures/Fighter");
            Pickups = content.Load<Texture2D>("Textures/Powerups");
            Crosshair = content.Load<Texture2D>("Textures/Crosshair");
            FontMain = content.Load<SpriteFont>("Fonts/fontMain");
            FontMainTitle = content.Load<SpriteFont>("Fonts/fontMainTitle");
            Boss = content.Load<Texture2D>("Textures/Boss");
            Particle = content.Load<Texture2D>("Textures/Particle");

            // Load Colours
            ColorBg = new Color(13, 31, 39);
            ColorHighLight = new Color(255, 211, 288);
            ColorHud = new Color(255, 233, 94);
            ColorLose = new Color(255, 163, 147);
            ColorPause = new Color(Color.Black, 0.75f);

            // Load Titles
            Title = "PEWPEWDO MAN";
            SubTitle = "BE A COOL TRIANGLE";
        }
    }
}
