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
    // ********************************************** WayPoint ******************************************* //

    /// <summary>
    ///  a single waypoint
    /// </summary>
    public class WayPoint
    {
        /// <summary>
        /// Its position
        /// </summary>
        public Vector2 pos;

        /// <summary>
        /// how fast to go
        /// </summary>
        public float speed;

        /// <summary>
        /// create a single waypoint
        /// </summary>
        /// <param name="posZ"></param>
        /// <param name="speedZ"></param>
        public WayPoint(Vector2 posZ, float speedZ)
        {
            pos = posZ;
            speed = speedZ;
        }

        /// <summary>
        /// create a single waypoint
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="speedZ"></param>
        public WayPoint(float x, float y, float speedZ)
        {
            pos = new Vector2(x, y);
            speed = speedZ;
        }
    }

    // ********************************************** WayPointList ******************************************* //

    /// <summary>
    /// A class for creating and managing lists of waypoints
    /// </summary>
    public class WayPointList
    {
        /// <summary>
        /// lst is public for convenience and extension it is not normally needed outside the class 
        /// </summary>
        public List<WayPoint> lst;

        /// <summary>
        /// currentLeg is public for convenience and extension it is not normally accessed outside the class
        /// </summary>
        public int currentLeg;

        /// <summary>
        /// +1 = forward -1 = backward
        /// </summary>
        public int dir { set; get; }

        /// <summary>
        /// 0=return false to nextLeg() but loop back to start
        /// 1=return false to nextLeg() but reverse direction
        /// 2=set invisible / inactive 
        /// </summary>
        public int wayFinished { set; get; }

        /// <summary>
        /// construct an empty waypoint list
        /// </summary>
        public WayPointList()
        {
            wayFinished = 0;
            reset();
        }

        /// <summary>
        /// Construct a 2 position waypoint list 
        /// </summary>
        /// <param name="w1"></param>
        /// <param name="w2"></param>
        /// <param name="wayFinishedZ">you probably want this to be 1 ro get back forward actions</param>
        public WayPointList(WayPoint w1, WayPoint w2, int wayFinishedZ)
        {
            wayFinished = wayFinishedZ;
            reset();
            add(w1);
            add(w2);
        }

        /// <summary>
        /// Construct a 3 position walpoint list
        /// </summary>
        /// <param name="w1"></param>
        /// <param name="w2"></param>
        /// <param name="w3"></param>
        /// <param name="wayFinishedZ"></param>
        public WayPointList(WayPoint w1, WayPoint w2, WayPoint w3, int wayFinishedZ)
        {
            wayFinished = wayFinishedZ;
            reset();
            add(w1);
            add(w2);
            add(w3);
        }

        /// <summary>
        /// Array property
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public WayPoint this[int index]
        {
            get
            {
                return lst[index];
            }
            //set
            //{
            //    document.TextArray[index] = value;
            //}
        }

        /// <summary>
        /// reset the list to none
        /// </summary>
        public void reset()
        {
            lst = new List<WayPoint>();
            currentLeg = 0;
            dir = 1;
        }

        /// <summary>
        /// count of waypoints
        /// </summary>
        /// <returns></returns>
        public int count()
        {
            return lst.Count();
        }

        /// <summary>
        /// returns the index for subsequent use as leg
        /// </summary>
        /// <param name="w"></param>
        /// <returns></returns>
        public int add(WayPoint w)
        {
            lst.Add(w);
            return lst.Count() - 1;
        }

        /// <summary>
        /// returns the index for subsequent use as leg
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="speedZ"></param>
        /// <returns></returns>
        public int add(float x, float y, float speedZ)
        {
            lst.Add(new WayPoint(x, y, speedZ));
            return lst.Count() - 1;
        }

        /// <summary>
        /// Returns the current waypoint
        /// </summary>
        /// <returns></returns>
        public WayPoint currentWaypoint()
        {
            return lst[currentLeg];
        }

        /// <summary>
        /// returns the index of the gurrent way point 
        /// </summary>
        /// <returns></returns>
        public int getCurrentLeg()
        {
            return currentLeg;
        }

        /// <summary>
        /// returns a particular waypoint 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public WayPoint getWayPoint(int i)
        {
            return lst[i];
        }

        /// <summary>
        /// returns false if the next leg is the end  
        /// </summary>
        /// <returns></returns>
        public bool nextLeg()
        {
            // 0=return false to nextLeg() but loop back to start
            // 1=return false to nextLeg() but reverse direction

            currentLeg = currentLeg + dir;
            if (currentLeg >= lst.Count())
            {
                if (wayFinished == 0)
                {
                    currentLeg = 0;
                    return false;
                }
                if (wayFinished >= 1)
                {
                    dir = dir * -1;
                    currentLeg = lst.Count() - 1;
                    return false;
                }
            }

            if (currentLeg < 0)
            {
                if (wayFinished == 0)
                {
                    currentLeg = lst.Count() - 1;
                    return false;
                }
                if (wayFinished >= 1)
                {
                    dir = dir * -1;
                    currentLeg = 0;
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// This would be rarely be used it sets the current leg of the way point as an index
        /// </summary>
        /// <param name="i"></param>
        public void setLeg(int i)
        {
            currentLeg = i;
        }


        /// <summary>
        /// Construct a circular path of waypoints 
        /// </summary>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        /// <param name="startAngle"></param>
        /// <param name="angleIncrement"></param>
        /// <param name="steps"></param>
        /// <param name="speed"></param>
        /// <param name="Xscale">usually 1 for a circle, vary from 1 to creat ovals</param>

        public void makePathCircle(Vector2 center, float radius, float startAngle, float angleIncrement, int steps, float speed, float Xscale)
        {
            lst = new List<WayPoint>();

            for (int i = 0; i < steps; i++)
            {
                Vector2 v = Util.moveByAngleDist(new Vector2(0, 0), startAngle + (i * angleIncrement), radius);
                v.X = v.X * Xscale;
                v = v + center;
                WayPoint w = new WayPoint(v, speed);
                add(w);
            }
        }

        /// <summary>
        /// Makes a zig zag path
        /// </summary>
        /// <param name="startPos"></param>
        /// <param name="endPos"></param>
        /// <param name="zigZag"></param>
        /// <param name="steps"></param>
        /// <param name="speed"></param>
        public void makePathZigZag(Vector2 startPos, Vector2 endPos, Vector2 zigZag, int steps, float speed)
        {
            lst = new List<WayPoint>();
            float zig = 1;
            for (int i = 0; i < steps; i++)
            {
                Vector2 v = new Vector2(0, 0);
                float lerp = (float)i / ((float)steps - 1);
                v.X = MathHelper.Lerp(startPos.X, endPos.X, lerp);
                v.Y = MathHelper.Lerp(startPos.Y, endPos.Y, lerp);
                v = v + (zig * zigZag);
                WayPoint w = new WayPoint(v, speed);
                add(w);
                zig = zig * (-1);
            }
            wayFinished = 1; // typically you want this
        }

        /// <summary>
        /// Make a parabolic path changing velocity by delta to -/+ limits in
        /// </summary>
        /// <param name="speed"></param>
        /// <param name="maxSteps"></param>
        /// <param name="startPos"></param>
        /// <param name="initVelocity"></param>
        /// <param name="delta"></param>
        /// <param name="limits"></param>
        public void makePathDelta(float speed, int maxSteps, Vector2 startPos, Vector2 initVelocity, Vector2 delta, Rectangle limits)
        {
            lst = new List<WayPoint>();
            Vector2 p = new Vector2(startPos.X, startPos.Y);
            Vector2 v = new Vector2(initVelocity.X, initVelocity.Y);
            WayPoint w = new WayPoint(p, speed);
            add(w);
            for (int i = 0; i < maxSteps - 1; i++)
            {
                //Vector2 v = new Vector2(p.X+v.X,p.Y+v.X);
                p = p + v;
                v = v + delta;
                w = new WayPoint(p, speed);
                add(w);
                if (!limits.Contains((int)p.X, (int)p.Y))
                {
                    break;
                }
            }
            wayFinished = 2; // typically you want this
        }


        /// <summary>
        /// draw it so it helps
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="cPoints"> color of points (or null if no points wanted)</param>
        /// <param name="cLines"> color of lines (or null if no lines wanted)</param>
        public void Draw(SpriteBatch sb, Color cPoints, Color cLines)
        {
            for (int i = 0; i < lst.Count(); i++)
            {
                WayPoint w = lst[i];
                if (cPoints != null) LineBatch.drawCrossX(sb, w.pos.X, w.pos.Y, 5, cPoints, cPoints);
                if (cLines != null && i > 0)
                {
                    WayPoint ww = lst[i - 1];
                    LineBatch.drawLine(sb, ww.pos.X, ww.pos.Y, w.pos.X, w.pos.Y, cLines);
                }
            }
        }

    }


}
