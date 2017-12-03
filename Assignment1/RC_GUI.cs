using System.Text;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Storage;


namespace RC_Framework
{
    /// <summary>
    /// Globals for all gui objects
    /// they must be initialised by calling initGUIGlobals
    /// </summary>
    public class GUI_Globals
    {
        /// <summary>
        /// tooltip font
        /// </summary>
        public static SpriteFont tTfont;  
        
        /// <summary>
        ///  default button font
        /// </summary>
        public static SpriteFont butFontfont;
 
        /// <summary>
        /// texture of a white single pixel square created in init
        /// </summary>
        public static Texture2D whiteTex; // texture of a white single pixel square 
 
        /// <summary>
        /// colour of default text
        /// </summary>
        public static Color defaultTextColor=Color.Black; 
        
        // tool tip globals for final render

        /// <summary>
        /// the current tool tip text
        /// </summary>
        public static String toolTipText = "";

        /// <summary>
        /// Count to remove the tool tip
        /// </summary>
        public static int tipCount = 0;

        /// <summary>
        /// max ticks for tool tip to stay
        /// </summary>
        public static int tipMax = 160;

        /// <summary>
        /// where the tool tip is displayed
        /// </summary>
        public static Vector2 tipPos; // tooltip pos
 
        /// <summary>
        /// the tool tip colour
        /// </summary>
        public static Color tTColor = Color.Blue; // tooltip color
        
        /// <summary>
        /// Tool tip background colour
        /// </summary>
        public static Color tTColorBack = Color.Yellow; // tooltip background color

        /// <summary>
        /// Default text colour
        /// </summary>
        public static Color defaultText = Color.Black;

        /// <summary>
        /// Initialise all gui gloabls
        /// </summary>
        /// <param name="device"></param>
        /// <param name="buttonFont"></param>
        /// <param name="toolTipFont"></param>
        /// <param name="defaultTextColorZ"></param>
        public static void initGUIGlobals(GraphicsDevice device, SpriteFont buttonFont, SpriteFont toolTipFont, Color defaultTextColorZ )
        {
            whiteTex = new Texture2D(device, 1, 1, false, SurfaceFormat.Color);
            whiteTex.SetData(new[] { Color.White });
            tTfont  = toolTipFont;
            butFontfont = buttonFont;
            defaultTextColor = defaultTextColorZ;
        }

        /// <summary>
        /// Draw the tool tips on the screen
        /// this needs to be called late in the level draw routine usuall just before the mouse is drawn
        /// </summary>
        /// <param name="sb"></param>
        public static void drawToolTip(SpriteBatch sb)
        {
            if (toolTipText != "")
            {
                Vector2 size = GUI_Globals.tTfont.MeasureString(toolTipText);

                sb.Draw(GUI_Globals.whiteTex, new Rectangle((int)GUI_Globals.tipPos.X, (int)GUI_Globals.tipPos.Y, (int)size.X, (int)size.Y), GUI_Globals.tTColorBack);
                sb.DrawString(GUI_Globals.tTfont, toolTipText, GUI_Globals.tipPos, GUI_Globals.tTColor);
            }
        }

        /// <summary>
        /// this needs to be piut in the level update routine to remove / update the tool tips
        /// </summary>
        public static void updateToolTips()
        {
            GUI_Globals.tipCount++;
            if (GUI_Globals.tipCount > GUI_Globals.tipMax)
            {
                GUI_Globals.toolTipText = "";
            }
       }
    }
    
    // ******************************* GUI_Control **************************************

    /// <summary>
    /// GUI parent class 
    /// This is the parent class for all gui objects
    /// </summary>
    public class GUI_Control : RC_RenderableBounded
    {
        /// <summary>
        /// Ny list of sub controls - usually you dont need to touch this
        /// </summary>
        public List<GUI_Control> controls = null;

        /// <summary>
        /// Do i have focus
        /// </summary>
        public bool focus=false; 

        /// <summary>
        /// My parent control or null
        /// </summary>
        public GUI_Control parent=null; 
        
