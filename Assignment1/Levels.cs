using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using RC_Framework;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;
using System.IO;

namespace Assignment1
{
    public class Levels
    {
        //public static string dir = @"C:\Users\Mark\Documents\School\Semester 1 2017\Games Programming Techniques\Projects\7160-Assignment1-u3154816\Assignment1\Assignment1\";
        //public static string dir = @"C:\Users\u3154816\Documents\7160-Assignment1-u3154816\Assignment1\Assignment1\";
        public static string dir = Directory.GetCurrentDirectory();

        public static string getLevelName()
        {
            string levelDisplay = "";
            switch (Resources.currPlayLevel)
            {
                case 0:
                    levelDisplay = "Level 1: The Citadel";
                    break;
                case 1:
                    levelDisplay = "Level 2: Shape Fortress";
                    break;
                case 2:
                    levelDisplay = "Level 3: Fighter's Valley";
                    break;
                case 10:
                    levelDisplay = "Level 4: Sanctuary of the Ill-Recieved";
                    break;
                case 11:
                    levelDisplay = "Level 5: Hexagon's Palace";
                    break;
            }
            return levelDisplay;
        }

        public static void handleLevelHotKeys(RC_GameStateManager gameStateManager)
        {
            // Handle Help Button
            if (PlayerInput.KeyOldPressed(Keys.F1))
                gameStateManager.pushLevel(8);

            // Handle Pausing
            if (PlayerInput.KeyPressed(Keys.P))
                gameStateManager.pushLevel(7);

            // Enable Level Cheat
            if (PlayerInput.KeyPressed(Keys.C))
                Resources.cheatingFlag = !Resources.cheatingFlag;
        }

        public static void handleLevelUpdate(GameTime gameTime, RC_GameStateManager gameStateManager, EnemySpawner enemySpawner, PickupSpawner pickupSpawner, PlayerGUI playerGUI, int scoreReq, int nxtLvl)
        {

            // Check our win conditions
            if (Resources.score > scoreReq)
            {
                gameStateManager.setLevel(nxtLvl);
            }

            Levels.handleLevelHotKeys(gameStateManager);

            // Update our Sprite Manager
            Game1.spriteManager.Update();

            // Update our Particle manager
            Game1.particleManager.Update(gameTime);

            // Update our Enemy Spawner
            enemySpawner.Update(Game1.spriteManager.count);

            // Update our Pickup Spawner
            pickupSpawner.Update(Game1.spriteManager.count);

        }

        public static void handleLevelBasicDrawing(SpriteBatch spriteBatch, PlayerGUI playerGUI, int ticksRemaining)
        {
            if (ticksRemaining >= 0)
            {
                Game1.spriteManager.player.Draw(spriteBatch);
                playerGUI.Draw(spriteBatch);
            }
            else
            {
                // Update our sprites
                Game1.spriteManager.Draw(spriteBatch);

                // Draw our Particles
                Game1.particleManager.Draw(spriteBatch);

                // Draw the cursor
                Vector2 cursorOffset = new Vector2(Resources.Crosshair.Width, Resources.Crosshair.Height);
                spriteBatch.Draw(Resources.Crosshair, PlayerInput.mousePosition - (cursorOffset / 2), Color.White);

                // Draw our gui
                playerGUI.Draw(spriteBatch);
            }
        }
    }

    class LevelOne : RC_GameStateParent
    {
        private int scoreRequired;
        private EnemySpawner enemySpawner = new EnemySpawner(true, false, false, true);
        private PickupSpawner pickupSpawner = new PickupSpawner(true, true, true);
        private PlayerGUI playerGUI;

        public override void InitializeLevel(GraphicsDevice g, SpriteBatch s, ContentManager c, RC_GameStateManager lm)
        {
            scoreRequired = Resources.levelOneNeeded;
            playerGUI = new PlayerGUI("" + scoreRequired);
            // Add our player
            //Game1.spriteManager.addSpriteActor(new Player(Resources.Player, Game1.screenSize / 2, Vector2.Zero));

            base.InitializeLevel(g, s, c, lm);
        }

        public override void EnterLevel(int fromLevelNum)
        {
            if (Game1.spriteManager.boss != null)
                Game1.spriteManager.boss.Kill(false);

            Game1.particleManager.ClearScene();
            Game1.spriteManager.clearScene(true);
            Resources.currPlayLevel = 0;
        }

        public override void Update(GameTime gameTime)
        {
            // Handle our player input
            PlayerInput.Update();


            // Update our level with modified variables pertaining to our play level
            Levels.handleLevelUpdate(gameTime, gameStateManager, enemySpawner, pickupSpawner, playerGUI, scoreRequired, 1);
        }

        public override void Draw(GameTime gameTime)
        {
            // Set the background, incase different
            graphicsDevice.Clear(Resources.ColorBg);

            spriteBatch.Begin(SpriteSortMode.Texture, BlendState.Additive);

            Levels.handleLevelBasicDrawing(spriteBatch, playerGUI, -1);

            spriteBatch.End();
        }
    }

