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
//using Microsoft.Xna.Framework.Net;
//using Microsoft.Xna.Framework.Storage;

namespace RC_Framework
{

    /// <summary>
    /// A list of sprites with some basic management functions
    /// It has the concept than sprites are visbile or not visible and
    /// active or not active 
    /// the function addspritereuse - reuses the inactive sprite slots
    /// </summary>
    public class SpriteList : RC_Renderable
    {
        Sprite3[] sprite;
        int numOfSprites; // the size of the array

        /// <summary>
        /// Lowest free sprite num (for use in external loops rarely needed)
        /// </summary>
        public int lowFree = 0; // lowest free sprite
        /// <summary>
        /// Highest free sprite num (for use in external loops rarely needed)
        /// </summary>
        public int highUsed = 0; // highest used
        int counter = 0;

        /// <summary>
        /// Constructor for spritelist
        /// </summary>
        public SpriteList()
        {
            initialise(200);
        }

        /// <summary>
        /// Constructor if you want to manage how many sprites
        /// </summary>
        /// <param name="NumOfSprites"></param>
        public SpriteList(int NumOfSprites)
        {
            initialise(NumOfSprites);
        }

        private void initialise(int NumOfSprites)
        {
            numOfSprites = NumOfSprites;
            sprite = new Sprite3[numOfSprites];
            for (int i = 0; i < numOfSprites; i++)
            {
                sprite[i] = null;
            }
        }

/// <summary>
/// Overload [ and ] for ease of use
/// </summary>
/// <param name="key"></param>
/// <returns></returns>
        public Sprite3 this[int key]
        {
            get
            {
                return getSprite(key);
            }
            set
            {
                //SetValue(key, value);
                setSprite(key, value);
            }
        }



        /// <summary>
        /// counts sprites in list (active, inactive , visible, not visible) fairly useless
        /// </summary>
        /// <returns></returns>
        public int count()
        {
            counter = 0;
            for (int i = 0; i < numOfSprites; i++)
            {
                if (sprite[i] != null) counter++;
            }
            return counter;
        }

        /// <summary>
        /// counts sprites in list (active only) 
        /// </summary>
        /// <returns></returns>
        public int countActive()
        {
            counter = 0;
            for (int i = 0; i < numOfSprites; i++)
            {
                if (sprite[i] != null)
                {
                    if (sprite[i].active) counter++;
                }
            }
            return counter;
        }

        /// <summary>
        /// Get max number of sprites in this list
        /// </summary>
        /// <returns></returns>
        public int getMaxNumOfSprites()
        {
            return numOfSprites;
        }

        /// <summary>
        /// get the index of the highest used sprite 
        /// </summary>
        /// <returns></returns>
        /// 
        // example of use  
        // <code> "for (int i = 0; i <mysprite.getHighestUsed()+1; i++)" </code>
        // dont forget to check for null (and visible or active if needed)
        public int getHighestUsed()
        {
            return highUsed;
        }

        /// <summary>
        /// Finds a free slot in the sprite list or -1 if it fails 
        /// </summary>
        /// <returns></returns>
        public int findFreeSprite()
        {
            for (int i = 0; i < numOfSprites; i++)
            {
                if (sprite[i] == null) return i;
            }
            return -1;
        }

        /// <summary>
        /// Finds a free slot in the sprite list or an inactive sprite -1 if it fails 
        /// </summary>
        /// <returns></returns>
        public int findFreeSpriteOrInactive()
        {
            for (int i = 0; i < numOfSprites; i++)
            {
                if (sprite[i] == null) return i;
                if (!sprite[i].active) return i;
                
            }
            return -1;
        }

        /// <summary>
        /// Add a sprite and return then return the number we added it at
        /// return -1 if not possible
        /// </summary>
        /// <param name="spriteZ"></param>
        /// <returns></returns>
        public int addSprite(Sprite3 spriteZ)
        {
            int i = findFreeSprite();
            if (i != -1)
            {
                setSprite(i, spriteZ);
                if (i > highUsed) highUsed = i;
                return i;
            }
            return -1;
        }

