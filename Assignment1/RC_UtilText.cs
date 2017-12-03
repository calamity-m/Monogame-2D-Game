using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework.Storage;


#pragma warning disable 1591 //sadly not yet fully commented

namespace RC_Framework
{
    /// <summary>
    /// A mix of texture generation routines and some standard textures 
    /// </summary>
    public static class UtilTexSI
    {
        public static Texture2D texWhite; // white pixel
        public static Texture2D texBlack; // Black pixel
        public static Texture2D texRedGreen; // redLhs->greenRhs
        public static Texture2D texRainbow; // 
        public static Texture2D texTransBlackLR; // 
        public static Texture2D texTransBlackRL; // 
        public static Texture2D texTransWhiteLR; // 
        public static Texture2D texTransWhiteRL; // 
        public static Texture2D texTransWhiteTB; // 
        public static Texture2D texTransWhiteBT; //         
        public static Texture2D texTransWhiteDiagLR; // 
        public static Texture2D texTransWhiteDiagRL; // 
        public static Texture2D texAlphaCenter16; // 
        public static Texture2D texGenericButton64x32; //

        // below are graphics for the 8 bit retro subsystem
        public static Color[] Pallete11; // actually an 8 colour pallete (like old 8 bit sys)
        public static String  Pal11;
        public static Texture2D[] eightBit;
        public static Texture2D[] eightBit32;
        public static Texture2D[] eightBit64;


        public static void initTextures(GraphicsDevice device)
        {
            texWhite = new Texture2D(device, 1, 1, false, SurfaceFormat.Color);
            texWhite.SetData(new[] { Color.White });
            texRedGreen = createTex(device, 64, 1, Color.Red, Color.Green);
            texRainbow = createTex(device, 64, 64, Color.Red, Color.Green,Color.Yellow, Color.Blue);
            texTransBlackLR = createTex(device, 64, 1, Color.Transparent, Color.Black); // 
            texTransBlackRL = createTex(device, 64, 1, Color.Black, Color.Transparent); // 
            texTransWhiteLR = createTex(device, 64, 1, new Color(255,255,255,255), Color.White); // 
            texTransWhiteRL = createTex(device, 64, 1, Color.White, new Color(255,255,255,255)); // 
            texTransWhiteTB = createTex(device, 64, 64, new Color(255,255,255,255), new Color(255,255,255,255), Color.White, Color.White); // 
            texTransWhiteBT = createTex(device, 64, 64, Color.White, Color.White, new Color(255,255,255,255), new Color(255,255,255,255)); //         
            texTransWhiteDiagLR = createTex(device, 64, 64, new Color(255, 255, 255, 128), Color.White, new Color(255,255,255,255), new Color(255, 255, 255, 128)); // 
            texTransWhiteDiagRL = createTex(device, 64, 64, new Color(255,255,255,255), new Color(255, 255, 255, 128), new Color(255, 255, 255, 128), Color.White); // 
            RC_Surface sur = new RC_Surface(64, 64, new Color(255,255,255,255));
            sur.addBorder(1, Color.White);
            texAlphaCenter16 = sur.createTex(device); //
            sur = new RC_Surface(64, 32, Color.Silver);
            sur.addSculptedBorder(new Rectangle(0,0,64,32),new Color(230,230,230),Color.Gray,2);
            texGenericButton64x32 = sur.createTex(device); //

        }