    class LevelTwo : RC_GameStateParent
    {

        private int scoreRequired;
        private EnemySpawner enemySpawner = new EnemySpawner(true, true, false, true);
        private PickupSpawner pickupSpawner = new PickupSpawner(true, true, true);
        private PlayerGUI playerGUI;

        public override void InitializeLevel(GraphicsDevice g, SpriteBatch s, ContentManager c, RC_GameStateManager lm)
        {
            scoreRequired = Resources.levelTwoNeeded;
            playerGUI = new PlayerGUI("" + scoreRequired);
            //ticksRemaining = ticksToWait;
            base.InitializeLevel(g, s, c, lm);
        }

        public override void EnterLevel(int fromLevelNum)
        {
            Resources.currPlayLevel = 1;
            Game1.particleManager.ClearScene();
            Game1.spriteManager.clearScene(true);
        }

        public override void Update(GameTime gameTime)
        {
            // Handle our player input
            PlayerInput.Update();
            // Update our level with modified variables pertaining to our play level
            Levels.handleLevelUpdate(gameTime, gameStateManager, enemySpawner, pickupSpawner, playerGUI, scoreRequired, 2);

        }

        public override void Draw(GameTime gameTime)
        {
            // Set the background, incase different
            graphicsDevice.Clear(Resources.ColorBg);

            spriteBatch.Begin(SpriteSortMode.Texture, BlendState.Additive);
            
            Levels.handleLevelBasicDrawing(spriteBatch, playerGUI, -1);

            spriteBatch.End();
        }
    }

    class LevelThree : RC_GameStateParent
    {

        private int scoreRequired;
        private EnemySpawner enemySpawner = new EnemySpawner(20, 50, false, false, true, false);
        private PickupSpawner pickupSpawner = new PickupSpawner(true, true, true);
        private PlayerGUI playerGUI;

        public override void InitializeLevel(GraphicsDevice g, SpriteBatch s, ContentManager c, RC_GameStateManager lm)
        {
            scoreRequired = Resources.levelThreeNeeded;
            playerGUI = new PlayerGUI("" + scoreRequired);
            //ticksRemaining = ticksToWait;
            base.InitializeLevel(g, s, c, lm);
        }

        public override void EnterLevel(int fromLevelNum)
        {
            Game1.spriteManager.clearScene(true);
            Game1.particleManager.ClearScene();
            Resources.currPlayLevel = 2;
        }

        public override void Update(GameTime gameTime)
        {

            // Handle our player input
            PlayerInput.Update();

            // Update our level with modified variables pertaining to our play level
            Levels.handleLevelUpdate(gameTime, gameStateManager, enemySpawner, pickupSpawner, playerGUI, scoreRequired, 10);

        }

        public override void Draw(GameTime gameTime)
        {
            // Set the background, incase different
            graphicsDevice.Clear(Resources.ColorBg);

            spriteBatch.Begin(SpriteSortMode.Texture, BlendState.Additive);

            Levels.handleLevelBasicDrawing(spriteBatch, playerGUI, -1);

            spriteBatch.End();
        }
    }

    class LevelSplash : RC_GameStateParent
    {

        private Texture2D splashScreen;
        private Sprite3 splash;
        private Color colorSplash;
        SaveGameData data;

        public override void LoadContent()
        {
            splashScreen = Content.Load<Texture2D>("Textures/SplashScreenTex");
            splash = new Sprite3(true, splashScreen, 0, 0);
            colorSplash = new Color(255, 211, 228);


            data = SaveGameManager.LoadGameState("savegame.xml");
            Resources.currPlayLevel = data.currLevel;
            Resources.score = data.currScore;
            Resources.volume = data.currVolume;
            MediaPlayer.Volume = Resources.volume;
            Resources.graphicsQuality = data.currGraphics;
            Game1.particleManager.setMaxParticleAmount(1500);
            int x = data.currLives;
            if (Game1.spriteManager.player != null)
                Game1.spriteManager.player.sprite.hitPoints = x;
        }

        public override void UnloadContent()
        {
            splashScreen.Dispose();
            splash.makeInactive = true;
        }

        public override void EnterLevel(int fromLevelNum)
        {
            LoadContent();
        }

        public override void ExitLevel()
        {
            UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            PlayerInput.Update();

            if (PlayerInput.anyPressed())
            {
                gameStateManager.setLevel(4);
            }

        }

        public override void Draw(GameTime gameTime)
        {

            graphicsDevice.Clear(colorSplash);

            spriteBatch.Begin(SpriteSortMode.Texture);

            if (splash != null) splash.draw(spriteBatch);

            spriteBatch.End();
        }
    }

    class LevelMenu : RC_GameStateParent
    {

        private Texture2D newgameTex, continueTex, optionsTex, howToPlayTex, exitTex, titleTex, triangleTex, hexagonTex;
        private Button buttonNewGame, buttonContinue, buttonOptions, buttonHowToPlay, buttonExit, buttonHighScore;
        private Sprite3 sTitle, sTriangle, sHexagon;
        private float xAligned = Game1.screenSize.X / 2;
        private float minorOffset = 5f;

