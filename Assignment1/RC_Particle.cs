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
    public struct Particle
    {
        public Texture2D tex;
        public bool active;
        public Vector2 pos; //position
        public Vector2 vel; //velocity
        public Vector2 delta; // delta velocity
        public Vector2 startSize;
        public Vector2 endSize;
        public Color startColor;
        public Color endColor;
        public Rectangle bounds; // my bounds
        public int AgeInTicks;
        public int maxAgeInTicks;

        //float lerpVal; // for working storage
        public float displayAngle;
        public Vector2 oldPos; //position
        public Rectangle destRectangle;
    }

    public class ParticleSystem : RC_Renderable
    {
        Random rnd;
        public Texture2D tex { set; get; }
        public bool generation{ set; get; } // true if generation of particles allowed
        public Vector2 startSize{ set; get; }
        public Vector2 endSize{ set; get; }
        public Vector2 sysPos{ set; get; }      // in screen co-ordinates
        public Color startColor{ set; get; }
        public Color endColor{ set; get; }
         
        public int ticksSystemDuration{ set; get; } // can be -1 to make it last forever
        
        public Rectangle bounds { set; get; } // in screen co ordinates
        public Vector2 delta { set; get; } // basic delta
        public Vector2 randomDelta { set; get; } // randomise at creation of the particle 
        public Vector2 windDelta { set; get; }  // trainsient delta
        public Vector2 resistance {set; get;} // multiply V by this each turn
        public Vector2 initialVelosity {set; get;}
        public Vector2 initialVelosityRandom {set; get;} // how much variation (its added)

        public int Origin {set; get;} // 0= point origin 1=Rectangle origin 
                           //2=waypointlist Origin sequential 3=waypointlist Origin random
        public Rectangle originRectangle { set; get; }
        public WayPointList originWayList {set; get;}
        public int totalNumberToMake {set; get;} // -1 = go forever
        public int initialNumberToMake {set; get;}
        public int subsequentNumberToMake {set; get;}
        public int ticksBetweenMakeEvents {set; get;} // if -1 then Random only 
        public int random10000Tomake {set; get;} // each update turn random to see if this turn we make particls
        public int ticksParticleDuration {set; get;}
        public bool setDisplayAngle { set; get; }
        public float displayAngleOffset { set; get; } // in radians

        public float initalAngleLow = 0; // in degrees
        public float initalAngleHigh = 0; // in degrees
        public float initalVelocityLow = 0;
        public float initalVelocityHigh = 0;
        
        /// <summary>
        /// defines override particle movement 
        /// 0 = nix - just use other movement params  
        /// 1 = towards moveTowardsPos
        /// 2 = away from moveTowardsPos
        /// 3 = drift towards moveTowardsPos
        /// 4 = drift away from moveTowardsPos
        /// </summary>
        public int moveTowards { set; get; } 
        public Vector2 moveTowardsPos { set; get; }
        public float moveToDrift { set; get; }
        
        int totalNumberMade;        
        int particlecount;
        int maxNum; // size of list
        int ticks;
        int ticksForMakeEvents;
        int wayListCount;

        Particle[] particles;

        public ParticleSystem(Vector2 posZ, int MaxnumZ, int ticksSystemDurationZ, int randomSeed)
        {
            rnd = new Random(randomSeed);
            active = false;
            generation = false;
            particlecount=0;
            sysPos = posZ;
            //totalNumberToMake = totalNumberToMakeZ;
            maxNum = MaxnumZ;
            ticksSystemDuration=ticksSystemDurationZ;
            Origin = 0;
            windDelta = new Vector2(0, 0);   // trainsient delta
            randomDelta = new Vector2(0, 0);
            resistance = new Vector2(1, 1); // multiply V by this each turn
            setDisplayAngle = false;
            moveTowards = 0;
            setDisplayAngle = true;
            moveTowardsPos = new Vector2(0,0);
            moveToDrift = 1;

            displayAngleOffset = 0;
            
            particles = new Particle[maxNum];
            for (int i = 0; i < maxNum; i++)
            {
                particles[i] = new Particle();
                particles[i].active = false;
            }
            Reset();
        }

        public override void Reset()
        {
            active = true; 
            generation = true;
            ticks=0;
            particlecount=0;
            ticksForMakeEvents = 0;
            totalNumberMade = 0;
            wayListCount = 0;
            for (int i = 0; i < maxNum; i++)
            {
                particles[i].active = false;
            }
        }

        public void setMandatory1(Texture2D texZ, Vector2 startSizeZ, Vector2 endSizeZ, Color startColorZ, Color endColorZ)
        {
        tex = texZ;
        startSize = startSizeZ; 
        endSize = endSizeZ; 
        startColor = startColorZ;
        endColor = endColorZ;
        }

        public void setMandatory2(int totalNumberToMakeZ, int initialNumberToMakeZ, int subsequentNumberToMakeZ,
                                  int ticksBetweenMakeEventsZ, int random10000TomakeZ) // each update turn random to see if this turn we make particls
        {
        totalNumberToMake=totalNumberToMakeZ;
        initialNumberToMake=initialNumberToMakeZ;
        subsequentNumberToMake=subsequentNumberToMakeZ;
        random10000Tomake=random10000TomakeZ;
        ticksBetweenMakeEvents=ticksBetweenMakeEventsZ;
        }

        public void setMandatory3(int ticksParticleDurationZ,  Rectangle boundsZ )
        {
        ticksParticleDuration=ticksParticleDurationZ;
        bounds=boundsZ;
        }

        public void setMandatory4(Vector2 deltaZ, Vector2 initialVelosityZ, Vector2 initialVelosityRandomZ)
        {
        delta=deltaZ;
        initialVelosity=initialVelosityZ;
        initialVelosityRandom=initialVelosityRandomZ;
        }

        public void activate()
        {
            active = true;
            generation = true;
            makeParticles(initialNumberToMake);
        }

        public void deActivate()
        {
            active = false;
            active = false;
            visible = false;
            generation = false; 
        }

        public void ifSafeToRemoveDeActivate()
        {
            for (int i = 0; i < maxNum; i++)
            {
                //particles[i] = new Particle();
                if (particles[i].active)
                {
                    return;  // at least one particle still active
                }
            }
            active = false;
            visible = false;
            generation = false;
        }

        public void makeParticles(int num)
        {
            if (!generation || !active) return; //no more particles to generate
            if (particlecount >= maxNum) return; // no posibility of making particls
            int cnt = 0;
            Vector2 randy; // for varous random things 
            for (int i = 0; i < maxNum; i++)
            {
                //particles[i] = new Particle();
                if (!particles[i].active)
                {
                    // make one here
                 particles[i].tex=tex;
                 particles[i].active = true;
                 if (Origin==0)
                    {
                        particles[i].pos.X = sysPos.X; //position
                        particles[i].pos.Y = sysPos.Y; //position
                    }
                 if (Origin == 1)
                    {
                    particles[i].pos.X = originRectangle.X+originRectangle.Width*(float)rnd.NextDouble(); //position
                    particles[i].pos.Y = originRectangle.Y+originRectangle.Height*(float)rnd.NextDouble(); //position
                    }
                 if (Origin == 2)
                 {
                     WayPoint wp = originWayList.getWayPoint(originWayList.getCurrentLeg());
                     originWayList.nextLeg();
                     particles[i].pos.X = wp.pos.X; //position
                     particles[i].pos.Y = wp.pos.Y; //position
                 }
                 if (Origin == 3)
                 {
                     int q = rnd.Next(originWayList.lst.Count());
                     WayPoint wp = originWayList.getWayPoint(q);
                     particles[i].pos.X = wp.pos.X; //position
                     particles[i].pos.Y = wp.pos.Y; //position
                 }

                particles[i].vel = initialVelosity; //velocity
                // add random bit
                randy = (new Vector2((float)rnd.NextDouble(), (float)rnd.NextDouble()) * initialVelosityRandom - (initialVelosityRandom / 2));
                particles[i].vel = particles[i].vel + randy;   
                particles[i].delta = delta + (randomDelta * new Vector2((float)rnd.NextDouble(), (float)rnd.NextDouble()) - randomDelta / 2);
                
                if (initalAngleHigh != 0)
                    {
                    //set a radial velocity
                        float angle = rnd.Next((int)initalAngleLow, (int)initalAngleHigh);
                        float velocity = rnd.Next((int)(initalVelocityLow * 100), (int)(initalVelocityHigh * 100))/100.0f;

                        Vector2 v = Util.moveByAngleDist(new Vector2(0, 0), Util.degToRad(angle), velocity);
                        particles[i].vel = v;
                    }

                particles[i].startSize=startSize;
                particles[i].endSize=endSize;
                particles[i].startColor=startColor;
                particles[i].endColor=endColor;
                particles[i].bounds=bounds; // my bounds
                particles[i].AgeInTicks=0;
                particles[i].maxAgeInTicks = ticksParticleDuration;
                particles[i].destRectangle.X = (int)particles[i].pos.X;
                particles[i].destRectangle.Y = (int)particles[i].pos.Y;
                particles[i].destRectangle.Width = (int)particles[i].startSize.X;
                particles[i].destRectangle.Height = (int)particles[i].startSize.Y;
                
                if (moveTowards == 1)
                {
                    // 0= nix  1= towards moveTowardsPos
                    particles[i].vel = ((moveTowardsPos-particles[i].pos) / (float)ticksParticleDuration) + randy;
                    particles[i].delta = new Vector2(0, 0); // usually you want this
                }

                if (moveTowards == 2)
                {
                    // 0= nix  1= towards moveTowardsPos
                    Vector2 v = (particles[i].pos - moveTowardsPos); 
                    v.Normalize();
                    particles[i].vel = (v*moveToDrift) + randy;
                    particles[i].delta = new Vector2(0, 0); // usually you want this
                }


                cnt++;
                particlecount++;
                totalNumberMade++;
                if (totalNumberMade >= totalNumberToMake) return;
                if (cnt >= num) return; // done them all
                
                if (particlecount >= maxNum) return; // no more possible
                }
            }

        }

        /// <summary>
        /// tests if any active particle contains the point
        /// Returns -1 if it misses all particles 
        /// otherwise it returns the particle number
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public int Contains(Vector2 point)
        {
            int retv = -1;
            for (int i = 0; i < maxNum; i++)
            {
                if (particles[i].active)
                {
                    if (particles[i].destRectangle.Contains(new Point((int) point.X, (int) point.Y)))
                    {
                        return i;
                    }
                }
            }
            return retv;
        }

        public void setWayListCount(int wlc)
        {
            wayListCount= wlc;
        }

        public override void Update(GameTime gameTime)
        {
        if (!active) return;
        ticks++;

        //Update the particles;
        updateParticles();

        // Should I exist
        if (ticks > ticksSystemDuration && ticksSystemDuration > 0)
            {
                generation = false;
                ifSafeToRemoveDeActivate();
                return;
            }

        if (totalNumberMade >= totalNumberToMake && totalNumberToMake > 0)
        {
            generation = false;
            ifSafeToRemoveDeActivate();
            return;
        }

        // make more particles ?
        ticksForMakeEvents++;
        if (ticksForMakeEvents >= ticksBetweenMakeEvents && ticksBetweenMakeEvents > 0)
            {
                ticksForMakeEvents = 0;
                makeParticles(subsequentNumberToMake);
            }

        if (rnd.Next(10000) < random10000Tomake)
            {  
            ticksForMakeEvents = 0;
            makeParticles(subsequentNumberToMake);
            }

        }

        public void updateParticles()
        {            
            if (!active) return;
            if (particlecount <= 0) return;

            for (int i = 0; i < maxNum; i++)
            {
                //particles[i] = new Particle();
                if (particles[i].active)
                {
                    particles[i].AgeInTicks++;
                    if (particles[i].AgeInTicks > particles[i].maxAgeInTicks)
                    {
                        particles[i].active = false;
                        particlecount--;
                    }
                    particles[i].vel = particles[i].vel + particles[i].delta;
                    particles[i].vel = particles[i].vel + windDelta;
                    particles[i].vel = particles[i].vel * resistance;

                    if (moveTowards == 3)
                    {
                        // 3 = towards moveTowardsPos
                        Vector2 v = (moveTowardsPos - particles[i].pos);
                        v.Normalize();
                        particles[i].vel = particles[i].vel+(v * moveToDrift);
                    }

                    if (moveTowards == 4)
                    {
                        // 4 = away from moveTowardsPos
                        Vector2 v = (particles[i].pos - moveTowardsPos);
                        v.Normalize();
                        particles[i].vel = particles[i].vel+(v * moveToDrift);
                    }


                    // CUSTOM
                    particles[i].vel *= 0.99f;

                    //particles[i].vel = particles[i].vel + (randomDelta*new Vector2((float)rnd.NextDouble(), (float)rnd.NextDouble())-randomDelta/2);
                    particles[i].oldPos.X = particles[i].pos.X;
                    particles[i].oldPos.Y = particles[i].pos.Y;
                    particles[i].pos = particles[i].pos + particles[i].vel;
                    if (setDisplayAngle)
                    {
                        float a = Util.getAngle(particles[i].pos, particles[i].oldPos);
                        particles[i].displayAngle = a+displayAngleOffset;
                    }                    
                    //if (particles[i].bounds != null)
                    //{
                        if (!particles[i].bounds.Contains(new Point((int)particles[i].pos.X,(int)particles[i].pos.Y))) 
                        {
                        particles[i].active = false;
                        particlecount--;
                        }
                    //}




                }
            }
        }

        public override void Draw(SpriteBatch sp)
        {
            if (!active) return;
            if (particlecount <= 0) return;

            for (int i = 0; i < maxNum; i++)
            {
                //particles[i] = new Particle();
                if (particles[i].active)
                {
                    float lerpv = (float)particles[i].AgeInTicks / (float)particles[i].maxAgeInTicks;
                    Color c = Color.Lerp(particles[i].startColor, particles[i].endColor, lerpv);
                    Vector2 siz = new Vector2(MathHelper.Lerp(particles[i].startSize.X,particles[i].endSize.X,lerpv),MathHelper.Lerp(particles[i].startSize.Y,particles[i].endSize.Y,lerpv));
                    Rectangle dest = new Rectangle((int)particles[i].pos.X,(int)particles[i].pos.Y, (int)siz.X, (int)siz.Y);
                    particles[i].destRectangle = dest;
                    if (setDisplayAngle)
                    {
                        //sp.Draw(particles[i].tex, dest, c);
                        sp.Draw(particles[i].tex, dest, null, c, particles[i].displayAngle,
                                new Vector2(tex.Width/2, tex.Height/2), SpriteEffects.None, 0.5f);
                    }
                    else
                    {
                        //sp.Draw(particles[i].tex, dest, c);                        
                        sp.Draw(particles[i].tex, dest, null, c, particles[i].displayAngle,
                                new Vector2(tex.Width/2, tex.Height/2), SpriteEffects.None, 0.5f);

                    }
                }
            }

        }

    }

}