        public static void init8Bit(GraphicsDevice device)
        {            
            Pallete11 = new Color[18];

            Pallete11[0]  = new Color(0, 0, 0);          // 000 = 0
            Pallete11[1]  = new Color(0, 0, 255);        // 001 = 1 Blue
            Pallete11[2]  = new Color(0, 255, 0);        // 010 = 2 Green
            Pallete11[3]  = new Color(0, 255, 255);      // 011 = 3
            Pallete11[4]  = new Color(255, 0, 0);        // 100 = 4 Red
            Pallete11[5]  = new Color(255, 0, 255);      // 101 = 5
            Pallete11[6]  = new Color(255, 255, 0);      // 110 = 6
            Pallete11[7]  = new Color(255, 255, 255);    // 111 = 7 White
            Pallete11[8]  = new Color(100, 100, 100);    // 1000 = 8
            Pallete11[9]  = new Color(100, 100, 255);    // 1001 = 9
            Pallete11[10] = new Color(100, 255,100);     // 1010 = 10 A
            Pallete11[11] = new Color(100, 255, 255);    // 1011 = 11 B
            Pallete11[12] = new Color(255,100,100);      // 1100 = 12 C
            Pallete11[13] = new Color(255, 100,255);     // 1101 = 13 D
            Pallete11[14] = new Color(255, 255, 100);    // 1110 = 14 E
            Pallete11[15] = Color.Silver;                // 1111 = 15 F
            Pallete11[16] = new Color(0, 0, 0, 0);       // 10000 = 16 " " Space trans black
            Pallete11[17] = new Color(255, 255, 255, 0); // 10000 = 16 "." Dot trans white

            Pal11 = "0123456789ABCDEF .";

            eightBit = new Texture2D[16];
            eightBit32 = new Texture2D[16];
            eightBit64 = new Texture2D[16];
            RC_Surface sur;

            String[] str0 = {"   77   ",
                             "  7777  ",
                             " 777777 ",
                             " 7    7 ",
                             " 777777 ",
                             "  7777  ",
                             "  7  7  ",
                             "  7  7  "};
            RC_Surface sur0 = new RC_Surface(str0, Pallete11, Pal11);
            eightBit[0] = sur0.createTex(device);
            sur = new RC_Surface(sur0, 4, 4, 0, 0, Color.Transparent, 0, 0);
            eightBit32[0] = sur.createTex(device);
            sur = new RC_Surface(sur0, 8,8, 0, 0, Color.Transparent, 0, 0);
            eightBit64[0] = sur.createTex(device);

            String[] str1 = {"   77   ",
                             "  7777  ",
                             " 777777 ",
                             " 7    7 ",
                             " 777777 ",
                             "  7777  ",
                             " 7    7 ",
                             "7      7"};
            RC_Surface sur1 = new RC_Surface(str1, Pallete11, Pal11);
            eightBit[1] = sur1.createTex(device);
            sur = new RC_Surface(sur1, 4, 4, 0, 0, Color.Transparent, 0, 0);
            eightBit32[1] = sur.createTex(device);
            sur = new RC_Surface(sur1, 8,8, 0, 0, Color.Transparent, 0, 0);
            eightBit64[1] = sur.createTex(device);


            String[] str2 = {"77    77",
                             "  7777  ",
                             " 777777 ",
                             "77    77",
                             "7 7777 7",
                             "7 7777 7",
                             " 77  77 ",
                             "        "};
            RC_Surface sur2 = new RC_Surface(str2, Pallete11, Pal11);
            eightBit[2] = sur2.createTex(device);
            sur = new RC_Surface(sur2, 4, 4, 0, 0, Color.Transparent, 0, 0);
            eightBit32[2] = sur.createTex(device);
            sur = new RC_Surface(sur2, 8, 8, 0, 0, Color.Transparent, 0, 0);
            eightBit64[2] = sur.createTex(device);

            String[] str3 = {"77    77",
                             "7 7777 7",
                             "77777777",
                             "77    77",
                             "  7777  ",
                             "  7777  ",
                             " 77  77 ",
                             "        "};
            RC_Surface sur3 = new RC_Surface(str3, Pallete11, Pal11);
            eightBit[3] = sur3.createTex(device);
            sur = new RC_Surface(sur3, 4, 4, 0, 0, Color.Transparent, 0, 0);
            eightBit32[3] = sur.createTex(device);
            sur = new RC_Surface(sur3, 8, 8, 0, 0, Color.Transparent, 0, 0);
            eightBit64[3] = sur.createTex(device);

            String[] str4 = {" 77  77 ",
                             "  7777  ",
                             "77777777",
                             "77    77",
                             "  7777  ",
                             "  7777  ",
                             " 77  77 ",
                             "  7  7  "};
            RC_Surface sur4 = new RC_Surface(str4, Pallete11, Pal11);
            eightBit[4] = sur4.createTex(device);
            sur = new RC_Surface(sur4, 4, 4, 0, 0, Color.Transparent, 0, 0);
            eightBit32[4] = sur.createTex(device);
            sur = new RC_Surface(sur4, 8, 8, 0, 0, Color.Transparent, 0, 0);
            eightBit64[4] = sur.createTex(device);

            String[] str5 = {"77    77",
                             "  7777  ",
                             "77777777",
                             "77    77",
                             "  7777  ",
                             "  7777  ",
                             " 77  77 ",
                             " 7    7 "};
            RC_Surface sur5 = new RC_Surface(str5, Pallete11, Pal11);
            eightBit[5] = sur5.createTex(device);
            sur = new RC_Surface(sur5, 4, 4, 0, 0, Color.Transparent, 0, 0);
            eightBit32[5] = sur.createTex(device);
            sur = new RC_Surface(sur5, 8, 8, 0, 0, Color.Transparent, 0, 0);
            eightBit64[5] = sur.createTex(device);

            String[] str6 = {" 7    7 ",
                             "  7777  ",
                             "22222222",
                             "77    77",
                             "  7777  ",
                             "  7777  ",
                             "777  777",
                             " 7    7 "};
            RC_Surface sur6 = new RC_Surface(str6, Pallete11, Pal11);
            eightBit[6] = sur6.createTex(device);
            sur = new RC_Surface(sur6, 4, 4, 0, 0, Color.Transparent, 0, 0);
            eightBit32[6] = sur.createTex(device);
            sur = new RC_Surface(sur6, 8, 8, 0, 0, Color.Transparent, 0, 0);
            eightBit64[6] = sur.createTex(device);

            String[] str7 = {" 7    7 ",
                             "  7777  ",
                             "44444444",
                             "77    77",
                             "  7777  ",
                             "  7777  ",
                             "777  777",
                             " 7    7 "};
            RC_Surface sur7 = new RC_Surface(str7, Pallete11, Pal11);
            eightBit[7] = sur7.createTex(device);
            sur = new RC_Surface(sur7, 4, 4, 0, 0, Color.Transparent, 0, 0);
            eightBit32[7] = sur.createTex(device);
            sur = new RC_Surface(sur7, 8, 8, 0, 0, Color.Transparent, 0, 0);
            eightBit64[7] = sur.createTex(device);

            String[] str8 = {"        ",
                             "  7777  ",
                             "DDDDDDDD",
                             "77    77",
                             "  7777  ",
                             "  7777  ",
                             "777  777",
                             " 7    7 "};
            RC_Surface sur8 = new RC_Surface(str8, Pallete11, Pal11);
            eightBit[8] = sur8.createTex(device);
            sur = new RC_Surface(sur8, 4, 4, 0, 0, Color.Transparent, 0, 0);
            eightBit32[8] = sur.createTex(device);
            sur = new RC_Surface(sur8, 8, 8, 0, 0, Color.Transparent, 0, 0);
            eightBit64[8] = sur.createTex(device);

            String[] str9 = {"        ",
                             "  7777  ",
                             "77777777",
                             "DD    DD",
                             "  7777  ",
                             "  7777  ",
                             "777  777",
                             " 7    7 "};
            RC_Surface sur9 = new RC_Surface(str9, Pallete11, Pal11);
            eightBit[9] = sur9.createTex(device);
            sur = new RC_Surface(sur9, 4, 4, 0, 0, Color.Transparent, 0, 0);
            eightBit32[9] = sur.createTex(device);
            sur = new RC_Surface(sur9, 8, 8, 0, 0, Color.Transparent, 0, 0);
            eightBit64[9] = sur.createTex(device);

            String[] str10 = {"        ",
                              "  7777  ",
                              "77777777",
                              "77    77",
                              "  DDDD  ",
                              "  7777  ",
                              "777  777",
                              " 7    7 "};
            RC_Surface sur10 = new RC_Surface(str10, Pallete11, Pal11);
            eightBit[10] = sur10.createTex(device);
            sur = new RC_Surface(sur10, 4, 4, 0, 0, Color.Transparent, 0, 0);
            eightBit32[10] = sur.createTex(device);
            sur = new RC_Surface(sur10, 8, 8, 0, 0, Color.Transparent, 0, 0);
            eightBit64[10] = sur.createTex(device);

            String[] str11 = {"        ",
                              "  7777  ",
                              "77777777",
                              "77    77",
                              "  7777  ",
                              "  DDDD  ",
                              "777  777",
                              " 7    7 "};
            RC_Surface sur11 = new RC_Surface(str11, Pallete11, Pal11);
            eightBit[11] = sur11.createTex(device);
            sur = new RC_Surface(sur11, 4, 4, 0, 0, Color.Transparent, 0, 0);
            eightBit32[11] = sur.createTex(device);
            sur = new RC_Surface(sur11, 8, 8, 0, 0, Color.Transparent, 0, 0);
            eightBit64[11] = sur.createTex(device);

            String[] str12 = {"                ",
                              "                ",
                              "                ",
                              "     7777777    ",
                              "  777777777777  ",
                              "7777777777777777",
                              "7777777777777777",
                              "777777  77777777"};
            RC_Surface sur12 = new RC_Surface(str12, Pallete11, Pal11);
            eightBit[12] = sur12.createTex(device);
            sur = new RC_Surface(sur12, 4, 4, 0, 0, Color.Transparent, 0, 0);
            eightBit32[12] = sur.createTex(device);
            sur = new RC_Surface(sur12, 8, 8, 0, 0, Color.Transparent, 0, 0);
            eightBit64[12] = sur.createTex(device);

            String[] str13 = {"        ",
                              "        ",
                              "        ",
                              "        ",
                              "    7   ",
                              "   777  ",
                              " 7777777",
                              " 7777777"};
            RC_Surface sur13 = new RC_Surface(str13, Pallete11, Pal11);
            eightBit[13] = sur13.createTex(device);
            sur = new RC_Surface(sur13, 4, 4, 0, 0, Color.Transparent, 0, 0);
            eightBit32[13] = sur.createTex(device);
            sur = new RC_Surface(sur13, 8, 8, 0, 0, Color.Transparent, 0, 0);
            eightBit64[13] = sur.createTex(device);

            String[] str14 = {"    7   ",
                              "    7   ",
                              "    7   ",
                              "   777  ",
                              "    7   ",
                              "        ",
                              "        ",
                              "        "};
            RC_Surface sur14 = new RC_Surface(str14, Pallete11, Pal11);
            eightBit[14] = sur14.createTex(device);
            sur = new RC_Surface(sur14, 4, 4, 0, 0, Color.Transparent, 0, 0);
            eightBit32[14] = sur.createTex(device);
            sur = new RC_Surface(sur14, 8, 8, 0, 0, Color.Transparent, 0, 0);
            eightBit64[14] = sur.createTex(device);

            String[] str15 = {"        ",
                              " 7     7",
                              "  7   7 ",
                              "   7 7  ",
                              "    7   ",
                              "   7 7  ",
                              "  7   7 ",
                              " 7     7"};
            RC_Surface sur15 = new RC_Surface(str15, Pallete11, Pal11);
            eightBit[15] = sur15.createTex(device);
            sur = new RC_Surface(sur15, 4, 4, 0, 0, Color.Transparent, 0, 0);
            eightBit32[15] = sur.createTex(device);
            sur = new RC_Surface(sur15, 8, 8, 0, 0, Color.Transparent, 0, 0);
            eightBit64[15] = sur.createTex(device);

        }