        public override void LoadContent()
        {
            // Load Textures
            continueTex = Content.Load<Texture2D>("Textures/Menu/Continue");
            optionsTex = Content.Load<Texture2D>("Textures/Menu/Options");
            howToPlayTex = Content.Load<Texture2D>("Textures/Menu/HowToPlay");
            exitTex = Content.Load<Texture2D>("Textures/Menu/Exit");
            titleTex = Content.Load<Texture2D>("Textures/Menu/Title");
            triangleTex = Content.Load<Texture2D>("Textures/Menu/TriangleMan");
            newgameTex = Content.Load<Texture2D>("Textures/Menu/NewGame");
            hexagonTex = Content.Load<Texture2D>("Textures/Menu/HexagonGuy");

            // Create sprites for title and display characters
            sTitle = new Sprite3(true, titleTex, 0, 0);
            sTriangle = new Sprite3(true, triangleTex, 0, 0);

            // Set sprite offsets
            sTitle.setHSoffset(new Vector2(titleTex.Width / 2, titleTex.Height / 2));
            sTriangle.setHSoffset(new Vector2(triangleTex.Width / 2, triangleTex.Height / 2));

            // Set Position of Sprites
            sTitle.setPos(new Vector2(xAligned, 0+sTitle.getHeight()));
            sTriangle.setPos(675, 275);

            buttonNewGame = new Button(newgameTex, new Vector2(xAligned, (95 + sTitle.getPosY()) + (newgameTex.Height)), Resources.ColorHighLight, Button.ButtonType.ResetGame, 0);
            buttonContinue = new Button(continueTex, new Vector2(xAligned, buttonNewGame.getPosY() + (minorOffset + continueTex.Height)), Resources.ColorHighLight, Button.ButtonType.ChangeLevelSet, Resources.currPlayLevel);
            buttonOptions = new Button(optionsTex, new Vector2(xAligned, buttonContinue.getPosY() + (minorOffset + optionsTex.Height)), Resources.ColorHighLight, Button.ButtonType.ChangeLevelSet, 5);
            buttonHowToPlay = new Button(howToPlayTex, new Vector2(xAligned, buttonOptions.getPosY() + (minorOffset + howToPlayTex.Height)), Resources.ColorHighLight, Button.ButtonType.ChangeLevelPush, 8);
            buttonExit = new Button(exitTex, new Vector2(xAligned, buttonHowToPlay.getPosY() + (minorOffset + exitTex.Height)), Resources.ColorHighLight, Button.ButtonType.Exit, 0);
            buttonHighScore = new Button(hexagonTex, new Vector2(135, 339), Resources.ColorLose, Button.ButtonType.ChangeLevelSet, 13);

        }

        public override void UnloadContent()
        {
            // Update all methods to use this shit
            // ContentManager myManager = new ContentManager(Content.ServiceProvider, Content.RootDirectory);
        }

        public override void EnterLevel(int fromLevelNum)
        {
            LoadContent();
        }

        public override void ExitLevel()
        {
            UnloadContent();
        }

        private bool mouseColliding(Sprite3 s)
        {
            return s.insideOrEq(PlayerInput.mousePosition.X, PlayerInput.mousePosition.Y);
        }

        public override void Update(GameTime gameTime)
        {

            PlayerInput.Update();

            // Update our buttons
            buttonNewGame.Update();
            buttonContinue.Update();
            buttonOptions.Update();
            buttonHowToPlay.Update();
            buttonExit.Update();
            buttonHighScore.Update();

            // Update Triangle Man
            if (mouseColliding(sTriangle))
                sTriangle.setColor(Resources.ColorHighLight);
            else
                sTriangle.setColor(Color.White);


        }

        public override void Draw(GameTime gameTime)
        {
            graphicsDevice.Clear(Resources.ColorBg);

            spriteBatch.Begin(SpriteSortMode.Deferred);

            // Draw our Buttons
            buttonNewGame.Draw(spriteBatch);
            buttonContinue.Draw(spriteBatch);
            buttonOptions.Draw(spriteBatch);
            buttonHowToPlay.Draw(spriteBatch);
            buttonExit.Draw(spriteBatch);
            buttonHighScore.Draw(spriteBatch);

            // Update Non Button Sprites
            sTitle.customDrawButton(spriteBatch);
            sTriangle.Draw(spriteBatch);

            // Draw the cursor
            Vector2 cursorOffset = new Vector2(Resources.Pointer.Width, Resources.Pointer.Height);
            spriteBatch.Draw(Resources.Pointer, PlayerInput.mousePosition - (cursorOffset / 2), Color.White);

            spriteBatch.End();
        }
    }

    class LevelOptions : RC_GameStateParent
    {

