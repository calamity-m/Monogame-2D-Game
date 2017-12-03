using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using RC_Framework;

namespace Assignment1
{
    public class PlayerGUI
    {
        private string requiredGoal = "";
        private float xMid = Game1.screenSize.X / 2;
        private float yMid = Game1.screenSize.Y / 2;
        private float yDisplay = 50;
        private float xDisplay = 25;
        private Color ColorGUI;

        public PlayerGUI(string requiredGoal)
        {
            this.requiredGoal = requiredGoal;
            ColorGUI = Resources.ColorHud;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Score
            HelperUtils.DrawStringLeft(spriteBatch, new Vector2(xDisplay, yDisplay), Resources.FontMain, 0.5f, "Score:", ColorGUI);
            HelperUtils.DrawStringLeft(spriteBatch, new Vector2(xDisplay, yDisplay + 25), Resources.FontMain, 0.5f, Resources.score.ToString(), ColorGUI);

            // Required
            HelperUtils.DrawStringRight(spriteBatch, new Vector2(Game1.screenSize.X - xDisplay, yDisplay), Resources.FontMain, 0.5f, "Required:", ColorGUI);
            HelperUtils.DrawStringRight(spriteBatch, new Vector2(Game1.screenSize.X - xDisplay, yDisplay + 25), Resources.FontMain, 0.5f, requiredGoal, ColorGUI);

            // Lives
            HelperUtils.DrawStringCentered(spriteBatch, new Vector2(xMid, yDisplay), Resources.FontMain, 0.5f, "Lives:", ColorGUI);
            if (Game1.spriteManager.player.sprite.hitPoints > 1)
                HelperUtils.DrawStringCentered(spriteBatch, new Vector2(xMid, yDisplay + 25), Resources.FontMain, 0.5f, Game1.spriteManager.player.sprite.hitPoints.ToString(), ColorGUI);
            else
                HelperUtils.DrawStringCentered(spriteBatch, new Vector2(xMid, yDisplay + 35), Resources.FontMain, 1f, Game1.spriteManager.player.sprite.hitPoints.ToString(), Resources.ColorLose);

            // Level
            HelperUtils.DrawStringCentered(spriteBatch, new Vector2(xMid, Game1.screenSize.Y - 45), Resources.FontMain, 0.5f, Levels.getLevelName(), ColorGUI);

            // Cheating
            if (Resources.cheatingFlag)
                HelperUtils.DrawStringCentered(spriteBatch, new Vector2(xMid, yMid), Resources.FontMain, 0.5f, "Cheating Enabled", ColorGUI);

            // Boss info if Applicable
            if (Resources.currPlayLevel == 11 && Game1.spriteManager.boss != null)
                HelperUtils.DrawStringCentered(spriteBatch, new Vector2(xMid, Game1.screenSize.Y - 85), Resources.FontMain, 0.5f, 
                    "Boss Hp" + Game1.spriteManager.boss.health, Resources.ColorLose);
        }
    }
}