        public static Texture2D createTex(GraphicsDevice device, int width, int height, Color topLeftC, Color topRightC,
             Color botLeftC, Color botRightC)
        {
            Texture2D tex = new Texture2D(device, width, height, false, SurfaceFormat.Color);
            Color[] data = new Color[width * height];
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    float lerpValX = (float)x / (float)width;
                    float lerpValY = (float)y / (float)height;
                    Color cTop = Color.Lerp(topLeftC, topRightC, lerpValX);
                    Color cBot = Color.Lerp(botLeftC, botRightC, lerpValX);
                    data[x + (y * width)] = Color.Lerp(cTop, cBot, lerpValY);
                }
            tex.SetData(data);
            return tex;
        }
        
        public static Texture2D createTex(GraphicsDevice device, int width, int height, Color topLeftC, Color topRightC)
        {
            Texture2D tex = new Texture2D(device, width, height , false, SurfaceFormat.Color);
            Color[] data = new Color[width*height];
            for (int x=0; x<width; x++)
                for (int y = 0; y < height; y++)
                {
                    float lerpVal = (float)x/(float)width;
                    data[x+(y*width)] = Color.Lerp(topLeftC, topRightC, lerpVal); 
                }
            tex.SetData(data);
            return tex;
        }

   }

    // *******************************************************************************************************

    /// <summary>
    /// Parent class for all the bitmapped fonts stored in code
    /// </summary>
    public class FontParent
    {
       public Texture2D[] fntTex = null; // font textures
       public int charSetSize = 96;
       public int charSetSizeExtra = 1;
       public virtual int charwidth() { return 8; } 

       int cvtCharToInt(char c)
       {
           int i = (int)c;
           if (i < 32 || i > charSetSize+32) i = charSetSize+1;
           return i - 32;
       }

        /// <summary>
        /// Draw a character at a given location in its default size
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="pos"></param>
        /// <param name="c"></param>
        /// <param name="col"></param>
       public void drawChar(SpriteBatch sb, Vector2 pos, char c, Color col)
       {
           int i = cvtCharToInt(c);
           sb.Draw(fntTex[i], pos, col);
       }

        /// <summary>
        /// draw a character in a bounding rectangle
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="dest"></param>
        /// <param name="c"></param>
        /// <param name="col"></param>
       public void drawChar(SpriteBatch sb, Rectangle dest, char c, Color col)
       {
           int i = cvtCharToInt(c);
           sb.Draw(fntTex[i], dest, col);
       }

        /// <summary>
        /// draw a complete string at a given location
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="pos"></param>
        /// <param name="ss"></param>
        /// <param name="col"></param>
       public void drawStr(SpriteBatch sb, Vector2 pos, string ss, Color col)
       {
           Vector2 p = new Vector2(pos.X, pos.Y);
           //string ss = s;

           for (int i = 0; i < ss.Length; i++)
           {
               int k = cvtCharToInt(ss[i]);
               sb.Draw(fntTex[k], p, col);
               p.X = p.X + charwidth() ;
           }
       }

        /// <summary>
        /// draw a string with specified character width and height
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="pos"></param>
        /// <param name="ss"></param>
        /// <param name="col"></param>
        /// <param name="charWidth"></param>
        /// <param name="charHeight"></param>
       public void drawStr(SpriteBatch sb, Vector2 pos, string ss, Color col, float charWidth, float charHeight)
       {
           Rectangle p = new Rectangle((int)pos.X, (int)pos.Y, (int)charWidth, (int)charHeight);
           //string ss = s;

           for (int i = 0; i < ss.Length; i++)
           {
               int k = cvtCharToInt(ss[i]);
               sb.Draw(fntTex[k], p, col);
               p.X = (int)(p.X + charWidth);
           }
       }

       /// <summary>
       /// Draw the string twice with two diferent colours and a small offset to create a shadow effect 
       /// </summary>
       /// <param name="sb"></param>
       /// <param name="pos"></param>
       /// <param name="ss"></param>
       /// <param name="col"></param>
       /// <param name="shadowCol"></param>
       /// <param name="charWidth"></param>
       /// <param name="charHeight"></param>
       /// <param name="xOffset"></param>
       /// <param name="yOffset"></param>
        public void drawStrShadow(SpriteBatch sb, Vector2 pos, string ss, Color col, Color shadowCol, float charWidth, float charHeight, int xOffset, int yOffset)
       {
           //Rectangle p = new Rectangle((int)pos.X, (int)pos.Y, (int)charWidth, (int)charHeight);
           Vector2 pos1 = new Vector2(pos.X+xOffset,pos.Y+yOffset);
           drawStr(sb, pos1, ss, shadowCol, charWidth, charHeight);
            drawStr(sb, pos, ss, col, charWidth, charHeight);
          
       }

        /// <summary>
       /// draw a complete string in a bounding rectangle
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="dest"></param>
        /// <param name="s"></param>
        /// <param name="col"></param>
       public void drawStr(SpriteBatch sb, Rectangle dest, string s, Color col)
       {
           int len = s.Length;
           if (len > 0)
           {
               Vector2 p = new Vector2(dest.X, dest.Y);
               int cw = (int)((float)dest.Width) / len;
               int ch = dest.Height;
               drawStr(sb, p, s, col, (float)cw, (float)ch);
           }
           else { return; }

       }

        /// <summary>
        /// To replace SpriteBatch.DrawString but with similar syntax
        /// always 12x12 aproximating 12 point font
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="text"></param>
        /// <param name="pos"></param>
        /// <param name="col"></param>
       public void drawString(SpriteBatch sb, string text, Vector2 pos, Color col)
       {
           drawStr(sb, pos, text, col, 12, 12);
       }

    }

    // --------------------------------------------------------------------------- SillyFont --------------------------------------

    /// <summary>
    /// Ok a low quality 8x8 font which is defined in code
    /// very handy for debugging and resizing, but not really of comercial quality
    /// </summary>
    public class SillyFont : FontParent
    {
        public RC_Surface[] fnt=null;
        public static Color[] Pallete11; // actually an 8 colour pallete (like old 8 bit sys)
        public static String Pal11;
        //public Texture2D[] fntTex = null;

        public SillyFont(GraphicsDevice gDevice, Color backGroundColor, Color foregroundColor) // recomed Trans white as background color
        {
            fnt = new RC_Surface[charSetSize+charSetSizeExtra]; // 64 characters form 32=space to 95 _ Underscore
                                      // last character is box = unknown
            fntTex = new Texture2D[charSetSize+charSetSizeExtra];

            Pallete11 = new Color[18];

            Pallete11[0] = backGroundColor;    // space   
            Pallete11[1] = foregroundColor;    // 8 white
 
            Pal11 = " 8";

            fnt[0] = new RC_Surface(8, 8, backGroundColor); // 32 Space

            String[] str1 = {"        ", // 33 !
                             "  8     ", // if possible last column is a space
                             "  8     ",
                             "  8     ",
                             "  8     ",
                             "        ",
                             "  8     ",
                             "        "};
            fnt[1] = new RC_Surface(str1, Pallete11, Pal11);

            String[] str2 = {"  8 8   ", // 34  ""
                             "        ",
                             "        ",
                             "        ",
                             "        ",
                             "        ",
                             "        ",
                             "        "};
            fnt[2] = new RC_Surface(str2, Pallete11, Pal11);

            String[] str3 = {"        ", // 35 #
                             "        ",
                             "  8  8  ",
                             " 888888 ",
                             "  8  8  ",
                             " 888888 ",
                             "  8  8  ",
                             "        "};
            fnt[3] = new RC_Surface(str3, Pallete11, Pal11);

            String[] str4 = {"        ", // 36 $
                             "   8    ",
                             "  888   ",
                             " 8      ",
                             "  888   ",
                             "     8  ",
                             "  888   ",
                             "   8    "};
            fnt[4]= new RC_Surface(str4, Pallete11, Pal11);

            String[] str5 = {" 8      ", // 37 %
                             "8 8   8 ",
                             " 8   8  ",
                             "    8   ",
                             "   8    ",
                             "  8  8  ",
                             " 8  8 8 ",
                             "     8  "};
            fnt[5] = new RC_Surface(str5, Pallete11, Pal11);

            String[] str6 = {"   88   ", // 38 &
                             "  8  8  ",
                             " 8  8   ",
                             " 88     ",
                             "8  8    ",
                             " 8  8 8 ",
                             "  8  8  ",
                             "   88 8 "};
            fnt[6] = new RC_Surface(str6, Pallete11, Pal11);

            String[] str7 = {"     8  ", // 39 '
                             "     8  ",
                             "        ",
                             "        ",
                             "        ",
                             "        ",
                             "        ",
                             "        "};
            fnt[7] = new RC_Surface(str7, Pallete11, Pal11);

            String[] str8 = {"    88  ", // 40 (
                             "   8    ",
                             "  8     ",
                             " 8      ",
                             " 8      ",
                             "  8     ",
                             "   8    ",
                             "    88  "};
            fnt[8] = new RC_Surface(str8, Pallete11, Pal11);

            String[] str9 = {"  88    ", // 41 )
                             "    8   ",
                             "     8  ",
                             "      8 ",
                             "      8 ",
                             "     8  ",
                             "    8   ",
                             "  88    "};
            fnt[9] = new RC_Surface(str9, Pallete11, Pal11);

            String[] str10 = {"        ", // 42 *
                              "   8    ",
                              "8888888 ",
                              "  888   ",
                              "  888   ",
                              " 8   8  ",
                              "8     8 ",
                              "        "};
            fnt[10] = new RC_Surface(str10, Pallete11, Pal11);

            String[] str11 = {"        ", // 43 +
                              "   8    ",
                              "   8    ",
                              " 88888  ",
                              "   8    ",
                              "   8    ",
                              "        ",
                              "        "};
            fnt[11] = new RC_Surface(str11, Pallete11, Pal11);

            String[] str12 = {"     8  ", // 44 '
                              "    8   ",
                              "        ",
                              "        ",
                              "        ",
                              "        ",
                              "        ",
                              "        "};
            fnt[12] = new RC_Surface(str12, Pallete11, Pal11);

            String[] str13 = {"        ", // 45 -
                              "        ",
                              "        ",
                              " 888888 ",
                              "        ",
                              "        ",
                              "        ",
                              "        "};
            fnt[13] = new RC_Surface(str13, Pallete11, Pal11);

            String[] str14 = {"        ", // 46 .
                              "        ",
                              "        ",
                              "        ",
                              "        ",
                              "    88  ",
                              "    88  ",
                              "        "};
            fnt[14] = new RC_Surface(str14, Pallete11, Pal11);

            String[] str15 = {"        ", // 47  /
                              "      8 ",
                              "     8  ",
                              "    8   ",
                              "   8    ",
                              "  8     ",
                              " 8      ",
                              "        "};
            fnt[15] = new RC_Surface(str15, Pallete11, Pal11);

            String[] str16 = {"        ", // 48 0
                              " 8888   ",
                              "8    8  ",
                              "8    8  ",
                              "8    8  ",
                              "8    8  ",
                              " 8888   ",
                              "        "};
            fnt[16] = new RC_Surface(str16, Pallete11, Pal11);

            String[] str17 = {"        ", // 49 1
                              "   8    ",
                              "  88    ",
                              "   8    ",
                              "   8    ",
                              "   8    ",
                              "  888   ",
                              "        "};
            fnt[17] = new RC_Surface(str17, Pallete11, Pal11);

            String[] str18 = {"        ", // 50 2
                              "  888   ",
                              " 8   8  ",
                              "     8  ",
                              "   88   ",
                              "  8     ",
                              " 88888  ",
                              "        "};
            fnt[18] = new RC_Surface(str18, Pallete11, Pal11);

            String[] str19 = {"        ", // 51 3
                              "  888   ",
                              "     8  ",
                              "  888   ",
                              "     8  ",
                              "     8  ",
                              "  888   ",
                              "        "};
            fnt[19] = new RC_Surface(str19, Pallete11, Pal11);

            String[] str20 = {"        ", // 52 4   
                              "    88  ",
                              "   8 8  ",
                              "  8  8  ",
                              " 888888 ",
                              "     8  ",
                              "     8  ",
                              "        "};
            fnt[20] = new RC_Surface(str20, Pallete11, Pal11);

            String[] str21 = {"        ", // 53 5
                              " 888888 ",
                              " 8      ",
                              "  8888  ",
                              "      8 ",
                              " 8    8 ",
                              "  8888  ",
                              "        "};
            fnt[21] = new RC_Surface(str21, Pallete11, Pal11);

            String[] str22 = {"        ", // 54 6
                              "    88  ",
                              "   8    ",
                              "  8888  ",
                              " 8    8 ",
                              " 8    8 ",
                              "  8888  ",
                              "        "};
            fnt[22] = new RC_Surface(str22, Pallete11, Pal11);

            String[] str23 = {"        ", //55 7 
                              " 888888 ",
                              "     8  ",
                              "    8   ",
                              "   8    ",
                              "  8     ",
                              " 8      ",
                              "        "};
            fnt[23] = new RC_Surface(str23, Pallete11, Pal11);

            String[] str24 = {"        ", // 56 8
                              "  888   ",
                              " 8   8  ",
                              "  888   ",
                              " 8   8  ",
                              " 8   8  ",
                              "  888   ",
                              "        "};
            fnt[24] = new RC_Surface(str24, Pallete11, Pal11);

            String[] str25 = {"        ", //57 9 
                              "  8888  ",
                              " 8    8 ",
                              " 8    8 ",
                              "  8888  ",
                              "    8   ",
                              "  88    ",
                              "        "};
            fnt[25] = new RC_Surface(str25, Pallete11, Pal11);

            String[] str26 = {"        ", // 58 :
                              "        ",
                              "   88   ",
                              "        ",
                              "        ",
                              "   88   ",
                              "        ",
                              "        "};
            fnt[26] = new RC_Surface(str26, Pallete11, Pal11);

            String[] str27 = {"        ", // 59 ;
                              "        ",
                              "    88  ",
                              "        ",
                              "        ",
                              "    88  ",
                              "   8    ",
                              "        "};
            fnt[27] = new RC_Surface(str27, Pallete11, Pal11);

            String[] str28 = {"        ", // 60 <
                              "        ",
                              "    88  ",
                              "  88    ",
                              " 8      ",
                              "  88    ",
                              "    88  ",
                              "        "};
            fnt[28] = new RC_Surface(str28, Pallete11, Pal11);

            String[] str29 = {"        ", // 61 =
                              "        ",
                              " 88888  ",
                              "        ",
                              " 88888  ",
                              "        ",
                              "        ",
                              "        "};
            fnt[29] = new RC_Surface(str29, Pallete11, Pal11);

            String[] str30 = {"        ", // 62 >
                              "        ",
                              "  88    ",
                              "    88  ",
                              "      8 ",
                              "    88  ",
                              "  88    ",
                              "        "};
            fnt[30] = new RC_Surface(str30, Pallete11, Pal11);

            String[] str31 = {"        ", // 63 ?
                              "  888   ",
                              " 8   8  ",
                              "     8  ",
                              "   88   ",
                              "  88    ",
                              "        ",
                              "  88    "};
            fnt[31] = new RC_Surface(str31, Pallete11, Pal11);

            String[] str32 = {"        ", // 64 @
                              "  8888  ",
                              " 8 88 8 ",
                              " 8 8  8 ",
                              " 8 8888 ",
                              " 8      ",
                              " 8    8 ",
                              "  8888  "};
            fnt[32] = new RC_Surface(str32, Pallete11, Pal11);

            String[] str33 = {"        ", // 65 A
                              "   8    ",
                              "  8 8   ",
                              " 8   8  ",
                              "8     8 ",
                              "8888888 ",
                              "8     8 ",
                              "        "};
            fnt[33] = new RC_Surface(str33, Pallete11, Pal11);

            String[] str34 = {"        ", // 66 B
                              " 88888  ",
                              " 8    8 ",
                              " 8 888  ",
                              " 8    8 ",
                              " 8    8 ",
                              " 88888  ",
                              "        "};
            fnt[34] = new RC_Surface(str34, Pallete11, Pal11);

            String[] str35 = {"        ", //  C  
                              "  8888  ",
                              " 8    8 ",
                              " 8      ",
                              " 8      ",
                              " 8    8 ",
                              "  8888  ",
                              "        "};
            fnt[35] = new RC_Surface(str35, Pallete11, Pal11);

            String[] str36 = {"        ", // D   
                              " 888    ",
                              " 8  88  ",
                              " 8    8 ",
                              " 8    8 ",
                              " 8  88  ",
                              " 888    ",
                              "        "};
            fnt[36] = new RC_Surface(str36, Pallete11, Pal11);

            String[] str37 = {"        ", // E   
                              " 888888 ",
                              " 8      ",
                              " 88888  ",
                              " 8      ",
                              " 8      ",
                              " 888888 ",
                              "        "};
            fnt[37] = new RC_Surface(str37, Pallete11, Pal11);

            String[] str38 = {"        ", // F   
                              " 888888 ",
                              " 8      ",
                              " 88888  ",
                              " 8      ",
                              " 8      ",
                              " 8      ",
                              "        "};
            fnt[38] = new RC_Surface(str38, Pallete11, Pal11);

            String[] str39 = {"        ", // G   
                              "  8888  ",
                              " 8    8 ",
                              " 8      ",
                              " 8  888 ",
                              " 8    8 ",
                              "  8888  ",
                              "        "};
            fnt[39] = new RC_Surface(str39, Pallete11, Pal11);

            String[] str40 = {"        ", // H   
                              " 8    8 ",
                              " 8    8 ",
                              " 888888 ",
                              " 8    8 ",
                              " 8    8 ",
                              " 8    8 ",
                              "        "};
            fnt[40] = new RC_Surface(str40, Pallete11, Pal11);

            String[] str41 = {"        ", // I   
                              " 88888  ",
                              "   8    ",
                              "   8    ",
                              "   8    ",
                              "   8    ",
                              " 88888  ",
                              "        "};
            fnt[41] = new RC_Surface(str41, Pallete11, Pal11);

            String[] str42 = {"        ", // J   
                              "      8 ",
                              "      8 ",
                              "      8 ",
                              " 8    8 ",
                              " 8    8 ",
                              "  8888  ",
                              "        "};
            fnt[42] = new RC_Surface(str42, Pallete11, Pal11);

            String[] str43 = {"        ", // K   
                              " 8  8   ",
                              " 8 8    ",
                              " 88     ",
                              " 8 8    ",
                              " 8  8   ",
                              " 8   8  ",
                              "        "};
            fnt[43] = new RC_Surface(str43, Pallete11, Pal11);

            String[] str44 = {"        ", // L   
                              " 8      ",
                              " 8      ",
                              " 8      ",
                              " 8      ",
                              " 8      ",
                              " 888888 ",
                              "        "};
            fnt[44] = new RC_Surface(str44, Pallete11, Pal11);

            String[] str45 = {"        ", // M   
                              "8     8 ",
                              "88   88 ",
                              "8 8 8 8 ",
                              "8  8  8 ",
                              "8     8 ",
                              "8     8 ",
                              "        "};
            fnt[45] = new RC_Surface(str45, Pallete11, Pal11);

            String[] str46 = {"        ", // N   
                              " 8    8 ",
                              " 88   8 ",
                              " 8 8  8 ",
                              " 8  8 8 ",
                              " 8   88 ",
                              " 8    8 ",
                              "        "};
            fnt[46] = new RC_Surface(str46, Pallete11, Pal11);

            String[] str47 = {"        ", // O   
                              "  88    ",
                              " 8  8   ",
                              "8    8  ",
                              "8    8  ",
                              " 8  8   ",
                              "  88    ",
                              "        "};
            fnt[47] = new RC_Surface(str47, Pallete11, Pal11);

            String[] str48 = {"        ", //P    
                              " 88888  ",
                              " 8    8 ",
                              " 88888  ",
                              " 8      ",
                              " 8      ",
                              " 8      ",
                              "        "};
            fnt[48] = new RC_Surface(str48, Pallete11, Pal11);

            String[] str49 = {"        ", // Q   
                              "  88    ",
                              " 8  8   ",
                              "8    8  ",
                              "8  8 8  ",
                              " 8  8   ",
                              "  88 8  ",
                              "        "};
            fnt[49] = new RC_Surface(str49, Pallete11, Pal11);

            String[] str50 = {"        ", // R   
                              " 88888  ",
                              " 8    8 ",
                              " 88888  ",
                              " 8 8    ",
                              " 8  8   ",
                              " 8   8  ",
                              "        "};
            fnt[50] = new RC_Surface(str50, Pallete11, Pal11);

            String[] str51 = {"        ", // S   
                              "  8888  ",
                              " 8    8 ",
                              "  88    ",
                              "    88  ",
                              "     88 ",
                              "  8888  ",
                              "        "};
            fnt[51] = new RC_Surface(str51, Pallete11, Pal11);

            String[] str52 = {"        ", //T    
                              " 88888  ",
                              "   8    ",
                              "   8    ",
                              "   8    ",
                              "   8    ",
                              "   8    ",
                              "        "};
            fnt[52] = new RC_Surface(str52, Pallete11, Pal11);

            String[] str53 = {"        ", // U   
                              " 8    8 ",
                              " 8    8 ",
                              " 8    8 ",
                              " 8    8 ",
                              " 8    8 ",
                              "  8888  ",
                              "        "};
            fnt[53] = new RC_Surface(str53, Pallete11, Pal11);

            String[] str54 = {"        ", // V   
                              " 8    8 ",
                              " 8    8 ",
                              " 8    8 ",
                              "  8  8  ",
                              "  8  8  ",
                              "   88   ",
                              "        "};
            fnt[54] = new RC_Surface(str54, Pallete11, Pal11);

            String[] str55 = {"        ", //W    
                              "8     8 ",
                              "8  8  8 ",
                              "8  8  8 ",
                              "8  8  8 ",
                              "8  8  8 ",
                              " 88 88  ",
                              "        "};
            fnt[55] = new RC_Surface(str55, Pallete11, Pal11);

            String[] str56 = {"        ", //X    
                              " 8    8 ",
                              "  8  8  ",
                              "   88   ",
                              "   88   ",
                              "  8  8  ",
                              " 8    8 ",
                              "        "};
            fnt[56] = new RC_Surface(str56, Pallete11, Pal11);

            String[] str57 = {"        ", // Y   
                              " 8    8 ",
                              "  8  8  ",
                              "   88   ",
                              "   8    ",
                              "   8    ",
                              "   8    ",
                              "        "};
            fnt[57] = new RC_Surface(str57, Pallete11, Pal11);

            String[] str58 = {"        ", // Z   
                              " 888888 ",
                              "     8  ",
                              "    8   ",
                              "   8    ",
                              "  8     ",
                              " 888888 ",
                              "        "};
            fnt[58] = new RC_Surface(str58, Pallete11, Pal11);

            String[] str59 = {" 8888   ", // 91 [
                              " 8      ",
                              " 8      ",
                              " 8      ",
                              " 8      ",
                              " 8      ",
                              " 8      ",
                              " 8888   "};
            fnt[59] = new RC_Surface(str59, Pallete11, Pal11);

            String[] str60 = {"        ", // 92 \
                              " 8      ",
                              "  8     ",
                              "   8    ",
                              "    8   ",
                              "     8  ",
                              "      8 ",
                              "        "};
            fnt[60] = new RC_Surface(str60, Pallete11, Pal11);

            String[] str61 = {"  8888  ", // 93 ]
                              "     8  ",
                              "     8  ",
                              "     8  ",
                              "     8  ",
                              "     8  ",
                              "     8  ",
                              "  8888  "};
            fnt[61] = new RC_Surface(str61, Pallete11, Pal11);

            String[] str62 = {"        ", // 94 ^
                              "  88    ",
                              " 8  8   ",
                              "        ",
                              "        ",
                              "        ",
                              "        ",
                              "        "};
            fnt[62] = new RC_Surface(str62, Pallete11, Pal11);

            String[] str63 = {"        ", // 95 _ underscore
                              "        ",
                              "        ",
                              "        ",
                              "        ",
                              "        ",
                              "        ",
                              " 888888 "};
            fnt[63] = new RC_Surface(str63, Pallete11, Pal11);

            String[] str64 = {"    8   ", // 96 back apostrophe
                              "     8  ",
                              "        ",
                              "        ",
                              "        ",
                              "        ",
                              "        ",
                              "        "};
            fnt[64] = new RC_Surface(str64, Pallete11, Pal11);

            String[] str65 = {"        ", // 97 a
                              "        ",
                              "   88   ",
                              "     8  ",
                              "  8888  ",
                              " 8   8  ",
                              "  888 8 ",
                              "        "};
            fnt[65] = new RC_Surface(str65, Pallete11, Pal11);

            String[] str66 = {"        ", // 98 b
                              " 8      ",
                              " 8      ",
                              " 8 888  ",
                              " 88   8 ",
                              " 88   8 ",
                              " 8 888  ",
                              "        "};
            fnt[66] = new RC_Surface(str66, Pallete11, Pal11);

            String[] str67 = {"        ", // 99 c
                              "        ",
                              "        ",
                              "  8888  ",
                              " 8      ",
                              " 8      ",
                              "  8888  ",
                              "        "};
            fnt[67] = new RC_Surface(str67, Pallete11, Pal11);

            String[] str68 = {"        ", // 100 d
                              "      8 ",
                              "      8 ",
                              "  888 8 ",
                              " 8   88 ",
                              " 8   88 ",
                              "  888 8 ",
                              "        "};
            fnt[68] = new RC_Surface(str68, Pallete11, Pal11);

            String[] str69 = {"        ", // 101 e
                              "        ",
                              "  88    ",
                              " 8  8   ",
                              " 8888   ",
                              " 8      ",
                              "  888   ",
                              "        "};
            fnt[69] = new RC_Surface(str69, Pallete11, Pal11);

            String[] str70 = {"        ", // 102 f
                              "        ",
                              "  888   ",
                              " 8      ",
                              " 8888   ",
                              " 8      ",
                              " 8      ",
                              "        "};
            fnt[70] = new RC_Surface(str70, Pallete11, Pal11);

            String[] str71 = {"        ", // 103 g
                              "        ",
                              "        ",
                              "  888   ",
                              " 8   8  ",
                              "  8888  ",
                              "     8  ",
                              "  888   "};
            fnt[71] = new RC_Surface(str71, Pallete11, Pal11);

            String[] str72 = {"        ", // 104 h
                              " 8      ",
                              " 8      ",
                              " 8      ",
                              " 8 88   ",
                              " 88  8  ",
                              " 8   8  ",
                              "        "};
            fnt[72] = new RC_Surface(str72, Pallete11, Pal11);

            String[] str73 = {"        ", // 105 i
                              "   8    ",
                              "        ",
                              "   8    ",
                              "   8    ",
                              "   8    ",
                              "   8    ",
                              "        "};
            fnt[73] = new RC_Surface(str73, Pallete11, Pal11);

            String[] str74 = {"        ", // 106 j
                              "   8    ",
                              "        ",
                              "   8    ",
                              "   8    ",
                              "   8    ",
                              "8  8    ",
                              " 88     "};
            fnt[74] = new RC_Surface(str74, Pallete11, Pal11);

            String[] str75 = {"        ", // 107 k
                              "        ",
                              " 8   8  ",
                              " 8  8   ",
                              " 888    ",
                              " 8  8   ",
                              " 8   8  ",
                              "        "};
            fnt[75] = new RC_Surface(str75, Pallete11, Pal11);

            String[] str76 = {"        ", // 108 l
                              "        ",
                              "   8    ",
                              "   8    ",
                              "   8    ",
                              "   8    ",
                              "   8    ",
                              "        "};
            fnt[76] = new RC_Surface(str76, Pallete11, Pal11);

            String[] str77 = {"        ", // 109 m
                              "        ",
                              "        ",
                              " 88 88  ",
                              "8  8  8 ",
                              "8  8  8 ",
                              "8     8 ",
                              "        "};
            fnt[77] = new RC_Surface(str77, Pallete11, Pal11);

            String[] str78 = {"        ", // 110 n
                              "        ",
                              "        ",
                              " 8 88   ",
                              " 88  8  ",
                              " 8   8  ",
                              " 8   8  ",
                              "        "};
            fnt[78] = new RC_Surface(str78, Pallete11, Pal11);

            String[] str79 = {"        ", // 111 o
                              "        ",
                              "        ",
                              "  888   ",
                              " 8   8  ",
                              " 8   8  ",
                              "  888   ",
                              "        "};
            fnt[79] = new RC_Surface(str79, Pallete11, Pal11);

            String[] str80 = {"        ", // 112 p          
                              "        ",
                              "        ",
                              " 888    ",
                              " 8  8   ",
                              " 888    ",
                              " 8      ",
                              " 8      "};
            fnt[80] = new RC_Surface(str80, Pallete11, Pal11);

            String[] str81 = {"        ", // q              
                              "        ",
                              "        ",
                              "   888  ",
                              "  8  8  ",
                              "   888  ",
                              "     8  ",
                              "     88 "};
            fnt[81] = new RC_Surface(str81, Pallete11, Pal11);

            String[] str82 = {"        ", // r              
                              "        ",
                              " 8      ",
                              " 8 88   ",
                              " 88  8  ",
                              " 8      ",
                              " 8      ",
                              "        "};
            fnt[82] = new RC_Surface(str82, Pallete11, Pal11);

            String[] str83 = {"        ", // s              
                              "        ",
                              "  888   ",
                              " 8      ",
                              "  888   ",
                              "     8  ",
                              "  888   ",
                              "        "};
            fnt[83] = new RC_Surface(str83, Pallete11, Pal11);

            String[] str84 = {"        ", // t              
                              "        ",
                              "   8    ",
                              " 88888  ",
                              "   8    ",
                              "   8    ",
                              "   8    ",
                              "        "};
            fnt[84] = new RC_Surface(str84, Pallete11, Pal11);

            String[] str85 = {"        ", // u              
                              "        ",
                              "        ",
                              " 8   8  ",
                              " 8   8  ",
                              " 8   8  ",
                              "  888   ",
                              "        "};
            fnt[85] = new RC_Surface(str85, Pallete11, Pal11);

            String[] str86 = {"        ", // v              
                              "        ",
                              "        ",
                              "8     8 ",
                              " 8   8  ",
                              "  8 8   ",
                              "   8    ",
                              "        "};
            fnt[86] = new RC_Surface(str86, Pallete11, Pal11);

            String[] str87 = {"        ", // w              
                              "        ",
                              "        ",
                              "8     8 ",
                              "8  8  8 ",
                              "8  8  8 ",
                              " 88 88  ",
                              "        "};
            fnt[87] = new RC_Surface(str87, Pallete11, Pal11);

            String[] str88 = {"        ", // x              
                              "        ",
                              " 8   8  ",
                              "  8 8   ",
                              "   8    ",
                              "  8 8   ",
                              " 8   8  ",
                              "        "};
            fnt[88] = new RC_Surface(str88, Pallete11, Pal11);

            String[] str89 = {"        ", //y               
                              "        ",
                              "        ",
                              " 8    8 ",
                              "  8  8  ",
                              "   88   ",
                              "   8    ",
                              " 88     "};
            fnt[89] = new RC_Surface(str89, Pallete11, Pal11);

            String[] str90 = {"        ", // z              
                              "        ",
                              "        ",
                              " 88888  ",
                              "    8   ",
                              "   8    ",
                              " 88888  ",
                              "        "};
            fnt[90] = new RC_Surface(str90, Pallete11, Pal11);

            String[] str91 = {"   8    ", // 123 {          
                              "  8     ",
                              "  8     ",
                              " 8      ",
                              "  8     ",
                              "  8     ",
                              "   8    ",
                              "        "};
            fnt[91] = new RC_Surface(str91, Pallete11, Pal11);

            String[] str92 = {"        ", // 124 |          
                              "   8    ",
                              "   8    ",
                              "   8    ",
                              "        ",
                              "   8    ",
                              "   8    ",
                              "   8    "};
            fnt[92] = new RC_Surface(str92, Pallete11, Pal11);

            String[] str93 = {"    8   ", // 125 }          
                              "     8  ",
                              "     8  ",
                              "      8 ",
                              "     8  ",
                              "     8  ",
                              "    8   ",
                              "        "};
            fnt[93] = new RC_Surface(str93, Pallete11, Pal11);

            String[] str94 = {"        ", // 126 ~          
                              "        ",
                              "  8     ",
                              " 8 8 8  ",
                              "    8   ",
                              "        ",
                              "        ",
                              "        "};
            fnt[94] = new RC_Surface(str94, Pallete11, Pal11);

            String[] str95 = {"        ", // Del            
                              "        ",
                              "        ",
                              "        ",
                              " 8 8 8  ",
                              " 8 8 8  ",
                              " 8 8 8  ",
                              "        "};
            fnt[95] = new RC_Surface(str95, Pallete11, Pal11);


            String[] str96= {"8888888 ", // 95 _ underscore
                             "8     8 ",
                             "8     8 ",
                             "8     8 ",
                             "8     8 ",
                             "8     8 ",
                             "8     8 ",
                             "8888888 "};
            fnt[96] = new RC_Surface(str96, Pallete11, Pal11);

            for (int i = 0; i < charSetSize + charSetSizeExtra; i++)
            {
                fntTex[i] = fnt[i].createTex(gDevice);
            }
        }

    }

    // --------------------------------------------------------- SillyFont16 -----------------------------------------------------------------------

    /// <summary>
    /// Simply the 8by8 font doubled in size - it tends to blur less when resized 
    /// </summary>
    public class SillyFont16 : FontParent
    {
        SillyFont sf = null;

        public override int charwidth() { return 16; } 

        public SillyFont16(GraphicsDevice gDevice, Color backGroundColor, Color foregroundColor)
        {
            sf=new SillyFont( gDevice,  backGroundColor,  foregroundColor);
            fntTex = new Texture2D[charSetSize + charSetSizeExtra];
            
            for (int i = 0; i < charSetSize + charSetSizeExtra; i++)
            {
                RC_Surface sur;
                sur = new RC_Surface( sf.fnt[i], 2, 2, 0, 0, Color.White, 0, 0);

                fntTex[i] = sur.createTex(gDevice);
            }
        }    
    }

}



// end