        private Texture2D minusTex, plusTex, medTex, lowTex, mediumTex, highTex, triangleTex;
        private Button buttonMinus, buttonPlus, buttonMed, buttonLow, buttonMedium, buttonHigh;
        private Sprite3 sTriangle;
        private string title, subtitle, instructions, graphics;
        private float xAlign = 50f;
        private float xOffset = 5f;

        public override void LoadContent()
        {
            minusTex = Content.Load<Texture2D>("Textures/Options/Minus");
            plusTex = Content.Load<Texture2D>("Textures/Options/Plus");
            medTex = Content.Load<Texture2D>("Textures/Options/Med");
            lowTex = Content.Load<Texture2D>("Textures/Options/Low");
            mediumTex = Content.Load<Texture2D>("Textures/Options/Medium");
            highTex = Content.Load<Texture2D>("Textures/Options/High");
            triangleTex = Content.Load<Texture2D>("Textures/Menu/TriangleMan");

            buttonMinus = new Button(minusTex, new Vector2(-35 + minusTex.Width, 215 + minusTex.Height), Resources.ColorHighLight, Button.ButtonType.VolumeControl, 0);
            buttonPlus = new Button(plusTex, new Vector2(xOffset + buttonMinus.getPosX() + minusTex.Width, buttonMinus.getPosY()), Resources.ColorHighLight, Button.ButtonType.VolumeControl, 1);
            buttonMed = new Button(medTex, new Vector2(xOffset + buttonPlus.getPosX() + minusTex.Width, buttonMinus.getPosY()), Resources.ColorHighLight, Button.ButtonType.VolumeControl, 2);

            buttonLow = new Button(lowTex, new Vector2(-35 + lowTex.Width, 375 + lowTex.Height), Resources.ColorHighLight, Button.ButtonType.GraphicsControl, 0);
            buttonMedium = new Button(mediumTex, new Vector2(xOffset + buttonLow.getPosX() + lowTex.Width, 375 + lowTex.Height), Resources.ColorHighLight, Button.ButtonType.GraphicsControl, 1);
            buttonHigh = new Button(highTex, new Vector2(xOffset + buttonMedium.getPosX() + lowTex.Width, 375 + lowTex.Height), Resources.ColorHighLight, Button.ButtonType.GraphicsControl, 2);

            sTriangle = new Sprite3(true, triangleTex, 0, 0);
            sTriangle.setHSoffset(new Vector2(triangleTex.Width / 2, triangleTex.Height / 2));
            sTriangle.setPos(new Vector2(675, 305));

            title = "Options:";
            subtitle = "Adjust to Your Hearts Content";
            instructions = "Press Any Key to Return to Main Menu";
            displayGraphicsQuality();
        }

        public override void UnloadContent()
        {

        }

        private void displayGraphicsQuality()
        {
            switch (Resources.graphicsQuality)
            {
                case 0:
                    graphics = "Low";
                    break;
                case 1:
                    graphics = "Medium";
                    break;
                case 2:
                    graphics = "High";
                    break;
            }
        }

        public override void Update(GameTime gameTime)
        {
            PlayerInput.Update();

            if (PlayerInput.anyPressed())
                Game1.levelManager.setLevel(4);

            displayGraphicsQuality();

            sTriangle.Update(gameTime);

            buttonMinus.Update();
            buttonPlus.Update();
            buttonMed.Update();

            buttonLow.Update();
            buttonMedium.Update();
            buttonHigh.Update();
        }

        public override void Draw(GameTime gameTime)
        {
            graphicsDevice.Clear(Resources.ColorBg);

            spriteBatch.Begin(SpriteSortMode.Deferred);

            buttonMinus.Draw(spriteBatch);
            buttonPlus.Draw(spriteBatch);
            buttonMed.Draw(spriteBatch);

            buttonLow.Draw(spriteBatch);
            buttonMedium.Draw(spriteBatch);
            buttonHigh.Draw(spriteBatch);

            spriteBatch.DrawString(Resources.FontMainTitle, title, new Vector2(xAlign, 15), Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
            spriteBatch.DrawString(Resources.FontMain, subtitle, new Vector2(xAlign, 25 + Resources.FontMain.MeasureString(title).Y), 
                Color.White, 0, Vector2.Zero, 0.65f, SpriteEffects.None, 0);

            spriteBatch.DrawString(Resources.FontMain, "Volume: "+(int) (Resources.volume*100), new Vector2(xAlign, 185),
                Color.White, 0, Vector2.Zero, 0.65f, SpriteEffects.None, 0);

            spriteBatch.DrawString(Resources.FontMain, "Graphics: " + graphics, new Vector2(xAlign, 345),
                Color.White, 0, Vector2.Zero, 0.65f, SpriteEffects.None, 0);

            HelperUtils.DrawStringCentered(spriteBatch, new Vector2(Game1.screenSize.X / 2, Game1.screenSize.Y - 50), Resources.FontMain, 0.65f, instructions, Color.White);

            sTriangle.Draw(spriteBatch);

            // Draw the cursor
            Vector2 cursorOffset = new Vector2(Resources.Pointer.Width, Resources.Pointer.Height);
            spriteBatch.Draw(Resources.Pointer, PlayerInput.mousePosition - (cursorOffset / 2), Color.White);

            spriteBatch.End();

        }

        public override void EnterLevel(int fromLevelNum)
        {
            LoadContent();
        }

        public override void ExitLevel()
        {
            UnloadContent();
        }
    }

