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

// ***************************** change Log ************************* //
// 31/3/17 - Fixed bug in moveByDeltaY
//
// ****************************************************************** //

namespace RC_Framework
{


    // ************************************************************************************************ //
    
    public class Sprite3Parent : RC_RenderableBounded
    {

        /// <summary>
        /// Bounding Box rectange in source pixels
        /// x and y are offset from the HOTSPOT
        /// Warning if the hotspot is in the middle of the image/frame then x,y will be negative
        /// </summary>
        protected Rectangle bb;

        /// <summary>
        /// the sprites location 
        /// </summary>
        protected Vector2 pos; // the screen position of the sprite top left/top right pixel or hotspot pixel

        /// <summary>
        /// Set by calling savePosition, and used in restorePosition
        /// </summary>
        public Vector2 savePos; // the screen position temp store for return to after colission

        /// <summary>
        /// the previous position it is updated if the Sprite3 movement systems are used
        /// and can be used to compute a display angle
        /// this is for you to use/set if you have your own movement system 
        /// </summary>
        public Vector2 oldPos;

        /// <summary>
        /// sets the sprite location (top left of image)
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public override void setPos(float x, float y)
        {
            oldPos.X = pos.X;
            oldPos.Y = pos.Y;
            pos.X = x;
            pos.Y = y;
        }

        /// <summary>
        /// sets the sprite location - ie its hotspot on the screen
        /// note the default hotspot is the top left of image
        /// </summary>
        /// <param name="p"></param>
        public void setPos(Vector2 p)
        {
            oldPos.X = pos.X;
            oldPos.Y = pos.Y;
            pos.X = p.X;
            pos.Y = p.Y;
        }

        /// <summary>
        /// sets the X sprite location - ie its hotspot on the screen
        /// note the default hotspot is the top left of image
        /// </summary>
        /// <param name="x"></param>
        public void setPosX(float x)
        {
            oldPos.X = pos.X;
            pos.X = x;
        }

        /// <summary>
        /// sets the Y sprite location - ie its hotspot on the screen
        /// note the default hotspot is the top left of image
        /// </summary>
        /// <param name="y"></param>
        public void setPosY(float y)
        {
            oldPos.Y = pos.Y;
            pos.Y = y;
        }

        /// <summary>
        /// returns the xpos of the sprite
        /// </summary>
        /// <returns></returns>
        public float getPosX()
        {
            return pos.X;
        }

        /// <summary>
        /// returns the ypos of the sprite
        /// </summary>
        /// <returns></returns>
        public float getPosY()
        {
            return pos.Y;
        }

        /// <summary>
        /// returns the position of the sprite
        /// </summary>
        /// <returns></returns>
        public Vector2 getPos()
        {
            return pos;
        }

        /// <summary>
        /// restore the current position
        /// note its virtual and overidden in Sprite3
        /// </summary>
        public virtual void restorePosition()
        {
            pos.X = savePos.X;
            pos.Y = savePos.Y;
        }

        /// <summary>
        /// Save the current position
        /// note its virtual and overidden in Sprite3
        /// </summary>
        public virtual void savePosition()
        {
            savePos.X = pos.X;
            savePos.Y = pos.Y;
        }

    }

    // ****************************************************************** Sprite3 *************************************************

    /*
     * The difence between Sprite3 and sprite2 is as follows
     * a) The defered movement system has been removed - it was difficult to use
     * b) A new savePosition and restorePosition (save current position) feature exists to allow save and restore of positions back after the collision code
     * c) Sprite 3 supports the bounded renderable property bounds as a property (but at this stage without allowing for rotation)
    */

    /// <summary>
    /// The RC_Framework Sprite3 Class
    /// This class assumes that the position and hotspot are the same place
    /// and that this place is also the center of rotation, as a result all movement / rotation and collision routines
    /// make use of this place
    /// note properties active, visible and colour are inherited from renderable
    /// </summary>
    public class Sprite3 : Sprite3Parent
    {

        /// <summary>
        /// Just a version to help code controll
        /// </summary>
        public const string SpriteVersion ="V3.05";

        // <summary>
        // this variable called active is available for use by user code and is used to identify
        // active sprites in a sprite list it defaults to true
        // it is used in conjunction with sprite list to manage activity of sprites in a list
        // </summary>
        //public bool active;

        // <summary>
        // is it visible
        // </summary>
        //public bool visible;

        public override Rectangle bounds
        {
            get
            {
                boundz = getImageDest();
                return boundz;
            }
            set { boundz = value; } // not usefull
        }

        /// <summary>
        /// this variable called kind is available for use by user code
        /// </summary>
        public int kind;   // not used by the framework, available for use by user code

        /// <summary>
        /// this variable called state is available for use by user code
        /// </summary>
        public int state;   // not used by the framework, available for use by user code

        /// <summary>
        /// Used by user for id or text purposes
        /// </summary>
        public string name;

        /// <summary>
        /// this variable called score is available for use by user code
        /// </summary>
        public float score;

        /// <summary>
        /// this variable called hitPoints is available for use by user code
        /// it is used by the attached renderable HealthBar if one is active
        /// </summary>
        public int hitPoints;

        /// <summary>
        /// this variable called maxHitPoints is available for use by user code
        /// it may get used by an attached renderable to draw a health bar (or any other purpose)
        /// it is used by the attached renderable HealthBar
        /// </summary>
        public int maxHitPoints;

        /// <summary>
        /// This variable called text is just for users to attach a string to the sprite
        /// it is used by the attached renderable AttachedText
        /// </summary>
        public string text = "";

        /// <summary>
        /// variables called varInt0, varInt1, varInt2 are available for use by user code
        /// </summary>
        public int varInt0;

        /// <summary>
        /// available for use by user code
        /// </summary>
        public int varInt1;
        /// <summary>
        /// available for use by user code
        /// </summary>
        public int varInt2;
        /// <summary>
        /// available for use by user code
        /// </summary>
        public int varInt3;
        /// <summary>
        /// available for use by user code
        /// </summary>
        public int varInt4;
        /// <summary>
        /// these variables called varBool0, varBool1 are available for use by user code
        /// </summary>
        public bool varBool0;
        /// <summary>
        /// available for use by user code
        /// </summary>
        public bool varBool1;
        /// <summary>
        /// available for use by user code single floatig point number
        /// </summary>
        public float varFloat1;

        /// <summary>
        /// bounding Sphere Radius for user to use this is in screen pixels not source pixels
        /// any resizing must be done by the user
        /// </summary>
        public float boundingSphereRadius;


        /// <summary>
        /// this variable called target is available for use by user code
        /// </summary>
        public Vector2 target;

        /// <summary>
        /// The sprites texture2d - do not change it arbitarily
        /// use setTexture(Texture2D tw) routine instead to preserve frame information
        /// </summary>
        public Texture2D tex;

        /// <summary>
        /// the direction of the sprite
        /// SpriteEffects.None, SpriteEffects.FlipHorizontaly, SpriteEffects.FlipVertically
        /// </summary>
        public SpriteEffects flip = SpriteEffects.None;

