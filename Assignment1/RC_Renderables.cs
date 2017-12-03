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

    // ********************************* Text renderable ************************************************* 

    /// <summary>
    /// this class is just a single line of stationary text
    /// </summary>
    class TextRenderable : RC_Renderable
    {
        public string text { get; set; }
        public Vector2 pos { get; set; }
        public SpriteFont font { get; set; }
        //public Color colour { get; set; }

        public TextRenderable(string textZ, Vector2 posZ, SpriteFont fontZ, Color colourZ)
        {
            active = true;
            text = textZ;
            pos = posZ;
            font = fontZ;
            colour = colourZ;
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.DrawString(font, text, pos, colour);
        }
    }

    // ************************************  texture fade (and resize as well) ****************

    /// <summary>
    /// Fades and resizes a texture 
    /// At end it can loop , go inactive or reverse
    /// its a fairly sophisticated tool usable for a lot of diferent eye candy effects
    /// remeber to run update
    /// </summary>
    class TextureFade : RC_Renderable //(and resize as well)
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

        public TextureFade(Texture2D texZ, Rectangle initFrameZ, Rectangle finalFrameZ, Color initColourZ, Color finalColourZ, int fadeTicksZ)
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

        public void setLoop(int loopQ)
        {
            loop = loopQ;
        }

        public override void Draw(SpriteBatch sb)
        {
            if (!active) return;
            sb.Draw(tex, curFrame, sourceFrame, curColour);
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
