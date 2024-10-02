using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Xml.Serialization;

namespace SpaceShooter
{
    // Main
    public class HUD
    {
        public int playerScore, screenWidth, screenHeight;
        public SpriteFont playerScoreFont;
        public Vector2 playerScorePos;
        public bool showHud;

        // Constructor
        public HUD()
        {
            playerScore = 0;
            showHud = true;
            screenWidth = 800;
            screenHeight = 950;
            playerScoreFont = null;
            playerScorePos = new Vector2(screenWidth - 450, 45);
        }

        // Load content
        public void LoadContent(ContentManager _content)
        {
            playerScoreFont = _content.Load<SpriteFont>("Fonts/georgia");
        }

        // Update
        public void Update(GameTime _gameTime)
        {
            // Get keyboard state
            KeyboardState keyState = Keyboard.GetState();
        }

        // Draw
        public void Draw(SpriteBatch _spriteBatch)
        {
            // If showhud = true then display the hud
            if (showHud)
            {
                _spriteBatch.DrawString(playerScoreFont, "Score - " + playerScore, playerScorePos, Color.Red);
            }
        }
    }
}