        /// <summary>
        /// movement vector when using the deltax deltay movement system
        /// </summary>
        protected Vector2 deltaSpeed; // the X, y speed (delta x, delta Y)

        /// <summary>
        /// the movement angle in radians if using the angle / speed movement system
        /// </summary>
        protected float moveAngle;    // in radians

        /// <summary>
        /// Just a place to save the move angle
        /// </summary>
        protected float saveMoveAngle;

        /// <summary>
        /// the movement angle if using the angle / speed movement system
        /// </summary>
        protected float moveSpeed;

        /// <summary>
        /// the amount to adjust move angle by each tick (radians)
        /// </summary>
        public float moveAngleDelta { get; set; }

        /// <summary>
        /// the amount to adjust display angle by each tick (radians)
        /// </summary>
        public float displayAngleDelta { get; set; }

        /// <summary>
        /// the display angle in radians - which may or may not be the move angle
        /// should be set using setDisplayAngle(float d)
        /// </summary>
        public float displayAngle; // in radians

        /// <summary>
        /// Just a place to store a save of the display angle
        /// </summary>
        protected float saveDisplayAngle;

        /// <summary>
        /// the display angle offset in radians 
        /// Added to display angle to make the sprite "point in direction 0"
        /// should be set using setDisplayAngleOffset(float d)
        /// </summary>
        public float displayAngleOffset; // in radians

        /// <summary>
        /// image source is used only if xframes = 0;
        /// can be used to create a sprite that uses only a part of an image
        /// set it using setImageSource(Rectangle r)
        /// </summary>
        protected Rectangle imageSource;

        /// <summary>
        /// Animation information - number of horizontal frames
        /// </summary>
        protected int Xframes; // number of x frames
        internal int XframeWidth;

        /// <summary>
        ///  Animation information - number of vertical frames
        /// </summary>
        protected int Yframes; // number of y frames
        internal int YframeHeight;

        /// <summary>
        /// Animation information - current horizontal frame number
        /// frame numbers start at 0
        /// </summary>
        protected int Xframe; // number of the current x frame

        /// <summary>
        /// Animation information - current vertical frame number
        /// frame numbers start at 0
        /// </summary>
        protected int Yframe; // number of the current y frame

        /// <summary>
        ///  The complete animation sequence that running now (if any)
        ///  this list contains x and y frame numbers and is itself indexed by frame
        /// </summary>
        protected Vector2[] animationSeq; // the frame sequence

        /// <summary>
        /// the frame withing sequence 'animationSeq'
        /// </summary>
        protected int frame;

        /// <summary>
        ///  the first animation frame withing sequence 'animationSeq'
        /// </summary>
        protected int firstFrame; // first anim frame

        /// <summary>
        /// the last animation frame withing sequence 'animationSeq'
        /// </summary>
        protected int lastFrame;  // last anim frame

        /// <summary>
        /// A flag telling what to do when the animation finishes
        /// 0=loop
        /// 1=set sprite invisible
        /// 2=set sprite inactive and invisible
        /// </summary>
        protected int animFinished;

        /// <summary>
        /// number of ticks between aniamtion frames at 60 frames a second this could be as much as 20 or more
        /// </summary>
        protected int ticksBetweenFrames; // as it implies

        /// <summary>
        /// just a counter as it implies for animation purposes
        /// </summary>
        protected int ticks;

        /// <summary>
        /// The width of the sprite on the screen
        /// </summary>
        protected float width;

        /// <summary>
        /// the height of the sprite on the screen in pixels
        /// </summary>
        protected float height; //

        /// <summary>
        /// the width of the meaningful part of the texture
        /// </summary>
        protected float texWidth; // Width of the useful part of the texture

        /// <summary>
        /// the height of the meaningful part of the texture
        /// </summary>
        protected float texHeight; // heigth of the useful psrt of the texture

        /// <summary>
        /// hotspot offset x and y from top left corner of
        /// either the image - or single frame if its an animation
        /// hotspot offet is in pixels in the souce image
        /// </summary>
        protected Vector2 hotSpotOffset; //

        /// <summary>
        /// Counter to delay a sprites visibility
        /// updated by the routine tick()
        /// </summary>
        private int TicksToVisible = -1;

        /// <summary>
        /// Counter to remove a sprites visibility
        /// to set it call set TicksToInvissible routine
        /// updated by the routine tick()
        /// </summary>
        private int TicksToInvisible = -1;

        /// <summary>
        /// this flag can be set to make timer ticked invisibility also inactive
        /// </summary>
        internal bool makeInactive = false;

        /// <summary>
        /// Internal structure so one can check bounding box positions
        /// this turns out to be a usefull variable for debugging and display
        /// also it can be moved to boundingPoly for a non AA bounding box
        /// </summary>
        public Rect4 bbTemp; // temporary for bounding box computation

        /// <summary>
        /// Advanced Bounding box polygon offsets - its in screen pixels
        /// Needs the polygon collision utility to use
        /// only partly supported
        /// </summary>
        public Polygon12 boundingPolyOffsets=null; 

        /// <summary>
        /// Advanced Bounding box not axis aligned - its in screen pixels
        /// Needs the polygon collision utility to use
        /// only partly supported updated by routine computeBoundingPolyFromPos
        /// </summary>
        public Polygon12 boundingPoly=null; 

        /// <summary>
        /// The waypoints for the waypoint move routines
        /// </summary>
        public WayPointList wayList { set; get; }

        /// <summary>
        /// Internal source rectangle - do not touch unless you know what you are doing
        /// source of image
        /// </summary>
        public Rectangle src ;

        /// <summary>
        /// Internal destination rectangle - do not touch unless you know what you are doing
        /// destination of image
        /// </summary>
        public Rectangle dest ; // destination of image

        /// <summary>
        /// Custom added for scale, basic probably implemented somewhere else but eh
        /// </summary>
        public float scale = 1f;

        /// <summary>
        /// Internal draw rotation - do not touch unless you know what you are doing
        /// </summary>
        internal Vector2 rotateImage;

        /// <summary>
        /// This is a property for an attached renderable
        /// This can be used to show things like life bars or values if hit
        /// Importantly the renderable knows of its parent sprite
        /// If you are using attached renderables, its necessary to call the sprites Update(GameTime gameTime) method
        /// </summary>
        public RC_RenderableAttached attachedRenderable
        {
            get { return renderableAttached; }
            set
            {
                renderableAttached = value;
                value.setParent(this);
            }
        }
  
        /// <summary>
        /// This is a property for a second attached renderable
        /// This can be used to show things like life bars or values if hit
        /// Importantly the renderable knows of its parent sprite
        /// If you are using attached renderables, its necessary to call the sprites Update(GameTime gameTime) method
        /// </summary>
        public RC_RenderableAttached attachedRenderable2
        {
            get { return renderableAttached2; }
            set
            {
                renderableAttached2 = value;
                value.setParent(this);
            }
        }

