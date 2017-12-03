using System;
using System.IO;
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
    /// This is a class that is just 4 points.
    /// The points usually refer to a rotated rectange, or polygon, but not necessarily;
    /// </summary>
    public class Rect4
    {
        /// <summary>
        /// The data in the 4 points class called Rect4
        /// </summary>
        public Vector2[] point;

        /// <summary>
        /// Default constructor (0,0) (0,0) (0,0) (0,0)</summary>
        public Rect4()
        {
            point = new Vector2[4];
            for (int i=0; i<4; i++)
            {
                point[i].X=0;
                point[i].Y=0;
            }
        }

        /// <summary>
        /// Construct from rectangle clockwise winding</summary>
        public Rect4(Rectangle r)
        {
            point = new Vector2[4];
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
        /// Copy constructor
        /// </summary>
        /// <param name="r"></param>
        public Rect4(Rect4 r) // Copy Constructor
        {
            point = new Vector2[4];
            point[0].X = r.point[0].X;
            point[0].Y = r.point[0].Y;

            point[1].X = r.point[1].X;
            point[1].Y = r.point[1].Y;

            point[2].X = r.point[2].X;
            point[2].Y = r.point[2].Y;

            point[3].X = r.point[3].X;
            point[3].Y = r.point[3].Y;
        }

        /// <summary>
        /// Rotates the rect4 by a given angle in radians
        /// </summary>
        /// <param name="centerOfRotation"></param>
        /// <param name="angleInRadians"></param>
        public void rotateRect(Vector2 centerOfRotation,float angleInRadians)
        {
            point[0]=Util.rotatePoint(point[0], centerOfRotation, -angleInRadians);
            point[1]=Util.rotatePoint(point[1], centerOfRotation, -angleInRadians);
            point[2]=Util.rotatePoint(point[2], centerOfRotation, -angleInRadians);
            point[3]=Util.rotatePoint(point[3], centerOfRotation, -angleInRadians);
        }

        /// <summary>
        /// Rotates the rect4 by a given angle in degrees
        /// </summary>
        /// <param name="centerOfRotation"></param>
        /// <param name="angleInDegrees"></param>
        public void rotateRectDeg(Vector2 centerOfRotation, float angleInDegrees)
        {
            rotateRect(centerOfRotation, angleInDegrees * (float)Math.PI / 180);
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
            float Bottom=point[0].Y;
            float Right=point[0].X;

            if (point[1].X < Left) Left = point[1].X;
            if (point[2].X < Left) Left = point[2].X;
            if (point[3].X < Left) Left = point[3].X;

            if (point[1].Y < Top) Top = point[1].Y;
            if (point[2].Y < Top) Top = point[2].Y;
            if (point[3].Y < Top) Top = point[3].Y;

            if (point[1].X > Right) Right = point[1].X;
            if (point[2].X > Right) Right = point[2].X;
            if (point[3].X > Right) Right = point[3].X;

            if (point[1].Y > Bottom) Bottom = point[1].Y;
            if (point[2].Y > Bottom) Bottom = point[2].Y;
            if (point[3].Y > Bottom) Bottom = point[3].Y;

            // now have bounds in Top, left bottomm and right - covert to rectangle

            return new Rectangle((int)Left,(int)Top,(int)(Right-Left),(int)(Bottom-Top));
        }
    }

  // ------------------------------- Util Class ------------------------------------------------------------------------

    /// <summary>
    /// Just a utility class for staic common usefull methods
    /// </summary>
    public class Util
    {

        public static double epsilon = 0.001; // for the aproximately equal routines 
        public static float epsilonf = 0.001f; // for the aproximately equal routines 
        
        /// <summary>
        /// Pythagorean distance
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static float dist(Vector2 p1, Vector2 p2)
        {
            Vector2 retv = p1 - p2;
            return retv.Length();
        }

        /// <summary>
        /// Rotate a single point about an arbitay center radians
        /// </summary>
        /// <param name="point"></param>
        /// <param name="centerOfRotation"></param>
        /// <param name="angleInRadians"></param>
        /// <returns></returns>
        public static Vector2 rotatePoint(Vector2 point, Vector2 centerOfRotation, float angleInRadians)
        {
            float tmpx, tmpy, tx, ty; // more temporaries than we really need but its very clear how it works with them
            Vector2 retv; // new value

            /* set to origin */
            tmpx = point.X - centerOfRotation.X;
            tmpy = point.Y - centerOfRotation.Y;

            // apply rotate
            tx = (tmpy * (float)Math.Sin(angleInRadians)) + (tmpx * (float)Math.Cos(angleInRadians));
            ty = (tmpy * (float)Math.Cos(angleInRadians)) - (tmpx * (float)Math.Sin(angleInRadians));

            retv.X = tx + centerOfRotation.X;
            retv.Y = ty + centerOfRotation.Y;
            return retv;
        }

        /// <summary>
        /// Rotate a single point about an arbitay center in degrees
        /// </summary>
        /// <param name="point"></param>
        /// <param name="centerOfRotation"></param>
        /// <param name="angleInDegrees"></param>
        /// <returns></returns>
        public static Vector2 rotatePointDeg(Vector2 point, Vector2 centerOfRotation, float angleInDegrees)
        {
            return rotatePoint(point, centerOfRotation, (float)(angleInDegrees * Math.PI / 180));

        }

        /// <summary>
        /// Convert degrees to radians
        /// </summary>
        /// <param name="angleInDegrees"></param>
        /// <returns></returns>
        public static float degToRad(float angleInDegrees) { return angleInDegrees * (float)Math.PI / 180; }

        /// <summary>
        /// convert radians to degrees
        /// </summary>
        /// <param name="angleInRadians"></param>
        /// <returns></returns>
        public static float radToDeg(float angleInRadians) { return angleInRadians * 180 / (float)Math.PI; }

        /// <summary>
        /// Move a single spot in x/y plane by a given distance at a given angle
        /// </summary>
        /// <param name="posZ"></param>
        /// <param name="angleInRadians"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        public static Vector2 moveByAngleDist(Vector2 posZ, float angleInRadians, float distance)
        {
            Vector2 retv = posZ + (distance * new Vector2((float)Math.Cos(angleInRadians), (float)Math.Sin(angleInRadians)));
            return retv;
        }

        /// <summary>
        /// Just return the x,y delta of a move in a given angle
        /// </summary>
        /// <param name="posZ"></param>
        /// <param name="angleInRadians"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        public static Vector2 moveByAngleDistDelta(Vector2 posZ, float angleInRadians, float distance)
        {
            Vector2 retv = new Vector2((float)Math.Cos(angleInRadians), (float)Math.Sin(angleInRadians)) * distance;
            return retv;
        }

        /// <summary>
        /// gets the angle in radians between p1 and p2
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static float getAngle(Vector2 p1, Vector2 p2)
        {
            return (float)(Math.Atan2(p1.Y - p2.Y, p1.X - p2.X));
        }



        /// <summary>
        /// Computes a final set of destination values for blit
        /// 
        /// sourceframe is the rectange on the texture in texture pixels
        /// sourceBB is the bounding box in texture co ordinates
        /// hotspot is the hotspot in texture co-ordinates
        /// desthotspot is the destination hotspot
        /// destwidth is the width of the destination
        /// destheight is the height of the destination
        /// dest is the output destination rectangle        
        /// destBB is the output bounding box rectangle 
        /// </summary>
        /// <param name="sourceframe"></param>
        /// <param name="sourceBB"></param>
        /// <param name="hotspot"></param>
        /// <param name="pos"></param>
        /// <param name="destwidth"></param>
        /// <param name="destheight"></param>
        /// <param name="dest"></param>
        /// <param name="destBB"></param>
        /// <param name="destHS"></param>
        public static void computeDestFromSource(Rectangle sourceframe, Rectangle sourceBB, Point hotspot, Vector2 pos, float destwidth, float destheight,
                                          ref Rectangle dest, ref Rectangle destBB, ref Vector2 destHS)
        {
            // compute scale factors
            float scaleX = destwidth / sourceframe.Width;
            float scaleY = destheight / sourceframe.Height;
            

            // compute dest HS 
            destHS.X = hotspot.X - sourceframe.X;
            destHS.Y = hotspot.Y - sourceframe.Y;

            //compute dest Rectangle
            dest.X = (int)((pos.X - destHS.X * scaleX)-(sourceframe.X * scaleX));
            dest.Y = (int)((pos.Y - destHS.Y * scaleY)-(sourceframe.Y * scaleY));
            dest.Width = (int)destwidth; 
            dest.Height = (int)destheight;

            // compute dest BB
            destBB.X = (int)(pos.X + (sourceBB.X - sourceframe.X) * scaleX - destHS.X * scaleX);
            destBB.Y = (int)(pos.Y + (sourceBB.Y - sourceframe.Y) * scaleY - destHS.Y * scaleY);
            destBB.Width = (int)(sourceBB.Width*scaleX);
            destBB.Height = (int)(sourceBB.Height*scaleY);
        }

        /// <summary>
        /// Search working directory and sub directories for file
        /// </summary>
        /// <param name="fname">The file being searched for</param>
        /// <returns>A valid file name with path or "" </returns>
        public static string findFile(string fname)
        {
            string p0 = @".\" + fname;
            if (File.Exists(p0)) { return p0; }
            string p1 = @"..\" + fname;
            if (File.Exists(p1)) { return p1; }
            string p2 = @"..\..\" + fname;
            if (File.Exists(p2)) { return p2; }
            string p3 = @"..\..\..\" + fname;
            if (File.Exists(p3)) { return p3; }
            string p4 = @"..\..\..\..\" + fname;
            if (File.Exists(p4)) { return p4; }
            string p5 = @"..\..\..\..\..\" + fname;
            if (File.Exists(p5)) { return p5; }
            string p6 = @"..\..\..\..\..\..\" + fname;
            if (File.Exists(p6)) { return p6; }
            string p7 = @"..\..\..\..\..\..\..\" + fname;
            if (File.Exists(p7)) { return p7; }
            string p8 = @"..\..\..\..\..\..\..\..\" + fname;
            if (File.Exists(p8)) { return p8; }
            string p9 = @"..\..\..\..\..\..\..\..\..\" + fname;
            if (File.Exists(p9)) { return p9; }
            return "";
        }

        /// <summary>
        /// Search working directory and sub directories for file and return the directory prefix
        /// </summary>
        /// <param name="fname">The file being searched for</param>
        /// <returns>A valid dir path ending with \ or "" </returns>
        public static string findDirWithFile(string fname)
        {
            string p0 = @".\" ;
            if (File.Exists(p0 + fname)) { return p0; }
            string p1 = @".\..\" ;
            if (File.Exists(p1 + fname)) { return p1; }            
            string p2 = @".\..\..\" ;
            if (File.Exists(p2 + fname)) { return p2; }            
            string p3 = @".\..\..\..\" ;            
            if (File.Exists(p3 + fname)) { return p3; }           
            string p4 = @".\..\..\..\..\" ;
            if (File.Exists(p4 + fname)) { return p4; }           
            string p5 = @".\..\..\..\..\..\" ;
            if (File.Exists(p5 + fname)) { return p5; }   
            string p6 = @".\..\..\..\..\..\..\" ;
            if (File.Exists(p6 + fname)) { return p6; }
            string p7 = @".\..\..\..\..\..\..\..\";
            if (File.Exists(p7 + fname)) { return p7; }
            string p8 = @".\..\..\..\..\..\..\..\..\";
            if (File.Exists(p8 + fname)) { return p8; }
            string p9 = @".\..\..\..\..\..\..\..\..\..\";
            if (File.Exists(p9 + fname)) { return p9; }
            return "";
        }

        /// <summary>
        /// Search executing assembly path and sub directories for file and return the directory prefix
        /// </summary>
        /// <param name="fname">The file being searched for</param>
        /// <returns>A valid dir path ending with \ or "" </returns>
        public static string findDirWithFileExe(string fname)
        {
            //string p = System.IO.Path.GetDirectoryName(Application.ExecutablePath); 
            string p =System.IO.Directory.GetCurrentDirectory();
            string p0 = p+@"\";
            if (File.Exists(p0 + fname)) { return p0; }
            string p1 = p+@"\..\";
            if (File.Exists(p1 + fname)) { return p1; }
            string p2 = p+@"\..\..\";
            if (File.Exists(p2 + fname)) { return p2; }
            string p3 = p+@"\..\..\..\";
            if (File.Exists(p3 + fname)) { return p3; }
            string p4 = p+@"\..\..\..\..\";
            if (File.Exists(p4 + fname)) { return p4; }
            string p5 = p+@"\..\..\..\..\..\";
            if (File.Exists(p5 + fname)) { return p5; }
            string p6 = p+@"\..\..\..\..\..\..\";
            if (File.Exists(p6 + fname)) { return p6; }
            string p7 = p+@"\..\..\..\..\..\..\..\";
            if (File.Exists(p7 + fname)) { return p7; }
            string p8 = p+@"\..\..\..\..\..\..\..\..\";
            if (File.Exists(p8 + fname)) { return p8; }
            string p9 = p+@"\..\..\..\..\..\..\..\..\..\";
            if (File.Exists(p9 + fname)) { return p9; }
            return "";
        }

        /// <summary>
        /// This will replace one colou with another in a 'Color' Format texture
        /// These apear to be the format read when a png 32 bit file is loaded
        /// </summary>
        /// <param name="tex"></param>
        /// <param name="cToReplace"></param>
        /// <param name="cToWrite"></param>
        /// <returns></returns>
        public static int ChangeColourInTexturePNG(Texture2D tex, Color cToReplace, Color cToWrite) // new color(0,0,0,0)
        {
        uint[] pixelData;
        int i = 0;
        //int x,y = 0;
        //Color col;
        uint temp;
        uint c1;
        uint c2;

        byte[] cc1=new byte[4];
        byte[] cc2=new byte[4];

        if (tex.Format != SurfaceFormat.Color) return 0; // throwing an exception might be a better idea than return
        //Color format is ARGB - in paint.net you need to select 32bit when saving

        cc1[3]=cToReplace.A;
        cc1[2]=cToReplace.R;
        cc1[1]=cToReplace.G;
        cc1[0]=cToReplace.B;
        c1=BitConverter.ToUInt32(cc1,0);

        cc2[3] = cToWrite.A;
        cc2[2] = cToWrite.R;
        cc2[1] = cToWrite.G;
        cc2[0] = cToWrite.B;
        c2=BitConverter.ToUInt32(cc2,0);

        pixelData  = new uint[tex.Width * tex.Height];
        tex.GetData(pixelData, 0, tex.Width * tex.Height);

        for (int xx = 0; xx < tex.Width; xx++)
        {
            for (int yy = 0; yy <tex.Height; yy++)
            {
            temp=pixelData[xx+yy*tex.Width];
            if (temp == c1) pixelData[xx+yy*tex.Width]=c2;
            }
        }
        tex.SetData(pixelData);
        return i;
        }

        /*
        /// <summary>
        /// converts a bgr32 format texture to a color format texture
        /// typically this is a bmp to png conversion in laymans terms
        /// if in doubt make levels = 1
        /// </summary>
        /// <param name="gd"></param>
        /// <param name="tex"></param>
        /// <param name="AlphaVal"></param>
        /// <param name="levels"></param>
        /// <returns></returns>
        public static Texture2D makeFormatColor(GraphicsDevice gd, Texture2D tex, byte AlphaVal, int levels)
        {
            Texture2D retv = new Texture2D(gd, tex.Width, tex.Height, levels, TextureUsage.AutoGenerateMipMap, SurfaceFormat.Color);

            if (tex.Format== SurfaceFormat.Bgr32)
            {
                uint[] pixelData = new uint[tex.Width * tex.Height];
                tex.GetData(pixelData, 0, tex.Width * tex.Height);

                for (int xx = 0; xx < tex.Width; xx++)
                {
                    for (int yy = 0; yy < tex.Height; yy++)
                    {
                        uint temp;
                        uint c2;
                        byte[] cc1 = new byte[4];
                        byte[] cc2 = new byte[4];
                        temp = pixelData[xx + yy * tex.Width];
                        cc1 = BitConverter.GetBytes(temp);
                        //cc1[3] = unused
                        //cc1[2] = r
                        //cc1[1] = g
                        //cc1[0] = b

                        cc2[3] = AlphaVal; // a
                        cc2[2] = cc1[2]; // r
                        cc2[1] = cc1[1]; // g
                        cc2[0] = cc1[0]; // b
                        c2=BitConverter.ToUInt32(cc2,0);
                        pixelData[xx + yy * tex.Width] = c2;
                    }
                }
                retv.SetData(pixelData);
                return retv;
            }
            return tex; // unchanged - not right format
        }

        /// <summary>
        /// Could work - if it does it will load a bmp - convert it Color format and add an alpha channel
        /// </summary>
        /// <param name="gd"></param>
        /// <param name="fileName"></param>
        /// <param name="transColor"></param>
        /// <returns></returns>
        public static Texture2D loadBmpWithTrans(GraphicsDevice gd, String fileName, Color transColor)
        {
            Texture2D tempTex = Texture2D.FromFile(gd, fileName);
            Texture2D retv = Util.makeFormatColor(gd, tempTex, 255, 1);
            Util.ChangeColourInTexturePNG(retv, transColor, Color.TransparentBlack);
            return retv;
        }
        */

        /// <summary>
        /// Returns sublocations around the edge and middle of a rectangle
        /// </summary>
        /// <param name="r"> the rectangle in question</param>
        /// <param name="subLocNum">the sub location from the list below
        /// 0=middle of rectange
        /// 1=mid of top
        /// 2=right top corner
        /// 3=mid of right side
        /// 4=right bottom corner
        /// 5=mid of bottom
        /// 6=left bottom corner
        /// 7=mid left side
        /// 8=top left corner
        /// </param>
        /// <returns></returns>
        public static Vector2 getSubLocation(Rectangle r, int subLocNum)
        {
            switch (subLocNum)
            {
                case 1: { return new Vector2(r.X + r.Width / 2, r.Y); }
                case 2: { return new Vector2(r.X + r.Width , r.Y); }
                case 3: { return new Vector2(r.X + r.Width , r.Y + r.Height/2); }
                case 4: { return new Vector2(r.X + r.Width , r.Y + r.Height); }
                case 5: { return new Vector2(r.X + r.Width / 2, r.Y + r.Height); }
                case 6: { return new Vector2(r.X , r.Y + r.Height); }
                case 7: { return new Vector2(r.X , r.Y + r.Height/2); }
                case 8: { return new Vector2(r.X , r.Y); }
                default : { return new Vector2(r.X + r.Width / 2, r.Y + r.Height/2); } // default is middle
            }
        }

        /// <summary>
        /// Create a new colour either lighter or darker than the previous one 
        /// </summary>
        /// <param name="c"></param>
        /// <param name="amount"> 1.1f = lighter   0.9f = darker </param>
        /// <returns></returns>
        public static Color lighterOrDarker(Color c, float amount)
        {
            float r = (c.R * amount); if(r>255) r=255;
            float g = (c.G * amount); if(g>255) g=255;
            float b = (c.B * amount); if(b>255) b=255;
            Color retv = new Color((byte)r, (byte)g, (byte)b, c.A);
            return retv;
        }

        // ********************** Start Rectangle routines *****************************************

        /// <summary>
        /// create a copy of a rectangle
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public static Rectangle newRectangle(Rectangle r)
        {
            return new Rectangle(r.X, r.Y, r.Width, r.Height);
        }

        /// <summary>
        /// create a copy of a nullable rectangle
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public static Rectangle? newRectangle(Rectangle? r)
        {
            if (r == null) return null;
            return new Rectangle(r.Value.X, r.Value.Y, r.Value.Width, r.Value.Height);
        }

        /// <summary>
        /// Creates a rectangle beside the existing one 
        /// if dirX == 1 its to the right
        /// if dirY == 1 its below
        /// if dirX == -1 its to the left
        /// if dirY == -1 its above
        /// Gap is the gap between usually 0
        /// </summary>
        /// <param name="r"></param>
        /// <param name="dirX"></param>
        /// <param name="dirY"></param>        
        /// <param name="gap"></param>
        /// <returns></returns>
        public static Rectangle newRectangleBeside(Rectangle r, int dirX, int dirY, int gap)
        {
            int x = r.X+(dirX*r.Width)+(dirX*gap);
            int y = r.Y + (dirY * r.Height) + (dirY * gap);
            return new Rectangle(x,y, r.Width, r.Height);
        }

        /// <summary>
        /// Creates a new rectangle inside or outside an existing one by gapX and gapY
        /// For creating concentric rectangles
        /// </summary>
        /// <param name="r"></param>
        /// <param name="gapX"></param>
        /// <param name="gapY"></param>
        /// <returns></returns>
        public static Rectangle newRectangleInside(Rectangle r, int gapX, int gapY)
        {
            int x = r.X + gapX;
            int y = r.Y + gapY;
            return new Rectangle(x, y, r.Width+(gapX*2), r.Height+(gapY*2));
        }
 


        // *********************** Equality routines ****************************************************

        /// <summary>
        /// aproximately equal (maximum diference is epsilon)
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="epsilon"></param>
        /// <returns></returns>
        public static bool aequal(int v1, int v2, int epsilon)
        {            
            // aproximately equal
            if (Math.Abs(v1 - v2) <= epsilon) return true;
            return false;
        }

        /// <summary>
        /// aproximately equal (maximum diference is epsilon)
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="epsilon"></param>
        /// <returns></returns>
        public static bool aequal(double v1, double v2, double epsilon)
        {
            // aproximately equal
            if (Math.Abs(v1 - v2) < epsilon) return true;
            return false;
        }

        /// <summary>
        /// aproximately equal (maximum diference is epsilon)
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="epsilon"></param>
        /// <returns></returns>
        public static bool aequal(float v1, float v2, float epsilon)
        {
            // aproximately equal
            if (Math.Abs(v1 - v2) < epsilon) return true;
            return false;
        }

        //   ************************************ start Collision Routines ***********************************************

        /// <summary>
        /// Returns the slope of the line x1,y1 to x2 y2
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <returns></returns>
        public static double slope(double x1, double y1, double x2, double y2)
        {
            return (y1 - y2) / (x1 - x2);
        }

        /// <summary>
        /// Aproximate interection of 2 line segments in 2d 
        /// this returns the following
        /// 0= all ok they inteset and the intersect is in x,y
        /// 1= they are both vertical lines
        /// 2= they are both horizontal lines
        /// 3= they dont intersesct in range
        /// Given it does a bounding box check first 1 and 2 indicate colisions
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <param name="x3"></param>
        /// <param name="y3"></param>
        /// <param name="x4"></param>
        /// <param name="y4"></param>
        /// <returns></returns>
        public static int intersect2D(ref double x, ref double y,
                 double x1, double y1, double x2, double y2,
                 double x3, double y3, double x4, double y4
                 )
        {
            double epsilon = 0.001; // good enough for 2D graphics where we need nearest pixel
            // this returns the following
            // 0= all ok they intersect and the intersect is in x,y
            // 1= they are both vertical lines 
            // 2= they are both horizontal lines
            // 3= they dont intersesct in range
            // Given it does a bounding box check first 1 and 2 indicate colisions

            // a is the line x1,y1-x2,y2
            // b is the line x3,y3-x4,y4
            double aXmin, aYmin, aXmax, aYmax;
            double bXmin, bYmin, bXmax, bYmax;
            aXmin = Math.Min(x1, x2) - epsilon; // must adjust by epsilon or it does not work RC april 2005
            aYmin = Math.Min(y1, y2) - epsilon;
            aXmax = Math.Max(x1, x2) + epsilon;
            aYmax = Math.Max(y1, y2) + epsilon;

            bXmin = Math.Min(x3, x4) - epsilon;
            bYmin = Math.Min(y3, y4) - epsilon;
            bXmax = Math.Max(x3, x4) + epsilon;
            bYmax = Math.Max(y3, y4) + epsilon;

            x = 0; // sane defaults
            y = 0; // sane defaults

            if (aXmin > bXmax) return 3; // basic bounding box check for speed
            if (aYmin > bYmax) return 3;
            if (bXmin > aXmax) return 3;
            if (bYmin > aYmax) return 3;

            // if we get here they might intersect
            double xadiff = Math.Abs(x1 - x2);
            double xbdiff = Math.Abs(x3 - x4);
            double yadiff = Math.Abs(y1 - y2);
            double ybdiff = Math.Abs(y3 - y4);

            double ma, ba; // part of y=mx+b for segment a
            double mb, bb; // part of y=mx+b for segment b

            if (xadiff <= epsilon && xbdiff <= epsilon)
            {// segments a and b are both vertical lines
                return 1;
            }

            if (yadiff <= epsilon && ybdiff <= epsilon)
            {// segments a and b are both horizontal lines
                return 2;
            }

            if (xadiff <= epsilon)
            {// segmenta is a vertical line
                mb = slope(x3, y3, x4, y4);
                bb = y3 - mb * x3;
                x = x1;
                y = mb * x + bb;
                if (y <= aYmax &&// is the hit in the bounding box ?
                    y <= bYmax &&
                    y >= aYmin &&
                    y >= bYmin) return 0; // its a hit
                return 3;
            }

            if (xbdiff <= epsilon)
            {// segmentb is a vertical line
                ma = slope(x1, y1, x2, y2);
                ba = y1 - ma * x1;
                x = x3;
                y = ma * x + ba;
                if (y <= aYmax &&// is the hit in the bounding box ?
                    y <= bYmax &&
                    y >= aYmin &&
                    y >= bYmin) return 0; // its a hit
                return 3;
            }

            ma = slope(x1, y1, x2, y2);
            mb = slope(x3, y3, x4, y4);
            if (aequal(ma, mb, epsilon))
            {
                // paralell lines - we treat this as a miss
                return 3;
            }

            ba = y1 - ma * x1;
            bb = y3 - mb * x3;

            x = (bb - ba) / (ma - mb); // x intersect
            y = ma * x + ba;         // y intersect

            if (x <= aXmax && // is the hit in the bounding box ?
                x <= bXmax &&
                x >= aXmin &&
                x >= bXmin &&
                y <= aYmax &&
                y <= bYmax &&
                y >= aYmin &&
                y >= bYmin) return 0; // its a hit
            return 3;
        }

        /// <summary>
        /// Aproximate interection of 2 line segments in 2d 
        /// this returns the following
        /// 0= all ok they intersect and the intersect is in x,y
        /// 1= they are both vertical lines
        /// 2= they are both horizontal lines
        /// 3= they dont intersesct in range
        /// Given it does a bounding box check first 1 and 2 indicate colisions
        /// </summary>
        /// <param name="colisionPoint"></param>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <returns></returns>
        public static int intersect2D(ref Vector2 colisionPoint,
                 Vector2 p0, Vector2 p1,
                 Vector2 p2, Vector2 p3)
        {
            int rc;
            double x=0,y=0;
            rc = intersect2D(ref x, ref y,
                p0.X, p0.Y, p1.X, p1.Y,
                p2.X, p2.Y, p3.X, p3.Y);
            colisionPoint.X = (float)x;
            colisionPoint.Y = (float)y;
            return rc;
        }

        /// <summary>
        /// Returns true if the point is inside the polygon
        /// it draws a line from the point to a point outside the polygon counting the number of intersects
        /// if intersects are an odd number its inside othewise its outside
        /// </summary>
        /// <param name="point"></param>
        /// <param name="poly"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static bool insidePoly(Vector2 point, Vector2[] poly, int count)
        {

            Vector2 testPoint = new Vector2(poly[0].X, point.Y); // constructs test line
            int kount = 0;
            int k;
            double x = 0;
            double y = 0;
            double xx = +900000; // remember intesects in case line goes through the conection of two lines and scores twice
            double yy = -900000; // set well outside the range of a screen
            for (int i = 0; i < count; i++)
            {
                if (poly[i].X < testPoint.X) testPoint.X = poly[i].X - 1; // guarantees testpoint is outside poly
                if (poly[i].Y < testPoint.Y) testPoint.Y = poly[i].Y - 1; // guarantees testpoint is outside poly and not a horizontal line which confuses intersect
            }
            for (int i = 1; i < count; i++)
            {

                k = Util.intersect2D(ref x, ref y, testPoint.X, testPoint.Y, point.X, point.Y,
                                        poly[i - 1].X, poly[i - 1].Y, poly[i].X, poly[i].Y);
                if (k == 0 && !(aequal(x, xx, 0.001) && aequal(y, yy, 0.001)))
                {
                    kount++;
                    xx = x;
                    yy = y;
                }
            }
            k = Util.intersect2D(ref x, ref y, testPoint.X, testPoint.Y, point.X, point.Y,
                                        poly[count - 1].X, poly[count - 1].Y, poly[0].X, poly[0].Y);
            if (k == 0 && !(aequal(x, xx, 0.001) && aequal(y, yy, 0.001)))
            {
                kount++;
                xx = x;
                yy = y;
            }

            if (kount % 2 == 1)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// A fairly fast but not totally accurate collision system for rect 4 structures
        /// Simply checks that the corner points in one are inside the outline of the other
        /// </summary>
        /// <returns></returns>
        public static bool collisionRect4Rect4Points(Rect4 r0, Rect4 r1)
        {
            // bool rc = false;
            Rectangle rr0 = r0.getAABoundingRect();
            Rectangle rr1 = r1.getAABoundingRect();
            if (!rr0.Intersects(rr1)) return false; // they cant colide the aa's dont collide
           // if (rr0.Intersects(rr1)) return true; // they cant colide the aa's dont collide

            if (insidePoly(r1.point[0], r0.point, 4)) return true;
            if (insidePoly(r1.point[1], r0.point, 4)) return true;
            if (insidePoly(r1.point[2], r0.point, 4)) return true;
            if (insidePoly(r1.point[3], r0.point, 4)) return true;

            if (insidePoly(r0.point[0], r1.point, 4)) return true;
            if (insidePoly(r0.point[1], r1.point, 4)) return true;
            if (insidePoly(r0.point[2], r1.point, 4)) return true;
            if (insidePoly(r0.point[3], r1.point, 4)) return true;

            return false;
        }

        /// <summary>
        /// A Slower accurate collision system for rect 4 structures
        /// checks for line intersections and point containment which i think is totally accurate
        /// </summary>
        /// <returns></returns>
        public static bool collisionRect4Rect4Lines(Rect4 r0, Rect4 r1)
        {
            // bool rc = false;
            Rectangle rr0 = r0.getAABoundingRect();
            Rectangle rr1 = r1.getAABoundingRect();
            if (!rr0.Intersects(rr1)) return false; // they cant colide the aa's dont collide

            if (insidePoly(r1.point[0], r0.point, 4)) return true;
            if (insidePoly(r0.point[0], r1.point, 4)) return true;

            for (int i = 0; i < 3; i++)
            {
                // construct a line for r0 point i to j
                int j = i + 1;
                if (j > 3) j = 0;
                Vector2 cp=new Vector2(0,0);
                int rc0 = intersect2D(ref cp, r0.point[i], r0.point[j], r1.point[0], r1.point[1]); if (rc0 != 3) return true;
                int rc1 = intersect2D(ref cp, r0.point[i], r0.point[j], r1.point[1], r1.point[2]); if (rc1 != 3) return true;
                int rc2 = intersect2D(ref cp, r0.point[i], r0.point[j], r1.point[2], r1.point[3]); if (rc2 != 3) return true;
                int rc3 = intersect2D(ref cp, r0.point[i], r0.point[j], r1.point[3], r1.point[0]); if (rc3 != 3) return true;
            }
            return false;
        }


        public static bool collisionPolygon12(Polygon12 r0, Polygon12 r1)
        {
            // bool rc = false;
            Rectangle rr0 = r0.getAABoundingRect();
            Rectangle rr1 = r1.getAABoundingRect();
            if (!rr0.Intersects(rr1)) return false; // they cant colide the aa's dont collide

            if (insidePoly(r1.point[0], r0.point, r0.numOfPoints)) return true;
            if (insidePoly(r0.point[0], r1.point, r1.numOfPoints)) return true;

            for (int i0 = 0; i0 < r0.numOfPoints; i0++)
            {
                int j0 = i0 + 1;
                if (j0 >= r0.numOfPoints) j0 = 0;
                for (int i1 = 0; i1 < r1.numOfPoints; i1++)
                {
                    // construct a line for r0 point i to j

                    int j1 = i1 + 1;
                    if (j1 >= r1.numOfPoints) j1 = 0;
                    Vector2 cp = new Vector2(0, 0);
                    int rc0 = intersect2D(ref cp, r0.point[i0], r0.point[j0], r1.point[i1], r1.point[j1]); if (rc0 != 3) return true;
                }
            }
            return false;
        }





        /// <summary>
        /// Returns the inner rectangle of a circle
        /// </summary>
        /// <param name="centerOfCircle"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public Rectangle innerRectangleOfCircle(Vector2 centerOfCircle, float radius)
        {
            float sin45 = 0.7071f;
            Rectangle retv = new Rectangle((int)(centerOfCircle.X-radius*sin45), (int)(centerOfCircle.Y-radius*sin45),(int)(radius*sin45),(int)(radius*sin45));
            return retv;
        }

        /// <summary>
        /// Returns the outer rectangle of a circle
        /// </summary>
        /// <param name="centerOfCircle"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public Rectangle outerRectangleOfCircle(Vector2 centerOfCircle, float radius)
        {        
            Rectangle retv = new Rectangle((int)(centerOfCircle.X-radius), (int)(centerOfCircle.Y-radius), (int)radius, (int)radius);
            return retv;
        }

        /// <summary>
        /// This is a fairly aproximate collision routine
        /// it would usually only be used with rectangles and circles of similar size
        /// </summary>
        /// <returns></returns>
        public bool collideCircleRectangle(Vector2 centerOfCircle, float radius, Rectangle rectBB)
        {
            Rectangle r = outerRectangleOfCircle(centerOfCircle, radius);
            if (!r.Intersects(rectBB)) return false; // outer bounding boxes dont collide so no colision possible
            r = innerRectangleOfCircle(centerOfCircle, radius);
            if (r.Intersects(rectBB)) return true; // inner bounding boxes collide so no colision is definite

            for (int i = 0; i < 9; i++)
            {
                Vector2 v = getSubLocation(rectBB, i);
                float d = dist(v, centerOfCircle);
                if (d < radius) return true;
            }
            return false;
        }

        /// <summary>
        /// This is a accurate routine
        /// </summary>
        /// <returns></returns>
        public bool collideCircleCircle(Vector2 centerOfCircle1, float radius1, Vector2 centerOfCircle2, float radius2)
        {
           float d = dist(centerOfCircle1, centerOfCircle2);
           if (d < (radius1+radius2)) return true;
           return false;
        }


// ********************** load routines to help XNA ***********************************************************

        public static Texture2D texFromFile(GraphicsDevice gd, String fName)
        {
            // note needs :using System.IO;
            Stream fs = new FileStream(fName, FileMode.Open);
            Texture2D rc = Texture2D.FromStream(gd, fs);
            fs.Close();
            return rc;
        }

// ********************************* timing functions ****************************************
        public static int secondsToTicks(float seconds, int ticksPerSecond)
        {
            return (int)(seconds * ticksPerSecond);
        }

        public static int secondsToTicks(float seconds, TimeSpan t)
        {
            // typical call is secondsToticks(2,TargetElapsedTime)
            // TargetElapsedTime is a variable in Game1
            return (int)(seconds / t.Milliseconds);
        }



// ********** some formatting commands to help my memory - no real reason for their existence except my memory **************

        public static string dbl0(double dd) // float 0 decimal places
        {
            return String.Format("{0:0.}", dd);
        }

        public static string dbl1(double dd) // float 1 decimal places
        {
            return String.Format("{0:0.0}", dd);
        }

        public static string dbl2(double dd) // float 2 decimal places
        {
            return String.Format("{0:0.00}", dd);
        }

        public static string dbl3(double dd) // float 3 decimal places
        {
            return String.Format("{0:0.000}", dd);
        }

        public static string int1(int dd) // two digit integer
        {
            return String.Format("{0,1}", dd);
        }
        public static string int2(int dd) // two digit integer
        {
            return String.Format("{0,2}", dd);
        }
        public static string int3(int dd) // two digit integer
        {
            return String.Format("{0,3}", dd);
        }

        public static string int4(int dd) // four digit integer
        {
            return String.Format("{0,4}", dd);
        }

        public static string int5(int dd) // five digit integer
        {
             return String.Format("{0,5}", dd);
        }

        public static string int6(int dd) // six digit integer
        {
            return String.Format("{0,6}", dd);
        }

        public static string int7(int dd) // seven digit integer
        {
            return String.Format("{0,7}", dd);
        }

    }

}

// end