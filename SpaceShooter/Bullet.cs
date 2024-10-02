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
    public class Bullet
    {
        public Rectangle boundingBox;
        public Texture2D bulletTexture;
        public Vector2 origin;
        public Vector2 position;
        public bool isVisible;
        public float speed;

        public Bullet(Texture2D _newTexture)
        {
            speed = 10f;
            bulletTexture = _newTexture;
            isVisible = false;
        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(bulletTexture, position, Color.White);
        }
    }
}