        /// <summary>
        ///  tool tip text string 
        /// </summary>
        public String tTtext="";
     
        //public override bool MouseMoveEvent(MouseState ms, MouseState previousMs) { return false; }
        //public override bool MouseUpEventLeft(float mouse_x, float mouse_y) { return false; }
        //public override bool MouseDownEventRight(float mouse_x, float mouse_y) { return false; }
        //public override bool MouseUpEventRight(float mouse_x, float mouse_y) { return false; }
        //public override bool KeyHitEvent(Keys keyHit) { return false; }

        /// <summary>
        /// Handle left mouse click
        /// </summary>
        /// <param name="mouse_x"></param>
        /// <param name="mouse_y"></param>
        /// <returns></returns>
        public override bool MouseDownEventLeft(float mouse_x, float mouse_y) 
        {
            if (controls == null) return false;
            if (controls.Count <= 0) return false;

            for (int i = 0; i < controls.Count; i++)
            {
                bool rc = controls[i].MouseDownEventLeft(mouse_x, mouse_y);
                if (rc) return true;
            }
            return false; 
        }

        /// <summary>
        /// handle mouse over event (eg tool tips)
        /// </summary>
        /// <param name="mouse_x"></param>
        /// <param name="mouse_y"></param>
        /// <returns></returns>
        public override bool MouseOver(float mouse_x, float mouse_y) // for tool tips
        {
            if (tTtext != "")
            {
                if (inside(mouse_x, mouse_y))
                {
                    if (GUI_Globals.toolTipText != tTtext)
                    {
                        GUI_Globals.tipCount = 0;
                        GUI_Globals.toolTipText = tTtext;
                        GUI_Globals.tipPos = new Vector2(mouse_x + 8, mouse_y - 12);
                    }
                }
                else
                {
                    if (GUI_Globals.toolTipText == tTtext)
                    {
                        GUI_Globals.toolTipText = "";
                    }
                }
            }
            
            if (controls == null) return false;
            if (controls.Count <= 0) return false;

            for (int i = 0; i < controls.Count; i++)
            {
                bool rc = controls[i].MouseOver(mouse_x, mouse_y);
                if (rc) return true;
            }
            return false;
        }

        /// <summary>
        /// draw my sub controls if any
        /// </summary>
        /// <param name="sb"></param>
        public virtual void drawSubControls(SpriteBatch sb)
        {
            if (controls == null) return;
            if (controls.Count <= 0) return;

            for (int i = 0; i < controls.Count; i++)
            {
                if (controls[i].visible) controls[i].Draw(sb);
            }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public GUI_Control()
        {
            bounds = new Rectangle(0, 0, 128, 16);
            colour = Color.White;
        }

        /// <summary>
        /// Set the tool tip for this control
        /// </summary>
        /// <param name="text"></param>
        public void setToolTip(String text)
        {
          tTtext = text; // tool tip works if set and tip is true
          //tip=false;
          GUI_Globals.tipCount = 0;
        }

        /// <summary>
        /// remove all sub controls
        /// </summary>
        public void Clear()
        {
            controls.Clear();
        }

        /// <summary>
        /// add a sub control making me its parent
        /// </summary>
        /// <param name="c"></param>
        public void AddControl(GUI_Control c)
        {
            if (controls == null) {controls = new List<GUI_Control>(); }
            controls.Add(c);
            c.parent = this;
        }

        /// <summary>
        /// get a control if you know its index
        /// </summary>
        /// <param name="cnum"></param>
        /// <returns></returns>
        public GUI_Control getControl(int cnum)
        {
            return controls[cnum];
        }

        /// <summary>
        /// Return a boolean that is true if the point is inside the bounds
        /// and the control is visible
        /// </summary>
        public Boolean inside(float x, float y)
        {
            if (!visible) return false;
            Vector2 v = getScreenPos();
            Rectangle temp = new Rectangle((int)v.X, (int)v.Y, bounds.Width, bounds.Height);
            if (x < temp.Left) return false;
            if (y < temp.Top) return false;
            if (x > temp.Right) return false;
            if (y > temp.Bottom) return false;
            return true;
        }

        /// <summary>
        /// clear focus on me and my sub controlls
        /// </summary>
        /// <param name="clearParent"></param>
        public void clearFocus(bool clearParent)
        {
            focus = false;
            if (controls ==null || controls.Count <= 0) return;

            for (int i = 0; i < controls.Count; i++)
            {
                controls[i].clearFocus(false);
            }
            if (clearParent && parent != null) parent.clearFocus(true);

        }

        /// <summary>
        /// Get my position on the actual screen as adjusted by my parents position
        /// </summary>
        /// <returns></returns>
        public Vector2 getScreenPos()
        {
            if (parent == null)
            {
                return new Vector2(bounds.X, bounds.Y);
            }
            else
            {
                return new Vector2(bounds.X, bounds.Y) + parent.getScreenPos();
            }
        }

        /// <summary>
        /// key hit handling
        /// </summary>
        /// <param name="keyHit"></param>
        /// <returns></returns>
        public override bool KeyHitEvent(Keys keyHit)
        {
            
            return passOnKeyhit(keyHit); 
        }

        /// <summary>
        /// Send the key hit to my sub controls
        /// </summary>
        /// <param name="keyHit"></param>
        /// <returns></returns>
        public bool passOnKeyhit(Keys keyHit)
        {
            if (controls == null) return false;
            if (controls.Count <= 0) return false;

            for (int i = 0; i < controls.Count; i++)
            {
                bool rc = controls[i].KeyHitEvent(keyHit);
                if (rc) return true;
            }
            return false;
        }

        /// <summary>
        /// Update 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            if (controls == null) return;
            if (controls.Count <= 0) return;

            for (int i = 0; i < controls.Count; i++)
            {
                controls[i].Update(gameTime);
            }
            return;
        }