        private RC_RenderableAttached renderableAttached = null;
        private RC_RenderableAttached renderableAttached2 = null;

        /// <summary>
        /// Default constructor
        /// </summary>
        public Sprite3()
        {
        initVars();
        }

        /// <summary>
        /// Standard Constructor
        /// </summary>
        /// <param name="visibleZ"></param>
        /// <param name="texZ"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Sprite3(bool visibleZ, Texture2D texZ, float x, float y)
        {
            initVars();
            visible = visibleZ;
            setTexture(texZ,true);
            setPos(x, y);
        }


       /// <summary>
       /// named Constructor with level
       /// </summary>
       /// <param name="visibleZ"></param>
       /// <param name="texZ"></param>
       /// <param name="x"></param>
       /// <param name="y"></param>
        /// <param name="nameZ"></param>
        public Sprite3(bool visibleZ, Texture2D texZ, String nameZ, float x, float y)
        {
            initVars();
            visible = visibleZ;
            setTexture(texZ,true);
            name=nameZ;
            setPos(x, y);
        }


        private void initVars()
        {
        visible=false;
        active=true;

        name = "";
        tag = 0;
        kind=0;
        state = 0;

        pos.X =0;
        pos.Y =0;

        oldPos.X=0;
        oldPos.Y=0;

        deltaSpeed.X=0; // the X, y speed (delta x, delta Y)
        deltaSpeed.Y=0; // the X, y speed (delta x, delta Y)
        moveAngle=0;
        moveSpeed=0;
        displayAngle=0;
        moveAngleDelta = 0;
        displayAngleDelta = 0;

        Xframes=1; // number of x frames
        Yframes=1; // number of y frames
        Xframe=0; // number of the current x frame
        Yframe=0; // number of the current y frame
        XframeWidth = 0;
        YframeHeight = 0;

        width=0; //
        height=0; //

        hotSpotOffset.X=0; // hotspot offset
        hotSpotOffset.Y=0; // hotspot offset

        bb.X=0; // Bounding Box offset
        bb.Y=0; // Bounding Box  offset
        bb.Width=0; // Bounding Box width
        bb.Height=0; // Bounding Box height

        tex = null;
        colour = Color.White;
        flip=SpriteEffects.None;

        texWidth=0; // Width of the useful part of the texture
        texHeight=0; // heigth of the useful psrt of the texture

        TicksToVisible = -1;
        TicksToInvisible = -1;
        }

        /// <summary>
        /// restore the current position
        /// </summary>
        public override void restorePosition()
        {
            pos.X = savePos.X;
            pos.Y = savePos.Y;            
            displayAngle = saveDisplayAngle;
            moveAngle = saveMoveAngle;
        }

        /// <summary>
        /// Save the current position
        /// </summary>
        public override void savePosition()
        {
            savePos.X = pos.X;
            savePos.Y = pos.Y;
            saveDisplayAngle = displayAngle;
            saveMoveAngle = moveAngle;
        }

        /// <summary>
        /// gets the sprite name if one was set or ""
        /// </summary>
        /// <returns></returns>
        public string getName()
        {
            return name;
        }

        private void computeFrameParams()
        {
            if (Xframes != 0 && Yframes != 0)
            {
                XframeWidth = (int)(texWidth / Xframes);
                YframeHeight = (int)(texHeight / Yframes);
            }
        }


        /// <summary>
        /// set the texture - and set a default texture width and height
        /// WARNING if computeNewSize is true all size information will be recomputed Xframes, Yframes
        /// WARNING if computeNewSize is false no size information will be recomputed
        /// Think about the value of computeNewSize its not always that obvious
        /// </summary>
        /// <param name="tw"></param>
        /// <param name="computeNewSize"></param>
        public void setTexture(Texture2D tw, bool computeNewSize)
        {
            if (tex == null || computeNewSize) // previously null or new size
            {
                tex = tw;
                setWidthHeightOfTexToTex();
                setWidthHeightToTex();
                computeFrameParams();
                setBBToWH();
            }
            else
            {  
                tex = tw;
            }
        }

        /// <summary>
        /// get the texturewrapper</summary>
        public Texture2D getTexture()
        {
            return tex;
        }

        /// <summary>
        /// Return a Rectangle structure which is the sprite boundary for screen graphic purposes
        /// I am not sure if this is used or of value any more (deprecated)
        /// </summary>
        public Rectangle imageRectangle()
        {
            return new Rectangle((int)pos.X,(int)pos.Y,(int)width,(int)height);
        }


        /// <summary>
        /// Return the axis aligned bounding box at the current position 
        /// even if the display is rotated
        /// </summary>
        public Rectangle getBoundingBoxAA()
        {
            Rectangle retv = new Rectangle(0, 0, 0, 0); // the bounding box 
            Rectangle dest = new Rectangle(0, 0, 0, 0);

            Rectangle src = getActualImageSource();
            Vector2 hs = new Vector2(hotSpotOffset.X, hotSpotOffset.Y);
                       
            Vector2 newhs = new Vector2(0, 0);

            Util.computeDestFromSource(src, bb, new Point((int)hs.X, (int)hs.Y), pos, width, height, ref dest, ref retv, ref newhs);
            bbTemp = new Rect4(retv);
            if (displayAngle + displayAngleOffset == 0) return retv;

            bbTemp.rotateRect(new Vector2(pos.X, pos.Y), displayAngle + displayAngleOffset);
            retv = bbTemp.getAABoundingRect();
            return retv;

            //return getBoundingBoxAAPos(pos);
        }

        /// <summary>
        /// Gets a Polygon12 structure which is the non axis aligned bounding box from the Rect4
        /// its a fairly slow routine, if you use getBoundingBoxAA() anyway just do copyRect4tempToBB()
        /// its faster
        /// </summary>
        /// <returns></returns>
        public Polygon12 setBoundingPolyFromBB()
        {
            getBoundingBoxAA();
            copyRect4tempToBoundingPoly();
            return boundingPoly;
        }

        /// <summary>
        /// copies the temp rect4 structure to non axis aligned bounding box  
        /// </summary>
        public void copyRect4tempToBoundingPoly()
        {
            boundingPoly = new Polygon12(bbTemp);
        }

        /// <summary>
        /// Computes the bounding polygon from the offsets and position
        /// </summary>
        public void computeBoundingPolyFromPos()
        {
            boundingPoly = new Polygon12(boundingPolyOffsets);
            for (int i = 0; i < boundingPolyOffsets.numOfPoints; i++)
            {
                boundingPoly.point[i].X = boundingPoly.point[i].X + getPosX();
                boundingPoly.point[i].Y = boundingPoly.point[i].Y + getPosY();
            }
        }

        /// <summary>
        /// retreives the bounding pollygon - which assumed that the user
        /// called necessary routines to set it one of:
        ///   *  copyRect4tempToBoundingPoly
        ///   *  setBoundingPolyFromBB
        ///   *  computeBoundingPolyFromPos
        /// </summary>
        /// <returns></returns>
        public Polygon12 getBoundingPoly()
        {
            return boundingPoly;
        }

