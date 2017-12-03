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
    /// Parent class for all renderables sprites and list of things to draw has active flag, color, and visible properties
    /// and draw, load contenet and update functions
    /// also mouse and keyhit functions
    /// 
    /// Recently added tag and tagInt to help making general purpose renderables 
    /// </summary>
    public class RC_Renderable
    {

        /// <summary>
        /// this variable called active is used by things like renderable_list, sprite_list and gui_list
        /// if a rewnderable is not active it means it can be deleted or overwritten
        /// it is used in conjunction with sprite list to manage activity of sprites in a list
        /// importantly inactive renderables should not be given Draw or update (or other UI) events
        /// </summary>
        public bool active { set; get; }

        /// <summary>
        /// This is the render colour if the renderable its usually pushed into the spritebatch draw unchanged
        /// </summary>
        public Color colour { get; set; }

        /// <summary>
        /// is it visible
        /// </summary>
        public bool visible { get; set; }

        /// <summary>
        /// this variable called tag is available for use by user code
        /// </summary>
        public float tag { get; set; }

        /// <summary>
        /// this variable called tagInt is available for use by user code
        /// </summary>
        public int tagInt { get; set; }

        /// <summary>
        /// a default constructor
        /// </summary>
        public RC_Renderable()
        {
            active = true;
            visible = true;
            colour = Color.White;
        }

        /// <summary>
        /// a more usefull constructor
        /// </summary>
        /// <param name="activeZ"></param>
        /// <param name="visibleZ"></param>
        /// <param name="colorZ"></param>
        public RC_Renderable(bool activeZ, bool visibleZ, Color colorZ)
        {
            active = activeZ;
            visible = visibleZ;
            colour = colorZ;
        }

        /// <summary>
        /// Copy constructor dont know if it will ever be used since its a parent class
        /// </summary>
        public RC_Renderable(RC_Renderable r)
        {            
            active = r.active;
            visible = r.visible;
            colour = r.colour;
            tag = r.tag;
            tagInt = r.tagInt;
        }

        /// <summary>
        /// Sets the blend colour for the sprite if in doubt use Color.White
        /// </summary>
        /// <param name="c"></param>
        public virtual void setColor(Color c)
        {
            colour = c;
        }

        /// <summary>
        /// Gets the sprite blend colour
        /// </summary>
        /// <returns></returns>
        public Color getColor()
        {
            return colour;
        }

        /// <summary>
        /// Standard draw routine which assumes the renderable knows where it is
        /// </summary>
        /// <param name="sb"></param>
        public virtual void Draw(SpriteBatch sb)
        {
        }

        public virtual void Update(GameTime gameTime)
        {
        }

        public virtual void LoadContent()
        {
        }

        public virtual void Reset()
        {
        }

        /// <summary>
        /// Set if the renderable is visible 
        /// </summary>
        /// <param name="v"></param>
        public void setVisible(bool v)
        {
            visible = v;
        }

        /// <summary>
        /// Um this should be obvious
        /// </summary>
        /// <returns></returns>
        public bool getVisible()
        {
            return visible;
        }

        /// <summary>
        /// Used to scroll the entire screen - in renderable simple but increases in complexity as the 
        /// parent child tree deepens (eg sprite3)
        /// </summary>
        /// <param name="x">the amount to scroll in x </param>
        /// <param name="y">the amount to scroll in y</param>
        /// <returns> true if successfull </returns>
        public virtual bool scrollMove(float x, float y)
        {
            return true;
        }

        /// <summary>
        /// Events in the this class can be consumed so default behavious must be to return false
        /// this will indicate that the event was not consumed
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="previousMs"></param>
        /// <returns></returns>
        public virtual bool MouseMoveEvent(MouseState ms, MouseState previousMs) { return false; }
        public virtual bool MouseDownEventLeft(float mouse_x, float mouse_y) { return false; }
        public virtual bool MouseUpEventLeft(float mouse_x, float mouse_y) { return false; }
        public virtual bool MouseDownEventRight(float mouse_x, float mouse_y) { return false; }
        public virtual bool MouseUpEventRight(float mouse_x, float mouse_y) { return false; }
        public virtual bool KeyHitEvent(Keys keyHit) { return false; }
        public virtual bool MouseOver(float mouse_x, float mouse_y) { return false; } // for tool tips and any other mouse overs

    }



    // ------------------------------------------------ RC_RenderableBounded ----------------------------------

    /// <summary>
    /// This class is a parent for a range of renderable class's with a rectangle for bounds
    /// it allows seting the bounds by routines that resize it for zooming  
    /// the bounds are definitly the render bounds not a collision box (though of course they may be the same)
    /// </summary>
    public class RC_RenderableBounded : RC_Renderable
    {

        public virtual Rectangle bounds
        {
            get { return boundz; }
            set { boundz = value; }
        }

        protected Rectangle boundz;

        /// <summary>
        /// Set the x and y position of this renderable
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public virtual void setPos(float x, float y)
        {
            boundz.X = (int)x;
            boundz.Y = (int)y;
        }

        /// <summary>
        /// Used to scroll the entire screen - while preserving old position 
        /// usually called in renderable List 
        /// Will have problemes with non integral scroll amounts (ie fractions)
        /// </summary>
        /// <param name="x">the amount to scroll in x </param>
        /// <param name="y">the amount to scroll in y</param>
        /// <returns> true if successfull </returns>
        public override bool scrollMove(float x, float y)
        {
            boundz.X = boundz.X + (int)Math.Round(x,0);
            boundz.Y = boundz.Y + (int)Math.Round(y,0);
            return true;
        }


        /// <summary>
        /// set the width and height
        /// </summary>
        /// <param name="w"></param>
        /// <param name="h"></param>
        public void setWidthHeight(int w, int h)
        {
            boundz.Width = w;
            boundz.Height = h;
        }

        /// <summary>
        /// Returns true if the renderable contains the point x,y
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool Contains(int x, int y)
        {
            return bounds.Contains(x, y);
        }

        /// <summary>
        /// Returns a rectangle which is the intersection of this renderable with another rectangle
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public Rectangle Intersect(Rectangle r)
        {
            return Rectangle.Intersect(bounds, r);
        }

        /// <summary>
        /// Returns true if the rectange intersects this this renderable
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public bool Intersects(Rectangle r)
        {
            return bounds.Intersects(r);
        }

        /// <summary>
        /// Returns Various points in the rectangle 
        /// </summary>
        /// <param name="subLocNum"></param>
        /// <returns></returns>
        public Vector2 getSubLocation(int subLocNum)
        {
            return Util.getSubLocation(bounds, subLocNum);
        }

        /// <summary>
        /// Returns true if inside or on boundary of r
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public bool inside(Rectangle r)
        {
            if (bounds.X < r.X) return false;
            if (bounds.X + bounds.Width > r.X + r.Width) return false;
            if (bounds.Y < r.Y) return false;
            if (bounds.Y + bounds.Height > r.Y + r.Height) return false;
            return true;
        }
    }


    // ********************************************************************* attached renderable ******************************************************
    /// <summary>
    /// A parent class for renderables attached to sprites that can be drawn after the sprite
    /// it lives off of the sprites update and draw sequences
    /// usefull for health bars and things
    /// </summary>
    public class RC_RenderableAttached : RC_Renderable
    {
        internal Sprite3 parent = null;

        public void setParent(Sprite3 parentZ)
        {
            parent = parentZ;
        }
    }

    // ***------------------------------****************************** renderable list *********************-------------------*********************
    /// <summary>
    /// a list of renderables
    /// the idea of the active flag is is that inactive renderables can have their place re-used and are no longer needed
    /// </summary>
    public class RC_RenderableList : RC_Renderable
    {

        public List<RC_Renderable> rlist;

        public RC_RenderableList()
        {
            rlist = new List<RC_Renderable>();
        }

        /// <summary>
        /// draw the list of renderables renderables are responsible for knowing their position
        /// </summary>
        /// <param name="sb"></param>
        public override void Draw(SpriteBatch sb)
        {
            for (int i = 0; i < rlist.Count; i++)
            {
                if (rlist[i].active)
                {
                    rlist[i].Draw(sb);
                }
            }
        }

        /// <summary>
        /// run update on the entire list of renderables 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < rlist.Count; i++)
            {
                if (rlist[i].active)
                {
                    rlist[i].Update(gameTime);
                }
            }
        }

        /// <summary>
        /// runs scroll move on all renderables 
        /// used to scroll the screen 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public override bool scrollMove(float x, float y)
        {
            bool rc = true;
            for (int i = 0; i < rlist.Count; i++)
            {
                if (rlist[i].active)
                {
                    if (rlist[i].scrollMove(x,y)) rc=false;
                }
            }
            return rc;
        }

        /// <summary>
        /// For initialisation
        /// </summary>
        public override void LoadContent()
        {
            for (int i = 0; i < rlist.Count; i++)
            {
                rlist[i].LoadContent();
            }
        }

        /// <summary>
        /// Adds to the list - fast but usually we would use the 'addReuse' method
        /// </summary>
        /// <param name="r"></param>
        public void addToEnd(RC_Renderable r)
        {
            rlist.Add(r);
        }

        /// <summary>
        /// standard way to add things to the list reusing old space to 
        /// stop the list growing to long
        /// </summary>
        /// <param name="r"></param>
        public void addReuse(RC_Renderable r)
        {
            int i = findInactive();
            if (i == -1) rlist.Add(r);
            else rlist[i] = r;
        }

        /// <summary>
        /// internal use mainly
        /// </summary>
        /// <returns></returns>
        public int findInactive()
        {
            for (int i = 0; i < rlist.Count; i++)
            {
                if (!rlist[i].active) return i;
            }
            return -1;
        }

        /// <summary>
        /// replaces returning the old value - not especially usefull
        /// </summary>
        /// <param name="num"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        public RC_Renderable replace(int num, RC_Renderable r)
        {
            RC_Renderable retv = rlist[num];
            rlist[num] = r;
            return retv;
        }

        /// <summary>
        /// gets a renderable i
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public RC_Renderable getRenderable(int i)
        {
            RC_Renderable retv = rlist[i];
            return retv;

        }

        /// <summary>
        /// Count of renderable 
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return rlist.Count;
        }

        /// <summary>
        /// If the renderable list contains RC_RenderableBounded it will return
        /// the first renderable index that this point in bounds or -1 if not found
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Contains(float x, float y)
        {
            for (int i = 0; i < rlist.Count; i++)
            {
                if (rlist[i] == null) continue;
                if (!rlist[i].active) continue;
                if (!rlist[i].visible) continue;
                if (((RC_RenderableBounded)(rlist[i])).bounds.Contains((int)x, (int)y))
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// Checks for collision with rectangle for bounded renderables only 
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public int collision(Rectangle r)
        {
            for (int i = 0; i < rlist.Count; i++)
            {
                if (rlist[i] == null) continue;
                if (!rlist[i].active) continue;
                if (!rlist[i].visible) continue;
                if (((RC_RenderableBounded)(rlist[i])).bounds.Intersects(r))
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// Sets colour for all active renderables
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public override void setColor(Color c)
        {
            for (int i = 0; i < rlist.Count; i++)
            {
                if (rlist[i] == null) continue;
                if (!rlist[i].active) continue;
                //if (!rlist[i].visible) continue;
                rlist[i].setColor(c);
            }
            return;
        }

        /// <summary>
        /// Counts the active renderables
        /// deprecated please use countActive
        /// </summary>
        /// <returns></returns>
        public int countOfActive()
        {

            return countActive();
        }

        public override bool MouseOver(float mouse_x, float mouse_y)
        {
            bool retv = false;
            for (int i = 0; i < rlist.Count; i++)
            {
                if (rlist[i] != null && rlist[i].active && rlist[i].visible)
                {
                    bool rc = rlist[i].MouseOver(mouse_x, mouse_y);
                    if (rc) retv = true;
                }
            }
            return retv;
        }

        public override bool MouseDownEventLeft(float mouse_x, float mouse_y)
        {
            bool retv = false;
            for (int i = 0; i < rlist.Count; i++)
            {
                if (rlist[i] != null && rlist[i].active && rlist[i].visible)
                {
                    bool rc = rlist[i].MouseDownEventLeft(mouse_x, mouse_y);
                    if (rc) retv = true;
                }
            }
            return retv;
        }

        /// <summary>
        /// count of active renderable
        /// </summary>
        /// <returns></returns>
        public int countActive()
        {
            int retv = 0;
            for (int i = 0; i < rlist.Count; i++)
            {
                if (rlist[i] == null) continue;
                if (rlist[i].active) retv++;
            }
            return retv;
        }
    }


}