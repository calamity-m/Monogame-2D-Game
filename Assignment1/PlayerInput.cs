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
    /// Based off of Michael Hoffman's xna tutorial - https://gamedevelopment.tutsplus.com/series/cross-platform-vector-shooter-xna--gamedev-10559
    /// </summary>
    public static class PlayerInput
    {
        private static KeyboardState kbState;
        private static KeyboardState prevkbState;
        private static MouseState mState;
        private static MouseState prevmState;

        /// <summary>
        /// Get the current mouse position
        /// </summary>
        public static Vector2 mousePosition
        {
            get { return new Vector2(mState.X, mState.Y); }
        }

        public static void Update()
        {
            prevkbState = kbState;
            prevmState = mState;

            kbState = Keyboard.GetState();
            mState = Mouse.GetState();
        }

        /// <summary>
        /// Check if key k is being pressed
        /// </summary>
        /// <param name="k"></param>
        /// <returns></returns>
        public static bool KeyPressed(Keys k)
        {
            return (kbState.IsKeyDown(k) && prevkbState.IsKeyUp(k));
        }

        /// <summary>
        /// Check if key k was pressed
        /// </summary>
        /// <param name="k"></param>
        /// <returns></returns>
        public static bool KeyOldPressed(Keys k)
        {
            return (kbState.IsKeyUp(k) && prevkbState.IsKeyDown(k));
        }

        /// <summary>
        /// Check if key k is being held
        /// </summary>
        /// <param name="k"></param>
        /// <returns></returns>
        public static bool keyHeld(Keys k)
        {
            return (kbState.IsKeyDown(k));
        }

        /// <summary>
        /// Check if mouse button 1 (left button) is pressed
        /// </summary>
        /// <returns></returns>
        public static bool m1Pressed()
        {   
            if (mState.LeftButton == ButtonState.Pressed)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Check if mouse button 1 (left button) was pressed
        /// </summary>
        /// <returns></returns>
        public static bool m1OldPressed()
        {
            if (prevmState.LeftButton == ButtonState.Pressed && mState.LeftButton == ButtonState.Released)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Check if any mouse button was pressed
        /// </summary>
        /// <returns></returns>
        public static bool anyOldMousePressed()
        {
            if (mState.LeftButton == ButtonState.Released && prevmState.LeftButton == ButtonState.Pressed)
                return true;

            if (mState.MiddleButton == ButtonState.Released && prevmState.MiddleButton == ButtonState.Pressed)
                return true;

            if (mState.RightButton == ButtonState.Released && prevmState.RightButton == ButtonState.Pressed)
                return true;

            return false;
        }

        /// <summary>
        /// Check if any keys being pressed
        /// </summary>
        /// <returns></returns>
        public static bool anyPressed()
        {
            if (kbState.GetPressedKeys().Length > 0)
                return true;
            else 
                return false;
        }

        /// <summary>
        /// Get movement direction in form of vector2, e.g.
        /// W responds to -1 for y and S responds to +1 for y
        /// </summary>
        /// <returns></returns>
        public static Vector2 GetDirection()
        {
            Vector2 dir = Vector2.Zero;
            dir.Y *= -1;

            if (kbState.IsKeyDown(Keys.W))
                dir.Y -= 1;
            if (kbState.IsKeyDown(Keys.S))
                dir.Y += 1;
            if (kbState.IsKeyDown(Keys.A))
                dir.X -= 1;
            if (kbState.IsKeyDown(Keys.D))
                dir.X += 1;

            // Clamp our dir vector
            if (dir.LengthSquared() > 1)
            {
                dir.Normalize();
            }

            return dir;
        }

        /// <summary>
        /// Get the mouse direction relative to the given vector
        /// </summary>
        /// <param name="relative"></param>
        /// <returns></returns>
        public static Vector2 getMouseDirection(Vector2 relative)
        {
            Vector2 dir = mousePosition - relative;

            if (dir == Vector2.Zero)
                return Vector2.Zero;
            else
                return Vector2.Normalize(dir);
        }
    }
}
