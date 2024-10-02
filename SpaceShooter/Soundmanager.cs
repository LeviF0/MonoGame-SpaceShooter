using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace SpaceShooter
{
    // Main
    public class Soundmanager
    {
        public SoundEffect playerShoot, explodeSound, hit, alarm;
        public Song bgMusic, menuMusic;

        // Constructor
        public Soundmanager()
        {
            playerShoot = null;
            explodeSound = null;
            hit = null;
            alarm = null;
            bgMusic = null;
            menuMusic = null;
        }

        public void LoadContent(ContentManager _content)
        {
            playerShoot = _content.Load<SoundEffect>("Sfx/playerShoot");
            explodeSound = _content.Load<SoundEffect>("Sfx/explosion");
            hit = _content.Load<SoundEffect>("Sfx/hit");
            alarm = _content.Load<SoundEffect>("Sfx/alarm");
            bgMusic = _content.Load<Song>("Music/bgMusic");
            menuMusic = _content.Load<Song>("Music/menuMusic");
            MediaPlayer.IsRepeating = true;
        }
    }
}