    class LevelPause : RC_GameStateParent
    {

        private ColorField pauseTransparency = null;
        private string pausedTitle = "PAUSED";
        private Texture2D continueTex, howToPlayTex, exitTex;
        private Button buttonContinue, buttonHowToPlay, buttonExit;

        public override void LoadContent()
        {
            continueTex = Content.Load<Texture2D>("Textures/Menu/Continue");
            howToPlayTex = Content.Load<Texture2D>("Textures/Menu/HowToPlay");
            exitTex = Content.Load<Texture2D>("Textures/Menu/Exit");

            float yOffset = 10f;

            /*
            buttonContinue = new Button(continueTex, new Vector2(Game1.screenSize.X / 2, 250), Resources.ColorHighLight, 9, 0);
            buttonHowToPlay = new Button(howToPlayTex, new Vector2(buttonContinue.getPosX(), buttonContinue.getPosY() + yOffset + howToPlayTex.Height), Resources.ColorHighLight, 1, 0);
            buttonExit = new Button(exitTex, new Vector2(buttonHowToPlay.getPosX(), buttonHowToPlay.getPosY() + yOffset + exitTex.Height), Resources.ColorHighLight, 0, 4);
            */
            buttonContinue = new Button(continueTex, new Vector2(Game1.screenSize.X / 2, 250), Resources.ColorHighLight, Button.ButtonType.ChangeLevelPop, 0);
            buttonHowToPlay = new Button(howToPlayTex, new Vector2(buttonContinue.getPosX(), buttonContinue.getPosY() + yOffset + howToPlayTex.Height), Resources.ColorHighLight, Button.ButtonType.ChangeLevelPush, 8);
            buttonExit = new Button(exitTex, new Vector2(buttonHowToPlay.getPosX(), buttonHowToPlay.getPosY() + yOffset + exitTex.Height), Resources.ColorHighLight, Button.ButtonType.ChangeLevelSet, 4);

            pauseTransparency = new ColorField(Resources.ColorPause, new Rectangle(0, 0, (int)Game1.screenSize.X, (int)Game1.screenSize.Y));
        }

        public override void EnterLevel(int fromLevelNum)
        {
            int currLevel = gameStateManager.getCurrentLevelNum();

        }

        public override void Update(GameTime gameTime)
        {
            PlayerInput.Update();

            buttonContinue.Update();
            buttonHowToPlay.Update();
            buttonExit.Update();

            if (PlayerInput.KeyPressed(Keys.Space))
                gameStateManager.popLevel();
        }

        public override void Draw(GameTime gameTime)
        {
            graphicsDevice.Clear(Color.Red);

            Game1.levelManager.prevStatePlayLevel.Draw(gameTime);

            spriteBatch.Begin();

            if (pauseTransparency != null)
                pauseTransparency.Draw(spriteBatch);

            HelperUtils.DrawStringCentered(spriteBatch, new Vector2(Game1.screenSize.X / 2, 75), Resources.FontMainTitle, 1f, pausedTitle, Color.White);

            HelperUtils.DrawStringCentered(spriteBatch, new Vector2(Game1.screenSize.X / 2, 135), Resources.FontMain, .65f, Levels.getLevelName(), Color.White);

            HelperUtils.DrawStringCentered(spriteBatch, new Vector2(Game1.screenSize.X / 2, 550), Resources.FontMain, .65f, "Press Any Keyboard Key to Resume", Color.White);

            buttonContinue.Draw(spriteBatch);
            buttonHowToPlay.Draw(spriteBatch);
            buttonExit.Draw(spriteBatch);

            // Draw Cursor
            Vector2 cursorOffset = new Vector2(Resources.Crosshair.Width, Resources.Crosshair.Height);
            spriteBatch.Draw(Resources.Crosshair, PlayerInput.mousePosition - (cursorOffset / 2), Color.White);
            spriteBatch.End();

        }
    }

    class LevelHowToPlay : RC_GameStateParent
    {
        ColorField transparency = null;
        Texture2D displayTex;
        Sprite3 display;

        public override void LoadContent()
        {
            transparency = new ColorField(new Color(Color.Black, 0.5f), new Rectangle(0, 0, (int)Game1.screenSize.X, (int)Game1.screenSize.Y));
            displayTex = Content.Load<Texture2D>("Textures/Menu/HowToPlayScreen");
            display = new Sprite3(true, displayTex, 0, 0);
            display.setHSoffset(new Vector2(displayTex.Width/2, displayTex.Height/2));
            display.setPos(Game1.screenSize / 2);
        }

        public override void Update(GameTime gameTime)
        {
            PlayerInput.Update();

            if (PlayerInput.anyPressed())
                gameStateManager.popLevel();
        }

