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
    public enum PickUpType
    {
        HealthPack,
        WeaponPowerUp,
        SpeedPowerUp
    }

    public class Pickup : SpriteActor
    {
        // type of pickup
        public PickUpType type;

        private Vector2[] animation = new Vector2[14];

        public Pickup(Texture2D tex, Vector2 position, Vector2 velocity, PickUpType type) : base(tex, position, velocity)
        {
            this.type = type;
            loadAnimations();
        }

        // Load powerup tileset
        public void loadAnimations()
        {
            
            sprite.setWidthHeight(16, 16);
            sprite.setWidthHeightOfTex(128, 32);
            sprite.setXframes(8);
            sprite.setYframes(2);
            sprite.setHSoffset(new Vector2(8, 8));
            sprite.setBB(-4, -4, 24, 24); // Make it easier for player to pickup items

            
            for (int i = 0; i < 14; i++)
            {
                if (i < 8)
                {
                    animation[i].X = i; animation[i].Y = 0;
                } else
                {
                    animation[i].X = i % 8; animation[i].Y = 1;
                }
            }
            
            
            switch (type)
            {
                case PickUpType.HealthPack:
                    sprite.setAnimationSequence(animation, 0, 2, 10);
                    break;
                case PickUpType.WeaponPowerUp:
                    sprite.setAnimationSequence(animation, 3, 9, 10);
                    break;
                case PickUpType.SpeedPowerUp:
                    sprite.setAnimationSequence(animation, 10, 13, 10);
                    break;
            }

            sprite.animationStart();
            
        }

        public void Destroy()
        {
            isFinished = true;
        }

        public override void Update()
        {
            sprite.animationTick();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch);
            if (PlayerInput.keyHeld(Keys.B))
                sprite.drawInfo(spriteBatch, Color.Red, Color.Green);
        }
    }
}
