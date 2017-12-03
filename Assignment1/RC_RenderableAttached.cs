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
    
    // ************************************************** Health bar class *************************************
    /// <summary>
    /// healthbar class to attach to sprite
    /// set alwaysdraw to true if you wan the bar at all times set it to false if you want it only when damaged
    /// </summary>
    public class HealthBar : RC_RenderableAttached
    {

        internal Color backColor;
        internal Color barOffColor;
        internal int barHeight;
        internal bool alwaysDraw; // true = will always draw     false=will draw only when damaged
        internal Vector2 offset = new Vector2(0, -1); // this positions the bar one pixel above the bounding box
        internal int gapOfbar = 1; // 0 would give no border at all

        public HealthBar(Color bar, Color backGround, Color barOffColorZ, int heightZ, bool alwaysDrawZ)
        {
            colour = bar;
            backColor = backGround;
            barHeight = heightZ;
            alwaysDraw = alwaysDrawZ;
            barOffColor = barOffColorZ;
        }

        public override void Draw(SpriteBatch sb)
        {
            if (parent == null) return;
            if (!active) return;
            if (!visible) return;
            int hp = parent.hitPoints;
            int maxhp = parent.maxHitPoints;
            if (!alwaysDraw && hp == maxhp) return; // dont draw
            Rectangle r = parent.getBoundingBoxAA();
            Rectangle hbBack = new Rectangle(r.X + (int)offset.X, r.Y + (int)offset.Y, r.Width, barHeight + gapOfbar * 2);
            hbBack.Y = hbBack.Y - gapOfbar * 2 - barHeight; //make some room
            Double ratio = (double)hp / (double)maxhp;
            Rectangle hb = new Rectangle(hbBack.X, hbBack.Y, (int)(hbBack.Width * ratio), barHeight);
            hb.Y = hb.Y + gapOfbar;
            Rectangle nothb = new Rectangle(hb.X + hb.Width, hb.Y, hbBack.Width - hb.Width, barHeight);
            sb.Draw(LineBatch._empty_texture, hbBack, backColor);
            sb.Draw(LineBatch._empty_texture, nothb, barOffColor);
            sb.Draw(LineBatch._empty_texture, hb, colour);
        }
    }

        //********************************************** AttachedText **************************************************************

/// <summary>
/// this class simply attaches a lump of text to the sprite
/// it can have its own text or use the text property of sprite
/// useing the text property of sprite is recomended
/// </summary>
    public class AttachedText : RC_RenderableAttached
    {

        public string text = ""; // this will override text in sprite if its used 
        public Vector2 offset = new Vector2(0, -1); // default is one pixel above
        public SpriteFont font;

        public AttachedText(Color colorZ, SpriteFont fontZ, Vector2 offsetZ, string textZ)
        {
            colour = colorZ;
            font = fontZ;
            text = textZ;
            offset = new Vector2(offsetZ.X, offsetZ.Y);
        }

        public AttachedText(Color colorZ, SpriteFont fontZ, Vector2 offsetZ)
        {
            colour = colorZ;
            font = fontZ;
            text = "";
            offset = new Vector2(offsetZ.X, offsetZ.Y);
        }

        public AttachedText(Color colorZ, SpriteFont fontZ)
        {
            colour = colorZ;
            font = fontZ;
            text = "";
        }

        public override void Draw(SpriteBatch sb)
        {
            if (parent == null) return;
            if (!active) return;
            if (!visible) return;
            Rectangle r = parent.getBoundingBoxAA();
            Vector2 pointSz = font.MeasureString("A");
            Vector2 pos = new Vector2(r.X, r.Y - pointSz.Y) + offset;
            if (text == "") { sb.DrawString(font, parent.text, pos, colour); return; }
            sb.DrawString(font, text, pos, colour);
        }

    }


    // ************************************  attached texture fade (and resize as well) ****************

    /// <summary>
    /// Fades and resizes a texture 
    /// At end it can loop , go inactive or reverse
    /// its a fairly sophisticated tool usable for a lot of diferent eye candy effects
    /// remeber to run update
    /// </summary>
    class AttachedTextureFade : RC_RenderableAttached //(and resize as well)
    {
        Color finalColour;
        Color initColour;
        Rectangle initFrame;
        Rectangle finalFrame;
        int fadeTicks;
        public int loop; // 0=end (go inactive), 1=Loop, 2=reverse
        int ticks;
        bool reverse;
        float lerp;
        public Rectangle sourceFrame { get; set; }

        Rectangle curFrame;
        Color curColour;
        Texture2D tex;

        public AttachedTextureFade(Texture2D texZ, Rectangle initFrameZ, Rectangle finalFrameZ, Color initColourZ, Color finalColourZ, int fadeTicksZ)
            : base()
        {
            finalColour = finalColourZ;
            initColour = initColourZ;
            fadeTicks = fadeTicksZ;
            tex = texZ;
            sourceFrame = new Rectangle(0, 0, tex.Width, tex.Height);
            initFrame = initFrameZ;
            finalFrame = finalFrameZ;
            Reset();
        }

        public void setloop(int loopQ)
        {
            loop = loopQ;
        }

        public override void Draw(SpriteBatch sb)
        {
            if (!active) return;
            Rectangle r = parent.getBoundingBoxAA();
            Rectangle c = Util.newRectangle(curFrame);
            c.X = c.X + r.X;
            c.Y = c.Y + r.Y;
            sb.Draw(tex, c, sourceFrame, curColour);
        }

        public override void Update(GameTime gameTime)
        {
            if (!active) return;
            ticks++;
            if (ticks > fadeTicks)
            {
                if (loop == 0)
                {
                    active = false;
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
            curFrame.X = (int)MathHelper.Lerp(initFrame.X, finalFrame.X, lerp);
            curFrame.Y = (int)MathHelper.Lerp(initFrame.Y, finalFrame.Y, lerp);
            curFrame.Width = (int)MathHelper.Lerp(initFrame.Width, finalFrame.Width, lerp);
            curFrame.Height = (int)MathHelper.Lerp(initFrame.Height, finalFrame.Height, lerp);
        }

        public override void Reset()
        {
            ticks = 0;
            reverse = false;
        }
    }


}