        public override void Draw(GameTime gameTime)
        {
            graphicsDevice.Clear(Resources.ColorBg);

            Game1.levelManager.prevStatePlayLevel.Draw(gameTime);

            spriteBatch.Begin();

            if (transparency != null)
                transparency.Draw(spriteBatch);

            display.Draw(spriteBatch);

            // Draw Cursor
            Vector2 cursorOffset = new Vector2(Resources.Pointer.Width, Resources.Pointer.Height);
            spriteBatch.Draw(Resources.Pointer, PlayerInput.mousePosition - (cursorOffset / 2), Color.White);
            spriteBatch.End();

        }
    }

    class LevelGameOver : RC_GameStateParent
    {
        Color ColorBgGameOver = new Color(64, 64, 64);
        string title = "You Died";
        string scoreDisplay = "Score:";
        string subTitle = "Press Any Key to Continue";
        SpriteFont font = Content.Load<SpriteFont>("Fonts/fontMainTitleLarge");
        // Hacky timer that forces player to see gameover screen
        private int timeWaitTicks = 50;
        private int timeWaitCurrent = 0;

        public override void EnterLevel(int fromLevelNum)
        {
            scoreDisplay = "Score: " + Resources.score;
            timeWaitCurrent = timeWaitTicks;

            // Another oversight
            int lvl = 0;
            if (Resources.currPlayLevel == 10)
                lvl = 3;
            else if (Resources.currPlayLevel == 11)
                lvl = 4;

            HighScoreManager.AddHighScore(Resources.score, lvl);

            // Now reset everything
            Resources.score = 0;
            Resources.currPlayLevel = 0;
            Game1.spriteManager.clearScene(true);
            Game1.particleManager.ClearScene();
            if (Game1.spriteManager.player != null) Game1.spriteManager.player.resetPlayer();
        }

        public override void Update(GameTime gameTime)
        {
            PlayerInput.Update();

            if (timeWaitCurrent <= 0)
            {

                if (PlayerInput.anyPressed())
                {
                    gameStateManager.setLevel(4);
                }
            }


            if (timeWaitCurrent > 0)
            {
                timeWaitCurrent--;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            graphicsDevice.Clear(ColorBgGameOver);

            spriteBatch.Begin();

            HelperUtils.DrawStringCentered(spriteBatch, new Vector2(Game1.screenSize.X / 2, Game1.screenSize.Y / 2 - 55), font, 1f, title, Color.White);

            HelperUtils.DrawStringCentered(spriteBatch, new Vector2(Game1.screenSize.X / 2, Game1.screenSize.Y / 2 + 35), Resources.FontMain, 0.5f, scoreDisplay, Resources.ColorLose);

            HelperUtils.DrawStringCentered(spriteBatch, new Vector2(Game1.screenSize.X / 2, Game1.screenSize.Y / 2 + 75), Resources.FontMain, 0.5f, subTitle, Color.White);

            // Draw Cursor
            Vector2 cursorOffset = new Vector2(Resources.Crosshair.Width, Resources.Crosshair.Height);
            spriteBatch.Draw(Resources.Pointer, PlayerInput.mousePosition - (cursorOffset / 2), Color.White);

            spriteBatch.End();
        }
    }


    class LevelFour : RC_GameStateParent
    {
        private int scoreRequired;
        private EnemySpawner enemySpawner = new EnemySpawner(true, true, true, true);
        private PickupSpawner pickupSpawner = new PickupSpawner(true, true, true);
        private PlayerGUI playerGUI;

        public override void InitializeLevel(GraphicsDevice g, SpriteBatch s, ContentManager c, RC_GameStateManager lm)
        {
            scoreRequired = Resources.levelFourNeeded;
            playerGUI = new PlayerGUI("" + scoreRequired);
            //ticksRemaining = ticksToWait;
            base.InitializeLevel(g, s, c, lm);
        }

        public override void EnterLevel(int fromLevelNum)
        {
            Game1.spriteManager.clearScene(true);
            Game1.particleManager.ClearScene();
            Resources.currPlayLevel = 10;
        }

        public override void Update(GameTime gameTime)
        {

            // Handle our player input
            PlayerInput.Update();

            // Update our level with modified variables pertaining to our play level
            Levels.handleLevelUpdate(gameTime, gameStateManager, enemySpawner, pickupSpawner, playerGUI, scoreRequired, 11);

        }

        public override void Draw(GameTime gameTime)
        {
            // Set the background, incase different
            graphicsDevice.Clear(Resources.ColorBg);

            spriteBatch.Begin(SpriteSortMode.Texture, BlendState.Additive);

            Levels.handleLevelBasicDrawing(spriteBatch, playerGUI, -1);

            spriteBatch.End();
        }
    }

    class LevelFiveBoss : RC_GameStateParent
    {
        private EnemySpawner enemySpawner = new EnemySpawner(false, false, false, false);
        private PickupSpawner pickupSpawner = new PickupSpawner(true, true, true);
        private PlayerGUI playerGUI;
        private bool initiatedPhase2and3;
        private int ticksWaitFinished = 150;
        private int ticksRemaining;