        /// <summary>
        /// Return the mid point of the axis aligned bounding box at the current position 
        /// </summary>
        public Vector2 getBoundingBoxMiddle()
        {
            Rectangle r = getBoundingBoxAA();
            return (new Vector2(r.X+r.Width/2.0f, r.Y+r.Height/2.0f));
        }

        // /// <summary>
        // /// Return the axis aligned bounding box even if the display is rotated
        // /// </summary>
        // /// <param name="p"> The position that the bounding box is calculated from </param>
        // /// <returns></returns>
        //public Rectangle getBoundingBoxAAPos(Vector2 p)
        //{
        //    Vector2 t = new Vector2(hotSpotOffset.X*((float)width/XframeWidth),hotSpotOffset.Y*((float)height/YframeHeight)); // hotspot after scaling
        //    if (displayAngle + displayAngleOffset == 0)
        //    {
        //        Rectangle retv = new Rectangle((int)(p.X + bb.X - t.X), (int)(p.Y + bb.Y - t.Y), (int)bb.Width, (int)bb.Height);
        //        bbTemp = new Rect4(retv);
        //        return retv;
        //    }
        //    else
        //    {
        //        Rectangle retv = new Rectangle((int)(p.X + bb.X - t.X), (int)(p.Y + bb.Y - t.Y), (int)bb.Width, (int)bb.Height);
        //        bbTemp = new Rect4(retv);
        //        bbTemp.rotateRect(new Vector2(p.X, p.Y), displayAngle + displayAngleOffset);
        //        retv = bbTemp.getAABoundingRect();
        //        return retv;
        //    }
        //}

        /// <summary>
        /// Return true if two sprites axis aligned bounding boxes collide at their current locations
        /// </summary>
        public bool collision(Sprite3 s2)
        {
            if (!s2.getActive()) return false;
            if (!s2.getVisible()) return false;
            return getBoundingBoxAA().Intersects(s2.getBoundingBoxAA());
        }

        /// <summary>
        /// Hacky as shit for getting mouse pos clicks
        /// Should probably just use 
        /// public Boolean inside(float x, float y) but keeping this here for safety
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public bool vectorCollides(Vector2 v)
        {
            Rectangle d = new Rectangle(0, 0, (int)v.X, (int)v.Y);
            return getBoundingBoxAA().Intersects(d);
        }

        /// <summary>
        /// Return a rectangle that represents a collision rectangle (the intersection of the two rectangles)</summary>
        public Rectangle collisionRect(Sprite3 s2)
        {
            return Rectangle.Intersect(getBoundingBoxAA(), s2.getBoundingBoxAA());
        }

        /// <summary>
        /// Return a boolean that is true if the point is inside the AA bounding box of the sprite</summary>
        public Boolean inside(float x, float y)
        {
            //  Boolean retv=true;
            Rectangle temp = getBoundingBoxAA();
            if (x <= temp.Left) return false;
            if (y <= temp.Top) return false;
            if (x >= temp.Right) return false;
            if (y >= temp.Bottom) return false;
            return true;
        }

        /// <summary>
        /// Return a boolean that is true if the point is inside or on the AA bounding box of the sprite</summary>
        public Boolean insideOrEq(float x, float y)
        {
            //  Boolean retv=true;
            Rectangle temp = getBoundingBoxAA();
            if (x < temp.Left) return false;
            if (y < temp.Top) return false;
            if (x > temp.Right) return false;
            if (y > temp.Bottom) return false;
            return true;
        }

        /// <summary>
        /// Used to scroll the entire screen - while preserving old position 
        /// usually called in spritelist 
        /// </summary>
        /// <param name="x">the amount to scroll in x </param>
        /// <param name="y">the amount to scroll in y</param>
        /// <returns> true if successfull </returns>
        public override bool scrollMove(float x, float y)
        {
            oldPos.X = oldPos.X+x;
            oldPos.Y = oldPos.Y+y;
            pos.X = pos.X + x;
            pos.Y = pos.Y + y;
            return true;
        }

        /// <summary>
        /// Moves the sprite by the delta x and delta y in deltaSpeed
        /// use setDeltaSpeed(Vector2 ds) to set the movement
        /// </summary>
        public void moveByDeltaXY()
        {
            oldPos.X = pos.X;
            oldPos.Y = pos.Y;
            pos.X = pos.X + deltaSpeed.X;
            pos.Y = pos.Y + deltaSpeed.Y;
        }

        /// <summary>
        /// Moves the sprite by the delta x in dx
        /// used to separate x and y when using dx/dy movement
        /// mainly to assist in two part collision detection
        /// </summary>
        public void moveByDeltaX(float dx)
        {
            oldPos.X = pos.X;
            pos.X = pos.X + dx;
        }

        /// <summary>
        /// Moves the sprite by the delta y in dy
        /// used to separate x and y when using dx/dy movement
        /// mainly to assist in two part collision detection
        /// </summary>
        public void moveByDeltaY(float dy)
        {
            oldPos.Y = pos.Y;
            pos.Y = pos.Y + dy;
        }

        /// <summary>
        /// Moves the sprite by the delta x and delta y in deltaSpeed
        /// uses an input delta so no need to call setDeltaSpeed(Vector2 ds) 
        /// </summary>
        public void moveByDeltaXY(Vector2 ds)
        {
            oldPos.X = pos.X;
            oldPos.Y = pos.Y;
            pos.X = pos.X + ds.X;
            pos.Y = pos.Y + ds.Y;
        }


        /// <summary>
        /// Moves the sprite towards the position pos
        /// </summary>
        /// <param name="posZ">the position to move towards</param>
        /// <param name="speed">the speed to move at </param>
        /// <param name="setDisplayAngle"> true if you want to orient the sprites display</param>
        /// <returns> true if we got there </returns>
        public bool moveTo(Vector2 posZ, float speed, bool setDisplayAngle)
        {
            bool retv = false;            
            oldPos.X = pos.X;
            oldPos.Y = pos.Y;
            float dist=(pos-posZ).Length();
            if (dist < 0.8) 
                return true;
            float s = speed;
            if (dist < speed) { s = dist; retv = true; }
            float moveAngle = angleTo(posZ); 
            Vector2 temp = Util.moveByAngleDist(pos, moveAngle, s);
            pos.X = temp.X;
            pos.Y = temp.Y;

            if (setDisplayAngle)
            {
                float angle = Util.getAngle(pos, oldPos);
                displayAngle = angle;
            }

            return retv;
        }

 /// <summary>
 /// Moves the sprite towards the waypoint 
 /// </summary>
 /// <param name="wayPoint">the waypoint to move to</param>
 /// <param name="setDisplayAngle">true if you want to orient the sprites display</param>
 /// <returns>true if we got there</returns>
        public bool moveTo(WayPoint wayPoint, bool setDisplayAngle)
        {
            return moveTo(wayPoint.pos, wayPoint.speed, setDisplayAngle);
        }

