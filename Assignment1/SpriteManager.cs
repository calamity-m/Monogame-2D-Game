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
    public class SpriteManager
    {
        // Internal handling lists
        private List<SpriteActor> sprites = new List<SpriteActor>();
        private List<Bullet> bullets = new List<Bullet>();
        private List<Enemy> enemies = new List<Enemy>();
        private List<Pickup> pickups = new List<Pickup>();
        private bool updating;
        private List<SpriteActor> tempList = new List<SpriteActor>();
        public Player player = null;
        // Silly idea, but works for a game with only one boss
        public Boss boss = null;

        public int count
        {
            get
            {
                return sprites.Count;
            }
        }

        /// <summary>
        /// Add a sprite actor to the sprite manager to later be updated and drawn
        /// </summary>
        /// <param name="sprite">SpriteActor object to be added</param>
        public void addSpriteActor(SpriteActor sprite)
        {
            // If we're currently working on our list of sprites, add them to a temp
            if (updating)
                tempList.Add(sprite);
            else
                addInternal(sprite);
        }

        // Internal handlnig of adding sprite
        private void addInternal(SpriteActor sprite)
        {
            sprites.Add(sprite);
            if (sprite is Bullet)
                bullets.Add(sprite as Bullet);
            else if
                (sprite is Enemy)
                enemies.Add(sprite as Enemy);
            else if (sprite is Pickup)
                pickups.Add(sprite as Pickup);
            else if (sprite is Player)
                player = sprite as Player;

            if (sprite is Enemy)
            {
                if (sprite is Boss)
                    boss = sprite as Boss;
            }
        }

        /// <summary>
        /// Handle all collisions between sprite actors
        /// </summary>
        public void handleCollision()
        {

            // Handle collision between all types of enemies
            for (int i = 0; i < enemies.Count; i++)
            {
                // For each enemy, check if they collide with another enemy
                for (int j = 0; j < enemies.Count; j++)
                {
                    if (isColliding(enemies[i], enemies[j]))
                    {
                        enemies[i].handleCollision(enemies[j]);
                        enemies[j].handleCollision(enemies[i]);
                    }
                }
            }

            // Handle collision between bullets and enemies
            for (int i = 0; i < bullets.Count; i++)
            {
                // if our source is enemy, then ignore any collisions between enemy bullets and enemies
                if (bullets[i].source is Enemy)
                    continue;

                for (int j = 0; j < enemies.Count; j++)
                {
                    if (isColliding(bullets[i], enemies[j]))
                    {
                        enemies[j].Destroy(true);
                        bullets[i].Destroy();
                    }
                }
            }

            // Handle collision between bullets and bullets
            for (int i = 0; i < bullets.Count; i++)
            {
                for (int j = 0; j < bullets.Count; j++)
                {
                    // Lets not collide with bullets from same sources
                    if (bullets[i].source is Player || bullets[j].source is Enemy)
                        continue;

                    if (isColliding(bullets[i], bullets[j]))
                    {
                        bullets[i].Destroy();
                    }
                }
            }

            // Handle collision between player and pickup
            if (player != null)
            {
                for (int i = 0; i < pickups.Count; i++)
                {
                    if (isColliding(pickups[i], player))
                    {
                        pickups[i].Destroy();
                        switch (pickups[i].type)
                        {
                            case PickUpType.HealthPack:
                                player.ConsumeHPPack(1);
                                break;
                            case PickUpType.WeaponPowerUp:
                                player.ConsumeWeaponPack();
                                break;
                            case PickUpType.SpeedPowerUp:
                                player.ConsumeSpeedPack(9);
                                break;
                        }
                    }
                }
            }

            // Handle collision between player and enemies
            if (player != null)
            {
                // If we're cheating, just exit our collision detection
                if (Resources.cheatingFlag)
                    return;

                for (int i = 0; i < enemies.Count; i++)
                {
                    // If our player is colliding with an enemy, apply damage and clear enemies, etc.
                    if (enemies[i].sprite.visible && isColliding(enemies[i], player))
                    {
                        player.PlayerHit(enemies[i].damage);
                        foreach (Enemy e in enemies)
                        {
                            e.Destroy(false);
                        }
                        foreach (Bullet b in bullets)
                        {
                            b.Destroy();
                        }
                        foreach (Pickup p in pickups)
                        {
                            p.Destroy();
                        }
                        Game1.particleManager.ClearScene();
                    }
                }
            }

            // Handle collision between player and enemy bullets
            if (player != null)
            {
                // If we're cheating, just exit our collision detection
                if (Resources.cheatingFlag)
                    return;

                for (int i = 0; i < bullets.Count; i++)
                {
                    if (bullets[i].source is Enemy)
                    {
                        // if our player is colliding with an enemy bullet, apply damage and clear enemies, etc.
                        if (isColliding(bullets[i], player))
                        {

                            foreach (Enemy e in enemies)
                            {
                                e.Destroy(false);
                            }
                            foreach (Bullet b in bullets)
                            {
                                b.Destroy();
                            }
                            foreach (Pickup p in pickups)
                            {
                                p.Destroy();
                            }
                            player.PlayerHit(1);
                            Game1.particleManager.ClearScene();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Clear scene of all sprites except player
        /// </summary>
        /// <param name="destroyBullets">Bullets destroyed y/n</param>
        public void clearScene(bool destroyBullets)
        {
            foreach (Enemy e in enemies)
            {
                e.Destroy(false);
            }

            if (destroyBullets)
            {
                foreach (Bullet b in bullets)
                {
                    b.Destroy();
                }
            }

            foreach (Pickup p in pickups)
            {
                p.Destroy();
            }
            
            sprites = sprites.Where(x => !x.isFinished).ToList();
            
        }

        // Checks if two sprites are colliding, uses internal RC_Framework collision handling
        private bool isColliding(SpriteActor s1, SpriteActor s2)
        {
            return s1.sprite.collision(s2.sprite);
        }

        public void Update()
        {
            updating = true;

            handleCollision();

            foreach (var sprite in sprites)
                sprite.Update();

            updating = false;

            // Wipe temporary list and let sprites be called on next frame
            foreach (var sprite in tempList)
                addInternal(sprite);

            //sprites.Add(sprite);

            tempList.Clear();

            // Remove finished/inactive sprites
            sprites = sprites.Where(x => !x.isFinished).ToList();
            bullets = bullets.Where(x => !x.isFinished).ToList();
            enemies = enemies.Where(x => !x.isFinished).ToList();
            pickups = pickups.Where(x => !x.isFinished).ToList();

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var sprite in sprites)
                sprite.Draw(spriteBatch);
        }
    }
}