        /// <summary>
        /// Add a sprite and return then number we added it at and allow inactive sprites to be oever written
        /// return -1 if not possible
        /// </summary>
        /// <param name="spriteZ"></param>
        /// <returns></returns>
        public int addSpriteReuse(Sprite3 spriteZ)
        {
            int i = findFreeSpriteOrInactive();
            if (i != -1)
            {
                setSprite(i, spriteZ);
                if (i > highUsed) highUsed = i;
                return i;
            }
            return -1;
        }


        /// <summary>
        /// return the particular sprite
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public Sprite3 getSprite(int i)
        {
            return sprite[i];
        }

        /// <summary>
        /// Remove a sprite 
        /// </summary>
        /// <param name="i"></param>
        public void deleteSprite(int i)
        {
            if (i < lowFree) lowFree = i;
            setSprite(i, null);
        }

        /// <summary>
        /// Replace or set a specifically numbered sprite
        /// </summary>
        /// <param name="i"></param>
        /// <param name="spriteZ"></param>
        public void setSprite(int i, Sprite3 spriteZ)
        {
            sprite[i] = spriteZ;
            if (spriteZ == null && i < lowFree) lowFree = i;
            if (spriteZ != null && i > highUsed) highUsed = i;
        }

        /// <summary>
        /// draw all visible sprites
        /// </summary>
        /// <param name="sb"></param>
        public override void Draw(SpriteBatch sb)
        {
            drawAll(sb);
        }

        /// <summary>
        /// draw all visible and active sprites
        /// </summary>
        /// <param name="sb"></param>
        public void drawAll(SpriteBatch sb)
        {
            for (int i = 0; i < highUsed + 1; i++)
            {
                if (sprite[i] != null)
                    if (sprite[i].getVisible() && sprite[i].active)
                        sprite[i].Draw(sb);
            }
        }

        /// <summary>
        /// animationTick tick all visible and active sprites
        /// </summary>
        public void animationTick()
        {
            for (int i = 0; i < highUsed + 1; i++)
            {
                if (sprite[i] != null && sprite[i].active)
                    if (sprite[i].getVisible())
                        sprite[i].animationTick();
            }
        }

        /// <summary>
        /// Run routine tick on all active sprites - this may alter their visiblity or active staus
        /// Tick is used in sprite3 to make a sprite visible after a certain time 
        /// </summary>
        public void Tick()
        {
            for (int i = 0; i < highUsed + 1; i++)
            {
                if (sprite[i] != null && sprite[i].active)
                    sprite[i].tick();
            }
        }

        /// <summary>
        /// Run setColor on all active sprites 
        /// </summary>
        public override void setColor(Color c)
        {
            for (int i = 0; i < highUsed + 1; i++)
            {
                if (sprite[i] != null && sprite[i].active)
                    sprite[i].setColor(c);
            }
        }