        public override void InitializeLevel(GraphicsDevice g, SpriteBatch s, ContentManager c, RC_GameStateManager lm)
        {
            playerGUI = new PlayerGUI("KILL HEXAGON GUY");
            //ticksRemaining = ticksToWait;
            base.InitializeLevel(g, s, c, lm);
        }

        public override void EnterLevel(int fromLevelNum)
        {
            Resources.currPlayLevel = 11;
            ticksRemaining = ticksWaitFinished;
            Game1.spriteManager.clearScene(true);
            Game1.particleManager.ClearScene();
            Console.WriteLine("Entering boss lvl");
            
            if (Game1.spriteManager.boss != null)
                Game1.spriteManager.boss.Kill(false);
            
            Game1.spriteManager.addSpriteActor(new Boss(Resources.Boss, new Vector2(Game1.screenSize.X / 2, 50), Vector2.Zero));
            enemySpawner = new EnemySpawner(false, false, false, false);
        }

        public override void Update(GameTime gameTime)
        {

            if (Game1.spriteManager.boss.bState == Boss.BossState.Phase2 && !initiatedPhase2and3)
            {
                enemySpawner = new EnemySpawner(20, 45, true, false, false, false);
                initiatedPhase2and3 = true;
            }

            if (Game1.spriteManager.boss.bState == Boss.BossState.Finished)
            {
                Game1.spriteManager.clearScene(false);
                Game1.particleManager.Update(gameTime);
                enemySpawner = new EnemySpawner(false, false, false, false);
                pickupSpawner = new PickupSpawner(false, false, false);
                Game1.spriteManager.boss.Kill(true);
                if (ticksRemaining <= 0)
                {
                    gameStateManager.setLevel(12);
                }

                if (ticksRemaining > 0)
                {
                    ticksRemaining--;
                }
            }

            // Handle our player input
            PlayerInput.Update();

            Levels.handleLevelHotKeys(gameStateManager);

            // Update our Sprite Manager
            Game1.spriteManager.Update();

            // Update our Particle Manager
            Game1.particleManager.Update(gameTime);

            // Update our Enemy Spawner
            enemySpawner.Update(Game1.spriteManager.count);

            // Update our Pickup Spawner
            pickupSpawner.Update(Game1.spriteManager.count);
            
        }

        public override void Draw(GameTime gameTime)
        {
            // Set the background, incase different
            graphicsDevice.Clear(Resources.ColorBg);

            spriteBatch.Begin(SpriteSortMode.Texture, BlendState.Additive);

            Levels.handleLevelBasicDrawing(spriteBatch, playerGUI, -1);

            Game1.particleManager.Draw(spriteBatch);

            spriteBatch.End();
        }
    }

    /// <summary>
    /// I have made too many classes for so many simple things, LevelWin should most likely be integrated with everything else
    /// </summary>
    class LevelWin : RC_GameStateParent
    {

        private Color colorWin = new Color(75, 174, 204);
        private SpriteFont font = Content.Load<SpriteFont>("Fonts/fontMainTitleLarge");
        private Texture2D continueTex;
        private Texture2D exitTex;
        private Button buttonHighScore;
        private Button buttonReturn;
        private int achievedScore;
        private int achievedLevel;

        public override void EnterLevel(int fromLevelNum)
        {
            achievedLevel = Resources.currPlayLevel;
            Resources.currPlayLevel = 0;
            achievedScore = Resources.score;
            Resources.score = 0;
            if (Game1.spriteManager.player != null) Game1.spriteManager.player.resetPlayer();
            LoadContent();
        }

        public override void LoadContent()
        {
            continueTex = Content.Load<Texture2D>("Textures/Menu/Continue");
            exitTex = Content.Load<Texture2D>("Textures/Menu/Exit");
            buttonHighScore = new Button(continueTex, new Vector2(Game1.screenSize.X / 2, 455), Resources.ColorHighLight, Button.ButtonType.ChangeLevelSet, 13);
            buttonReturn = new Button(exitTex, new Vector2(Game1.screenSize.X / 2, buttonHighScore.getPosY() + exitTex.Height + 10), 
                Resources.ColorHighLight, Button.ButtonType.ChangeLevelSet, 4);

            // Silly oversight by me
            if (achievedLevel == 10)
                achievedLevel = 3;
            else if (achievedLevel == 11)
                achievedLevel = 4;

            HighScoreManager.AddHighScore(achievedScore, achievedLevel);
        }

        public override void Update(GameTime gameTime)
        {
            PlayerInput.Update();

            buttonHighScore.Update();
            buttonReturn.Update();
        }