        /// <summary>
        /// Set my colour and all my sub controlls colour
        /// usefull to control transparency
        /// </summary>
        /// <param name="c"></param>
        /// <param name="passOn"></param>
        public void setColor(Color c, bool passOn)
        {
            colour = c;
            if (passOn)
            {
                for (int i = 0; i < controls.Count; i++)
                {
                    controls[i].colour = c;
                }
            }
        }

    }

// ---------------------------------------------------------- Frame ------------------------------------

    /// <summary>
    /// Simple Gui class with 1 image its usually a container for other classes
    /// </summary>
    public class Frame : GUI_Control
    {
        Texture2D tex;

        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="t"></param>
        /// <param name="pos"></param>
        public Frame(Texture2D t, Rectangle pos)
        {
            tex = t;
            bounds = pos;

        }

        /// <summary>
        /// standard Draw
        /// </summary>
        /// <param name="sb"></param>
        public override void Draw(SpriteBatch sb)
        {
            if (!visible) return;
            Vector2 pos = getScreenPos();
            Rectangle r = new Rectangle((int)pos.X, (int)pos.Y, bounds.Width, bounds.Height);
            sb.Draw(tex, r, colour);
            drawSubControls(sb);
        }
    }

    // ---------------------------------------------------------- Button2 ------------------------------------

    /// <summary>
    /// Simple Gui class with 2 images - button up and button down 
    /// </summary>
    public class Button2 : GUI_Control
    {
        Texture2D up;
        Texture2D down;

        /// <summary>
        /// current state 0=up 1=down
        /// </summary>
        public int state; 
 
        /// <summary>
        /// true if clicked - 
        /// you poll this in update
        /// and once the click is actiopned you reset it 
        /// </summary>
        public bool wasClicked;

        /// <summary>
        /// Button2 constructor sizes to the texture
        /// </summary>
        /// <param name="upZ"></param>
        /// <param name="downZ"></param>
        /// <param name="pos"></param>
        public Button2(Texture2D upZ, Texture2D downZ, Vector2 pos)
        {
            up = upZ;
            down = downZ;
            bounds = new Rectangle((int)pos.X, (int)pos.Y, up.Width, up.Height);
            wasClicked = false;
        }
        
