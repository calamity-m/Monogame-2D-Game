using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

#pragma warning disable 1591 //sadly not yet fully commented

namespace RC_Framework
{
    /// <summary>
    /// parent class for all position factories 
    /// </summary>
    public abstract class RC_PositionFactory
    {
        public abstract Vector2 getNextPos(); // return the next position
        public virtual void init() {}
        public virtual void reset() {}
        public virtual void draw(SpriteBatch sb, Color col) { }
    }

    // ----------------------------- RC_posInrectangle ---------------------------------------------------------

    /// <summary>
    /// Creates positions inside the rectangle inclusive of top and left
    /// exclusive of right and bottom
    /// </summary>
    public class RC_posInrectangle : RC_PositionFactory
    {
        Random rnd=null;
        Rectangle rect;

        public RC_posInrectangle(Rectangle r, Random rndZ)
        {
            rect =  Util.newRectangle(r);
            rnd = rndZ;
        }

        public override Vector2 getNextPos()
        {
            Vector2 retv = new Vector2();
            retv.X = rnd.Next(rect.X, rect.X + rect.Width);
            retv.Y = rnd.Next(rect.Y, rect.Y + rect.Height);
            return retv;
        }

        public override void draw(SpriteBatch sb, Color col)
        {
            LineBatch.drawLineRectangle(sb,rect,col);
        }
    }


    // ----------------------------- RC_posInCircle ---------------------------------------------------------

    /// <summary>
    /// Creates positions inside the rectangle inclusive of top and left
    /// exclusive of right and bottom
    /// </summary>
    public class RC_posInCircle : RC_PositionFactory
    {
        Random rnd=null;
        Vector2 pos = new Vector2(0,0);
        float radiusMin=10;
        float radiusMax=10;

        public RC_posInCircle(Vector2 posZ, float radiusMinZ, float radiusMaxZ, Random rndZ)
        {
            pos =  new Vector2(posZ.X,posZ.Y);
            rnd = rndZ;
            radiusMin = radiusMinZ;
            radiusMax = radiusMaxZ;
        }

        public override Vector2 getNextPos()
        {
            double angle = rnd.NextDouble()*2*Math.PI;
            double radius = radiusMin +(radiusMax-radiusMin)*rnd.NextDouble();
            Vector2 retv = Util.moveByAngleDist(pos,(float)angle,(float)radius);
            return retv;
        }

        public override void draw(SpriteBatch sb, Color col)
        {
            LineBatch.drawCircle(sb,col,pos,radiusMin,19,1);
            LineBatch.drawCircle(sb,col,pos,radiusMax,19,1);
        }
    }


}
