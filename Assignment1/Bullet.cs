using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Assignment1
{
    /// <summary>
    /// Based off of Michael Hoffman's xna tutorial - https://gamedevelopment.tutsplus.com/series/cross-platform-vector-shooter-xna--gamedev-10559
    /// </summary>
    public class Bullet : SpriteActor
    {
        // Add source into the constructor, so we can use bullet class for both players and enemies
        public SpriteActor source;

        /// <summary>
        /// Create a basic bullet
        /// </summary>
        /// <param name="tex">bullet texture</param>
        /// <param name="position">initial position</param>
        /// <param name="velocity">inital velocity</param>
        /// <param name="src">source from which this button was spawned (e.g. player, enemy, etc.)</param>
        /// <param name="autoBB">automatically set BB</param>
        public Bullet(Texture2D tex, Vector2 position, Vector2 velocity, SpriteActor src, bool autoBB) : base(tex, position, velocity)
        {
            source = src;
            if (!autoBB) sprite.setBB(spriteSize.X/2-(Resources.Bullet.Width/2), spriteSize.Y/2-(Resources.Bullet.Height/2), Resources.Bullet.Width, Resources.Bullet.Height);
            sprite.boundingSphereRadius = 15;
        }

        /// <summary>
        /// Destroy this bullet next update cycle/frame
        /// </summary>
        public void Destroy()
        {
            isFinished = true;
        }

        public override void Update()
        {

            if (velocity.LengthSquared() > 0)
            {
                orientation = HelperUtils.ConvertToAngleAim(velocity);
            }

            position += velocity;

            // Update our internal sprite position for collision detection
            sprite.setPos(position);

            // Add in deleting off-screen bullets
            Rectangle bound = new Rectangle(0, 0, (int)(Game1.screenSize.X), (int)(Game1.screenSize.Y));

            if (!bound.Contains(position.X, position.Y))
            {
                isFinished = true;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            sprite.customDraw(spriteBatch, position, sprite.getColor(), orientation, spriteSize);
            if (PlayerInput.keyHeld(Keys.B))
            {
                sprite.drawInfo(spriteBatch, Color.Red, Color.Green);
                sprite.drawBoundingSphere(spriteBatch, Color.Red);
            }
        }

    }
}
