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

#pragma warning disable 1591 //sadly not yet fully commented

namespace RC_Framework
{
    // ----------------------------------------------------------------------------------------------------------

    public class PanZoomStage : RC_Renderable
    {

        public Texture2D tex { set; get; }
        public Rectangle initDest { set; get; }
        public Rectangle finalDest { set; get; }
        public Rectangle initSource { set; get; }
        public Rectangle finalSource { set; get; }
        public int ticksToTransit { set; get; }
        public int cntTicksToTransit { set; get; }
        public Color initColour { set; get; }
        public Color finalColour { set; get; }
        public bool done { set; get; }

        public PanZoomStage()
        {
            tex = null;
            reset();
        }

        public void reset()
        {
            cntTicksToTransit = 0;
            done = false;
        }

        public override void Update(GameTime gameTime)
        {
            if (cntTicksToTransit >= ticksToTransit)
            {
                done = true;
                return;
            }
            cntTicksToTransit++;
        }

        public override void Draw(SpriteBatch sb)
        {
            float lerpV = (float)cntTicksToTransit / (float)ticksToTransit;
            Rectangle src = new Rectangle((int)MathHelper.Lerp(initSource.X, finalSource.X, lerpV),
                                            (int)MathHelper.Lerp(initSource.Y, finalSource.Y, lerpV),
                                            (int)MathHelper.Lerp(initSource.Width, finalSource.Width, lerpV),
                                            (int)MathHelper.Lerp(initSource.Height, finalSource.Height, lerpV));
            Rectangle dest = new Rectangle((int)MathHelper.Lerp(initDest.X, finalDest.X, lerpV),
                                            (int)MathHelper.Lerp(initDest.Y, finalDest.Y, lerpV),
                                            (int)MathHelper.Lerp(initDest.Width, finalDest.Width, lerpV),
                                            (int)MathHelper.Lerp(initDest.Height, finalDest.Height, lerpV));
            Color c = Color.Lerp(initColour, finalColour, lerpV);
            sb.Draw(tex, dest, src, c);
        }
    }

    public class PanZoomSequence : RC_Renderable
    {
        public List<PanZoomStage> lst; // my list of transitions
        public Texture2D defaultTex { set; get; }
        public Rectangle defaultDest { set; get; }
        public Color defaultColour { set; get; }
        int currentStage;
        bool done = true;
        int ticks;

        public PanZoomSequence(Rectangle destZ, Texture2D defaultTexZ, Color defaultColorZ)
        {
            defaultTex = defaultTexZ;
            defaultDest = destZ;
            defaultColour = defaultColorZ;
            lst = new List<PanZoomStage>();
            reset();
        }

        public void reset()
        {
            currentStage = 0;
            done = false;
            ticks = 0;
        }

        public void addStage(PanZoomStage s)
        {
            lst.Add(s);
            reset();
        }

        public void addStage(Texture2D texZ, int ticksToTransitZ, Color initColourZ, Color finalColourZ,
                             Rectangle initDestZ, Rectangle finalDestZ, Rectangle initSourceZ, Rectangle finalSourceZ)
        {
            PanZoomStage p = new PanZoomStage();
            p.tex = texZ;
            p.ticksToTransit = ticksToTransitZ;
            p.initColour = initColourZ;
            p.finalColour = finalColourZ;
            p.initDest = initDestZ;
            p.finalDest = finalDestZ;
            p.initSource = initSourceZ;
            p.finalSource = finalSourceZ;
            p.reset();
            addStage(p);
        }

        public void addStage(int ticksToTransitZ, Rectangle initSourceZ)
        {
            PanZoomStage p = new PanZoomStage();
            p.tex = defaultTex;
            p.ticksToTransit = ticksToTransitZ;
            p.initColour = defaultColour;
            p.finalColour = defaultColour;
            p.initDest = defaultDest;
            p.finalDest = defaultDest;
            p.initSource = initSourceZ;
            p.finalSource = initSourceZ;
            p.reset();
            addStage(p);
        }

        public void addStage(int ticksToTransitZ, Rectangle initSourceZ, Rectangle finalSourceZ)
        {
            PanZoomStage p = new PanZoomStage();
            p.tex = defaultTex;
            p.ticksToTransit = ticksToTransitZ;
            p.initColour = defaultColour;
            p.finalColour = defaultColour;
            p.initDest = defaultDest;
            p.finalDest = defaultDest;
            p.initSource = initSourceZ;
            p.finalSource = finalSourceZ;
            p.reset();
            addStage(p);
        }


        public bool Done()
        {
            return done;
        }

        public override void Update(GameTime gameTime)
        {
            if (lst.Count == 0) return;

            PanZoomStage p = lst[currentStage];

            if (ticks == 0)
            {
                p.reset();
            }
            ticks++;
            p.Update(gameTime);
            if (p.done)
            {
                if (currentStage >= lst.Count() - 1)
                {
                    done = true;
                    return;
                }
                currentStage++;
            }

        }

        public override void Draw(SpriteBatch sb)
        {
            if (lst.Count == 0) return;

            PanZoomStage p = lst[currentStage];

            p.Draw(sb);

        }

    }

}
