using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Assignment1
{
    /// <summary>
    /// Basic class filled with helpful methods and functions/utilities
    /// </summary>
    public static class HelperUtils
    {
        /// <summary>
        /// Taken from Michael Hoffman's xna tutorial - https://gamedevelopment.tutsplus.com/series/cross-platform-vector-shooter-xna--gamedev-10559
        /// Converts a vector 2 to a float orientation/rotation angle
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static float ConvertToAngleAim(Vector2 v)
        {
            return (float)Math.Atan2(v.Y, v.X);
        }

        /// <summary>
        /// Based off of Michael Hoffman's xna tutorial - https://gamedevelopment.tutsplus.com/series/cross-platform-vector-shooter-xna--gamedev-10559
        /// returns a random float between min and max
        /// </summary>
        /// <param name="rand"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static float RandFloat(Random rand, float min, float max)
        {
            return (float)rand.NextDouble() * (max - min) + min;
        }

        /// <summary>
        /// Taken from Michael Hoffman's xna tutorial - https://gamedevelopment.tutsplus.com/series/cross-platform-vector-shooter-xna--gamedev-10559
        /// returns a vector 2 created from a given angle and magnitude/length
        /// </summary>
        /// <param name="angle"></param>
        /// <param name="magnitude"></param>
        /// <returns></returns>
        public static Vector2 FromPolar(float angle, float magnitude)
        {
            return magnitude * new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
        }

        /// <summary>
        /// Taken from Michael Hoffman's xna tutorial - https://gamedevelopment.tutsplus.com/series/cross-platform-vector-shooter-xna--gamedev-10559
        /// return a color from hsv values
        /// </summary>
        /// <param name="h"></param>
        /// <param name="s"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Color HSVToColor(float h, float s, float v)
        {
            if (h == 0 && s == 0)
                return new Color(v, v, v);

            float c = s * v;
            float x = c * (1 - Math.Abs(h % 2 - 1));
            float m = v - c;

            if (h < 1) return new Color(c + m, x + m, m);
            else if (h < 2) return new Color(x + m, c + m, m);
            else if (h < 3) return new Color(m, c + m, x + m);
            else if (h < 4) return new Color(m, x + m, c + m);
            else if (h < 5) return new Color(x + m, m, c + m);
            else return new Color(c + m, m, x + m);
        }

        /// <summary>
        /// Draw a string with centre as origin
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="position"></param>
        /// <param name="font"></param>
        /// <param name="scale"></param>
        /// <param name="display"></param>
        /// <param name="color"></param>
        public static void DrawStringCentered(SpriteBatch spriteBatch, Vector2 position, SpriteFont font, float scale, string display, Color color)
        {
            Vector2 origin = font.MeasureString(display) / 2;
            spriteBatch.DrawString(font, display, position, color, 0, origin, scale, SpriteEffects.None, 0);
        }

        /// <summary>
        /// Draw a string with left middle as origin
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="position"></param>
        /// <param name="font"></param>
        /// <param name="scale"></param>
        /// <param name="display"></param>
        /// <param name="color"></param>
        public static void DrawStringLeft(SpriteBatch spriteBatch, Vector2 position, SpriteFont font, float scale, string display, Color color)
        {
            Vector2 origin = new Vector2(0, font.MeasureString(display).Y / 2);
            spriteBatch.DrawString(font, display, position, color, 0, origin, scale, SpriteEffects.None, 0);
        }

        /// <summary>
        /// Draw a string with right middle as origin
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="position"></param>
        /// <param name="font"></param>
        /// <param name="scale"></param>
        /// <param name="display"></param>
        /// <param name="color"></param>
        public static void DrawStringRight(SpriteBatch spriteBatch, Vector2 position, SpriteFont font, float scale, string display, Color color)
        {
            Vector2 origin = new Vector2(font.MeasureString(display).X, font.MeasureString(display).Y / 2);
            spriteBatch.DrawString(font, display, position, color, 0, origin, scale, SpriteEffects.None, 0);
        }

        /// <summary>
        /// Draw a normal string to screen
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="position"></param>
        /// <param name="font"></param>
        /// <param name="scale"></param>
        /// <param name="display"></param>
        /// <param name="color"></param>
        public static void DrawString(SpriteBatch spriteBatch, Vector2 position, SpriteFont font, float scale, string display, Color color)
        {
            spriteBatch.DrawString(font, display, position, color, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
        }

        /// <summary>
        /// Clamp a float between min and max, created due to not knowing MathHelper.Clamp existed
        /// </summary>
        /// <param name="val"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static float Clamp(float val, float min, float max)
        {
            // Didn't see MathHelper.Clamp(val, min, max) before

            if (val < min)
                return min;
            else if (val > max)
                return max;
            else
                return val;

        }
    }
}