        /// <summary>
        /// Move the sprite around the waypoint list
        /// </summary>
        /// <returns></returns>
        public bool moveWayPointList(bool setDisplayAngle)
        {

            WayPoint w = wayList.currentWaypoint();
            bool rc = moveTo(w.pos, w.speed, setDisplayAngle);
            if (rc)
            {
                bool z = wayList.nextLeg();
                if (wayList.wayFinished == 2 && !z)
                {
                    visible = false;
                    active = false;
                }
            }
            return rc;
        }

        /// <summary>
        /// Puts the sprite at the start of the waypoint list
        /// </summary>
        /// <returns></returns>
        public void moveToStartOfWayPoints()
        {
            if (wayList == null) return;
            WayPoint w = wayList.getWayPoint(0);
            setPos(w.pos);
        }

        /// <summary>
        /// Moves the sprite by the delta x and delta y in deltaSpeed
        /// </summary>
        public void moveByAngleSpeed()
        {   
            oldPos.X = pos.X;
            oldPos.Y = pos.Y;
            Vector2 temp = Util.moveByAngleDist(pos, moveAngle, moveSpeed);            
            pos.X = temp.X;
            pos.Y = temp.Y; 
        }

        /// <summary>
        /// Adjust 
        /// </summary>
        public void adjustAngles()
        {
            moveAngle = moveAngle + moveAngleDelta;
            displayAngle = displayAngle + displayAngleDelta;
            if (moveAngle > MathHelper.Pi * 2) moveAngle = moveAngle - MathHelper.Pi * 2;
            if (displayAngle > MathHelper.Pi * 2) displayAngle = displayAngle - MathHelper.Pi * 2;
        }

        /// <summary>
        /// Move with screen (or world wrap)
        /// </summary>
        /// <param name="r"></param>
        public void moveByAngleSpeedLimit(Rectangle r)
        {
            oldPos.X = pos.X;
            oldPos.Y = pos.Y;
            Vector2 temp = Util.moveByAngleDist(pos, moveAngle, moveSpeed);
            pos.X = temp.X;
            pos.Y = temp.Y;

            if (pos.X < r.X) pos.X = pos.X + r.Width;
            if (pos.Y < r.Y) pos.Y = pos.Y + r.Height;
            if (pos.X > r.X + r.Width) pos.X = pos.X - r.Width; 
            if (pos.Y > r.Y + r.Height) pos.Y = pos.Y - r.Height;
            
            return;
        }

        /// <summary>
        /// gets the flip parameter 
        /// (which sets sprite effects for horizontal and vertical flipping)
        /// </summary>
        /// <returns></returns>
        public SpriteEffects getFlip()
        {
            return flip;
        }

        /// <summary>
        /// sets the flip paramentser 
        /// </summary>
        /// <param name="se"></param>
        public void setFlip(SpriteEffects se)
        {
            flip = se;
        }

        /// <summary>
        /// sets the width and height of the sprite in display pixels
        /// </summary>
        /// <param name="w"></param>
        /// <param name="h"></param>
        public void setWidthHeight(float w, float h)
        {
            width = w;
            height = h;
        }

        /// <summary>
        /// sets the width and height of the sprite to what was read in from the texture
        /// This is usefull when you want the whole image
        /// </summary>
        public void setWidthHeightToTex()
        {
            if (tex != null)
            {
                width = tex.Width;
                height = tex.Height;
            }
            else
            {
                width = 0;
                height = 0;
            }
        }

        /// <summary>
        /// Just set the sprite width in display pixels
        /// </summary>
        /// <param name="w"></param>
        public void setWidth(float w)
        {
            width = w;
        }

        /// <summary>
        /// just set the sprite height in display pixels
        /// </summary>
        /// <param name="h"></param>
        public void setHeight(float h)
        {
            height = h;
        }

        /// <summary>
        /// get the width in display pixels
        /// </summary>
        /// <returns></returns>
        public float getWidth()
        {
            return width;
        }

        /// <summary>
        /// get the height in display pixels
        /// </summary>
        /// <returns></returns>
        public float getHeight()
        {
            return height;
        }

        /// <summary>
        /// This sets the usefull part of the texture (which is assumed to be the top left bit)
        /// its necessary because not all textures conform to the power of 2 rule and are 
        /// upsized to a power of two
        /// </summary>
        /// <param name="w"></param>
        /// <param name="h"></param>
        public void setWidthHeightOfTex(float w, float h)
        {
            texWidth = w; // Width of the useful part of the texture
            texHeight = h; // heigth of the useful psrt of the texture
            computeFrameParams();
        }
        
        /// <summary>
        /// Just sets the active part of the texture to the whole texture
        /// </summary>
        public void setWidthHeightOfTexToTex()
        {
            if (tex != null) setWidthHeightOfTex(tex.Width,tex.Height); // heigth of the useful psrt of the texture
        }

        /// <summary>
        /// Sets a source image for the sprite (usually less than the total texture) 
        /// WARNING This will alter the Xframes, Yframes
        /// </summary>
        /// <param name="r">The source image rectangle</param>
        public virtual void setImageSource(Rectangle r)
        {
            Xframes = 0;
            Yframes = 0;
            imageSource = new Rectangle(r.X, r.Y, r.Width, r.Height);
        }

        /// <summary>
        /// returns the image source parameter for the sprite for the current frame use getActualImageSource()
        /// </summary>
        /// <returns></returns>
        public Rectangle getImageSource()
        {
            return new Rectangle(imageSource.X, imageSource.Y, imageSource.Width, imageSource.Height);
        }


        /// <summary>
        /// Where we get the sprite image from not intended for external use
        /// </summary>
        public Rectangle getActualImageSource()
        {
            if (Xframes == 1 && Yframes == 1)
            {
                return new Rectangle(0, 0, (int)texWidth, (int)texHeight);
            }
            else
            {
                if (Xframes == 0)
                {
                    return imageSource;
                }
                else
                {
                    Rectangle r = new Rectangle();
                    r.Width = (int)XframeWidth;
                    r.Height = (int)YframeHeight;
                    r.X = Xframe * r.Width;
                    r.Y = Yframe * r.Height;
                    return r;
                }
            }
        }

        /// <summary>
        /// Where we would draw the sprite if axis aligned not intended for external use
        /// </summary>
        internal Rectangle getImageDest()
        {
            return new Rectangle((int)pos.X, (int)pos.Y, (int)width, (int)height);
        }

        /// <summary>
        /// Draws the sprite if it is Visible (ie if the visible property is set)
        /// </summary>
        public override void Draw(SpriteBatch sb)
        {
            draw(sb);
        }

        /// <summary>
        /// Prepares the internal variables src,dest and rotateImage for drawing
        /// It can be used for - writing virtual screen handlers that zoom, resize and pan 
        /// </summary>
        public void prepareToDraw()
        {
            src = getActualImageSource();
            dest = getImageDest();
            rotateImage.X = hotSpotOffset.X;
            rotateImage.Y = hotSpotOffset.Y;
        }