        /// <summary>
        /// Button2 constructor 
        /// </summary>
        /// <param name="upZ"></param>
        /// <param name="downZ"></param>
        /// <param name="pos"></param>
        public Button2(Texture2D upZ, Texture2D downZ, Rectangle pos)
        {
            up = upZ;
            down = downZ;
            bounds = pos;
            wasClicked = false;
        }

        /// <summary>
        /// Draw yes a draw standard draw nothing more
        /// </summary>
        /// <param name="sb"></param>
        public override void Draw(SpriteBatch sb)
        {
            if (!visible) return;
            Vector2 pos = getScreenPos();
            Rectangle r = new Rectangle((int)pos.X, (int)pos.Y, bounds.Width, bounds.Height);
            if (state == 0)
                sb.Draw(up, r, colour);
            if (state == 1)
                sb.Draw(down, r, colour);
            drawSubControls(sb);
            //drawToolTip(sb);
        }

        /// <summary>
        /// the mouse clicks me
        /// </summary>
        /// <param name="mouse_x"></param>
        /// <param name="mouse_y"></param>
        /// <returns></returns>
        public override bool MouseDownEventLeft(float mouse_x, float mouse_y)
        {
            // note a button is unlikey to have children so its considered a terminal node
            if (inside(mouse_x, mouse_y))
            {
                wasClicked = true;
                if (state == 0) state = 1; else state = 0;
                return true;
            }
            return false;
        }
    }

// -------------------------------------------------- Button Single Image ------------------------------------

    /// <summary>
    /// ButtonSI Simple Gui button with 1 image but two colours to show when clicked 
    /// </summary>
    public class ButtonSI : GUI_Control
    {
        Texture2D up;
        Color colClicked;
        
        /// <summary>
        /// 0=up 1=down
        /// </summary>
        public int state;
 
        /// <summary>
        /// true if clicked - 
        /// you poll this in update
        /// and once the click is actiopned you reset it 
        /// </summary>
        public bool wasClicked;
        int clickTimeTicks = 0;
        
        /// <summary>
        /// Time in ticks to show click
        /// </summary>
        public int clickTimeMax = 30;

        /// <summary>
        ///  Optional text string 
        /// </summary>
        public String TextToDisplay="";
        
        /// <summary>
        /// Font to display text
        /// </summary>
        public SpriteFont tfont;

        /// <summary>
        /// Where to show the text 
        /// </summary>
        public Vector2 textOffset = new Vector2(5, 3);

        /// <summary>
        /// ButtonSI Constructor - resizes to texture
        /// </summary>
        /// <param name="upZ"></param>
        /// <param name="cClicked"></param>
        /// <param name="pos"></param>
        public ButtonSI(Texture2D upZ, Color cClicked, Vector2 pos)
        {
            up = upZ;
            bounds = new Rectangle((int)pos.X, (int)pos.Y, up.Width, up.Height);
            wasClicked = false;
            colClicked = cClicked;
        }

        /// <summary>
        /// ButtonSI Constructor 
        /// </summary>
        /// <param name="upZ"></param>
        /// <param name="cClicked"></param>
        /// <param name="pos"></param>
        public ButtonSI(Texture2D upZ, Color cClicked, Rectangle pos)
        {
            //init();
            up = upZ;
            bounds = pos;
            wasClicked = false;
            colClicked = cClicked;
        }

        /// <summary>
        /// set the text and font
        /// </summary>
        /// <param name="t"></param>
        /// <param name="f"></param>
        public void setText(String t, SpriteFont f)
        {
        // Optional text string 
        TextToDisplay=t;
        tfont=f;
        }

        /// <summary>
        /// Draw draw draw BOREING
        /// </summary>
        /// <param name="sb"></param>
        public override void Draw(SpriteBatch sb)
        {
            if (!visible) return;
            Vector2 pos = getScreenPos();
            Rectangle r = new Rectangle((int)pos.X, (int)pos.Y, bounds.Width, bounds.Height);
            if (state == 0)
                sb.Draw(up, r, colour);
            if (state == 1)
                sb.Draw(up, r, colClicked);
            
            if (TextToDisplay!="")
            {
                sb.DrawString(tfont, TextToDisplay, new Vector2(pos.X + textOffset.X, pos.Y + textOffset.Y), GUI_Globals.defaultTextColor);
            }
            drawSubControls(sb);
        }

