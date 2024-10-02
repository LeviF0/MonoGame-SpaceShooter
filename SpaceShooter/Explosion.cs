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
    public class Explosion
    {
        public Texture2D explosionTexture;
        public Vector2 position, origin;
        public float timer, interval;
        public int currentFrame, spriteWidth, spriteHeight;
        public Rectangle sourceRect;
        public bool isVisible;

        // Constructor
        public Explosion(Texture2D _newTexture, Vector2 _newPosition)
        {
            position = _newPosition;
            explosionTexture = _newTexture;
            timer = 0f;
            interval = 20f;
            currentFrame = 1;
            spriteWidth = 90;
            spriteHeight = 90;
            isVisible = true;
        }

        // Load content
        public void LoadContent(ContentManager _content)
        {

        }

        // Update
        public void Update(GameTime _gameTime)
        {
            // Increase timer by the number of miliseconds since update was last called  
            timer += (float)_gameTime.ElapsedGameTime.TotalMilliseconds;

            // Check the timer is more than the chosen interval
            if (timer > interval)
            {
                // Show next frame
                currentFrame++;
                // Reset timer
                timer = 0;
            }

            // If on the last frame, make the explosion invisible and reset currentFrame to beginning of sprite sheet
            if (currentFrame == 9)
            {
                currentFrame = 0;
                isVisible = false;
            }

            sourceRect = new Rectangle(currentFrame * spriteWidth, 0, spriteWidth, spriteHeight);
            origin = new Vector2(sourceRect.Width / 3, sourceRect.Height / 4);
        }

        // Draw
        public void Draw(SpriteBatch _spriteBatch)
        {
            // if visible then draw
            if (isVisible == true)
            {
                _spriteBatch.Draw(explosionTexture, position, sourceRect, Color.White, 0f, origin, 1.0f, SpriteEffects.None, 0);
            }
        }
    }
}
