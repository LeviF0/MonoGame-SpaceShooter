using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.IO;
using System.Net.NetworkInformation;

namespace SpaceShooter
{
    // Main
    public class Player
    {
        public Texture2D falconTexture, bulletTexture, healthTexture;
        public Vector2 position, healthbarPos;
        public int speed, health;

        // Collision variables
        public Rectangle boundingBox, healthRectangle;
        public bool isColliding;

        // Bullet variables
        public List<Bullet> bulletList;
        public float bulletDelay;

        // Sfx variables
        Soundmanager sm = new Soundmanager();

        // Constructor
        public Player()
        {
            falconTexture = null;
            position = new Vector2(375, 900);
            speed = 10;
            isColliding = false;
            bulletList = new List<Bullet>();
            bulletDelay = 1;
            healthbarPos = new Vector2(50, 50);
            health = 200;
        }

        // Load content
        public void LoadContent(ContentManager _content)
        {
            falconTexture = _content.Load<Texture2D>("Sprites/falcon");
            bulletTexture = _content.Load<Texture2D>("Sprites/bullet");
            healthTexture = _content.Load<Texture2D>("Sprites/health");
            sm.LoadContent(_content);
        }

        // Draw 
        public void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(falconTexture, position, Color.White);
            _spriteBatch.Draw(healthTexture, healthRectangle, Color.White);
            foreach (Bullet b in bulletList)
            {
                b.Draw(_spriteBatch);
            }
        }

        // Update
        public void Update(GameTime _gameTime)
        {
            // Getting keyboard state
            KeyboardState keyState = Keyboard.GetState();

            // Bounding box for the player 
            boundingBox = new Rectangle((int)position.X, (int)position.Y, falconTexture.Width, falconTexture.Height);

            // Set rectangle for the health bar
            healthRectangle = new Rectangle((int)healthbarPos.X, (int)healthbarPos.Y, health, 25);

            // Fire bullets
            if (keyState.IsKeyDown(Keys.Space))
            {
                Shoot();
            }
            UpdateBullets();

            // Ship controls
            if (keyState.IsKeyDown(Keys.W))
            {
                position.Y = position.Y - speed;
            }
            if (keyState.IsKeyDown(Keys.A))
            {
                position.X = position.X - speed;
            }
            if (keyState.IsKeyDown(Keys.S))
            {
                position.Y = position.Y + speed;
            }
            if (keyState.IsKeyDown(Keys.D))
            {
                position.X = position.X + speed;
            }

            // Keep player ship in screen bounds
            if (position.X <= 0)
            {
                position.X = 0;
            }
            if (position.X >= 800 - falconTexture.Width)
            {
                position.X = 800 - falconTexture.Width;
            }
            if (position.Y <= 0)
            {
                position.Y = 0;
            }
            if (position.Y >= 950 - falconTexture.Height)
            {
                position.Y = 950 - falconTexture.Height;
            }
        }

        // Shoot method: used to set starting position of bullets
        public void Shoot()
        {
            // Shoot only if the bullet delay resets
            if (bulletDelay >= 0)
            {
                sm.playerShoot.Play();
                bulletDelay--;
            }

            // If the bullet delay is at 0:
            // create a new bullet at the player's position, make it visible on the screen, then add that bullet to the list
            if (bulletDelay <= 0)
            {
                Bullet newBullet = new Bullet(bulletTexture);
                newBullet.position = new Vector2(position.X + 32 - newBullet.bulletTexture.Width / 2, position.Y + 30);

                // Making the bullet visible
                newBullet.isVisible = true;

                // Counts the amount of bullets in the list
                if (bulletList.Count() < 20)
                {
                    bulletList.Add(newBullet);
                }

            }
            // Reset bullet delay
            if (bulletDelay <= 0)
            {
                bulletDelay = 10;
            }
        }

        // Update bullets
        public void UpdateBullets()
        {
            // For each bullet in our bulletList: update the movement and if the bullet hits the top of the sscreen remove it from the list.
            foreach (Bullet b in bulletList)
            {
                // Collider for every bullet in the bulletlist
                b.boundingBox = new Rectangle((int)b.position.X, (int)b.position.Y, b.bulletTexture.Width, b.bulletTexture.Height);
                // Update bullet position
                b.position.Y = b.position.Y - b.speed;

                // If the bullet hits the top of the screen, then make visible false
                if (b.position.Y <= 0)
                {
                    b.isVisible = false;
                }
            }
            // If any of the bullets in the list are not visible if they aren't then remove the bullet from the list
            for (int i = 0; i < bulletList.Count(); i++)
            {
                if (!bulletList[i].isVisible)
                {
                    bulletList.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}