        /// <summary>
        /// Run Update on all active sprites; but 
        /// most of the time you actually want one of (or more of):
        /// 
        /// Tick()   - visible/invisible and active/inactive timer ticker
        /// AnimationTick()  - to advance animation frames if necessary
        /// moveByDeltaXY()  - move the sprites
        /// moveByDeltaX(dx)  - move the sprites
        /// moveByDeltaY(dy)  - move the sprites
        /// moveByAngleSpeed() - move the sprites
        /// moveByAngleSpeeedLimit(Rectangle r) - move the sprites within a limited area
        /// scrollMove(float x, float y) - these sprites need to scroll 
        /// adjustAngles()  - rotate the sprites display and movement angles 
        /// moveVisibleWayPoints() - move sprites in the list towards their waypoints
        /// 
        /// By separating these routine so you use only the ones you need we keep 
        /// the frame rate up - but it does get annoying i guess to code more lines than needed
        /// 
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < highUsed + 1; i++)
            {
                if (sprite[i] != null && sprite[i].active)
                    sprite[i].Update(gameTime);
            }
        }

        /// <summary>
        /// Draw all active sprites
        /// </summary>
        /// <param name="sb"></param>
        public void drawActive(SpriteBatch sb)
        {
            for (int i = 0; i < highUsed + 1; i++)
            {
                if (sprite[i] != null)
                    if (sprite[i].getVisible() && sprite[i].active)
                        sprite[i].draw(sb);
            }
        }

        /// <summary>
        /// Calls moveByAngleSpeed(); on all active sprites in the list
        /// </summary>
        public void moveByAngleSpeed()
        {
            moveActiveAD();
        }

        /// <summary>
        /// Move active by angle/dist
        /// </summary>
        public void moveActiveAD()
        {
            for (int i = 0; i < highUsed + 1; i++)
            {
                if (sprite[i] != null)
                    if (sprite[i].active)
                        sprite[i].moveByAngleSpeed();
            }
        }

        /// <summary>
        /// runs moveByAngleSpeeedLimit on all active sprites
        /// </summary>
        /// <param name="r"></param>
        public void moveByAngleSpeeedLimit(Rectangle r)
        {
            for (int i = 0; i < highUsed + 1; i++)
            {
                if (sprite[i] != null)
                    if (sprite[i].active)
                        sprite[i].moveByAngleSpeedLimit(r);
            }
        }

        /// <summary>
        /// Adjust all angles (move and display) by relevant deltas
        /// </summary>
        public void adjustAngles()
        {
            adjustAnglesActive();
        }


        /// <summary>
        /// Adjust all angles (move and display) by relevant deltas
        /// </summary>
        public void adjustAnglesActive()
        {
            for (int i = 0; i < highUsed + 1; i++)
            {
                if (sprite[i] != null)
                    if (sprite[i].active)
                    {
                        sprite[i].adjustAngles();
                    }
            }
        }

        /// <summary>
        /// Removes (makes inactive) all sprites whos bouding vbox is outside r 
        /// returns a count of the number of sprites removed
        /// </summary>
        /// <param name="r"></param>
        public int removeIfOutside(Rectangle r)
        {
            int rc = 0;
            for (int i = 0; i < highUsed + 1; i++)
            {
                if (sprite[i] != null)
                    if (sprite[i].active)
                    {
                        if (!r.Intersects(sprite[i].getBoundingBoxAA()))
                        {
                            sprite[i].active = false;
                            sprite[i].visible = false;
                            rc++;
                        }
                    }
            }
            return rc;
        }

        /// <summary>
        /// Move visible or active by delat x delta y
        /// </summary>
        public void moveDeltaXY_Visible()
        {
            for (int i = 0; i < highUsed + 1; i++)
            {
                if (sprite[i] != null)
                    if (sprite[i].active || sprite[i].getVisible())
                        sprite[i].moveByDeltaXY();
            }
        }

        /// <summary>
        /// Move active by delta x delta y
        /// </summary>
        public void moveDeltaXY()
        {
            for (int i = 0; i < highUsed + 1; i++)
            {
                if (sprite[i] != null)
                    if (sprite[i].active)
                    {
                        sprite[i].moveByDeltaXY();
                    }
            }
        }

        /// <summary>
        /// Call scrollmove to move all sprites 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public override bool scrollMove(float x, float y)
        {
            bool rc = true;
            for (int i = 0; i < highUsed + 1; i++)
            {
                if (sprite[i] != null)
                    if (sprite[i].active)
                    {
                        if (!sprite[i].scrollMove(x, y)) rc = false;
                    }
            }
            return rc;
        }


        /// <summary>
        /// Move active by a delta x 
        /// </summary>
        public void moveDeltaX(float dx)
        {
            for (int i = 0; i < highUsed + 1; i++)
            {
                if (sprite[i] != null)
                    if (sprite[i].active)
                    {
                        sprite[i].moveByDeltaX(dx);
                    }
            }
        }

        /// <summary>
        /// Move active by a delta y 
        /// </summary>
        public void moveDeltaY(float dy)
        {
            for (int i = 0; i < highUsed + 1; i++)
            {
                if (sprite[i] != null)
                    if (sprite[i].active)
                    {
                        sprite[i].moveByDeltaY(dy);
                    }
            }
        }

        /// <summary>
        /// Move all active visible sprites by their waypoint
        /// </summary>
        public void moveVisibleWayPoints()
        {
            for (int i = 0; i < highUsed + 1; i++)
            {
                if (sprite[i] != null)
                    if (sprite[i].getVisible() && sprite[i].getActive())
                        sprite[i].moveWayPointList(false);
            }
        }


        /// <summary>
        /// Draw BB and hotspot for all visible sprites 
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="colorBB"></param>
        /// <param name="colorHS"></param>
        public void drawInfo(SpriteBatch sb, Color colorBB, Color colorHS)
        {
            for (int i = 0; i < highUsed + 1; i++)
            {
                if (sprite[i] != null)
                    if (sprite[i].getVisible() && sprite[i].active) sprite[i].drawInfo(sb, colorBB, colorHS);
            }
        }

        /// <summary>
        /// draw Bounding Spheres in list
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="colorBS"></param>
        public void drawBoundingSphere(SpriteBatch sb, Color colorBS)
        {
            for (int i = 0; i < highUsed + 1; i++)
            {
                if (sprite[i] != null)
                {
                    if (sprite[i].getVisible() && sprite[i].active) sprite[i].drawBoundingSphere(sb, colorBS);
                }
            }
        }

        /// <summary>
        /// Draw the waypoints
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="colorW"></param>
        public void drawWayLists(SpriteBatch sb, Color colorW)
        {
            for (int i = 0; i < highUsed + 1; i++)
            {
                if (sprite[i] != null)
                    if (sprite[i].getVisible())
                        if (sprite[i].wayList != null) sprite[i].wayList.Draw(sb, colorW, colorW);
            }
        }


        /// <summary>
        /// draws the rotated bounding box for all visible sprites
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="color"></param>
        public void drawRect4(SpriteBatch sb, Color color)
        {
            for (int i = 0; i < highUsed + 1; i++)
            {
                if (sprite[i] != null)
                    if (sprite[i].getVisible())
                    {
                        sprite[i].getBoundingBoxAA(); // to put values in 
                        sprite[i].drawRect4(sb, color);
                    }
            }
        }

        /// <summary>
        /// Returns the index of the first (ie lowest numbered) 
        /// sprite it colides with -1 if no colission
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public int collisionAA(Sprite3 s)
        {
            return collisionAA(s, -1);
        }

        /// <summary>
        /// Returns the index of the first (ie lowest numbered) 
        /// sprite it colides with -1 if no collision
        /// the self parameter is there to allow the avoidance of collision texting against yourself
        /// </summary>
        /// <param name="s">the sprite to test the list against</param>
        /// <param name="self">the number of a single sprite to avoid testing</param>
        /// <returns></returns>
        public int collisionAA(Sprite3 s, int self)
        {
            if (s==null) return -1;
            if (!s.getVisible()) return -1;
            if (!s.getActive()) return -1;
            for (int i = 0; i < highUsed + 1; i++)
            {
                if (sprite[i] != null && sprite[i].active)
                    if (sprite[i].getVisible() && 
                        i != self && s.collision(sprite[i]))
                    {
                        return i;
                    }
            }
            return -1;
        }

        /// <summary>
        /// returns  index of the first visible sprite that collides with r
        /// if no colission it returns -1 
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public int collisionWithRect(Rectangle r)
        {
            for (int i = 0; i < highUsed + 1; i++)
            {
                if (sprite[i] != null && sprite[i].active)
                    if (sprite[i].getVisible())
                        if (r.Intersects(sprite[i].getBoundingBoxAA()))
                        {
                            return i;
                        }
            }
            return -1;
        }


        /// <summary>
        /// Returns the number of the first sprite found 'under' the spot
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public int pointInList(Vector2 point)
        {
            for (int i = 0; i < highUsed + 1; i++)
            {
                if (sprite[i] != null && sprite[i].active)
                    if (sprite[i].getVisible() && sprite[i].inside(point.X, point.Y))
                    {
                        return i;
                    }
            }
            return -1;
        }

        /// <summary>
        /// Find inactive sprite or -1 if not found 
        /// </summary>
        /// <returns></returns>
        public int findInactive()
        {
            for (int i = 0; i < highUsed + 1; i++) if (sprite[i] != null)
                {
                    if (!sprite[i].active) return i;
                }
            return -1;
        }

        // ************************ method methods (warning to beginners metaprogramming stay clear) ***********************************
        /// <summary>
        /// Run the method on each active sprite
        /// return true only if all functions return true
        /// the function ha the formal parameters bool name(Sprite3 s)
        /// </summary>
        /// <param name="methodName"></param>
        /// <returns></returns>
        public bool RunTheMethod(Func<Sprite3, bool> methodName)
        {
            bool retv = true;
            for (int i = 0; i < highUsed + 1; i++)
            {
                if (sprite[i] != null && sprite[i].active)      
                    {
                        bool rc = methodName(sprite[i]);
                    if (!rc) retv = false;
                    }
            }
            return retv;
        }


    }

}