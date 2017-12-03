using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using RC_Framework;

namespace Assignment1
{
    /// <summary>
    /// Based off Michael Hoffman's xna tutorial - https://gamedevelopment.tutsplus.com/series/cross-platform-vector-shooter-xna--gamedev-10559
    /// Modified to use Sprite3 as a base for collision and drawing
    /// </summary>
    public abstract class SpriteActor
    {
        // Our Sprite Object
        public Sprite3 sprite;

        // Colour used for transparency, power-ups et.c
        public Color color = Color.White;

        // Flag used for deleting or updating sprites
        public bool isFinished;

        // Sprite Orientation
        public float orientation;

        // Sprite Position
        public Vector2 position;

        // Sprite Velocity
        public Vector2 velocity;

        // Sprite Size
        public Vector2 spriteSize;

        // Basic Sprite Constructor
        public SpriteActor(Texture2D tex, Vector2 position, Vector2 velocity)
        {
            spriteSize = new Vector2(tex.Width, tex.Height);
            this.position = position;
            this.velocity = velocity;
            sprite = new Sprite3(true, tex, position.X - tex.Width, position.Y - tex.Height);
            sprite.setHSoffset(new Vector2(spriteSize.X / 2, spriteSize.Y / 2));
            sprite.setBBToTexture();
        }

        // Implemented in derived classes
        public abstract void Update();

        // Implemented in derived classes
        public abstract void Draw(SpriteBatch spriteBatch);

    }
}
