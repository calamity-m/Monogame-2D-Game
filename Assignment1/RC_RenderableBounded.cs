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

    // ************************************ colorField *******************************************

    /// <summary>
    /// This renderable is just a single rectangle of one colour, it requires that linebatch has been initialised
    /// </summary>
    public class ColorField : RC_RenderableBounded
    {
        
        public ColorField(Color c, Rectangle r)
        {
            bounds = new Rectangle(r.X,r.Y,r.Width,r.Height);
            colour = c;
        }
        
        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(LineBatch._empty_texture, bounds, colour);
        }
    }

    // ********************************* Bordered Text renderable ************************************************* 

    /// <summary>
    /// A line of text in a frame so to speak the border will have illusion of 3D if its not pure black or white
    /// </summary>
    public class TextRenderableBordered : RC_RenderableBounded 
    {
        public string text { get; set; }
        public SpriteFont font { get; set; }
        int borderSize;
        public Vector2 textOffset;
        Color backgroundColour;

        public int kind { get; set; } // for user use 
        //public int tag { get; set; }  // for user use

        public TextRenderableBordered(string textZ, Rectangle posZ, SpriteFont fontZ, Color txtCcolor, Color backColor, int borderWidth)
        {
            active = true;
            text = textZ;
            bounds = posZ;
            font = fontZ;
            colour = txtCcolor;
            borderSize = borderWidth;
            textOffset = new Vector2(3, 2);
            backgroundColour = backColor;
        }

        public override void Draw(SpriteBatch sb)
        {
            LineBatch.drawFillRectangleBorder(sb, bounds, backgroundColour, borderSize);
            sb.DrawString(font, text, new Vector2(bounds.X+textOffset.X,bounds.Y+textOffset.Y), colour);
        }

    }

    // ********************************* Annekes Button built out of two textures ************************************************* 

    /// <summary>
    /// A line of text in a frame so to speak the border will have illusion of 3D if its not pure black or white
    /// </summary>
    public class Button : RC_RenderableBounded
    {
        public string text { get; set; }
        public SpriteFont font { get; set; }
        public Vector2 textOffset;
        Color fontColor;
        Texture2D texBack;
        Color mainColour;
        Color mouseOverColour;

        public int kind { get; set; } // for user use 
        //public int tag { get; set; }  // for user use

        public Button(Texture2D backGroundTex, string textZ, SpriteFont fontZ, Vector2 pos, Color fontColorZ, Color backColor)
        {
            active = true;
            visible = true;
            text = textZ;
            texBack = backGroundTex;
            bounds = new Rectangle((int)pos.X, (int)pos.Y, texBack.Width, texBack.Height);
            font = fontZ;
            colour = backColor;

            mainColour = backColor;
            mouseOverColour = new Color(backColor.R + 50, backColor.G + 50, backColor.B + 50); 

            fontColor = fontColorZ;
            Vector2 siz = font.MeasureString(text);
            float x = (texBack.Width - siz.X) / 2;
            float y = (texBack.Height - siz.Y) / 2;
            textOffset = new Vector2(x, y);
        }

        public override void Draw(SpriteBatch sb)
        {
            //LineBatch.drawFillRectangleBorder(sb, bounds, backgroundColour, borderSize);
            sb.Draw(texBack, bounds, colour);
            sb.DrawString(font, text, new Vector2(bounds.X + textOffset.X, bounds.Y + textOffset.Y), fontColor);
        }

        public override bool MouseOver(float mouse_x, float mouse_y)
        {
            // put your mouse over code here anne

            if (this.Contains((int)mouse_x, (int)mouse_y))
            {
                if (this.colour == Color.Gray)
                {
                    //do nothing
                }
                else
                {

                    this.colour = mouseOverColour;
                }
            }
            else if (this.colour != Color.Gray)
            {
                this.colour = mainColour;
            }
            return false;
        }

    }

    // ************************************* TextRenderableFade ******************************************

    /// <summary>
    /// Text that fades Good for scores and things eventually sets itself inactive
    /// Also you can set drift so it moves up or down as it fades
    /// You must call update to make it fade
    /// </summary>
    class TextRenderableFade : TextRenderable
    {
        Color finalColour;
        int ticks;
        int fadeTicks;
        public Vector2 drift; // to make it move up or down
        Vector2 curPos;
        Color curColour;

        public TextRenderableFade(string textZ, Vector2 posZ, SpriteFont fontZ, Color colourZ, Color finalColourZ, int fadeTicksZ)
            : base(textZ, posZ, fontZ, colourZ)
        {
        finalColour=finalColourZ;
        ticks=0;
        fadeTicks=fadeTicksZ;
        drift.X = 0;
        drift.Y = 0;
        curPos.X = posZ.X;
        curPos.Y = posZ.Y;
        }
        
        public override void Draw(SpriteBatch sb)
        {
            if (!active) return;
            sb.DrawString(font, text, curPos, curColour);
        }
        
        public override void Update(GameTime gameTime)
        {
            if (!active) return;
            ticks++;
            if (ticks > fadeTicks)
            {
                active = false;
                visible = false;
                return;
            }
            curColour=Color.Lerp(colour, finalColour, (float)ticks/(float)fadeTicks);
            curPos = curPos + drift;
        }

        public override void Reset()
        {
            ticks=0;
            curPos.X = pos.X;
            curPos.Y = pos.Y;
        }
    }

    // ************************************* TextRenderableFlash ******************************************

    /// <summary>
    /// Flashing text
    /// need to call update to make it flash
    /// </summary>
    class TextRenderableFlash : TextRenderable
    {
   
        int ticks;
        int flashTicks;
        bool isShown;

        public TextRenderableFlash(string textZ, Vector2 posZ, SpriteFont fontZ, Color colourZ, int flashTicksZ)
            : base(textZ, posZ, fontZ, colourZ)
        {
            colour = colourZ;
            ticks = 1;
            flashTicks = flashTicksZ;
            isShown = true;
        }

        public override void Draw(SpriteBatch sb)
        {
            if (!active || !visible) return;
            if (isShown)sb.DrawString(font, text, pos, colour);
        }

        public override void Update(GameTime gameTime)
        {
            if (!active) return;
            ticks++;
            if (ticks % flashTicks == 0)
            {
                isShown = !isShown;
                return;
            }
        }

        public override void Reset()
        {
            ticks = 1;
        }
    }

    
    // ************************************  circle fade (and resize as well) ****************

    /// <summary>
    /// Fades and resizes a circle 
    /// At end it can loop , go inactive or reverse
    /// its a fairly sophisticated tool usable for a lot of diferent eye candy effects
    /// remeber to run update
    /// </summary>
    class CircleFade : RC_RenderableBounded //(and resize as well)
    {
        Color finalColour;
        Color initColour;
        Vector2 initPosition;
        Vector2 finalPosition;
        float initRadius;
        float finalRadius;
        int fadeTicks;
        public int loop; // 0=end (go inactive), 1=Loop, 2=reverse
        int ticks;
        bool reverse;
        float lerp;

        Color curColour;
        Vector2 curPos;
        float curRadius;
       

        public CircleFade(Vector2 initPositionZ, Vector2 finalPositionZ, float initRadiusZ, float finalRadiusZ, Color initColourZ, Color finalColourZ, int fadeTicksZ)
            : base()
        {
            finalColour = finalColourZ;
            initColour = initColourZ;
            fadeTicks = fadeTicksZ;
            initPosition = initPositionZ;
            finalPosition = finalPositionZ;
            initRadius = initRadiusZ;
            finalRadius= finalRadiusZ;
            Reset();
        }

        public void setLoop(int loopQ)
        {
            loop = loopQ;
        }

        public override void Draw(SpriteBatch sb)
        {
            if (!active) return; 
            if (!visible) return;
            LineBatch.drawCircle(sb, curColour, curPos,curRadius,19,1);
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
            curPos.X = (float)MathHelper.Lerp(initPosition.X, finalPosition.X, lerp);
            curPos.Y = (float)MathHelper.Lerp(initPosition.Y, finalPosition.Y, lerp);
            curRadius = (float)MathHelper.Lerp(initRadius, finalRadius, lerp);
       }

        public override void Reset()
        {
            ticks = 0;
            reverse = false;
        }
    }


    // ******************************************** Scroll Back Ground *********************************

    /// <summary>
    /// Scrolling background
    /// Horizontal or vertical but not both
    /// </summary>
    public class ScrollBackGround : RC_RenderableBounded
    {
        //Rectangle screenRectangle;
        Rectangle sourceRectangle;

        Rectangle source1;
        Rectangle source2;

        Rectangle screen1;
        Rectangle screen2;
        
        float scrollV;
        float scrollH;
        float scrollDelta;

        Texture2D tex;
        int direction; // 0=none 1=vertical 2=horizontal

        //public Color colour {set; get;}
        
        public ScrollBackGround(Rectangle boundsZ)
        {
        bounds = new Rectangle(boundsZ.X, boundsZ.Y, boundsZ.Width, boundsZ.Height);
        scrollDelta=1;
        colour=Color.White;
        Reset();
        }
        /// <summary>
        /// direction  0=none 1=vertical 2=horizontal
        /// </summary>
        /// <param name="texZ"></param>
        /// <param name="source"></param>
        /// <param name="dest"></param>
        /// <param name="delta"></param>
        /// <param name="directionZ"></param>
        public ScrollBackGround(Texture2D texZ, Rectangle source, Rectangle dest, float delta, int directionZ)
        {
        scrollDelta=delta;
        direction=directionZ;
        tex=texZ;
        //screenRectangle=dest;
        bounds = new Rectangle(dest.X, dest.Y, dest.Width, dest.Height);
        sourceRectangle=source;
        colour=Color.White;
        }

        public void setScrollSpeed(float speed)
        {
            scrollDelta = speed;
        }

        public override void Draw(SpriteBatch sb)
        {
            if (direction == 0) sb.Draw(tex, bounds, sourceRectangle, Color.White);

            if (direction == 1) //Vertical scroll
            {
                int s = (int)(sourceRectangle.Height - scrollV);
                float ratio = (float)bounds.Height / (float)sourceRectangle.Height;
                
                source1 = new Rectangle(sourceRectangle.X, sourceRectangle.Y, sourceRectangle.Width, sourceRectangle.Height -(int)scrollV);
                screen1 = new Rectangle(bounds.X, bounds.Y + (int)(scrollV * ratio), bounds.Width, bounds.Height - (int)(scrollV * ratio));
                
                source2 = new Rectangle(sourceRectangle.X, sourceRectangle.Y+s, sourceRectangle.Width, (int)scrollV);
                screen2 = new Rectangle(bounds.X, bounds.Y, bounds.Width, (int)(scrollV * ratio));
                
                sb.Draw(tex, screen1, source1, colour); 
                sb.Draw(tex, screen2, source2, colour);

                //spriteBatch.Draw(tex3, new Rectangle(0, scrollV, 800, 600 - scrollV), new Rectangle(0, 0, 800, 600 - scrollV), Color.White);
                //spriteBatch.Draw(tex3, new Rectangle(0, 0, 800, scrollV), new Rectangle(0, s, 800, scrollV), Color.White);
            }

            if (direction == 2) //Horizontal scroll
            {
                int s = (int)(sourceRectangle.Width - scrollH);
                float ratio = (float)bounds.Width / (float)sourceRectangle.Width;
                
                source1 = new Rectangle(sourceRectangle.X, sourceRectangle.Y, sourceRectangle.Width-(int)scrollH, sourceRectangle.Height);
                screen1 = new Rectangle(bounds.X + (int)(scrollH * ratio), bounds.Y, bounds.Width - (int)(scrollH * ratio), bounds.Height);
                
                source2 = new Rectangle(sourceRectangle.X+s, sourceRectangle.Y, (int)scrollH, sourceRectangle.Height );
                screen2 = new Rectangle(bounds.X, bounds.Y, (int)(scrollH * ratio), bounds.Height);
                
                sb.Draw(tex, screen1, source1, colour); 
                sb.Draw(tex, screen2, source2, colour);
                
                //spriteBatch.Draw(tex3, new Rectangle(scrollH, 0, 800-scrollH, 600), new Rectangle(0, 0, 800 - scrollH, 600), Color.White);
                //spriteBatch.Draw(tex3, new Rectangle(0, 0, scrollH, 600), new Rectangle(s, 0, scrollH, 600), Color.White);
            }

        }

        public override void Update(GameTime gameTime)
        {
        scrollV = scrollV+scrollDelta;
        if (scrollV > sourceRectangle.Height) scrollV = 0;
        if (scrollV < 0) scrollV = sourceRectangle.Height;
            
        scrollH = scrollH+scrollDelta; 
        if (scrollH > sourceRectangle.Width) scrollH = 0;
        if (scrollH < 0) scrollH = sourceRectangle.Width;
        }

        public override void Reset()
        {
        scrollV = 0;
        scrollH = 0;
        } 
    }

   

   
    // **************************************************** Blinking Boxes *************************************************************************************

    /// <summary>
    /// Boxes that blink randomly - for eyecandy on control pannels and computers and things
    ///  blinktype = 0 for totally random new colour any other is a delta for cycling
    ///  starttype = 0 for totally random new colour 1 = same colour 2 = cycle colours
    ///  Example calling sequence is
    ///        r8a = new BlinkingBoxes(new Rectangle(10, 10, 200, 100), new Vector2(60, 30), new Vector2(2, 4), 4, 3, 5, 12);
    ///        r8a.setTiming(30, 30, 0, 0);
    ///        r8a.addColor(Color.White);
    ///        r8a.addColor(Color.Gray);
    ///        r8a.addColor(new Color(50,200,7));
    ///        r8a.addColor(Color.Red);
    ///        r8a.addColor(Color.Green);
    ///        
    ///        After that call Update in update and Draw in draw
    /// </summary>
    public class BlinkingBoxes : RC_RenderableBounded
    {
        // notes uses bounds but not colour

        public Texture2D tex=null; // optuonal texture

        public Vector2 sizeOfBox;
        public Vector2 gapBetweenBoxes;
        public int numOfBoxesX;
        public int numOfBoxesY;
        int numOfColors;
        int numOfColorsUsed=0;
        Color[] colors;
        Rectangle[,] boxBounds;
        int[,] boxColor;
        Random randy;
        int ticks = 0;

        int updateModulo = 1;
        float percentChanceOfChange = 1.0f;
        int blinkType = 0; // 0=totally random new colour any other is a delta for cycling  
        int startType = 0; // 0=totally random new colour 1 = same colour 2 = cycle colours

        public BlinkingBoxes(Rectangle boundz, Vector2 sizeOfBoxx, Vector2 gapBetweenBoxez,
            int numOfBoxezX, int numOfBoxezY, int numOfColorz, int randomSeed)
        {
            bounds = boundz;
            sizeOfBox=sizeOfBoxx;
            gapBetweenBoxes=gapBetweenBoxez;
            numOfBoxesX=numOfBoxezX;
            numOfBoxesY=numOfBoxezY;
            numOfColors=numOfColorz;
            colors = new Color[numOfColorz];
            boxBounds = new Rectangle[numOfBoxesX,numOfBoxesY];
            boxColor = new int[numOfBoxesX,numOfBoxesY];
            randy = new Random( randomSeed );
        }

        public void setStockColours()
        {
            colors = new Color[8];
            addColor(Color.Black);
            addColor(Color.White);
            addColor(Color.Red);
            addColor(Color.Green);
            addColor(Color.Blue);
            addColor(Color.Cyan);
            addColor(Color.Magenta);
            addColor(Color.Yellow);
        }

        public void setStockColoursWithTrans()
        {
            colors = new Color[10];
            addColor(new Color(0,0,0,0));
            addColor(Color.Black);
            addColor(Color.White);
            addColor(Color.Red);
            addColor(Color.Green);
            addColor(Color.Blue);
            addColor(Color.Cyan);
            addColor(Color.Magenta);
            addColor(Color.Yellow);
            addColor(new Color(255,255,255,0));
        }

        public void setStockColoursWithLotsTrans()
        {
            colors = new Color[12];
            addColor(new Color(0,0,0,0));
            addColor(new Color(0,0,0,0));
            addColor(Color.Black);
            addColor(Color.White);
            addColor(Color.Red);
            addColor(Color.Green);
            addColor(Color.Blue);
            addColor(Color.Cyan);
            addColor(Color.Magenta);
            addColor(Color.Yellow);
            addColor(new Color(255,255,255,0));
            addColor(new Color(255,255,255,0));
        }

        public void setTiming(int updateModuloZ, float percentChanceOfChangeZ, int blinkTypeZ, int startTypeZ)
        {
        updateModulo = updateModuloZ;
        percentChanceOfChange = percentChanceOfChangeZ;
        blinkType = blinkTypeZ; // 0=totally random new colour any other is a delta for cycling  
        startType = startTypeZ; // 0=totally random new colour 1 = same colour 2 = cycle colours
        Reset();
        }
       
        public void addColor(Color c)
        {
           colors[numOfColorsUsed] = c;
           numOfColorsUsed++;
           Reset();
        }

        public override void Reset() // start
        {
            boxBounds = new Rectangle[numOfBoxesX, numOfBoxesY];
            boxColor = new int[numOfBoxesX, numOfBoxesY];
            int cycle = 0;
            for (int x=0; x<numOfBoxesX; x++)
                for (int y = 0; y < numOfBoxesY; y++)
                { 
                    Rectangle r = new Rectangle((int)(bounds.X+sizeOfBox.X*x+gapBetweenBoxes.X*x),
                                                (int)(bounds.Y+sizeOfBox.Y*y+gapBetweenBoxes.Y*y),
                                                (int)(sizeOfBox.X), (int)(sizeOfBox.Y));
                    boxBounds[x, y] = r; new Rectangle();
                    if (startType == 2) // 0=totally random new colour 1 = same colour 2 = cycle colours
                    {
                        boxColor[x, y] = cycle;
                        cycle++;
                        if (cycle >= numOfColorsUsed) cycle = 0;
                    }
                    if (startType == 1) // 0=totally random new colour 1 = same colour 2 = cycle colours
                    {
                        boxColor[x, y] = 0;
                    }
                    if (startType == 0) // 0=totally random new colour 1 = same colour 2 = cycle colours
                    {
                        boxColor[x, y] = randy.Next(0, numOfColorsUsed);
                    }
                }
        }

        public override void Draw(SpriteBatch sb)
        {
            if (!active) return;
            if (!visible) return;
            for (int x = 0; x < numOfBoxesX; x++)
                for (int y = 0; y < numOfBoxesY; y++)
                {
                    if (tex == null)
                    {
                        sb.Draw(LineBatch._empty_texture, boxBounds[x, y], colors[boxColor[x, y]]);
                    }
                    else
                    {
                        sb.Draw(tex, boxBounds[x, y], colors[boxColor[x, y]]);
                    }
                }
        }

        public override void Update(GameTime gameTime)
        {
            //return;
            if (!active) return;
            ticks++;
            if (ticks % updateModulo != 0) return;
            for (int x = 0; x < numOfBoxesX; x++)
                for (int y = 0; y < numOfBoxesY; y++)
                    {
                        if ( randy.Next(0,10000) < percentChanceOfChange*100 )
                        {
                            if (blinkType == 0) // 0=totally random new colour 
                            {
                               boxColor[x, y] = randy.Next(0, numOfColorsUsed);
                            }
                            else
                            {
                                boxColor[x, y] = boxColor[x, y]+blinkType;
                                while (boxColor[x, y] >= numOfColorsUsed) boxColor[x, y] = boxColor[x, y] - numOfColorsUsed;
                            }
                        }
                    }

            
        }
    }

    // ----------------------------------------- Repeat Image -----------------------------------------------------------
    /// <summary>
    /// A List of images like a lives counter
    /// the destination rectange is the destination of just 1 image (the first image)
    /// If source is null then the whole image is used
    /// setting a background colour gives a background (helpfull for debugging)
    /// setting a gapgives you a gap
    /// </summary>
    public class RepeatImage : RC_RenderableBounded
    {
        internal Texture2D tex { set; get; }
        internal Rectangle? source { set; get; }
        //public Rectangle dest {set; get;} - now uses bounds
        public Color backgroundColor = Color.Transparent;
        public int gap = 0; 
        internal int maxNumber=1;
        internal int currNum=0;
        internal int width =0;

        public RepeatImage(Texture2D texZ, Rectangle? sourceZ, Rectangle destZ, Color colourZ, int maxNumberZ, int startNum)
        {
            tex = texZ;
            source = sourceZ;
            if (source == null)
            {
                source = new Rectangle(0, 0, tex.Width, tex.Height);
            }
            width = destZ.Width;
            colour = colourZ;
            maxNumber = maxNumberZ;
            currNum = startNum; 
            bounds = new Rectangle(destZ.X, destZ.Y, destZ.Width*maxNumber, destZ.Height);
        }

        public int addNum(int numToAdd)
        {
            currNum = currNum + numToAdd;
            if (currNum < 0) currNum=0;
            if (currNum > maxNumber) currNum = maxNumber;
            return currNum;
        }

        public void setNum(int num)
        {
            currNum = num;
            if (currNum < 0) currNum=0;
            if (currNum > maxNumber) currNum = maxNumber;
        }

        public override void Draw(SpriteBatch sb)
        {
            if (backgroundColor.A != 0) // not transparent
            {
                sb.Draw(LineBatch._empty_texture, bounds, backgroundColor);
            }
            if (currNum <=0) return;
            for (int i=0; i< currNum; i++)
            {
                sb.Draw(tex, new Rectangle(i*width+i*gap, bounds.Y, width, bounds.Height), source, colour);
            }
            
        }
    }

    // ----------------------------------------- ImageBackground -----------------------------------------------------------
    /// <summary>
    /// A background image with source, colour and destination
    /// Can Automatically resize to viewport consequently it needs the viewport passed in by graphics device
    /// If the source is null the whole texture is used
    /// </summary>
    public class ImageBackground : RC_RenderableBounded
    {
        public Texture2D tex { set; get; }
        public Rectangle? source { set; get; }
        //public Rectangle dest {set; get;} - now uses bounds

        public ImageBackground(Texture2D texZ, Rectangle? sourceZ, Rectangle destZ, Color colourZ)
        {
            tex = texZ;
            source = sourceZ;
            if (source == null)
            {
                source = new Rectangle(0, 0, tex.Width, tex.Height);
            }
            bounds = new Rectangle(destZ.X, destZ.Y, destZ.Width, destZ.Height);
            colour = colourZ;
        }

        public ImageBackground(Texture2D texZ, Color colourZ, GraphicsDevice g)
        {
            tex = texZ;
            source = new Rectangle(0, 0, tex.Width, tex.Height);
            bounds = new Rectangle(0, 0, g.Viewport.Width, g.Viewport.Height);
            colour = colourZ;
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(tex, bounds, source, colour);
        }
    }

    
