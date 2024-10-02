using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace SpaceShooter
{
    // Main
    public class Enemy
    {
        // e is for enemy 
        public Rectangle eBoundingBox;
        public Texture2D eTexture, eBulletTexture;
        public Vector2 ePosition;
        public int eSpeed, eHealth, eBulletDelay;
        public bool eIsVisible;
        public List<Bullet> eBulletList;

        // Constructor
        public Enemy(Texture2D _newTexture, Vector2 _newPosition, Texture2D _newBulletTexture)
        {
            eBulletList = new List<Bullet>();
            eTexture = _newTexture;
            eBulletTexture = _newBulletTexture;
            eHealth = 5;    
            ePosition = _newPosition;
            eBulletDelay = 60;
            eSpeed = 5;
            eIsVisible = true;
        }

        // Update 
        public void Update(GameTime _gameTime)
        {
            // Update collison rectangle
            eBoundingBox = new Rectangle((int)ePosition.X, (int)ePosition.Y, eTexture.Width, eTexture.Height);

            // Update enemy movement
            ePosition.Y += eSpeed;

            // Move enemy back to top of the screen if it flies of bottom
            if(ePosition.Y >= 950)
            {
                ePosition.Y = -75;
            }

            UpdateEnemyBullets();
            EnemyShoot();
        }

        // Draw
        public void Draw(SpriteBatch _spriteBatch)
        {
            // Draw enemy Tie Fighter   
            _spriteBatch.Draw(eTexture, ePosition, Color.White);

            // Draw enemy bullets
            foreach(Bullet eB in eBulletList)
            {
                eB.Draw(_spriteBatch);
            }
        }

        // Update bullets
        public void UpdateEnemyBullets()
        {
            // For each bullet in our bulletList: update the movement and if the bullet hits the top of the sscreen remove it from the list.
            foreach (Bullet eB in eBulletList)
            {
                // Collider for every bullet in the bulletlist
                eB.boundingBox = new Rectangle((int)eB.position.X, (int)eB.position.Y, eB.bulletTexture.Width, eB.bulletTexture.Height);
                // Update bullet position
                eB.position.Y = eB.position.Y + eB.speed;

                // If the bullet hits the top of the screen, then make visible false
                if (eB.position.Y >= 950)
                {
                    eB.isVisible = false;
                }
            }
            // If any of the bullets in the list are not visible if they aren't then remove the bullet from the list
            for (int i = 0; i < eBulletList.Count(); i++)
            {
                if (!eBulletList[i].isVisible)
                {
                    eBulletList.RemoveAt(i);
                    i--;
                }
            }
        }

        // Enemy shooting
        public void EnemyShoot()
        {
            // Shoot only if bullet delay resets, just like the player
            if(eBulletDelay >= 0)
            {
                eBulletDelay--;
            }
            if(eBulletDelay <= 0)
            {
                // Create new bullet and position it front and center of the enemy
                Bullet newBullet = new Bullet(eBulletTexture);
                newBullet.position = new Vector2(ePosition.X + eTexture.Width / 2 - newBullet.bulletTexture.Width / 2, ePosition.Y + 30);

                newBullet.isVisible = true;

                if(eBulletList.Count() <= 20)
                {
                    eBulletList.Add(newBullet);
                }
            }
            // Reset enemy bullet delay
            if(eBulletDelay == 0)
            {
                eBulletDelay = 60;
            }
        }
    }
}
