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
    public class Starfield
    {
        public Texture2D backgroundTexture;
        public Vector2 bgPos1, bgPos2;
        public int speed;

        // Constructor
        public Starfield()
        {
            backgroundTexture = null;
            bgPos1 = new Vector2(0, 0);
            bgPos2 = new Vector2(0, -950);
            speed = 15;
        }

        // Load content
        public void LoadContent(ContentManager _content)
        {
            backgroundTexture = _content.Load<Texture2D>("Sprites/space");
        }

        // Draw
        public void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(backgroundTexture, bgPos1, Color.White);
            _spriteBatch.Draw(backgroundTexture, bgPos2, Color.White);
        }

        // Update
        public void Update(GameTime _gameTime)
        {
            // Setting the speed for the backround scrolling
            bgPos1.Y = bgPos1.Y + speed;
            bgPos2.Y = bgPos2.Y + speed;

            // Scroll background (repeating)
            if (bgPos1.Y >= 950)
            {
                bgPos1.Y = 0;
                bgPos2.Y = -950;
            }
        }
    }
}