// ******************************** grid background ******************************************************

    /// <summary>
    /// Draws a grid in a rectangle
    /// It can be acurately positioned by changing Offset
    /// It can be animated by changing delta and updateModulo
    /// Vertoical or horizontal lines may be omitted by setting drawVertical or drawHorizontal to false
    /// 
    /// Ticks to lerp and linecolour2 can be set to change the grid colour so it sort of pulsates colour
    /// example setup may be:
    ///        r9b = new GridBackGround(new Rectangle(10, 300, 200, 200), Color.Red, Color.BlanchedAlmond, new Vector2(30, 20), 2);
    ///        r9b.delta = new Vector2(1, 0); // speed of animation
    ///        r9b.updateModulo = 2; // try 30 to slow it a bit
    ///
    ///  or:      
    /// 
    ///        r9c = new GridBackGround(new Rectangle(260, 10, 200, 200), Color.Red, Color.Silver, new Vector2(30, 20), 2);
    ///        r9c.delta = new Vector2(0f,0.2f); // speed of animation
    ///        r9c.updateModulo = 2; // try 30 to slow it a bit
    ///        r9c.ticksToLerp = 120;
    ///        r9c.lineColour2 = new Color(255, 255, 255);
    /// </summary>
    public class GridBackGround : RC_RenderableBounded
    {
        Color lineColour1;
        public Color lineColour2 = Color.Black;
        public int ticksToLerp = 1;
        Vector2 lineSpacing;
        int lineWidth;
        public Vector2 offset;
        public Vector2 delta;
        public bool drawBackground = true;
        int ticks = 0;    
        public int updateModulo =1;
        public bool drawVertical = true;
        public bool drawHorizontal = true;

        public GridBackGround(Rectangle boundZ, Color lineColor, Color backColor, Vector2 spacing, int lineWidthZ)
        {
            bounds = Util.newRectangle(boundZ);
            colour = backColor;
            lineColour1 = lineColor;
            lineSpacing = spacing;
            lineWidth = lineWidthZ;
            offset = new Vector2(0,0);
            delta = new Vector2(0,0);
        }

        public override void Draw(SpriteBatch sb)
        {
            int linesX = (int)(bounds.Width/lineSpacing.X)+1;
            int linesY = (int)(bounds.Height/lineSpacing.Y)+1;

            Vector2 v1 = new Vector2();
            Vector2 v2 = new Vector2();
            v1.Y = bounds.Y; 
            v2.Y = bounds.Y+bounds.Height;
            if (drawBackground ) sb.Draw(LineBatch._empty_texture, bounds, colour);
            int v = Math.Abs(ticks % ticksToLerp - (ticksToLerp/2));
            Color c;
            float ll= (float)v / ((float)ticksToLerp / 2);
            if (ticksToLerp == 1) c = lineColour1;
            else 
                c = Color.Lerp(lineColour1, lineColour2, ll);
            for (int x=0; x<linesX; x++)
                {
                       
                    for (int i = 0; i < lineWidth; i++)
                        {                    
                        v1.X = offset.X + lineSpacing.X*x + bounds.X+i; 
                        v2.X = v1.X;
                        if (v1.X < bounds.X + bounds.Width && drawVertical)
                           {
                            LineBatch.drawLine(sb, c, v1, v2);
                           }
                         }
                }            
            
            v1.X = bounds.X; 
            v2.X = bounds.X+bounds.Width;
            for (int y=0; y<linesY; y++)
               {
               
                  for (int i = 0; i < lineWidth; i++)
                   { 
                   v1.Y = offset.Y + lineSpacing.Y * y + bounds.Y+i;
                   v2.Y = v1.Y;
                   if (v1.Y < bounds.Y + bounds.Height && drawHorizontal)
                       {
                           LineBatch.drawLine(sb, c, v1, v2);
                       }
                   }
               }
        }


        public override void Update(GameTime gameTime)
        {
        ticks++;    
        if (ticks % updateModulo != 0) return;
        offset = offset + delta;
        if (offset.X > lineSpacing.X) offset.X = offset.X - lineSpacing.X;
        if (offset.Y > lineSpacing.Y) offset.Y = offset.Y - lineSpacing.Y;
        }

        public override void Reset()
        {
        offset.X = 0;
        offset.Y = 0;
        } 
    }

    // ********************************************************************  TextureFlash ** ******************************************************************

    public class TextureFlash : RC_RenderableBounded
    {
        public Texture2D tex { set; get; }
        public Rectangle? source { set; get; }
        //public Rectangle dest {set; get;} - now uses bounds

        bool isTransparent = false;
        int ticks=0;
        public int ticksWhenTrasnsparent;
        public int ticksWhenVisible; 

        public TextureFlash(Texture2D texZ, Rectangle? sourceZ, Rectangle destZ, Color colourZ, int ticksVisible, bool startTransparent)
        {
            tex = texZ;
            source = sourceZ;
            if (source == null)
            {
                source = new Rectangle(0, 0, tex.Width, tex.Height);
            }
            bounds = new Rectangle(destZ.X, destZ.Y, destZ.Width, destZ.Height);
            colour = colourZ;
            isTransparent = startTransparent;
            ticksWhenTrasnsparent = ticksVisible;
            ticksWhenVisible = ticksVisible; 
            if (startTransparent) ticks = ticksWhenVisible; 
        }

        public TextureFlash(GraphicsDevice g, Texture2D texZ, Color colourZ, int ticksVisible, bool startTransparent)
        {
            tex = texZ;
            source = new Rectangle(0, 0, tex.Width, tex.Height);
            bounds = new Rectangle(0, 0, g.Viewport.Width, g.Viewport.Height);
            colour = colourZ;
            isTransparent = startTransparent;
            ticksWhenTrasnsparent = ticksVisible;
            ticksWhenVisible = ticksVisible;
            if (startTransparent) ticks = ticksWhenVisible; 
        }

        public override void Draw(SpriteBatch sb)
        {
           if (!isTransparent) sb.Draw(tex, bounds, source, colour);
        }

        public override void Update(GameTime gameTime)
        {
            ticks++;
            int rem = ticks % (ticksWhenTrasnsparent + ticksWhenVisible);
            if (rem < ticksWhenVisible)
            {
                isTransparent=false;
            }
            else
            { 
                isTransparent=true;
            }
        }
  
    }


    // ******************************************************************* MultiRenderable *******************************************************************

        /// <summary>
        /// This class is just up to 4 bounded renderables for coalessing renderables to gethr in more usefull ways, (eg paralax backgrounds)
        /// </summary>
        /// 
    public class MultiRenderable : RC_RenderableBounded
    {
        public RC_RenderableBounded sb1 = null;
        public RC_RenderableBounded sb2 = null;
        public RC_RenderableBounded sb3 = null;
        public RC_RenderableBounded sb4 = null;

        public MultiRenderable(Rectangle boundsZ,
                                   RC_RenderableBounded sbb1, RC_RenderableBounded sbb2,
                                   RC_RenderableBounded sbb3, RC_RenderableBounded sbb4)
        {
            // set unused ones to null 
            bounds = new Rectangle(boundsZ.X, boundsZ.Y, boundsZ.Width, boundsZ.Height);
            sb1 = sbb1;
            sb2 = sbb2;
            sb3 = sbb3;
            sb4 = sbb4;
            if (sb1 != null) sb1.bounds = bounds;
            if (sb2 != null) sb2.bounds = bounds;
            if (sb3 != null) sb3.bounds = bounds;
            if (sb4 != null) sb4.bounds = bounds;
        }

        public void setBounds(Rectangle boundsZ)
        {
            bounds = new Rectangle(boundsZ.X, boundsZ.Y, boundsZ.Width, boundsZ.Height);
            if (sb1 != null) sb1.bounds = bounds;
            if (sb2 != null) sb2.bounds = bounds;
            if (sb3 != null) sb3.bounds = bounds;
            if (sb4 != null) sb4.bounds = bounds;
        }


        public override void Update(GameTime gameTime)
        {
            if (sb1 != null) sb1.Update(gameTime);
            if (sb2 != null) sb2.Update(gameTime);
            if (sb3 != null) sb3.Update(gameTime);
            if (sb4 != null) sb4.Update(gameTime);
        }

        public override void Draw(SpriteBatch sb)
        {
            if (sb1 != null) sb1.Draw(sb);
            if (sb2 != null) sb2.Draw(sb);
            if (sb3 != null) sb3.Draw(sb);
            if (sb4 != null) sb4.Draw(sb);
        }
    }


}


