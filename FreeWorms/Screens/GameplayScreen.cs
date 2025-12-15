using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System.Xml.Serialization;
using FarseerPhysics.Dynamics;
using FreeWorms.Environment;
using Gamelib.Image;
using FreeWorms.GameManager;
using FreeWorms.Data;


namespace FreeWorms
{

    class GameplayScreen : GameScreen
    {


        private SpriteFont hudFont;
        private int LevelIndex;
        public Camera Camera;
        public Map Map;

        public GameState GameState;
        public Player[] Players; 

        private Texture2D MapImage, Block;
        private System.Drawing.Bitmap Bitmap;
        private List<Sprite> SpriteImages;

        private float pauseAlpha;
        // Meta-Maze game state.

        private bool wasContinuePressed;

        private GamePadState gamePadState;
        private KeyboardState keyboardState;
        private TouchCollection touchState;
        private MouseState mouseState;
        private string DebugInfo;
        private int CurrentPlayer = 0;
        //private AccelerometerState accelerometerState;
        // When the time remaining is less than the warning time, it blinks on the hud

        private static readonly TimeSpan WarningTime = TimeSpan.FromSeconds(30);

        private Vector2 CamPos;



		public GameplayScreen(ScreenManager screenManager)
        {
            ScreenManager = screenManager;

            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            Global.DebugMode = false;


            SpriteImages = Global.Game.GameContent.DIRManager.SpriteManager.Sprites;
            MapImage = Global.Game.GameContent.ImageManager.GetImage(ScreenManager.GraphicsDevice, 1);
            Bitmap = Global.Game.GameContent.ImageManager.Bitmaps[1];

            //MapImage.SaveAsPng(File.Create("map.png"), MapImage.Width, MapImage.Height);
            Map = new Map(MapImage, Bitmap);

            MapState.Init();

            Physics.Init();

            Players = new Player[4];

            for (int i = 0; i < 4; i++)
                Players[i] = new Player(i);

            LevelIndex = 0;

            DebugInfo = "";

            GameState = new GameState();

            GameState.Init(Players);

        }




        public override void LoadContent()
        {
            ScreenManager.Game.ResetElapsedTime();

            hudFont = ScreenManager.content.Load<SpriteFont>("Fonts/Hud");

            Block = ScreenManager.content.Load<Texture2D>("Sprites/rect");

            Map.SetTerrain(Physics.World);

            Camera = new Camera(2400, 1000, 
                ScreenManager.GraphicsDevice.Viewport.Width, ScreenManager.GraphicsDevice.Viewport.Height);

            CamPos = new Vector2(Map.Terrain.Image.Width / 2, Map.Terrain.Image.Height / 2);



            Camera.setX((int)CamPos.X);
            Camera.setY((int)CamPos.Y);


            Camera.Pan(CamPos);

        }



        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {


            base.Update(gameTime, otherScreenHasFocus, false);

          

            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
            {
                pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            }
            else
            {
                pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);
            }


            Physics.Update(gameTime);


            Camera.Update();

            if (Players != null)
            for (int i = 0; i < Players.Length; i++)
                if (Players[i] != null)
                    Players[i].Update(gameTime);

            HandleInput();

        }

        private void HandleInput()
        {
            // get all of our input states
            keyboardState = Keyboard.GetState();
            gamePadState = GamePad.GetState(PlayerIndex.One);
            touchState = TouchPanel.GetState();
            mouseState = Mouse.GetState();
            //accelerometerState = Accelerometer.GetState();


            // Exit the game when back is pressed.
            if (keyboardState.IsKeyDown(Keys.Escape) ||
               gamePadState.IsButtonDown(Buttons.Start))
            {
                ExitScreen();
                ScreenManager.AddScreen(new BackgroundScreen(), PlayerIndex.One);
                ScreenManager.AddScreen(new MainMenuScreen(), PlayerIndex.One);


            }
            else if (keyboardState.IsKeyDown(Keys.F1))
            {

                Global.DebugMode = !Global.DebugMode;

            }
            else if (keyboardState.IsKeyDown(Keys.J))
            {

                Camera.ToPan = false;
                Camera.IncrementX(-15);

            }
            else if (keyboardState.IsKeyDown(Keys.I))
            {
                Camera.ToPan = false;
                Camera.IncrementY(-15);

            }
            else if (keyboardState.IsKeyDown(Keys.K))
            {
                Camera.ToPan = false;
                Camera.IncrementY(15);

            }
            else if (keyboardState.IsKeyDown(Keys.L))
            {
                Camera.ToPan = false;
                Camera.IncrementX(15);

            }

            bool continuePressed =
                keyboardState.IsKeyDown(Keys.Space) ||
                gamePadState.IsButtonDown(Buttons.A);

            int MouseX = mouseState.X;
            int MouseY = mouseState.Y;


            if (mouseState.X > MouseX)
            {
                Camera.ToPan = false;
                Camera.IncrementX(15);

            }
            else if (mouseState.X < MouseX)
            {
                Camera.ToPan = false;
                Camera.IncrementX(-15);

            }

            if (Players != null)
                for (int i = 0; i < Players.Length; i++)
                    if (Players[CurrentPlayer] != null)
                    {
                        
                        Players[CurrentPlayer].HandleInput(keyboardState, gamePadState);

                    }

            wasContinuePressed = continuePressed;
        }


