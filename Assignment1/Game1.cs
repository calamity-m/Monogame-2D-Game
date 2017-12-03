using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using RC_Framework;

namespace Assignment1
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // Screensize 
        public static Vector2 screenSize = new Vector2(800, 600);

        // Basic static managers used throughout all/most states
        public static SpriteManager spriteManager = new SpriteManager();
        public static ParticleManager particleManager = new ParticleManager(1500, 150);
        public static RC_GameStateManager levelManager;

        // Do we want to exit?
        public static bool exitGame;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 600;
            graphics.PreferredBackBufferWidth = 800;
            Content.RootDirectory = "Content";
            this.Window.Title = "PEWPEWDO MAN";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Setup basics
            Resources.LoadContent(Content);
            SoundManager.LoadContent(Content);
            LineBatch.init(GraphicsDevice);
            HighScoreManager.Initialize();
            SaveGameManager.Initialize();
            // Setup our media palyer
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(SoundManager.getSong());
            MediaPlayer.Volume = Resources.volume;

            levelManager = new RC_GameStateManager();

            // Add our player that we will keep throughout the entire run of this game
            Game1.spriteManager.addSpriteActor(new Player(Resources.Player, Game1.screenSize / 2, Vector2.Zero));

            // heaps of levels is most likely in-efficient use of state management, better to combine large amounts of these levels
            // and then have minor nested state management, e.g. combining levels 1-4 and menu/options, etc. However, due to the scale
            // of this game and time limits, I believe the below code suffices.

            levelManager.AddLevel(0, new LevelOne());
            levelManager.getLevel(0).InitializeLevel(GraphicsDevice, spriteBatch, Content, levelManager);

            levelManager.AddLevel(1, new LevelTwo());
            levelManager.getLevel(1).InitializeLevel(GraphicsDevice, spriteBatch, Content, levelManager);

            levelManager.AddLevel(2, new LevelThree());
            levelManager.getLevel(2).InitializeLevel(GraphicsDevice, spriteBatch, Content, levelManager);

            levelManager.AddLevel(3, new LevelSplash());
            levelManager.getLevel(3).InitializeLevel(GraphicsDevice, spriteBatch, Content, levelManager);

            levelManager.AddLevel(4, new LevelMenu());
            levelManager.getLevel(4).InitializeLevel(GraphicsDevice, spriteBatch, Content, levelManager);

            levelManager.AddLevel(5, new LevelOptions());
            levelManager.getLevel(5).InitializeLevel(GraphicsDevice, spriteBatch, Content, levelManager);

            levelManager.AddLevel(6, new LevelLoad());
            levelManager.getLevel(6).InitializeLevel(GraphicsDevice, spriteBatch, Content, levelManager);

            // Preferrable to load in pause content now
            levelManager.AddLevel(7, new LevelPause());
            levelManager.getLevel(7).InitializeLevel(GraphicsDevice, spriteBatch, Content, levelManager);
            levelManager.getLevel(7).LoadContent();

            // Preferrable to load in how to play content now (works on push/pop stack same as pause)
            levelManager.AddLevel(8, new LevelHowToPlay());
            levelManager.getLevel(8).InitializeLevel(GraphicsDevice, spriteBatch, Content, levelManager);
            levelManager.getLevel(8).LoadContent();

            levelManager.AddLevel(9, new LevelGameOver());
            levelManager.getLevel(9).InitializeLevel(GraphicsDevice, spriteBatch, Content, levelManager);

            levelManager.AddLevel(10, new LevelFour());
            levelManager.getLevel(10).InitializeLevel(GraphicsDevice, spriteBatch, Content, levelManager);

            levelManager.AddLevel(11, new LevelFiveBoss());
            levelManager.getLevel(11).InitializeLevel(GraphicsDevice, spriteBatch, Content, levelManager);

            levelManager.AddLevel(12, new LevelWin());
            levelManager.getLevel(12).InitializeLevel(GraphicsDevice, spriteBatch, Content, levelManager);

            levelManager.AddLevel(13, new LevelHighScores());
            levelManager.getLevel(13).InitializeLevel(GraphicsDevice, spriteBatch, Content, levelManager);

            levelManager.setLevel(3);

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            // TEST
            Content.Unload();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (exitGame)
                Exit();

            // Check if our game window is in focus
            if (!IsActive)
                return;
            
            levelManager.getCurrentLevel().Update(gameTime);

            if (PlayerInput.KeyPressed(Keys.K))
                Resources.score += 1000;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Resources.ColorBg);

            levelManager.getCurrentLevel().Draw(gameTime);

            base.Draw(gameTime);
        }

        /// <summary>
        /// Save game on exit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        protected override void OnExiting(object sender, EventArgs args)
        {
            SaveGameManager.SaveGame();
            base.OnExiting(sender, args);
        }
    }
}
