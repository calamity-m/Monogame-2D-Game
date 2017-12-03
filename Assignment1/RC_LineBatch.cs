// Linebatch.cs
// originally written by David Amador and made available on 26th jan 2010 on http://www.david-amador.com/2010/01/drawing-lines-in-xna/
// Modified by R.Cox March 2011 for use in RC_Framework
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework.Storage;



namespace RC_Framework
{
    /// <summary>
    /// Line Batch
    /// For drawing lines in a spritebatch
    /// </summary>
    static public class LineBatch
    {
        /// <summary>
        /// Just a single white pixel texture
        /// </summary>
        static public Texture2D _empty_texture;
        //static private bool      _set_data = false;

        /// <summary>
        /// Call this with a GraphicsDevice to initialise the default white texture
        /// </summary>
        /// <param name="device"></param>
        static public void init(GraphicsDevice device)
        {
            _empty_texture = new Texture2D(device, 1, 1, false, SurfaceFormat.Color);
            _empty_texture.SetData(new[] { Color.White });
            //_set_data = true;
        }

        /// <summary>
        /// Draw a single line 
        /// </summary>
        /// <param name="batch"></param>
        /// <param name="color"></param>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        static public void drawLine(SpriteBatch batch, Color color,
                                    Vector2 point1, Vector2 point2)
        {

            drawLine(batch, color, point1, point2, 0);
        }

        /// <summary>
        /// Draw a sinle line
        /// </summary>
        /// <param name="batch"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <param name="color"></param>
        static public void drawLine(SpriteBatch batch, float x1, float y1, float x2, float y2, Color color)
        {

            drawLine(batch, color, new Vector2(x1,y1), new Vector2(x2,y2), 0);
        }

        /// <summary>
        /// Draw a rectange as a line 
        /// </summary>
        /// <param name="batch"></param>
        /// <param name="r"></param>
        /// <param name="c"></param>
        static public void drawLineRectangle(SpriteBatch batch, Rectangle r, Color c)
        {
            drawLine(batch, c, new Vector2(r.X, r.Y), new Vector2(r.X + r.Width, r.Y), 0);
            drawLine(batch, c, new Vector2(r.X, r.Y), new Vector2(r.X, r.Y + r.Height), 0);
            drawLine(batch, c, new Vector2(r.X + r.Width, r.Y), new Vector2(r.X + r.Width, r.Y + r.Height), 0);
            drawLine(batch, c, new Vector2(r.X, r.Y + r.Height), new Vector2(r.X + r.Width, r.Y + r.Height), 0);
        }

        
        /// <summary>
        /// Draw a line rectange around a rectangle
        /// the rectange is 1 pixel wide and 1 pixel outside therectangel 
        /// </summary>
        /// <param name="batch"></param>
        /// <param name="r"></param>
        /// <param name="c"></param>        
        static public void drawLineRectangleOuter(SpriteBatch batch, Rectangle r, Color c)
        {
            r.X = r.X - 1;
            r.Y = r.Y - 1;
            r.Width = r.Width + 2;
            r.Height = r.Height+2;
            drawLine(batch, c, new Vector2(r.X, r.Y), new Vector2(r.X + r.Width, r.Y), 0);
            drawLine(batch, c, new Vector2(r.X, r.Y + r.Height), new Vector2(r.X, r.Y), 0);
            drawLine(batch, c, new Vector2(r.X + r.Width, r.Y), new Vector2(r.X + r.Width, r.Y + r.Height), 0);
            drawLine(batch, c, new Vector2(r.X + r.Width, r.Y + r.Height), new Vector2(r.X, r.Y + r.Height), 0);
        }

        /// <summary>
        /// Draw a filled rectangle 
        /// </summary>
        /// <param name="batch"></param>
        /// <param name="r"></param>
        /// <param name="c"></param>
        static public void drawFillRectangle(SpriteBatch batch, Rectangle r, Color c)
        {
            //drawLine(batch, c, new Vector2(r.X, r.Y), new Vector2(r.X + r.Width, r.Y), 0);
            //drawLine(batch, c, new Vector2(r.X, r.Y), new Vector2(r.X, r.Y + r.Height), 0);
            //drawLine(batch, c, new Vector2(r.X + r.Width, r.Y), new Vector2(r.X + r.Width, r.Y + r.Height), 0);
            //drawLine(batch, c, new Vector2(r.X, r.Y + r.Height), new Vector2(r.X + r.Width, r.Y + r.Height), 0);
            batch.Draw(_empty_texture, r, c);
        }

