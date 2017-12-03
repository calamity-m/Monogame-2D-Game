using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;



namespace RC_Framework
{
//  *********************************************************  RC_GameStateParent  *************************************************************
    /// <summary>
    /// Parent Stagre class all levels inherit from this
    /// </summary>
    public abstract class RC_GameStateParent
    {
        // the following block of variables are candidates to become global variables used in many places 
        public static GraphicsDevice graphicsDevice;
        public static SpriteBatch spriteBatch;
        
        public static ContentManager Content;
        public static RC_GameStateManager gameStateManager;

        public static KeyboardState keyState; // for convenience - not really needed
        public static KeyboardState prevKeyState; // for convenience not really needed

        public static float mouse_x = 0;
        public static float mouse_y = 0; // for convenience not really needed
        public static MouseState currentMouseState; // for convenience not really needed
        public static MouseState previousMouseState; // for convenience not really needed

        public static SpriteFont font1; // use if you want again not really needed

        public virtual void InitializeLevel(GraphicsDevice g, SpriteBatch s, ContentManager c, RC_GameStateManager lm)
        {
            graphicsDevice = g;
            spriteBatch = s;
            Content = c;
            gameStateManager = lm;
        }

        public virtual void LoadContent() { }
        public virtual void UnloadContent() { }
        public virtual void EnterLevel(int fromLevelNum) { } // runs on set and push
        public virtual void ExitLevel() { } // runs on set and pop
        public virtual void Update(GameTime gameTime) { }
        public abstract void Draw(GameTime gameTime);

        /// <summary>
        /// Utility routine to set up keyboard and mouse
        /// </summary>
        public static void getKeyboardAndMouse()
        {

            prevKeyState = keyState;
            keyState = Keyboard.GetState();

            previousMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();

            mouse_x = currentMouseState.X;
            mouse_y = currentMouseState.Y;
        }

    }

    // ***************************************************************************  RC_GameStateManager  ****************************************************

    /// <summary>
    /// To manage levels  
    /// </summary>
    public class RC_GameStateManager
    {
        RC_GameStateParent[] states;
        RC_GameStateParent cur=null; //current_State;        
        RC_GameStateParent prevState=null; //previous state;
        public RC_GameStateParent prevStatePlayLevel=null; //previous state;
        int curLevNum;
        //int prevPlayLevNum=0;

        int[] levelStack;
        int sp; // stack pointer

        public RC_GameStateManager()
        {
            init(100);

        }

        private void init(int numLevelz)
        {
            states = new RC_GameStateParent[numLevelz];
            for (int i = 0; i < numLevelz; i++) states[i] = null;
            levelStack = new int[30];
            sp = 0;
            setEmptyLevel();
        }

        public void AddLevel(int levNum, RC_GameStateParent lev)
        {
            states[levNum] = lev;
        }

        public RC_GameStateParent getLevel(int levNum)
        {
            return states[levNum];
        }

        public void setLevel(int levNum)
        {
            prevState = cur;
          
            states[levNum].EnterLevel(curLevNum);
            cur = states[levNum];  
            prevStatePlayLevel = cur; // to call draw
            prevState.ExitLevel();
            curLevNum = levNum;

            RC_GameStateParent.prevKeyState = Keyboard.GetState();
            RC_GameStateParent.keyState = Keyboard.GetState(); // fix legacy keystate issues
            RC_GameStateParent.previousMouseState = Mouse.GetState();
            RC_GameStateParent.currentMouseState = Mouse.GetState();
        }

        public void pushLevel(int levNum)
        {
            prevState = cur;
            states[levNum].EnterLevel(curLevNum);
            levelStack[sp] = curLevNum;
            cur = states[levNum];
            curLevNum = levNum;
            sp++;
        }

        public int popLevel()
        {
            sp--;
            prevState = cur;
            cur = states[levelStack[sp]];
            curLevNum = levelStack[sp];
            prevState.ExitLevel();

            RC_GameStateParent.prevKeyState = Keyboard.GetState();
            RC_GameStateParent.keyState = Keyboard.GetState(); // fix legacy keystate issues
            RC_GameStateParent.previousMouseState = Mouse.GetState();
            RC_GameStateParent.currentMouseState = Mouse.GetState();
            return curLevNum;
        }

        public void setEmptyLevel()
        {
            cur = new EmptyState();
            curLevNum = -1; // hmm i guess empty level is now level -1
        }

        public RC_GameStateParent getCurrentLevel()
        {
            return cur;
        }

        public int getCurrentLevelNum()
        {
            return curLevNum;
        }
    }

    //      ************************************************ Empty State **************************************************

    /// <summary>
    /// A default 'empty' level to fix probelms of having nothing to Draw or Update
    /// in short this class exists as a do nothing placeholder
    /// we need it in game initialisation and startup and it helps to simplify teaching 
    /// </summary>
    class EmptyState : RC_GameStateParent
    {
        public override void Draw(GameTime gameTime)
        {
        }
    }
}