        /// <summary>
        /// Handle mouse click
        /// </summary>
        /// <param name="mouse_x"></param>
        /// <param name="mouse_y"></param>
        /// <returns></returns>
        public override bool MouseDownEventLeft(float mouse_x, float mouse_y)
        {
            // note a button is unlikey to have children so its considered a terminal node
            if (inside(mouse_x, mouse_y))
            {
                wasClicked = true;
                clickTimeTicks = 0;
                state=1;
                return true;
            }
            return false;
        }
   
        /// <summary>
        /// straightforward Update
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            clickTimeTicks++;
            if (clickTimeTicks > clickTimeMax)
            {
            state=0;
            wasClicked = false;
            }

        }
    }
    
    // -------------------------------------------------- TextBox ------------------------------------

    /// <summary>
    /// TextBox simple gui control for text display
    /// </summary>
    public class TextBox : GUI_Control
    {
        Texture2D tex;
        SpriteFont font;

        /// <summary>
        /// The text to display
        /// </summary>
        public string text;

        /// <summary>
        /// Where to show the text
        /// </summary>
        public Vector2 textOffset;
        Color textColor;

        /// <summary>
        /// TextBox Constructor resizes to texture
        /// </summary>
        /// <param name="texZ"></param>
        /// <param name="pos"></param>
        /// <param name="fonty"></param>
        /// <param name="s"></param>
        public TextBox(Texture2D texZ, Vector2 pos, SpriteFont fonty, string s)
        {
            //init();
            font = fonty;
            tex = texZ;
            bounds = new Rectangle((int)pos.X, (int)pos.Y, tex.Width, tex.Height);
            text = s;
            textOffset = new Vector2(5, 3);
            textColor = GUI_Globals.defaultTextColor;
        }

        /// <summary>
        /// TextBox constructor
        /// </summary>
        /// <param name="texZ"></param>
        /// <param name="pos"></param>
        /// <param name="fonty"></param>
        /// <param name="s"></param>
        public TextBox(Texture2D texZ, Rectangle pos, SpriteFont fonty, string s)
        {
            //init();
            font = fonty;
            tex = texZ;
            bounds = pos;
            text = s;
            textOffset = new Vector2(5, 3);
            textColor = GUI_Globals.defaultTextColor;
        }

        /// <summary>
        /// Draw
        /// </summary>
        /// <param name="sb"></param>
        public override void Draw(SpriteBatch sb)
        {
            if(!visible)return;
            Vector2 pos = getScreenPos();
            Rectangle r = new Rectangle((int)pos.X, (int)pos.Y, bounds.Width, bounds.Height);
            sb.Draw(tex, r, colour);
            if (text != "") sb.DrawString(font, text, new Vector2(pos.X + textOffset.X, pos.Y + textOffset.Y), textColor);
            drawSubControls(sb);
        }

        /// <summary>
        /// handle left mouse click
        /// </summary>
        /// <param name="mouse_x"></param>
        /// <param name="mouse_y"></param>
        /// <returns></returns>
        public override bool MouseDownEventLeft(float mouse_x, float mouse_y)
        {
            if (controls == null || controls.Count > 0) return false; 
            
                if (inside(mouse_x, mouse_y))
                {
                    // hand to my children controls
                    for (int i = 0; i < controls.Count; i++)
                    {
                        bool rc = controls[i].MouseDownEventLeft(mouse_x, mouse_y);
                        if (rc) return true;
                    }
                }
            
            return false;
         }

        /// <summary>
        /// Just and update
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
                if (controls != null && controls.Count > 0)
                {
                    // hand to my children controls
                    for (int i = 0; i < controls.Count; i++)
                    {
                        controls[i].Update(gameTime);
                    }
                }

        }

        /// <summary>
        /// Handle key hit
        /// </summary>
        /// <param name="keyHit"></param>
        /// <returns></returns>
        public override bool KeyHitEvent(Keys keyHit)
        {
            return passOnKeyhit(keyHit);
        }
    }

 // -------------------------------------------------- InputBox ------------------------------------

    /// <summary>
    /// get the text from the user
    /// </summary>
    public class InputBox : GUI_Control
    {
        Texture2D tex;
        SpriteFont font;
        
        /// <summary>
        /// the text that has been typed in or originaly put there
        /// </summary>
        public string text;
        int maxLength;
        int ticks;

        /// <summary>
        /// where to display the text
        /// </summary>
        public Vector2 textOffset;
        
        /// <summary>
        /// InputBox constructor resizes to image width
        /// </summary>
        /// <param name="texZ"></param>
        /// <param name="pos"></param>
        /// <param name="fonty"></param>
        /// <param name="s"></param>
        /// <param name="maxLen"></param>
        public InputBox(Texture2D texZ, Vector2 pos, SpriteFont fonty, string s, int maxLen)
        {
            font = fonty;
            tex = texZ;
            bounds = new Rectangle((int)pos.X, (int)pos.Y, tex.Width, tex.Height);
            text = s;
            ticks = 0;
            textOffset = new Vector2(7, 5);
            maxLength = maxLen;
            //colour = GUI_Globals.defaultText;
        }

        /// <summary>
        /// InputBox constructor
        /// </summary>
        /// <param name="texZ"></param>
        /// <param name="pos"></param>
        /// <param name="fonty"></param>
        /// <param name="s"></param>
        /// <param name="maxLen"></param>
        public InputBox(Texture2D texZ, Rectangle pos, SpriteFont fonty, string s, int maxLen)
        {
            font = fonty;
            tex = texZ;
            bounds = pos;
            text = s;
            ticks = 0;
            textOffset = new Vector2(7, 5);
            maxLength = maxLen;
            //colour = GUI_Globals.defaultText;
        }

        /// <summary>
        /// Get the text as an integer
        /// </summary>
        /// <returns></returns>
        public int asInteger()
        {
            int rc=0;
            try
            {
                rc = Convert.ToInt32(text);
                return rc;
            }
            catch
            {
                return rc;
            }
        }

        /// <summary>
        /// We all know Draw
        /// </summary>
        /// <param name="sb"></param>
        public override void Draw(SpriteBatch sb)
        {
            if (!visible) return;
            Vector2 pos = getScreenPos();
            Rectangle r = new Rectangle((int)pos.X, (int)pos.Y, bounds.Width, bounds.Height);
            sb.Draw(tex, r, colour);
            if ((ticks / 20) % 2 == 0 && focus)
            {
                sb.DrawString(font, text + "|", new Vector2(pos.X + textOffset.X, pos.Y + textOffset.Y), GUI_Globals.defaultTextColor);
            }
            else
            {
                sb.DrawString(font, text, new Vector2(pos.X + textOffset.X, pos.Y + textOffset.Y), GUI_Globals.defaultTextColor);
            }
            drawSubControls(sb);
        }

        /// <summary>
        /// Upadte 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime) 
        {
            ticks++;
        }

        /// <summary>
        /// Handle key hit
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override bool KeyHitEvent(Keys key)
        {
            if (!focus) return false; 
            if (!visible) return false;
            
            if (key == Keys.Back && text.Length > 0) // Process Backspace
            {
                text = text.Remove(text.Length - 1, 1);
                return true;
            }

            if (key == Keys.Space)
            {
                text = text + " "; // we may need to process more keys seperately eg ;            
                return true;
            }

            if (key == Keys.Enter)
            {
                return true; // return was pressed
            }

            
            String t = key.ToString();
            String k = t;
            if (t == "D0") k = "0";
            if (t == "D1") k = "1";
            if (t == "D2") k = "2";
            if (t == "D3") k = "3";
            if (t == "D4") k = "4";
            if (t == "D5") k = "5";
            if (t == "D6") k = "6";
            if (t == "D7") k = "7";
            if (t == "D8") k = "8";
            if (t == "D9") k = "9";
            if (t == "OemPeriod") k = ".";
            if (text.Length < maxLength && k.Length == 1) text = text + k;
            //if (text.Length < maxLength ) text = text + k;
            return true; 
        }

        /// <summary>
        /// handle mouse down
        /// </summary>
        /// <param name="mouse_x"></param>
        /// <param name="mouse_y"></param>
        /// <returns></returns>
        public override bool MouseDownEventLeft(float mouse_x, float mouse_y)
        {
            if (!visible) return false;
            if (inside(mouse_x, mouse_y))
            {
                if (controls !=null && controls.Count > 0)
                {
                    // hand to my children controls
                    for (int i = 0; i < controls.Count; i++)
                    {
                        bool rc = controls[i].MouseDownEventLeft(mouse_x, mouse_y);
                        if (rc) return true;
                    }
                }
                parent.clearFocus(true);
                focus = true;
                return true;
            }
            return false;
        }

    }

    // -------------------------------------------------- StringList Box ------------------------------------

    /// <summary>
    /// StringList gui control for multi line text display
    /// </summary>
    public class StringListBox : GUI_Control
    {
        Texture2D tex;
        SpriteFont font;
        StringList slist = new StringList();

        /// <summary>
        /// where to show the text top left corner
        /// </summary>
        public Vector2 textOffset;
        Color textColor;

        /// <summary>
        /// StringListBox Constructor 
        /// </summary>
        /// <param name="texZ"></param>
        /// <param name="pos"></param>
        /// <param name="fonty"></param>
        /// <param name="slst"></param>
        public StringListBox(Texture2D texZ, Rectangle pos, SpriteFont fonty, StringList slst)
        {
            font = fonty;
            tex = texZ;
            bounds = pos;
            slist.copy(slst);
            textOffset = new Vector2(5, 3);
            textColor = GUI_Globals.defaultTextColor;
        }

        /// <summary>
        /// Draw
        /// </summary>
        /// <param name="sb"></param>
        public override void Draw(SpriteBatch sb)
        {
            if (!active) return;
            if (!visible) return;
            Vector2 pos = getScreenPos();
            int ypos=0;
            Rectangle r = new Rectangle((int)pos.X, (int)pos.Y, bounds.Width, bounds.Height);
            sb.Draw(tex, r, colour);
            for (int i = 0; i < slist.Count; i++)
            {
                String text = slist[i];
                if (text == "") {text = " ";}
                sb.DrawString(font, text, new Vector2(pos.X + textOffset.X, ypos+pos.Y + textOffset.Y), textColor);
                Vector2 itemSize = font.MeasureString(text);
                ypos = ypos + (int)(itemSize.Y+0.5) + 2; 
            }
            drawSubControls(sb);
        }

        /// <summary>
        /// handle left mouse click
        /// </summary>
        /// <param name="mouse_x"></param>
        /// <param name="mouse_y"></param>
        /// <returns></returns>
        public override bool MouseDownEventLeft(float mouse_x, float mouse_y)
        {
            if (!active) return false;
            if (controls == null || controls.Count > 0) return false;

            if (inside(mouse_x, mouse_y))
            {
                // hand to my children controls
                for (int i = 0; i < controls.Count; i++)
                {
                    bool rc = controls[i].MouseDownEventLeft(mouse_x, mouse_y);
                    if (rc) return true;
                }
            }

            return false;
        }

        
        /// <summary>
        /// Update the controll
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            if (!active) return; 
            if (controls != null && controls.Count > 0)
            {
                // hand to my children controls
                for (int i = 0; i < controls.Count; i++)
                {
                    controls[i].Update(gameTime);
                }
            }

        }

        /// <summary>
        /// handle key hit
        /// </summary>
        /// <param name="keyHit"></param>
        /// <returns></returns>
        public override bool KeyHitEvent(Keys keyHit)
        {
            if (!active) return false;
            return passOnKeyhit(keyHit);
        }
    }
  
}