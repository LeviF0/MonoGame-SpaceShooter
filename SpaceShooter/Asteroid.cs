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
    public class Asteroid
    {
        public Rectangle boundingBox;
        public Texture2D asteroidTexture;
        public Vector2 position;
        public Vector2 origin;
        public float rotationAngle;
        public int speed;

        public bool isVisible;
        Random random = new Random();
        public float randX, randY; // Randy hehehe

        // Constructor
        public Asteroid(Texture2D _newTexture, Vector2 _newPosition)
        {
            position = _newPosition;
            asteroidTexture = _newTexture;
            speed = 4;
            isVisible = true;

            randX = random.Next(0, 750);
            randY = random.Next(-600, -50);
        }

        // Update
        public void Update(GameTime _gameTime)
        {
            // Set bounding box for collision which draws an invisible box around the asteroid
            boundingBox = new Rectangle((int)position.X, (int)position.Y, 45, 45);

            // Update asteroid movement
            position.Y = position.Y + speed;
            if (position.Y >= 950)
            {
                position.Y = -50;
            }
        }

        // Draw
        public void Draw(SpriteBatch _spriteBatch)
        {
            if (isVisible)
            {
                _spriteBatch.Draw(asteroidTexture, position, Color.White);
                //_spriteBatch.Draw(asteroidTexture, position, null, Color.White, rotationAngle, origin, 1.0f, SpriteEffects.None, 0f);
            }
        }
    }
}