        /// <summary>
        /// Custom Draw
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void customDraw(SpriteBatch spriteBatch, Vector2 position, Color color, float orientation, Vector2 size)
        {
            if (visible && active && tex != null)
            {
                prepareToDraw();
                spriteBatch.Draw(tex, position, src, color, orientation, size / 2f, 1f, 0, 0);
                //spriteBatch.Draw(tex, position, null, color, orientation, new Vector2(size.X / 2, size.Y / 2), 1f, flip, 0);
                if (attachedRenderable != null) attachedRenderable.Draw(spriteBatch);
                if (attachedRenderable2 != null) attachedRenderable2.Draw(spriteBatch);
            }
        }

        public void customDraw(SpriteBatch spriteBatch, float orientation, Vector2 size)
        {
            if (visible && active && tex != null)
            {
                prepareToDraw();
                spriteBatch.Draw(tex, dest, src, getColor(), orientation, size / 2f, flip, 0.5f);
                //spriteBatch.Draw()
                if (attachedRenderable != null) attachedRenderable.Draw(spriteBatch);
                if (attachedRenderable2 != null) attachedRenderable2.Draw(spriteBatch);
            }
        }

        public void customDrawButton(SpriteBatch spriteBatch)
        {
            if (visible && active && tex != null)
            {
                prepareToDraw();
                Vector2 pos = new Vector2(dest.X, dest.Y);
                spriteBatch.Draw(tex, pos, src, getColor(), displayAngle + displayAngleOffset, 
                    new Vector2(getWidth(),getHeight()) / 2f, scale, 0, 0);
                if (attachedRenderable != null) attachedRenderable.Draw(spriteBatch);
                if (attachedRenderable2 != null) attachedRenderable2.Draw(spriteBatch);
            }
        }

        /// <summary>
        /// Draws the sprite if it is Visible (ie if the visible property is set)
        /// </summary>
        public void draw(SpriteBatch sb)
        {
            if (visible && active && tex != null)
            {
                prepareToDraw();
                sb.Draw(tex, dest,src, getColor(), displayAngle+displayAngleOffset, rotateImage, flip, 0.5f);
                if (attachedRenderable != null) attachedRenderable.Draw(sb);
                if (attachedRenderable2 != null) attachedRenderable2.Draw(sb);
            }
        }

        /// <summary>
        /// Gets the point about which to rotate the image in Image co-ordinates
        /// </summary>
        /// <returns></returns>
        public Vector2 getRotateImage()
        {
            Vector2 retv = new Vector2();
            if (Xframes==1 && Yframes==1)
            {
                retv.X = hotSpotOffset.X;
                retv.Y = hotSpotOffset.Y;
            }
            else
            {
                retv.X = hotSpotOffset.X + (XframeWidth * Xframe);
                retv.Y = hotSpotOffset.Y + (YframeHeight * Yframe);
            }
            return retv;
        }

        /// <summary>
        /// Sets the bounding box relative to the hotspot (or position)
        /// if the hotspot is in the middle of the source image there is a fair chance
        /// that bb.X and bb.Y will be negative
        /// </summary>
        /// <param name="bbXoffset"></param>
        /// <param name="bbYoffset"></param>
        /// <param name="bbWidthZ"></param>
        /// <param name="bbHeightZ"></param>
        public void setBB(float bbXoffset, float bbYoffset, float bbWidthZ, float bbHeightZ)
        {
            bb.X = Convert.ToInt32(bbXoffset);
            bb.Y = Convert.ToInt32(bbYoffset);
            bb.Width = Convert.ToInt32(bbWidthZ); // Bounding Box width 
            bb.Height = Convert.ToInt32(bbHeightZ); // Bounding Box height 
        }

        /// <summary>
        /// lol for basic sprites set the BB to the texture with and height
        /// </summary>
        public void setBBToTexture()
        {
            bb.X = 0;
            bb.Y = 0;
            bb.Width = tex.Width; // Bounding Box width 
            bb.Height = tex.Height; // Bounding Box height 
        }

        /// <summary>
        /// lol for less basic sprites set the BB to the texture active width and active height
        /// </summary>
        public void setBBToTextureActive()
        {
            bb.X = 0;
            bb.Y = 0;
            bb.Width = tex.Width; // Bounding Box width 
            bb.Height = tex.Height; // Bounding Box height 
        }


        /// <summary>
        /// set the bounding box to the width and height of the sprite
        /// typically this is a bounding box that follows the image boundary
        /// with a hotspot in the image top left corner
        /// </summary>
        public void setBBToWH()
        {
            bb.X = 0;
            bb.Y = 0;
            bb.Width = (int)width; // Bounding Box width 
            bb.Height = (int)height; // Bounding Box height
        }

        /// <summary>
        /// set the bounding box to the width and height of A single frame
        /// typically this is a bounding box that follows the image boundary
        /// with a hotspot in the image top left corner
        /// </summary>
        public void setBBToFrameInTex()
        {
            bb.X = 0;
            bb.Y = 0;
            bb.Width = (int)(texWidth/Xframes); // Bounding Box width 
            bb.Height = (int)(texWidth/Yframes); // Bounding Box height
        }

        /// <summary>
        /// Sets the bounding box to a fraction of the texture width and height 
        /// eg setBBToFraction(0.6) would creat a small bounding box whose dimensions are 
        /// 0.6 of the full sprite dimensions with the bboffset sane if the hotspot is 0,0
        /// </summary>
        public void setBBFractionOfTexCentered(float f)
        {
            bb.Width = Convert.ToInt32(tex.Width * f); // Bounding Box width 
            bb.Height = Convert.ToInt32(tex.Height * f); // Bounding Box height           
            bb.X = Convert.ToInt32(tex.Width * (1 - f) / 2);
            bb.Y = Convert.ToInt32(tex.Height * (1 - f) / 2);
        }

        /// <summary>
        /// Sets the bounding box to a fraction of the texture width and height 
        /// eg setBBToFraction(0.6, 5, 7) would creat a small bounding box whose dimensions are 
        /// 0.6 of the full sprite dimensions with the bboffset at 5,7
        /// </summary>
        public void setBBFractionOfTex(float f, int xoffset, int yoffset)
        {
            bb.Width = Convert.ToInt32(tex.Width * f); // Bounding Box width 
            bb.Height = Convert.ToInt32(tex.Height * f); // Bounding Box height           
            bb.X = xoffset;
            bb.Y = yoffset;
        }

        /// <summary>
        /// Sets the bounding box to a fraction of the texture width and height 
        /// eg setBBToFraction(0.6) would creat a small bounding box whose dimensions are 
        /// 0.6 of the full sprite dimensions with the bboffset sane if the hotspot is 0,0
        /// </summary>
        public void setBBandHSFractionOfTexCentered(float f)
        {
            bb.Width = Convert.ToInt32(tex.Width * f); // Bounding Box width 
            bb.Height = Convert.ToInt32(tex.Height * f); // Bounding Box height           
            bb.X = (int)Math.Round(tex.Width * (1 - f)/2.0); // 
            bb.Y = (int)Math.Round(tex.Height * (1 - f)/2.0); // 
            hotSpotOffset.X = tex.Width / 2;
            hotSpotOffset.Y = tex.Height / 2;
        }


