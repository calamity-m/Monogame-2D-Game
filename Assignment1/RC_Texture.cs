using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


#pragma warning disable 1591 //sadly not yet fully commented

namespace RC_Framework
{
    public class RC_Texture
    {
        public static GraphicsDevice gd=null;
        
        public string name=""; // the name for 
        public string directoryAndFilename = "";
        public string fileName = "";
        public Texture2D texQ = null;
        public int index=0; // where am I in the list
        public int widthActive=-1; // the width of the active texture
        public int heightActive=-1; // the height of the active texture


        public static void setGraphicsDevice(GraphicsDevice gdQ)
        {
            gd = gdQ;
        }

        public RC_Texture(string nameQ, string directoryAndFilenameQ)
        {
            name = nameQ;
            directoryAndFilename = directoryAndFilenameQ;
            index = 0;
        }

        public RC_Texture(string nameQ, string directoryAndFilenameQ, int indexQ)
        {
            name = nameQ;
            directoryAndFilename = directoryAndFilenameQ;
            index = indexQ;
        }

        public void setWidthHeight(int w, int h)
        {
            widthActive = w;
            heightActive = h;
        }
        
        public bool loadFile()
        {
            if (texQ != null) return true;
            texQ = Util.texFromFile(gd, directoryAndFilename);
            if (texQ == null) return false;
            setFilename();
            setWidthHeight(texQ.Width,texQ.Height);
            return true;
        }

        public void setFilename()
        {
            if (directoryAndFilename == "") return; // just in case its a manual add
            fileName = Path.GetFileName(directoryAndFilename);
            if (name == "")
            {
                name = Path.GetFileNameWithoutExtension(directoryAndFilename);
            }
        }

        public Texture2D tex()
        {
            if (texQ == null) loadFile();
            return texQ;
        }

        public Texture2D Tex()
        {            
            if (texQ == null) loadFile();
            return texQ;
        }

    }

        // ********************************************** RC_TextureList *************************************************

    public class RC_TextureList
    {
        public List<RC_Texture> lst; // my list of textures

        public static GraphicsDevice gd = null;

        public string lastFname = "";
        public string lastName = "";
        public int lastFnameIndex = -1;
        public int lastNameIndex = -1;



        public RC_TextureList(GraphicsDevice gdQ)
        {
            gd = gdQ;
            lst = new List<RC_Texture>();
        }

        public static void setGraphicsDevice(GraphicsDevice gdQ)
        {
            gd = gdQ;
        }

        public Texture2D findName(string nameQ)
        {
            int j = -1;
            if (lastName == nameQ) return lst[lastNameIndex].tex(); // just did it a moment agao no need to repeat
            for (int i = 0; i < lst.Count; i++)
            {
                if (nameQ == lst[i].name) { j = i; break; }
            }
            if (j != -1)
            {
                lastName = nameQ;
                lastNameIndex = j;
                return lst[j].tex();
            }
            else
            {
                return null;
            }
        }

        public Texture2D findFName(string fnameQ)
        {
            int j = -1;
            if (lastFname == fnameQ) return lst[lastFnameIndex].tex(); // just did it a moment agao no need to repeat
            for (int i = 0; i < lst.Count; i++)
            {
                if (fnameQ == lst[i].fileName) { j = i; break; }
            }
            if (j != -1)
            {
                lastFname = fnameQ;
                lastFnameIndex = j;
                return lst[j].tex();
            }
            else
            {
                return null;
            }
        }

        public RC_Texture this[int i]
        {
            get { return lst[i]; }
            set { lst[i] = value; }
        }

        public int add(string nameQ, string directoryAndFilenameQ)
        {
            if (gd != null) RC_Texture.gd = gd;
            RC_Texture temp = new RC_Texture(nameQ, directoryAndFilenameQ);
            int index = lst.Count;
            lst.Add(temp);
            temp.index = index;
            return index;
        }

     }

// *********************************** Test Class for texturelist ************************************************************

    public class testTextureList : RC_RenderableBounded
    {
        RC_TextureList texl;
        int idx; // current index

        public testTextureList(RC_TextureList texlQ)
        {
            texl = texlQ;
        }

        public void showNext()
        {
            idx++;
            if (idx >= texl.lst.Count) idx = 0;
        }

        public override void Draw(SpriteBatch sb)
        {
            bounds = new Rectangle(0, 0, texl[idx].widthActive, texl[idx].heightActive);
            sb.Draw(texl[idx].tex(), bounds, colour);
        }
    }

    }