        public override void Draw(GameTime gameTime)
        {



            //Matrix cameraTransform = Matrix.CreateTranslation(-Maze.cameraPosition, 0.0f, 0.0f);
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;

            Global.ScreenWidth = viewport.Width;
            Global.ScreenHeight = viewport.Height;

            if (viewport.Width >= 1280)
                Global.ScaleWidth = 4;
            else if (viewport.Width >= 1024)
                Global.ScaleWidth = 3;
            else if (viewport.Width >= 800)
                Global.ScaleWidth = 2;
            else if (viewport.Width >= 600)
                Global.ScaleWidth = 1;

            if (viewport.Height >= 960)
                Global.ScaleHeight = 4;
            else if (viewport.Height >= 800)
                Global.ScaleHeight = 3;
            else if (viewport.Height >= 600)
                Global.ScaleHeight = 2;
            else if (viewport.Height >= 480)
                Global.ScaleHeight = 1;


            ScreenManager.SpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend,
                null, null, null, null, Camera.TranslationMatrix);


            base.Draw(gameTime);

            if (MapImage != null)
                ScreenManager.SpriteBatch.Draw(MapImage,  new Rectangle(0, 0, MapImage.Width, MapImage.Height), Color.White);

            if (Map != null)
                foreach (TerrainBlock block in Map.Terrain.Blocks)
                    ScreenManager.SpriteBatch.Draw(Block, new Rectangle((int)block.Body.Position.X, (int)block.Body.Position.Y,
                       block.Rectangle.Width, block.Rectangle.Height), Color.White);
            ScreenManager.SpriteBatch.Draw(Block, new Rectangle((int)Map.Terrain.Boundary.Body.Position.X, (int)Map.Terrain.Boundary.Body.Position.Y, Map.Terrain.Image.Width, Map.Terrain.Image.Height), Color.White);


            if (Players != null)
                for (int i = 0; i < Players.Length; i++)
                    if (Players[i] != null)
                        Players[i].Draw(ScreenManager.SpriteBatch);

            DrawHud(ScreenManager.SpriteBatch, gameTime);


           ScreenManager.SpriteBatch.End();

        }

        private void DrawHud(SpriteBatch spriteb, GameTime gameTime)
        {

            Rectangle titleSafeArea = new Rectangle(100, 100, 640, 200);
            Vector2 hudLocation = new Vector2(titleSafeArea.X + Camera.Position.X, titleSafeArea.Y + Camera.Position.Y);
            Vector2 center = new Vector2(titleSafeArea.X + titleSafeArea.Width / 2f, titleSafeArea.Y + titleSafeArea.Height / 2f);

            // Draw time remaining. Uses modulo division to cause blinking when the
            // er is running out of time.
            string timeString = "TIME: " + gameTime.ElapsedGameTime.TotalSeconds;
            Color timeColor = Color.Red;
            //if (Maze.TimeRemaining > WarningTime || Maze.CurrentRoom.ReachedExit || Convert.ToInt32(Maze.TimeRemaining.TotalSeconds) % 2 == 0)
            // {
            // timeColor = Color.White;
            //}
            // else
            //{
            // timeColor = Color.Red;
            //}

            string debug = Players[0].Team.Worms[Players[0].Team.Current].Body.Position.X + "  " +
                Players[0].Team.Worms[Players[0].Team.Current].Body.Position.Y;

            if (Global.DebugMode)
                DrawShadowedString(hudFont, timeString, hudLocation, timeColor);
            else
                DrawShadowedString(hudFont, debug, hudLocation, timeColor);
            // Draw score
            // float timeHeight = hudFont.MeasureString(timeString).Y;
            // DrawShadowedString(hudFont, "SCORE: " + Maze.Score.ToString(), hudLocation + new Vector2(0f, timeHeight * 1.2f), Color.White);

            // Determine the status overlay message to show.
            Texture2D status = null;


            if (status != null)
            {
                // Draw status message.
                Vector2 statusSize = new Vector2(status.Width, status.Height);
                spriteb.Draw(status, center - statusSize / 2, Color.White);
            }
            

        }

        private void DrawShadowedString(SpriteFont font, string value, Vector2 position, Color color__1)
        {
            //ScreenManager.SpriteBatch.DrawString(font, value, position + new Vector2(1f, 1f), Color.Black);
            ScreenManager.SpriteBatch.DrawString(font, value, position, color__1);
        }
    }
}