        /// <summary>
        /// Get the bouding rectange in screen co ordinates - relative to the sprite pos
        /// </summary>
        /// <returns></returns>
        public Rectangle getBB()
        {
            return bb;
        }

  
        /// <summary>
        /// Set the hotspot offset 
        /// Note that this is in original image pixels not pixels after resize 
        /// </summary>
        /// <param name="offset"></param>
        public void  setHSoffset(Vector2 offset)
        {
        hotSpotOffset.X=offset.X;
        hotSpotOffset.Y=offset.Y;
        }

        /// <summary>
        /// set the Hotspot offset
        /// </summary>
        /// <returns></returns>
        public Vector2 getHSOffset()
        {
            return hotSpotOffset;
        }


        /// <summary>
        /// draws a bounding box
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="c"></param>
        public virtual void drawBB(SpriteBatch sb, Color c) // 
        {
            Rectangle bb = getBoundingBoxAA();
            LineBatch.drawLineRectangle(sb, bb, c);
        }

        /// <summary>
        /// Draws the hotspot/position of the sprite
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="c"></param>
        public void drawHS(SpriteBatch sb, Color c) // 
        {
            //Vector2 rotateImage = getRotateImage();
            LineBatch.drawCross(sb, pos.X, pos.Y, 3, c, c);
        }

        /// <summary>
        /// draw bounding box and hotspot
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="colorBB"></param>
        /// <param name="colorHS"></param>
        public void drawInfo(SpriteBatch sb, Color colorBB, Color colorHS)  
        {
            drawBB(sb,colorBB);
            drawHS(sb, colorHS);
        }

        /// <summary>
        /// draw bounding sphere if not zero
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="colorBS"></param>
        public void drawBoundingSphere(SpriteBatch sb, Color colorBS)
        {
            if (boundingSphereRadius > 0)
            {
                LineBatch.drawCircle(sb,colorBS,new Vector2(pos.X,pos.Y),boundingSphereRadius,19,1);
            }
        }

        /// <summary>
        /// Draws the bounding polygon and the hotspot if it exists
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="colorBB"></param>
        /// <param name="colorHS"></param>
        public void drawBoundingPoly(SpriteBatch sb, Color colorBB, Color colorHS)
        {
            if (boundingPoly != null)
            {   
                drawHS(sb, colorHS);
                LineBatch.drawPolygon12(sb, boundingPoly, colorBB);
            }
        }

        /// <summary>
        /// For debugging only to make sure rotations are where they are expected to be
        /// You must have called get bounding box Axis aligned first
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="c"></param>
        public void drawRect4(SpriteBatch sb, Color c)
        {
            LineBatch.drawRect4(sb, bbTemp, c);
        }

        /// <summary>
        /// Set a speed vector for the deltax/deltay move system
        /// </summary>
        /// <param name="ds"></param>
        public void setDeltaSpeed(Vector2 ds)
        {
            deltaSpeed = new Vector2(ds.X,ds.Y); // the X, y speed (delta x, delta Y)
        }

        /// <summary>
        /// get the speed deltas for the deltax deltay move system
        /// </summary>
        /// <returns></returns>
        public Vector2 getDeltaSpeed()
        {
            return deltaSpeed;
        }

        /// <summary>
        /// Get the current movemnet angle for the angle / speed move system 
        /// </summary>
        /// <returns></returns>
        public float getMoveAngleRadians()    // in radians
        {
            return moveAngle;
        }

        /// <summary>
        /// Set the current movemnet angle in radians for the angle / speed move system
        /// </summary>
        /// <param name="m"></param>
        public void setMoveAngleRadians(float m)    // in radians
        {
            moveAngle = m;
        }

        /// <summary>
        /// Set the current movemnet angle for the angle / speed move system
        /// </summary>
        /// <param name="m"></param>
        public void setMoveAngleDegrees(float m)    // in degrees
        {
            moveAngle = Util.degToRad(m);
        }

        /// <summary>
        /// Get the current movemnet speed for the angle / speed move system
        /// </summary>
        /// <returns></returns>
        public float getMoveSpeed()
        {
            return moveSpeed;
        }

        /// <summary>
        /// Set the current movemnet speed for the angle / speed move system
        /// </summary>
        /// <param name="m"></param>
        public void setMoveSpeed(float m)
        {
            moveSpeed=m;
        }

        /// <summary>
        /// get display angle in radians
        /// </summary>
        /// <returns></returns>
        public float getDisplayAngleRadians() // in radians
        {
            return displayAngle;
        }

        /// <summary>
        /// set display angle in radians
        /// </summary>
        /// <returns></returns>
        public void setDisplayAngleRadians(float d) // in radians
        {
            displayAngle = d;
        }

        /// <summary>
        /// Set display angle in degrees
        /// </summary>
        /// <param name="d"></param>
        public void setDisplayAngleDegrees(float d) // in degrees
        {
            displayAngle = Util.degToRad(d);
        }

        /// <summary>
        /// set the display angle offset in radians - which is the angle added to the displayAngle to make the sprite face in direction 0
        /// </summary>
        /// <param name="d"></param>
        public void setDisplayAngleOffset(float d) // in radians
        {
            displayAngleOffset = d;
        }

        /// <summary>
        /// set the display angle offset - which is the angle added to the displayAngle to make the sprite face in direction 0
        /// </summary>
        /// <param name="d"></param>
        public void setDisplayAngleOffsetDegrees(float d) // in degrees
        {
            displayAngleOffset = Util.degToRad(d);
        }

        /// <summary>
        /// get the display angle offset
        /// </summary>
        /// <returns></returns>
        public float getDisplayAngleOffset() // in radians
        {
            return displayAngleOffset;
        }


        /// <summary>
        /// Set the number of horizontal frames of animation
        /// </summary>
        /// <param name="f"></param>
        public void setXframes(int f) // 
        {
            Xframes = f;
            computeFrameParams();
        }

        /// <summary>
        /// Set the number of vertical frames of animation
        /// </summary>
        /// <param name="f"></param>
        public void setYframes(int f) // 
        {
            Yframes = f;
            computeFrameParams();
        }

        /// <summary>
        /// get the number of vertical frames of animation
        /// </summary>
        /// <returns></returns>
        public int getYframes() // 
        {
            return Yframes;
        }
        
        /// <summary>
        /// set the number of vertical frames of animation
        /// </summary>
        /// <returns></returns>
        public int getXframes() // 
        {
            return Xframes;
        }

        /// <summary>
        /// get the current horizontal frame
        /// </summary>
        /// <returns></returns>
        public int getXframe() // 
        {
            return Xframe;
        }

        /// <summary>
        /// get the current vertical frame
        /// </summary>
        /// <returns></returns>
        public int getYframe() // 
        {
            return Yframe;
        }
        
