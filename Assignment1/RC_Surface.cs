using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace RC_Framework
{
    /// <summary>
    /// The Purpose of this class is to provide a canvas (perhaps it sould have been labled RC_canvas)
    /// It provides a few routines to work with actual pixels witout needing to load an image 
    /// use createTex(GraphicsDevice device) to convert it to a texture which is what you need to actually render the surface
    /// Surfaces are always rectanglar 
    /// </summary>
    public class RC_Surface
    {
        Color[] data;
        int w; //width
        int h; // height    

        /// <summary>
        /// Default constructor produces 1 white pixel
        /// the idea being that you then run init to get a meaningfull surface
        /// </summary>
        public RC_Surface()
        {
            // lol you better call init if you use this or else you get a white pixel
            init(1, 1, Color.White, Color.White, Color.White, Color.White);
        }

        public RC_Surface(int width, int height, Color c)
        {
            init(width, height, c, c, c, c);
        }

        public RC_Surface(int width, int height, Color topLeftC, Color topRightC, Color botLeftC, Color botRightC)
        {
            init(width, height, topLeftC, topRightC, botLeftC, botRightC);
        }

        public RC_Surface(String[] lines, Color[] pal, String palIndex)
        {
            int ww = lines[0].Length;
            int hh = lines.Length;
            init(ww, hh, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent);
            // now set pixels 
            for (int y = 0; y < hh; y++)
            {
                String s = lines[y];
                for (int x = 0; x < ww; x++)
                {
                    char ch = s[x];
                    int j = palIndex.IndexOf(ch);
                    if (j != -1)
                    {
                        setColor(x, y, pal[j]);
                    }
                }
            }
        }

        /// <summary>
        /// Enlarges a surface - possibly with a gap between pixels and with the possibility of padding it out a bit to get a power of 2
        /// </summary>
        /// <param name="sur"></param>
        /// <param name="xFactor"></param>
        /// <param name="yFactor"></param>
        /// <param name="xGap"></param>
        /// <param name="yGap"></param>
        /// <param name="defColor"></param>
        /// <param name="xPad"></param>
        /// <param name="yPad"></param>
        public RC_Surface(RC_Surface sur, int xFactor, int yFactor, int xGap, int yGap, Color defColor, int xPad, int yPad)
        {
            int ww = sur.w * xFactor + (sur.w - 1) * xGap + xPad;
            int hh = sur.h * yFactor + (sur.h - 1) * yGap + yPad;
            init(ww, hh, defColor, defColor, defColor, defColor);
            for (int x = 0; x < sur.w; x++)
            {
                for (int y = 0; y < sur.h; y++)
                {
                    for (int xx = 0; xx < xFactor; xx++)
                    {
                        for (int yy = 0; yy < yFactor; yy++)
                        {
                            int xxx = x * xFactor + xx + (xGap * x);
                            int yyy = y * yFactor + yy + (yGap * y);
                            Color c = sur.getColor(x, y);
                            setColor(xxx, yyy, c);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Initialise a colour field with a diferent colour at each corner
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="topLeftC"></param>
        /// <param name="topRightC"></param>
        /// <param name="botLeftC"></param>
        /// <param name="botRightC"></param>
        public void init(int width, int height,
                         Color topLeftC, Color topRightC, Color botLeftC, Color botRightC)
        {
            w = width;
            h = height;
            data = new Color[width * height];
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    float lerpValX = (float)x / (float)width;
                    float lerpValY = (float)y / (float)height;
                    Color cTop = Color.Lerp(topLeftC, topRightC, lerpValX);
                    Color cBot = Color.Lerp(botLeftC, botRightC, lerpValX);
                    data[x + (y * width)] = Color.Lerp(cTop, cBot, lerpValY);
                }
        }

        /// <summary>
        /// Set a pixel colour
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="c"></param>
        public void setColor(int x, int y, Color c)
        {
            data[x + (y * w)] = c;
        }

        /// <summary>
        /// get or retreive a pixel colour
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public Color getColor(int x, int y)
        {
            Color c = data[x + (y * w)];
            return c;
        }

        /// <summary>
        /// Copies the surface to an already existing texture
        /// </summary>
        /// <param name="tex"></param>
        public void setInTexture2D(Texture2D tex)
        {
            tex.SetData(data);
        }

        /// <summary>
        /// gets pixel data from an existing texture
        /// </summary>
        /// <param name="tex"></param>
        public void getFromTexture2D(Texture2D tex)
        {
            w = tex.Width;
            h = tex.Height;
            data = new Color[w * h];
            tex.GetData(data);
        }

        /// <summary>
        /// Draws a 1 pixel wide rectangle of a given colour on the surface
        /// </summary>
        /// <param name="r"></param>
        /// <param name="c"></param>
        public void addLineRectangle(Rectangle r, Color c)
        {
            drawHline(r.X, r.Y, r.X + r.Width - 1, c, c);
            drawHline(r.X, r.Y + r.Height - 1, r.X + r.Width - 1, c, c);
            drawVline(r.X, r.Y, r.Y + r.Height - 1, c, c);
            drawVline(r.X + r.Width - 1, r.Y, r.Y + r.Height - 1, c, c);
        }

 
        /// <summary>
        /// Draws a filled rectangle of a given colour 
        ///(can be slow. it often is if its any significant size)
        /// </summary>
        /// <param name="r"></param>
        /// <param name="c"></param>
        public void addFillRectangle(Rectangle r, Color c)
        {
            for (int y = 0; y < r.Height; y++)
            {
                drawHline(r.X, r.Y + y, r.X + r.Width - 1, c, c);
            }
        }

        /// <summary>
        /// Sets a colour in the entire surface
        /// </summary>
        /// <param name="c"></param>
        public void setColor(Color c)
        {
            addFillRectangle(new Rectangle(0, 0, w, h), c);
        }

        /// <summary>
        /// Draws a coloured border on the surface
        /// </summary>
        /// <param name="borderWidth"></param>
        /// <param name="borderColor"></param>
        public void addBorder(int borderWidth, Color borderColor)
        {
            for (int i = 0; i < borderWidth; i++)
            {
                addLineRectangle(new Rectangle(0 + i, 0 + i, w - i * 2, h - i * 2), borderColor);
            }
        }

        /// <summary>
        /// Adds a border in which the top and left are a diferent colour to the rest of the border
        /// can be used to create a 3D look           
        /// 
        /// The following lines are a hint on how to get a 3d border look
        /// Color cLight = Util.lighterOrDarker(c, 1.4f);
        /// Color cDark = Util.lighterOrDarker(c, 0.6f);
        /// </summary>
        /// <param name="r"></param>
        /// <param name="cLeftTop"></param>
        /// <param name="cBotRight"></param>
        /// <param name="bWidth"></param>
        public void addSculptedBorder(Rectangle r, Color cLeftTop, Color cBotRight, int bWidth)
        {
            //Color cLight = Util.lighterOrDarker(c, 1.4f);
            //Color cDark = Util.lighterOrDarker(c, 0.6f);

            for (int i = 0; i < bWidth; i++)
            {
                drawHline(r.X + i, r.Y + i, r.X + r.Width - i - 1, cLeftTop, cLeftTop);
                drawVline(r.X + i, r.Y + i, r.Y + r.Height - i - 1, cLeftTop, cLeftTop);
                drawVline(r.X - i + r.Width - 1, r.Y + i + 1, r.Y + r.Height - i - 1, cBotRight, cBotRight);
                drawHline(r.X + i, r.Y + r.Height - i - 1, r.X + r.Width - i - 1, cBotRight, cBotRight);
            }
        }

        /// <summary>
        /// Draws a horizontal line
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y"></param>
        /// <param name="x2"></param>
        /// <param name="x1C"></param>
        /// <param name="x2C"></param>
        public void drawHline(int x1, int y, int x2, Color x1C, Color x2C)
        {
            if (x1 < 0) x1 = 0;
            if (x2 < 0) x2 = 0;
            if (y < 0) y = 0;
            if (x1 >= w) x1 = w - 1;
            if (x2 >= w) x2 = w - 1;
            if (y >= h) y = h - 1;
            int xLow, xHigh;
            xLow = Math.Min(x1, x2);
            xHigh = Math.Max(x1, x2);
            for (int x = xLow; x <= xHigh; x++)
            {
                float lerpVal = (float)(x - xLow) / (float)(xHigh - xLow);
                Color c = Color.Lerp(x1C, x2C, lerpVal);
                setColor(x, y, c);
            }
        }

        /// <summary>
        /// Draws a vertical line
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y1"></param>
        /// <param name="y2"></param>
        /// <param name="y1C"></param>
        /// <param name="y2C"></param>
        public void drawVline(int x, int y1, int y2, Color y1C, Color y2C)
        {
            if (x < 0) x = 0;
            if (y2 < 0) y2 = 0;
            if (y1 < 0) y1 = 0;
            if (x >= w) x = w - 1;
            if (y2 >= h) y2 = h - 1;
            if (y1 >= h) y1 = h - 1;
            int yLow, yHigh;
            yLow = Math.Min(y1, y2);
            yHigh = Math.Max(y1, y2);
            for (int y = yLow; y <= yHigh; y++)
            {
                float lerpVal = (float)(y - yLow) / (float)(yHigh - yLow);
                Color c = Color.Lerp(y1C, y2C, lerpVal);
                setColor(x, y, c);
            }
        }

        /// <summary>
        /// creates a new texture that has a copy of the existing surface image
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        public Texture2D createTex(GraphicsDevice device)
        {
            Texture2D tex = new Texture2D(device, w, h, false, SurfaceFormat.Color);
            tex.SetData<Color>(data);
            return tex;
        }
    }

}
