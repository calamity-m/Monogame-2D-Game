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
    public class RC_resizeText : RC_RenderableBounded
    {
        public string text { get; set; }
        public Rectangle dest { get; set; }

        SillyFont sf = null;
        /// <summary>
        /// Single line of text thats resizable and bounded
        /// if you male the foregroundColor white then the white font can be re-coloured on draw if you set colour
        /// </summary>
        /// <param name="gd"></param>
        /// <param name="textZ"></param>
        /// <param name="boundz"></param>
        /// <param name="backGroundColor"></param>
        /// <param name="foregroundColor"></param>
        public RC_resizeText(GraphicsDevice gd, string textZ, Rectangle boundz, Color backGroundColor, Color foregroundColor)
        {
            active = true;
            visible = true;
            text = textZ;
            colour = Color.White;
            bounds = boundz;
            sf = new SillyFont(gd, backGroundColor, foregroundColor);
        }

        public override void Draw(SpriteBatch sb)
        {
            sf.drawStr(sb, bounds, text, colour);
        }
    }

}
