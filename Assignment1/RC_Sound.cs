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

#pragma warning disable 1591 //sadly not yet fully commented

namespace RC_Framework
{
    // *********************************** LimitSound ******************************************************

    /// <summary>
    /// Limits a sound to playing a certian number of instances of itself (stops muddy sound)
    /// Warning this needs a call in Update
    /// </summary>
    public class LimitSound
    {
        SoundEffect se;
        SoundEffectInstance[] sei;
        int numOfSounds;
        int counter;
        int soundsThisTick=0;

        /// <summary>
        /// Create it using a soundeffect
        /// </summary>
        /// <param name="soundEffect"></param>
        /// <param name="numOfSoundz"></param>
        public LimitSound(SoundEffect soundEffect, int numOfSoundz)
        {
            numOfSounds = numOfSoundz + 1;
            se = soundEffect;
            init();
        }

        /// <summary>
        /// create it using a sound name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="numOfSoundz"></param>
        /// <param name="c"></param>
        public LimitSound(string name, int numOfSoundz, ContentManager c)
        {
            se = c.Load<SoundEffect>(name);
            numOfSounds = numOfSoundz + 1;
            init();
        }

        private void init()
        {
            soundsThisTick=0;
            counter = 0;
            sei = new SoundEffectInstance[numOfSounds];
            for (int i = 0; i < numOfSounds; i++)
            {
                sei[i] = se.CreateInstance();
            }
        }

        /// <summary>
        /// play the sound from the start
        /// </summary>
        public void playSound()
        {
            if (soundsThisTick >= numOfSounds) return;
            sei[counter].Play(); // play it
            soundsThisTick++;
            counter++;
            if (counter >= numOfSounds) counter = 0;
            sei[counter].Stop(); // stop one for next play
        }

        /// <summary>
        /// Play it only if one is not already playing
        /// </summary>
        public void playSoundIfOk()
        {
            // first find one not playing
            for (int i = 0; i < numOfSounds - 1; i++)
            {
                if (sei[i].State == SoundState.Stopped)
                {
                    sei[i].Play();
                    return;
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            soundsThisTick = 0;
        }
             
    }


}