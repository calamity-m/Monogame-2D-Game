using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using RC_Framework;

namespace Assignment1
{
    /// <summary>
    /// In-efficent button class
    /// </summary>
    public class Button
    {
        // Internal button sprite
        private Sprite3 button;
        // Color for when mouse is inside button
        private Color highLight;
        // Scale for when mouse is inside button
        private float highLightScale = 1.1f;
        // Next level to be set on button select (if applicable)
        private int nextLevel;
        // Button type
        private ButtonType type;

        // Button handling variables
        private Random rand = new Random();
        private bool playedSound;

        public enum ButtonType
        {
            ChangeLevelSet,
            ChangeLevelPush,
            ChangeLevelPop,
            ResetGame,
            Exit,
            VolumeControl,
            GraphicsControl
        }

        /// <summary>
        /// Create a basic button
        /// </summary>
        /// <param name="tex">Button texture</param>
        /// <param name="pos">Position</param>
        /// <param name="highLight">Highlighted colour</param>
        /// <param name="buttonType">Button type</param>
        /// <param name="nextLevel">nextlevel/value if applicable</param>
        public Button(Texture2D tex, Vector2 pos, Color highLight, ButtonType buttonType, int nextLevel)
        {
            // Initialise our Button
            this.highLight = highLight;
            this.nextLevel = nextLevel;
            type = buttonType;
            button = new Sprite3(true, tex, 0, 0);
            button.setHSoffset(new Vector2(button.getWidth() / 2, button.getHeight() / 2));
            button.setPos(pos);
        }

        /// <summary>
        /// Act when our button is entered
        /// </summary>
        public void buttonEntered()
        {
            button.setColor(highLight);
            button.scale = highLightScale;
            if (!playedSound)
            {
                SoundManager.getMenuSelect().Play(0.2f * Resources.volume, HelperUtils.RandFloat(rand, -0.2f, 0.2f), 0);
                playedSound = true;
            }

            switch (type)
            {
                case ButtonType.ChangeLevelSet:
                    if (PlayerInput.m1OldPressed())
                    {
                        SoundManager.getMenuSelect().Play(0.2f * Resources.volume, HelperUtils.RandFloat(rand, -0.2f, 0.2f), 0);
                        Game1.levelManager.setLevel(nextLevel);
                    }
                    break;
                case ButtonType.ChangeLevelPush:
                    if (PlayerInput.m1OldPressed())
                    {
                        SoundManager.getMenuSelect().Play(0.2f * Resources.volume, HelperUtils.RandFloat(rand, -0.2f, 0.2f), 0);
                        Game1.levelManager.pushLevel(nextLevel);
                    }
                    break;
                case ButtonType.ChangeLevelPop:
                    if (PlayerInput.m1OldPressed())
                        Game1.levelManager.popLevel();
                    break;
                case ButtonType.ResetGame:
                    if (PlayerInput.m1OldPressed())
                    {
                        SoundManager.getMenuSelect().Play(0.2f * Resources.volume, HelperUtils.RandFloat(rand, -0.2f, 0.2f), 0);
                        Resources.score = 0;
                        Resources.currPlayLevel = 0;
                        Game1.spriteManager.clearScene(true);
                        Game1.particleManager.ClearScene();
                        if (Game1.spriteManager.player != null)
                            Game1.spriteManager.player.resetPlayer();
                        Game1.levelManager.setLevel(0);
                        //Game1.ResetLevel
                    }
                    break;
                case ButtonType.Exit:
                    if (PlayerInput.m1OldPressed())
                        Game1.exitGame = true;
                    break;
                case ButtonType.VolumeControl:
                    if (PlayerInput.m1Pressed())
                    {
                        if (nextLevel == 0)
                            Resources.volume -= 0.01f;
                        else if (nextLevel == 1)
                            Resources.volume += 0.01f;
                        else if (nextLevel == 2)
                            Resources.volume = 0.5f;
                        Resources.volume = HelperUtils.Clamp(Resources.volume, 0f, 1f);
                        MediaPlayer.Volume = Resources.volume;
                    }
                    break;
                case ButtonType.GraphicsControl:
                    if (PlayerInput.m1Pressed())
                    {
                        if (nextLevel == 0)
                            Resources.graphicsQuality = 0;
                        else if (nextLevel == 1)
                            Resources.graphicsQuality = 1;
                        else if (nextLevel == 2)
                            Resources.graphicsQuality = 2;

                        Game1.particleManager.setMaxParticleAmount(1500);
                    }
                    break;
            }


        }

        /// <summary>
        /// Act when our button is exited
        /// </summary>
        public void buttonExited()
        {
            button.setColor(Color.White);
            button.scale = 1f;
            playedSound = false;
        }

        /// <summary>
        /// Check if the mouse is colliding with sprite s
        /// </summary>
        /// <param name="s">sprite/button to be checking collision with</param>
        /// <returns></returns>
        private bool mouseColliding(Sprite3 s)
        {
            return s.insideOrEq(PlayerInput.mousePosition.X, PlayerInput.mousePosition.Y);
        }

        public Vector2 getPos()
        {
            return button.getPos();
        }

        public float getPosY()
        {
            return button.getPosY();
        }

        public float getPosX()
        {
            return button.getPosX();
        }

        public void Update()
        {
            // Check if button is being entered or exited
            if (mouseColliding(button))
                buttonEntered();
            else
                buttonExited();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw our button
            button.customDrawButton(spriteBatch);

            if (PlayerInput.keyHeld(Keys.B))
                button.drawInfo(spriteBatch, Color.Red, Color.Green);
        }

    }
}