        /// <summary>
        /// Get the current frame number which is an index into the 
        /// animation frame sequence data updated by setAnimationSequence 
        /// </summary>
        /// <returns></returns>
        public int getFrame() // 
        {
            return frame;
        }

        /// <summary>
        /// set the current vertical frame
        /// </summary>
        /// <param name="f"></param>
        public void setYframe(int f) // 
        {
            Yframe = f;
        }

        /// <summary>
        /// Set the current horizontal frame
        /// </summary>
        /// <param name="f"></param>
        public void setXframe(int f) // 
        {
            Xframe = f;
        }

        /// <summary>
        /// Set the current frame number which is an index into the 
        /// animation frame sequence data updated by setAnimationSequence 
        /// </summary>
        /// <param name="f"></param>
        public void setFrame(int f) // 
        {
            frame = f;
        }

        /// <summary>
        /// Set an animation sequenece
        /// </summary>
        /// <param name="animSeq"></param>
        /// <param name="firstFrameZ"></param>
        /// <param name="lastFrameZ"></param>
        /// <param name="ticksBetweenFramesZ"></param>
        public void setAnimationSequence(Vector2[] animSeq, int firstFrameZ, int lastFrameZ, int ticksBetweenFramesZ) // the frame sequence
        {

            animationSeq = animSeq;
            firstFrame = firstFrameZ;
            lastFrame = lastFrameZ;
            ticksBetweenFrames = ticksBetweenFramesZ;
        }

        /// <summary>
        /// Upadate the animation sequence
        /// This routine is usually run in the update routine
        /// </summary>
        public void animationTick()
        {
            if (animationSeq == null) return;
            ticks++;
            if (ticksBetweenFrames != 0)
            if (ticks % ticksBetweenFrames == 0)
            {
                frame = frame + 1;
                if (frame > lastFrame) 
                {
                // 0=loop
                // 1=set sprite invisible
                // 2=set sprite inactive and invisible
                if (animFinished==0) frame = firstFrame;
                if (animFinished > 0)
                {
                    visible = false;
                    frame = firstFrame;
                }
                if (animFinished == 2)
                    {
                        active = false;
                        visible=false;
                        frame = firstFrame;
                    }

                }
                Xframe = (int)animationSeq[frame].X;
                Yframe = (int)animationSeq[frame].Y;
            }
        }

        /// <summary>
        /// What to do when the animation is finished
        /// 0=loop
        /// 1=set sprite invisible
        /// 2=set sprite inactive and invisible
        /// </summary>
        /// <param name="whatToDo"></param>
        public void setAnimFinished(int whatToDo)
        {
            animFinished = whatToDo;
        }

        /// <summary>
        /// Set the start parameters for the animation sequence
        /// </summary>
        public void animationStart()
        {
            frame = firstFrame;
            ticks = 0;
            Xframe = (int)animationSeq[frame].X;
            Yframe = (int)animationSeq[frame].Y;
        }

        /// <summary>
        /// Set the start parameters for the animation sequence allowing for some additional information
        /// </summary>
        /// <param name="ticksZ"></param>
        /// <param name="frameZ"></param>
        public void animationStart(int ticksZ, int frameZ)
        {
            frame = frameZ;
            ticks = ticksZ;
            Xframe = (int)animationSeq[frame].X;
            Yframe = (int)animationSeq[frame].Y;
        }

        /// <summary>
        /// Distance to another sprite
        /// </summary>
        /// <param name="sprite"></param>
        /// <returns></returns>
        public float distanceTo(Sprite3 sprite)
        {
            return distanceTo(sprite.getPos());
        }

        /// <summary>
        /// Distance to an arbitary point
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public float distanceTo(Vector2 point)
        {
            return (pos - point).Length();
        }

        /// <summary>
        /// Aligns the display angle to the sprites current movement angle based on oldpos - 
        /// only works if the sprite is actually moving in a way that oldPos is updated correctly
        /// so use setPos to set position (only one per update call)
        /// </summary>
        public void alignDisplayAngle()
        {
            if (oldPos.X == pos.X && oldPos.Y == pos.Y) return;
            setDisplayAngleRadians((float)(angleTo(oldPos)+Math.PI));
        }

        /// <summary>
        /// computes the angle to another sprite
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public float angleTo(Sprite3 s)
        {
            return angleTo(s.getPos());
        }

        /// <summary>
        /// computes the angle to an arbitary point
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public float angleTo(Vector2 point)
        {
            return Util.getAngle(point,pos);
        }

        /// <summary>
        /// get the active flag
        /// </summary>
        /// <returns></returns>
        public bool getActive() { return active; }

        /// <summary>
        /// set the active flag
        /// </summary>
        /// <param name="activeZ"></param>
        public void setActive(bool activeZ) { active = activeZ; }

        /// <summary>
        /// set the active flag
        /// </summary>
        /// <param name="activeZ"></param>
        public void setActiveAndVisible(bool activeZ) { active = activeZ; visible = activeZ; }

        /// <summary>
        /// sets the number if ticks till the sprite becomes visible
        /// </summary>
        /// <param name="ticks"></param>
        public void setTicksToVisible(int ticks)
        {
        TicksToVisible=ticks;
        }

        /// <summary>
        /// Sets the ticks untill the sprite becomes invisible
        /// optionally inactive as well
        /// to use it you must call tick() in update for the sprite
        /// </summary>
        /// <param name="ticks"></param>
        /// <param name="inactive"></param>
        public void setTicksToInvisible(int ticks, bool inactive)
        {
        TicksToInvisible = ticks;
        makeInactive=inactive;
        }

        /// <summary>
        /// Tick the visible / invisible ticker
        /// Tick is used in sprite 3 to make a sprite visible after a certain time
        /// see the routine setTicksToVisible to set the tick counter
        /// </summary>
        public virtual void tick()
        {
            if (TicksToVisible > 0)
            {
                TicksToVisible--;
                if (TicksToVisible == 0) setVisible(true);
            }

            if (TicksToInvisible > 0)
            {
                TicksToInvisible--;
                if (TicksToInvisible == 0)
                {
                    setVisible(false);
                    if (makeInactive) active = false;
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (active)
            {
                if (attachedRenderable != null) attachedRenderable.Update(gameTime);
                if (attachedRenderable2 != null) attachedRenderable2.Update(gameTime);
            }
        }

    }


    // ************************************************************************************************ //

    /// <summary>
    /// This is a parent class for classes that create sprites - generators and such
    /// </summary>
    public class SpriteFactoryParent : RC_Renderable // from renderable to give us update loops and ability to use lists 
    {
        /// <summary>
        /// Generate a sprite kind and param is optionnal
        /// </summary>
        /// <param name="kind"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public virtual Sprite3 generate(int kind, int param)
        {
            return null;
        }

        /// <summary>
        /// Generate a sprite kind and param is optionnal
        /// </summary>
        /// <returns></returns>
        public virtual Sprite3 generate()
        {
            return generate(0, 0);
        }

    }



}