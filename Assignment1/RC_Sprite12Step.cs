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

    public class Sprite12Step : Sprite3
    {
        int dirAnim;

        Vector2[] seqUp;
        Vector2[] seqRight;
        Vector2[] seqDown;
        Vector2[] seqLeft;

        Vector2[] seqUpStand;
        Vector2[] seqRightStand;
        Vector2[] seqDownStand;
        Vector2[] seqLeftStand;

        public Vector2[] seqCustom1;
        public Vector2[] seqCustom2;

        public int[] firstFrameA;
        public int[] lastFrameA;
        public int[] ticksFrameA; 

        // critical value direction and animation dirAnim 
        // 0 = animate up
        // 1 = animate right
        // 2 = animate down
        // 3 = animate left
        //  
        // 4 = stand up
        // 5 = stand right
        // 6 = stand down
        // 7 = stand left
        // 
        // 8 = Custom1 
        // 9 = Custom2 


        public Sprite12Step(bool visibleZ, Texture2D texZ, float x, float y, int xsize, int ysize, int dirAnimInit) : base(visibleZ, texZ, x, y)
        {
                setXframes(3);
                setYframes(4);
                setWidthHeight(xsize, ysize);
                dirAnim = dirAnimInit;
                setHSBottom();

                Vector2[] seq = new Vector2[4];
                seq[0].X = 1; seq[0].Y = 0;
                seq[1].X = 2; seq[1].Y = 0;
                seq[2].X = 1; seq[2].Y = 0;
                seq[3].X = 0; seq[3].Y = 0;
                seqUp = seq;

                seq = new Vector2[4];
                seq[0].X = 1; seq[0].Y = 1;
                seq[1].X = 2; seq[1].Y = 1;
                seq[2].X = 1; seq[2].Y = 1;
                seq[3].X = 0; seq[3].Y = 1;
                seqRight = seq;

                seq = new Vector2[4];
                seq[0].X = 1; seq[0].Y = 2;
                seq[1].X = 2; seq[1].Y = 2;
                seq[2].X = 1; seq[2].Y = 2;
                seq[3].X = 0; seq[3].Y = 2;
                seqDown = seq;

                seq = new Vector2[4];
                seq[0].X = 1; seq[0].Y = 3;
                seq[1].X = 2; seq[1].Y = 3;
                seq[2].X = 1; seq[2].Y = 3;
                seq[3].X = 0; seq[3].Y = 3;
                seqLeft = seq;

                

                seq = new Vector2[2]; // just so it does not crash
                seq[0].X = 1; seq[0].Y = 3;
                seq[1].X = 2; seq[1].Y = 3;
                seqCustom1 = seq;
                seq = new Vector2[2]; // just so it does not crash
                seq[0].X = 1; seq[0].Y = 3;
                seq[1].X = 2; seq[1].Y = 3;
                seqCustom2 = seq; 

                setAnimFinished(0); // set the animation to loop
               
                firstFrameA = new int[10]; 
                lastFrameA = new int[10];
                ticksFrameA = new int[10];

                for (int i = 0; i < 4; i++)
                {
                    firstFrameA[i] = 0;
                    lastFrameA[i] = 3;
                    ticksFrameA[i] = 12;
                }

            for (int i = 4; i < 10; i++)
                {
                    firstFrameA[i] = 0;
                    lastFrameA[i] = 0;
                    ticksFrameA[i] = 12;
                }

            setStandAsStamp(false);
            animationStart12(dirAnim);
        }

        /// <summary>
        /// Set Hotspot to bottom of animation - only works on frame animated sprites
        /// </summary>
        public void setHSBottom()
        {
            setHSoffset(new Vector2(XframeWidth/2,YframeHeight));
               //setBB(-XframeWidth / 2, -YframeHeight, XframeWidth, YframeHeight);
               //setBB(0,0, XframeWidth, YframeHeight);
        }

        /// <summary>
        /// Set Hotspot to middle of animation - only works on frame animated sprites
        /// </summary>
        public void setHSMiddle()
        {               
            setHSoffset(new Vector2(XframeWidth/2,YframeHeight/2));
            //setBB(0,0, XframeWidth, YframeHeight);
        }

        public void setStandAsStill()
        {
            Vector2[] seq = new Vector2[1];
            seq[0].X = 1; seq[0].Y = 0;
            seqUpStand = seq;

            seq = new Vector2[1];
            seq[0].X = 1; seq[0].Y = 1;
            seqRightStand = seq;

            seq = new Vector2[1];
            seq[0].X = 1; seq[0].Y = 2;
            seqDownStand = seq;
            
            seq = new Vector2[1];
            seq[0].X = 1; seq[0].Y = 3;
            seqLeftStand = seq;

            for (int i = 4; i < 8; i++)
            {
                firstFrameA[i] = 0;
                lastFrameA[i] = 0;
            }

        }

        public void setStandAsStamp(bool LeftRightStill)
        {
            Vector2[] seq = new Vector2[2];
            seq[0].X = 0; seq[0].Y = 0;
            seq[1].X = 2; seq[1].Y = 0;
            seqUpStand = seq;

            seq = new Vector2[2];
            if (LeftRightStill)
            {
                seq[0].X = 1; seq[0].Y = 1;
                seq[1].X = 1; seq[1].Y = 1;
            }
            else
            {                
                seq[0].X = 0; seq[0].Y = 1;
                seq[1].X = 2; seq[1].Y = 1;
            }
            seqRightStand = seq;

            seq = new Vector2[2];
            seq[0].X = 0; seq[0].Y = 2;
            seq[1].X = 2; seq[1].Y = 2;
            seqDownStand = seq;

            seq = new Vector2[2];
            if (LeftRightStill)
            {
                seq[0].X = 1; seq[0].Y = 3;
                seq[1].X = 1; seq[1].Y = 3;
            }
            else
            {   
                seq[0].X = 0; seq[0].Y = 3;
                seq[1].X = 2; seq[1].Y = 3;
            }
                seqLeftStand = seq;

            for (int i = 4; i < 8; i++)
            {
                firstFrameA[i] = 0;
                lastFrameA[i] = 1;
            }
        }

        /// <summary>
        /// Only change dir if its needed 
        /// </summary>
        public void changeDirIfNeeded(int dirAnimVal)
        {
            if (dirAnimVal != dirAnim) animationStart12(dirAnimVal);
        }

        public void animationStart12(int dirAnimVal)
        {
            dirAnim = dirAnimVal;
            switch (dirAnim)
            {
                case 0 : 
                setAnimationSequence(seqUp, firstFrameA[dirAnim], lastFrameA[dirAnim], ticksFrameA[dirAnim]);
                break;
                case 1 : 
                setAnimationSequence(seqRight, firstFrameA[dirAnim], lastFrameA[dirAnim], ticksFrameA[dirAnim]);
                break;
                case 2 : 
                setAnimationSequence(seqDown, firstFrameA[dirAnim], lastFrameA[dirAnim], ticksFrameA[dirAnim]);
                break;
                case 3:
                setAnimationSequence(seqLeft, firstFrameA[dirAnim], lastFrameA[dirAnim], ticksFrameA[dirAnim]);
                break;
                case 4:
                setAnimationSequence(seqUpStand, firstFrameA[dirAnim], lastFrameA[dirAnim], ticksFrameA[dirAnim]);
                break;
                case 5:
                setAnimationSequence(seqRightStand, firstFrameA[dirAnim], lastFrameA[dirAnim], ticksFrameA[dirAnim]);
                break;
                case 6:
                setAnimationSequence(seqDownStand, firstFrameA[dirAnim], lastFrameA[dirAnim], ticksFrameA[dirAnim]);
                break;
                case 7:
                setAnimationSequence(seqLeftStand, firstFrameA[dirAnim], lastFrameA[dirAnim], ticksFrameA[dirAnim]);
                break;
                case 8:
                setAnimationSequence(seqCustom1, firstFrameA[dirAnim], lastFrameA[dirAnim], ticksFrameA[dirAnim]);
                break;
                case 9:
                setAnimationSequence(seqCustom2, firstFrameA[dirAnim], lastFrameA[dirAnim], ticksFrameA[dirAnim]);
                break;
                default:                
                    setAnimationSequence(seqDownStand, firstFrameA[dirAnim], lastFrameA[dirAnim], ticksFrameA[dirAnim]);
                break;
            }
            animationStart();
        }
    }

}

//end 