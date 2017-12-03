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
    public class ColorTicker
    {

        Color finalColour;
        Color initColour;
        int fadeTicks;
        public int loop; // 0=end (stay final colour), 1=Loop, 2=reverse
        int ticks;
        bool reverse;
        float lerp;
        Color curColour;
        
        public ColorTicker()
        {
            ticks = 0;
            fadeTicks = 10;        
            finalColour = Color.Black;
            initColour = Color.White;        
            reverse = false;
            lerp = 0; 
            curColour = initColour;
            loop = 0;
        }

        public ColorTicker(ColorTicker c)
        {
        finalColour = c.finalColour;
        initColour = c.initColour;
        ticks = c.ticks;
        fadeTicks = c.fadeTicks;
        loop = c.loop; // 0=end (stay final colour), 1=Loop, 2=reverse
        reverse = c.reverse;
        lerp = c.lerp;
        curColour = c.curColour;
        }

        public ColorTicker(Color fromColour, Color toColour, int fadeTicksQ)
        {
            finalColour = toColour;
            initColour = fromColour;
            ticks = 0;
            fadeTicks = fadeTicksQ;
            loop = 0; // 0=end (stay final colour), 1=Loop, 2=reverse
            reverse = false;
            lerp = 0;
            curColour = initColour;
        }

        public ColorTicker(Color fromColour, Color toColour, float secondsQ, int ticksPerSecond)
        {
            finalColour = toColour;
            initColour = fromColour;
            ticks = 0;
            fadeTicks = (int)(secondsQ * ticksPerSecond);
            loop = 0; // 0=end (stay final colour), 1=Loop, 2=reverse
            reverse = false;
            lerp = 0;
            curColour = initColour;
        }

        public bool finished()
        {
            if (ticks > fadeTicks) return true;
            return false;
        }

        public void reset()
        {
            ticks = 0;
            curColour = initColour;
            reverse = false;
        }

        public Color currColor()
        {
            return curColour;
        }

        public Color currColorInverse()
        {
            lerp = (float)ticks / (float)fadeTicks;
            if (!reverse) lerp = 1 - lerp;

            Color curColourI = Color.Lerp(initColour, finalColour, lerp);
            return curColourI;
        }

        public void Update()
        {
            ticks++;
            if (ticks > fadeTicks)
            {
                if (loop == 0)
                {
                    return;
                }
                if (loop == 1)
                {
                    ticks = 0;
                    return;
                }
                if (loop == 2)
                {
                    ticks = 0;
                    reverse = !reverse;
                    return;
                }


            }
            lerp = (float)ticks / (float)fadeTicks;
            if (reverse) lerp = 1 - lerp;

            curColour = Color.Lerp(initColour, finalColour, lerp);
        }

        public void setLoop(int loopQ)
        {
            loop = loopQ;
        }
    }


    public class Polygon12
    {
        /// <summary>
        /// The data in the poly12 class called 
        /// </summary>
        public Vector2[] point;

        public int numOfPoints=0;
        public const int maxNumOfPoints = 12;

        /// <summary>
        /// Default constructor (0,0) (0,0) (0,0) (0,0)</summary>
        public Polygon12()
        {
            point = new Vector2[maxNumOfPoints];
            //for (int i = 0; i < maxNumOfPoints; i++) // i beleive thgis happens automatically in C#
            //{
            //    point[i].X = 0;
            //    point[i].Y = 0;
            //}
        }

        /// <summary>
        /// Construct from Rectangle clockwise winding
        /// </summary>
        /// <param name="r"></param>
        public Polygon12(Rectangle r)
        {
            point = new Vector2[maxNumOfPoints];
            numOfPoints = 4;
            point[0].X = r.Left;
            point[0].Y = r.Top;

            point[1].X = r.Right;
            point[1].Y = r.Top;

            point[2].X = r.Right;
            point[2].Y = r.Bottom;

            point[3].X = r.Left;
            point[3].Y = r.Bottom;
        }

        /// <summary>
        /// Construct from Rect4 clockwise winding
        /// </summary>
        /// <param name="r"></param>
        public Polygon12(Rect4 r)
        {
            point = new Vector2[maxNumOfPoints];
            numOfPoints = 4;
            point[0].X = r.point[0].X;
            point[0].Y = r.point[0].Y;

            point[1].X = r.point[1].X;
            point[1].Y = r.point[1].Y;

            point[2].X = r.point[2].X;
            point[2].Y = r.point[2].Y;

            point[3].X = r.point[3].X;
            point[3].Y = r.point[3].Y;
        }

        public Polygon12(Vector2 center, int sides, float radius, float initialAngleRadians)
        {
            Vector2 p = new Vector2(center.X, center.Y + radius);
            Vector2 prev = new Vector2(0, 0);

            point = new Vector2[maxNumOfPoints];
            numOfPoints = sides;

            for (int i = 0; i < sides; i++)
            {
                Vector2 pp = Util.rotatePoint(p, center, (float)(Math.PI * 2 / (sides) * i)+initialAngleRadians);
                point[i].X = pp.X;
                point[i].Y = pp.Y;
                prev = pp;
            }
        }

        /// <summary>
        /// Copy Constructor
        /// </summary>
        /// <param name="r"></param>
        public Polygon12(Polygon12 r) 
        {
            point = new Vector2[maxNumOfPoints];
            numOfPoints = r.numOfPoints;
            for (int i = 0; i < numOfPoints; i++)
            {
                point[i].X = r.point[i].X;
                point[i].Y = r.point[i].Y;
            }
        }

        /// <summary>
        /// Adds a point to the polygon silently fails if 12 points already used 
        /// </summary>
        /// <param name="pointQ"></param>
        public void addPoint(Vector2 pointQ)
        {
            if (numOfPoints >= maxNumOfPoints) return;
            point[numOfPoints].X = pointQ.X;
            point[numOfPoints].Y = pointQ.Y;
            numOfPoints++;
        }

        /// <summary>
        /// Adds a point to the polygon silently fails if 12 points already used 
        /// </summary>
         public void addPoint(float x, float y)
        {
            if (numOfPoints >= maxNumOfPoints) return;
            point[numOfPoints].X = x;
            point[numOfPoints].Y = y;
            numOfPoints++;
        }

        /// <summary>
        /// Rotates the Polygon12 by a given angle in radians
        /// </summary>
        /// <param name="centerOfRotation"></param>
        /// <param name="angleInRadians"></param>
        public void rotatePolygon12(Vector2 centerOfRotation, float angleInRadians)
        {
            for (int i = 0; i < numOfPoints; i++)
            {
                point[i] = Util.rotatePoint(point[i], centerOfRotation, -angleInRadians);
            }
        }

        /// <summary>
        /// Rotates the Polygon12 by a given angle in degrees
        /// </summary>
        /// <param name="centerOfRotation"></param>
        /// <param name="angleInDegrees"></param>
        public void rotatePolygon12Deg(Vector2 centerOfRotation, float angleInDegrees)
        {
            rotatePolygon12(centerOfRotation, angleInDegrees * (float)Math.PI / 180);
        }

        /// <summary>
        /// This returns an axis aligned bounding box based on the four corners of Rect4.
        /// The points should be a convex polygon, but this routine will work in all cases
        /// (note it can probably be done faster using the Max and Min functions but it deliberately this way so students can understand it)
        /// </summary>
        public Rectangle getAABoundingRect()
        {
            float Top = point[0].Y;
            float Left = point[0].X;
            float Bottom = point[0].Y;
            float Right = point[0].X;

            for (int i = 1; i < numOfPoints; i++) // start at 1 cause we did 0 in initialise
            {

                if (point[i].X < Left) Left = point[i].X;
                if (point[i].Y < Top) Top = point[i].Y;
                if (point[i].X > Right) Right = point[i].X;
                if (point[i].Y > Bottom) Bottom = point[i].Y;

            }
            // now have bounds in Top, left bottomm and right - covert to rectangle

            return new Rectangle((int)Left, (int)Top, (int)(Right - Left), (int)(Bottom - Top));
        }
    }


// *********************************************** MakeTex Code Shell  *************************************************************************
    /*
    /// <summary>
    /// MakeTex a class for making square textures of a given colour - its a code shell not a class
    /// </summary>
    public class MakeTex
    {
        public static Texture2D rectangleBorder(GraphicsDevice device, Rectangle r, Color c, Color borderC, int borderWidth)
        {
            Texture2D tex = new Texture2D(device, r.Width, r.Height, false, SurfaceFormat.Color);
            Color[] data = new Color[r.Width * r.Height];

            //Color cLight = Util.lighterOrDarker(c, 1.4f);
            //Color cDark = Util.lighterOrDarker(c, 0.6f);

            for (int x = 0; x < r.Width; x++)
            {
            }
            return tex;
        }

    }
    */
}