        public override void Draw(GameTime gameTime)
        {
            graphicsDevice.Clear(colorWin);

            spriteBatch.Begin();

            HelperUtils.DrawStringCentered(spriteBatch, new Vector2(Game1.screenSize.X/2, 100), font, 1f, "You Win", Color.White);
            HelperUtils.DrawStringCentered(spriteBatch, new Vector2(Game1.screenSize.X / 2, 195), Resources.FontMain, 0.5f, "It turns out you are an alien life form", Color.White);
            HelperUtils.DrawStringCentered(spriteBatch, new Vector2(Game1.screenSize.X / 2, 225), Resources.FontMain, 0.5f, "Invading an innocent planet created from shapes.", Color.White);
            HelperUtils.DrawStringCentered(spriteBatch, new Vector2(Game1.screenSize.X / 2, 255), Resources.FontMain, 0.5f, "You absolute monster. I love you.", Color.White);

            HelperUtils.DrawStringCentered(spriteBatch, new Vector2(Game1.screenSize.X / 2, 300), Resources.FontMain, 0.5f, "You got a Score of: " + achievedScore, Resources.ColorHud);

            HelperUtils.DrawStringCentered(spriteBatch, new Vector2(Game1.screenSize.X / 2, 345), Resources.FontMain, 0.5f, "Press Continue to view High Scores", Color.White);
            HelperUtils.DrawStringCentered(spriteBatch, new Vector2(Game1.screenSize.X / 2, 375), Resources.FontMain, 0.5f, "Press Exit to be a Monster again", Color.White);



            // Draw the cursor
            Vector2 cursorOffset = new Vector2(Resources.Pointer.Width, Resources.Pointer.Height);
            spriteBatch.Draw(Resources.Pointer, PlayerInput.mousePosition - (cursorOffset / 2), Color.White);

            buttonHighScore.Draw(spriteBatch);
            buttonReturn.Draw(spriteBatch);


            spriteBatch.End();

        }
    }

    class LevelHighScores : RC_GameStateParent
    {

        private Color colorWin = new Color(75, 174, 204);
        private HighScoreData data;
        private int timeWaitTicks = 15;
        private int timeWaitCurrent = 0;

        public override void EnterLevel(int fromLevelNum)
        {
            LoadContent();
        }

        public override void LoadContent()
        {
            data = HighScoreManager.LoadHighScores("highscores.xml");
            timeWaitCurrent = timeWaitTicks;
        }

        public override void Update(GameTime gameTime)
        {
            PlayerInput.Update();

            if (timeWaitCurrent <= 0)
            {

                if (PlayerInput.anyPressed() || PlayerInput.anyOldMousePressed())
                {
                    gameStateManager.setLevel(4);
                }
            }


            if (timeWaitCurrent > 0)
            {
                timeWaitCurrent--;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            graphicsDevice.Clear(colorWin);

            spriteBatch.Begin();

            HelperUtils.DrawStringCentered(spriteBatch, new Vector2(Game1.screenSize.X / 2, 75), Resources.FontMainTitle, 1f, "Highscores Table:", Color.White);

            int yDisplay = 135;

            HelperUtils.DrawStringRight(spriteBatch, new Vector2(Game1.screenSize.X / 2 - 95, yDisplay), Resources.FontMain, 0.5f, "Place:", Color.White);
            HelperUtils.DrawStringLeft(spriteBatch, new Vector2(Game1.screenSize.X / 2 - 75, yDisplay), Resources.FontMain, 0.5f, "Score:", Color.White);
            HelperUtils.DrawStringLeft(spriteBatch, new Vector2(Game1.screenSize.X / 2 + 65, yDisplay), Resources.FontMain, 0.5f, "Level:", Color.White);

            for (int i = 0; i < data.count; i++)
            {
                yDisplay += 30;
                HelperUtils.DrawStringRight(spriteBatch, new Vector2(Game1.screenSize.X / 2 - 95, yDisplay), Resources.FontMain, 0.5f, i+1 + ".", Color.White);
                HelperUtils.DrawStringLeft(spriteBatch, new Vector2(Game1.screenSize.X / 2 - 75, yDisplay), Resources.FontMain, 0.5f, "" + data.score[i], Color.White);
                HelperUtils.DrawStringLeft(spriteBatch, new Vector2(Game1.screenSize.X / 2 + 65, yDisplay), Resources.FontMain, 0.5f, "" + data.level[i], Color.White);
            }

            HelperUtils.DrawStringCentered(spriteBatch, new Vector2(Game1.screenSize.X / 2, 515), Resources.FontMain, 0.5f, "Press any key to return to the Main Menu.", Color.White);
            HelperUtils.DrawStringCentered(spriteBatch, new Vector2(Game1.screenSize.X / 2, 545), Resources.FontMain, 0.5f, "Its your decision afterall", Color.White);


            // Draw the cursor
            Vector2 cursorOffset = new Vector2(Resources.Pointer.Width, Resources.Pointer.Height);
            spriteBatch.Draw(Resources.Pointer, PlayerInput.mousePosition - (cursorOffset / 2), Color.White);


            spriteBatch.End();
        }
    }

    class LevelLoad : RC_GameStateParent
    {

        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(GameTime gameTime)
        {

        }
    }
}
