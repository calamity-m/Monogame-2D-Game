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

    public class ParticleManager
    {
        // Internal particle system list
        private List<ParticleSystem> particleSystems = new List<ParticleSystem>();

        // Max amounts of particles
        private int maxParticleAmount;

        // How long each created system will last
        private int systemDurationTicks;

        private Random rand = new Random();

        /// <summary>
        /// Create particle manager
        /// </summary>
        /// <param name="maxParticleAmount">max amount of particles to be created</param>
        /// <param name="systemDurationTicks">each created system's duration</param>
        public ParticleManager(int maxParticleAmount, int systemDurationTicks)
        {
            this.maxParticleAmount = maxParticleAmount;
            this.systemDurationTicks = systemDurationTicks;
        }

        /// <summary>
        /// Create an enemy explosion type
        /// </summary>
        /// <param name="particle">particle texture</param>
        /// <param name="particleAmount">amount of particles</param>
        /// <param name="durationTicks">duration of particles</param>
        /// <param name="pos">spawn position</param>
        public void CreateExplosionEnemy(Texture2D particle, int particleAmount, int durationTicks, Vector2 pos)
        {
            ParticleSystem particleSystem = new ParticleSystem(pos, maxParticleAmount, systemDurationTicks, rand.Next());
            float hue1 = HelperUtils.RandFloat(rand, 0, 6);
            float hue2 = (hue1 + HelperUtils.RandFloat(rand, 0, 2)) % 6f;
            Color startColor = HelperUtils.HSVToColor(hue1, 0.5f, 1);
            Color endColor = new Color(HelperUtils.HSVToColor(hue2, 0.5f, 1), 0.3f);
            particleSystem.setMandatory1(particle, new Vector2(6, 2), new Vector2(8, 3), startColor, endColor);
            particleSystem.setMandatory2(70, 65, 5, 0, 0);
            particleSystem.setMandatory3(durationTicks, new Rectangle(0, 0, (int)Game1.screenSize.X, (int)Game1.screenSize.Y));
            particleSystem.setMandatory4(new Vector2(0, 0), new Vector2(1, 0), new Vector2(0, 0));
            particleSystem.initalAngleLow = 0.1f;
            particleSystem.initalAngleHigh = 360f;
            particleSystem.initalVelocityLow = 2f;
            particleSystem.initalVelocityHigh = 3.8f;
            particleSystem.activate();

            particleSystems.Add(particleSystem);
        }

        /// <summary>
        /// Create a boss explosion type
        /// </summary>
        /// <param name="particle">particle texture</param>
        /// <param name="particleAmount">amount of particles</param>
        /// <param name="durationTicks">duration of particles</param>
        /// <param name="pos">spawn position</param>
        public void CreateExplosionBoss(Texture2D particle, int particleAmount, int durationTicks, Vector2 pos)
        {
            ParticleSystem particleSystem = new ParticleSystem(pos, maxParticleAmount, systemDurationTicks, rand.Next());
            float hue1 = HelperUtils.RandFloat(rand, 0, 6);
            float hue2 = (hue1 + HelperUtils.RandFloat(rand, 0, 2)) % 6f;
            Color startColor = HelperUtils.HSVToColor(hue1, 0.5f, 1);
            Color endColor = new Color(HelperUtils.HSVToColor(hue2, 0.5f, 1), 0.3f);
            particleSystem.setMandatory1(particle, new Vector2(8, 3), new Vector2(11, 5), startColor, endColor);
            particleSystem.setMandatory2(150, 70, 50, 25, 0);
            particleSystem.setMandatory3(durationTicks, new Rectangle(0, 0, (int)Game1.screenSize.X, (int)Game1.screenSize.Y));
            particleSystem.setMandatory4(new Vector2(0, 0), new Vector2(1, 0), new Vector2(0, 0));
            particleSystem.initalAngleLow = 0.1f;
            particleSystem.initalAngleHigh = 360f;
            particleSystem.initalVelocityLow = 2f;
            particleSystem.initalVelocityHigh = 3.8f;
            particleSystem.activate();

            particleSystems.Add(particleSystem);
        }

        public void setMaxParticleAmount(int amount)
        {
            switch (Resources.graphicsQuality)
            {
                case 0:
                    amount = MathHelper.Clamp(amount, 0, 0);
                    break;
                case 1:
                    amount = MathHelper.Clamp(amount, 0, 350);
                    break;
                case 2:
                    break;
            }
            Console.WriteLine("Amount: " + amount);
            maxParticleAmount = amount;
        }

        /// <summary>
        /// Clear the scene/manager of any active particles
        /// </summary>
        public void ClearScene()
        {
            foreach (ParticleSystem p in particleSystems)
            {
                p.active = false;
            }

        }

        public void Update(GameTime gameTime)
        {
            /*
            if (Resources.graphicsQuality == 0)
            {
                particleSystems = particleSystems.Where(p => p.active).ToList();
                return;
            }*/

            // Update our particle systems/generators
            foreach (ParticleSystem p in particleSystems)
            {
                p.Update(gameTime);
            }


            particleSystems = particleSystems.Where(p => p.active).ToList();

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Resources.graphicsQuality == 0)
                return;

            // Draw our particle systems/generators
            foreach (ParticleSystem p in particleSystems)
            {
                p.Draw(spriteBatch);
            }
        }


    }
}
