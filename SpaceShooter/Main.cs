using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using static System.Formats.Asn1.AsnWriter;
using System.Net.NetworkInformation;
using System.Reflection.Metadata;
using Microsoft.Xna.Framework.Media;

namespace SpaceShooter
{
    // Main
    public class Main : Game
    {
        // State enum
        public enum State
        {
            Menu,
            Playing,
            Gameover,
            Win
        }

        public GraphicsDeviceManager _graphics;
        public SpriteBatch _spriteBatch;
        Random random = new Random();
        public Texture2D menuImage, gameoverImage, winScreen;

        // Lists
        List<Asteroid> asteroidList = new List<Asteroid>();
        List<Enemy> enemyList = new List<Enemy>();
        List<Explosion> explosionList = new List<Explosion>();

        // Instantiating the Player and Starfield objects
        Player p = new Player();
        Starfield sf = new Starfield();
        HUD hud = new HUD();
        Soundmanager sm = new Soundmanager();

        // Set first state
        State gameState = State.Menu;

        // Constructor
        public Main()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            _graphics.IsFullScreen = false;
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 950;
            Window.Title = "2D Space Shooter";
            IsMouseVisible = true;
            menuImage = null;
            gameoverImage = null;
            winScreen = null;
        }

        // Init
        protected override void Initialize()
        {
            base.Initialize();
        }