        /// <summary>
        /// Draw a filled rectangle with a sculpted boorder
        /// </summary>
        /// <param name="batch"></param>
        /// <param name="r"></param>
        /// <param name="c"></param>
        /// <param name="bWidth"></param>
        static public void drawFillRectangleBorder(SpriteBatch batch, Rectangle r, Color c, int bWidth)
        {
            batch.Draw(_empty_texture, r, c);
            Color cLight = Util.lighterOrDarker(c, 1.4f);
            Color cDark = Util.lighterOrDarker(c, 0.6f);

            for (int i = 0; i < bWidth; i++)
            {
                drawLine(batch, cLight, new Vector2(r.X+i, r.Y+i), new Vector2(r.X + r.Width-i, r.Y+i), 0);
                drawLine(batch, cLight, new Vector2(r.X+i, r.Y+i), new Vector2(r.X+i, r.Y + r.Height-i), 0);
                drawLine(batch, cDark, new Vector2(r.X-i + r.Width, r.Y+i), new Vector2(r.X + r.Width-i, r.Y + r.Height-i), 0);
                drawLine(batch, cDark, new Vector2(r.X+i, r.Y + r.Height-i), new Vector2(r.X + r.Width-i, r.Y + r.Height-i), 0);
            }
        }

        /// <summary>
        /// Draw a line cross like and X
        /// </summary>
        /// <param name="batch"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="size"></param>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        static public void drawCrossX(SpriteBatch batch, float x1, float y1, float size, Color c1, Color c2)
        {
            drawLine(batch, c1, new Vector2(x1 - size, y1 - size), new Vector2(x1+size, y1+size), 0);
            drawLine(batch, c2, new Vector2(x1 + size, y1 - size), new Vector2(x1-size, y1+size), 0);
        }

        /// <summary>
        /// Draw a line cross like a +
        /// </summary>
        /// <param name="batch"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="size"></param>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        static public void drawCross(SpriteBatch batch, float x1, float y1, float size, Color c1, Color c2)
        {
            drawLine(batch, c1, new Vector2(x1 , y1 - size), new Vector2(x1 , y1+size), 0);
            drawLine(batch, c2, new Vector2(x1 + size, y1), new Vector2(x1-size, y1), 0);
        }

        /// <summary>
        /// Draw an arbitary 4 sided figure from a rect 4 structure 
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="r"></param>
        /// <param name="c"></param>
        static public void drawRect4(SpriteBatch sb, Rect4 r, Color c)
        {
            drawLine(sb, c, new Vector2(r.point[0].X, r.point[0].Y), new Vector2(r.point[1].X, r.point[1].Y), 0);
            drawLine(sb, c, new Vector2(r.point[1].X, r.point[1].Y), new Vector2(r.point[2].X, r.point[2].Y), 0);
            drawLine(sb, c, new Vector2(r.point[2].X, r.point[2].Y), new Vector2(r.point[3].X, r.point[3].Y), 0);
            drawLine(sb, c, new Vector2(r.point[3].X, r.point[3].Y), new Vector2(r.point[0].X, r.point[0].Y), 0);
        }

        /// <summary>
        /// Draws a polygon12 
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="r"></param>
        /// <param name="c"></param>
        static public void drawPolygon12(SpriteBatch sb, Polygon12 r, Color c)
        {
            for (int i = 0; i < r.numOfPoints - 1; i++)
            {
                drawLine(sb, c, new Vector2(r.point[i].X, r.point[i].Y), new Vector2(r.point[i+1].X, r.point[i+1].Y), 0);
            }
            drawLine(sb, c, new Vector2(r.point[r.numOfPoints - 1].X, r.point[r.numOfPoints - 1].Y), new Vector2(r.point[0].X, r.point[0].Y), 0);
        }

        /// <summary>
        /// Draw a line circle 
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="c"></param>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        /// <param name="sides"></param>
        /// <param name="xScale"></param>
        static public void drawCircle(SpriteBatch sb, Color c, Vector2 center, float radius, int sides, float xScale)
        {
           Vector2 p=new Vector2(center.X,center.Y+radius);
           Vector2 prev=new Vector2(0,0);

            for (int i = 0; i <= sides; i++)
            {
             Vector2 pp=Util.rotatePoint(p,center,(float)(Math.PI*2/(sides-1)*i));
             if (i != 0)
             {
             drawLine(sb, c, pp, prev, 0); 
             }
             prev = pp;   
            }

        }

        //static public void drawLine(SpriteBatch batch, float X1, float Y1, float X2, float Y2, Color cLine)
        //{
        //    DrawLine(batch, cLine, new Vector2(X1,Y1), new Vector2(X2,Y2), 0);
        //}

        /// <summary>
        /// Draw a line into a SpriteBatch
        /// </summary>
        /// <param name="batch">SpriteBatch to draw line</param>
        /// <param name="color">The line color</param>
        /// <param name="point1">Start Point</param>
        /// <param name="point2">End Point</param>
        /// <param name="Layer">Layer or Z position</param>
        static public void drawLine(SpriteBatch batch, Color color, Vector2 point1,
                                    Vector2 point2, float Layer)
        {
            //Check if data has been set for texture
            //Do this only once otherwise
           //if (!_set_data)
            //{
            //   messagebox here
            //    _empty_texture.SetData(new[] { Color.White });
            //    _set_data = true;
            //}

            double angle = (double)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            float length = (point2 - point1).Length();

            batch.Draw(_empty_texture, point1, null, color,
                       (float)angle, Vector2.Zero, new Vector2(length, 1),
                       SpriteEffects.None, Layer);
        }
    }
}