        // Load content
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            p.LoadContent(Content);
            hud.LoadContent(Content);
            sf.LoadContent(Content);
            sm.LoadContent(Content);
            menuImage = Content.Load<Texture2D>("Sprites/menuBackground");
            gameoverImage = Content.Load<Texture2D>("Sprites/gameoverImage");
            winScreen = Content.Load<Texture2D>("Sprites/winScreen");
            MediaPlayer.Play(sm.menuMusic);
        }

        // Update
        protected override void Update(GameTime _gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Updating player state
            switch (gameState)
            {
                case State.Playing:
                    // Updating enemy's and checking collision of the enemyship to playership
                    foreach (Enemy e in enemyList)
                    {
                        // Check if enemy ship is colliding with player
                        if (e.eBoundingBox.Intersects(p.boundingBox))
                        {
                            explosionList.Add(new Explosion(Content.Load<Texture2D>("Sprites/explosion"), new Vector2(p.position.X, p.position.Y)));
                            // Play the hit sound
                            sm.hit.Play();
                            // Remove health from the player
                            p.health -= 40;
                            // Remove the enemy
                            e.eIsVisible = false;
                        }

                        // Check enemy bullet collision with player 
                        for (int i = 0; i < e.eBulletList.Count(); i++)
                        {
                            if (p.boundingBox.Intersects(e.eBulletList[i].boundingBox))
                            {
                                explosionList.Add(new Explosion(Content.Load<Texture2D>("Sprites/explosion"), new Vector2(p.position.X, p.position.Y)));
                                sm.hit.Play();
                                p.health -= 20;
                                // Remove the bullet
                                e.eBulletList[i].isVisible = false;
                            }
                        }

                        // Check player bullet collision with enemy 
                        for (int i = 0; i < p.bulletList.Count(); i++)
                        {
                            if (p.bulletList[i].boundingBox.Intersects(e.eBoundingBox))
                            {
                                sm.explodeSound.Play();
                                // Plays the explosion animation at the enemy's position
                                explosionList.Add(new Explosion(Content.Load<Texture2D>("Sprites/explosion"), new Vector2(e.ePosition.X, e.ePosition.Y)));
                                hud.playerScore += 20;
                                // Removes player bullet
                                p.bulletList[i].isVisible = false;
                                e.eIsVisible = false;
                            }
                        }
                        e.Update(_gameTime);
                    }

                    // For each asteroid in the Asteroid list, update it and check for collisions
                    foreach (Asteroid a in asteroidList)
                    {
                        // Check to see if any of the asteroids are colliding with the player ship, if they are... set isVisible to false, which in turn removes them from the asteroid list
                        if (a.boundingBox.Intersects(p.boundingBox))
                        {
                            explosionList.Add(new Explosion(Content.Load<Texture2D>("Sprites/explosion"), new Vector2(a.position.X, a.position.Y)));
                            sm.hit.Play();
                            p.health -= 40;
                            a.isVisible = false;
                        }

                        // Loop trough bullet list if any asteriods come in contact with these bullets, then destroy bullet and asteroid
                        for (int i = 0; i < p.bulletList.Count(); i++)
                        {
                            if (a.boundingBox.Intersects(p.bulletList[i].boundingBox))
                            {
                                sm.explodeSound.Play();
                                explosionList.Add(new Explosion(Content.Load<Texture2D>("Sprites/explosion"), new Vector2(a.position.X, a.position.Y)));
                                hud.playerScore += 10;
                                a.isVisible = false;
                                p.bulletList.ElementAt(i).isVisible = false;
                            }
                        }
                        a.Update(_gameTime);
                    }

                    // If player health hits 0 then go to the gameover state
                    if (p.health <= 0)
                    {
                        // Stop music
                        sm.alarm.Play();
                        MediaPlayer.Stop();
                        gameState = State.Gameover;
                    }
                    // If player score hits 1000 then go to win state
                    if (hud.playerScore >= 1000)
                    {
                        // Stop music
                        MediaPlayer.Stop();
                        gameState = State.Win;
                    }

                    // Update explosions   
                    foreach (Explosion ex in explosionList)
                    {
                        ex.Update(_gameTime);
                    }

                    sf.Update(_gameTime);
                    p.Update(_gameTime);
                    ManageExplosions();
                    LoadAsteriods();
                    LoadEnemies();
                    break;

                // Updating Menu state
                case State.Menu:
                    // Get keyboard state
                    KeyboardState keyStateMenu = Keyboard.GetState();
                    if (keyStateMenu.IsKeyDown(Keys.Enter))
                    {
                        gameState = State.Playing;
                        MediaPlayer.Play(sm.bgMusic);
                    }
                    break;

                // Updating Gameover state
                case State.Gameover:
                    p.position = new Vector2(375, 900);
                    // Reset the player health back to full
                    p.health = 200;
                    // Clears the enemies and asteroids of the screen so that when you reset, the playing field will be empty
                    enemyList.Clear();
                    asteroidList.Clear();
                    // Get keyboard state
                    KeyboardState keyStateGameOver = Keyboard.GetState();
                    // If in the gameover screen a user hits the M key, return to the main menu
                    if (keyStateGameOver.IsKeyDown(Keys.M))
                    {
                        gameState = State.Menu;
                        hud.playerScore = 0;
                        MediaPlayer.Play(sm.menuMusic);
                    }
                    else if (keyStateGameOver.IsKeyDown(Keys.Enter))
                    {
                        gameState = State.Playing;
                        hud.playerScore = 0;
                        MediaPlayer.Play(sm.bgMusic);
                    }
                    break;

                // Updating Win state
                case State.Win:
                    p.position = new Vector2(375, 900);
                    // Reset the player health back to full
                    p.health = 200;
                    // Clears the enemies and asteroids of the screen so that when you reset, the playing field will be empty
                    enemyList.Clear();
                    asteroidList.Clear();
                    // Get keyboard state
                    KeyboardState keyStateWin = Keyboard.GetState();
                    if (keyStateWin.IsKeyDown(Keys.M))
                    {
                        gameState = State.Menu;
                        hud.playerScore = 0;
                        MediaPlayer.Play(sm.menuMusic);
                    }
                    break;
            }
            base.Update(_gameTime);
        }

        // Draw
        protected override void Draw(GameTime _gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();

            switch (gameState)
            {
                // Drawing Playing state
                case State.Playing:
                    sf.Draw(_spriteBatch);
                    p.Draw(_spriteBatch);
                    // For each explosion in the Explosion list, draw it
                    foreach (Explosion ex in explosionList)
                    {
                        ex.Draw(_spriteBatch);
                    }

                    // For each asteroid in the Asteroid list, draw it
                    foreach (Asteroid a in asteroidList)
                    {
                        a.Draw(_spriteBatch);
                    }

                    // For each enemy in the enemy list, draw it
                    foreach (Enemy e in enemyList)
                    {
                        e.Draw(_spriteBatch);
                    }

                    hud.Draw(_spriteBatch);
                    break;

                // Drawing Menu state
                case State.Menu:
                    // Drawing backround image
                    _spriteBatch.Draw(menuImage, new Vector2(0, 0), Color.White);
                    break;

                // Drawing Gameover state
                case State.Gameover:
                    _spriteBatch.Draw(gameoverImage, new Vector2(0, 0), Color.White);
                    _spriteBatch.DrawString(hud.playerScoreFont, "" + hud.playerScore.ToString(), new Vector2(450, 395), Color.Orange);
                    break;

                // Drawing Win state
                case State.Win:
                    _spriteBatch.Draw(winScreen, new Vector2(0, 0), Color.White);
                    break;
            }
            _spriteBatch.End();
            base.Draw(_gameTime);
        }

        // Load asteroids
        public void LoadAsteriods()
        {
            // Creating random variables for the x and y axis' for the asteroids
            int randX = random.Next(0, 750);
            int randY = random.Next(-600, -50); // Randy hehheh

            // If there are less than 5 asteroids on the screen, then create more until there are 5 again.
            if (asteroidList.Count() < 5)
            {
                asteroidList.Add(new Asteroid(Content.Load<Texture2D>("Sprites/asteroid"), new Vector2(randX, randY)));
            }

            // If any of the asteroids in the list were destroyed or invisible, then remove them from the list.
            for (int i = 0; i < asteroidList.Count; i++)
            {
                if (!asteroidList[i].isVisible)
                {
                    asteroidList.RemoveAt(i);
                    i--;
                }
            }
        }

        // Load enemies
        public void LoadEnemies()
        {
            // Creating random variables for the x and y axis' for the asteroids
            int randX = random.Next(0, 750);
            int randY = random.Next(-600, -50); // Randy hehheh

            // If there are less than 3 enemies on the screen, then create more until there are 3 again.
            if (enemyList.Count() < 3)
            {
                enemyList.Add(new Enemy(Content.Load<Texture2D>("Sprites/tieFighter"), new Vector2(randX, randY), Content.Load<Texture2D>("Sprites/enemyBullet")));
            }

            // If any of the enemies in the list were destroyed or invisible, then remove them from the list.
            for (int i = 0; i < enemyList.Count; i++)
            {
                if (!enemyList[i].eIsVisible)
                {
                    enemyList.RemoveAt(i);
                    i--;
                }
            }
        }

        // Manage explions
        public void ManageExplosions()
        {
            for (int i = 0; i < explosionList.Count(); i++)
            {
                if (!explosionList[i].isVisible)
                {
                    explosionList.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}