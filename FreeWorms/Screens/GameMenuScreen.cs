using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System.Xml.Serialization;


namespace FreeWorms
{

    class GameMenuScreen: GameScreen
    {


        private SpriteFont menuFont;
        private int LevelIndex;

        private float pauseAlpha;
        // Meta-Maze game state.

        private bool wasContinuePressed;

        private GamePadState gamePadState;
        private KeyboardState keyboardState;
        private TouchCollection touchState;
        private MouseState mouseState;
        private string DebugInfo;
        private MenuButton[] MenuButtons;
        private Texture2D Title;

        private static readonly TimeSpan WarningTime = TimeSpan.FromSeconds(30);




		public GameMenuScreen(SpriteFont font)
        {

            menuFont = font;
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            Global.DebugMode = false;
            MenuButtons = new MenuButton[4];
            DebugInfo = "";

        }




        public override void LoadContent()
        {

            Title = Utils.GetTexture(ScreenManager.GraphicsDevice, "graphics/MainMenu/wgn.bmp");

            MenuButtons[0] = new MenuButton(new Rectangle(50, 200, 240, 150));
            MenuButtons[1] = new MenuButton(new Rectangle(400, 200, 240, 150));
            MenuButtons[2] = new MenuButton(new Rectangle(50, 400, 240, 150));
            MenuButtons[3] = new MenuButton(new Rectangle(400, 400, 240, 150));

            MenuButtons[0].Image = Utils.GetTexture(ScreenManager.GraphicsDevice,"graphics/MainMenu/1up.bmp");
            MenuButtons[1].Image = Utils.GetTexture(ScreenManager.GraphicsDevice, "graphics/MainMenu/multi.bmp");
            MenuButtons[2].Image = Utils.GetTexture(ScreenManager.GraphicsDevice, "graphics/MainMenu/net.bmp");
            MenuButtons[3].Image = Utils.GetTexture(ScreenManager.GraphicsDevice, "graphics/MainMenu/options.bmp");


            ScreenManager.Game.ResetElapsedTime();


           

        }



        public override void Update(Microsoft.Xna.Framework.GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {


            base.Update(gameTime, otherScreenHasFocus, false);


            Point Cursor = new Point(mouseState.X, mouseState.Y);

            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
            {
                pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            }
            else
            {
                pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);
            }

            HandleInput(Cursor);

            if (MenuButtons[0].Bounds.Contains(Cursor))
                DebugInfo = "Play singleplayer mode.";
            else
                DebugInfo = "FreeFreeWorms Project 2019.";


        }

        private void HandleInput(Point Cursor)
        {
            // get all of our input states
            mouseState = Mouse.GetState();
            keyboardState = Keyboard.GetState();
            gamePadState = GamePad.GetState(PlayerIndex.One);
            touchState = TouchPanel.GetState();
           


            // Exit the game when back is pressed.
            if (keyboardState.IsKeyDown(Keys.Escape) ||
               gamePadState.IsButtonDown(Buttons.Start))
            {
                ExitScreen ();
                ScreenManager.AddScreen(new BackgroundScreen(), PlayerIndex.One);
                ScreenManager.AddScreen(new MainMenuScreen(), PlayerIndex.One);


            }
            else if (keyboardState.IsKeyDown(Keys.F1))
            {

                Global.DebugMode = !Global.DebugMode;
                
            }



            bool continuePressed =
                keyboardState.IsKeyDown(Keys.Space) ||
                gamePadState.IsButtonDown(Buttons.A);
                 
           if (mouseState.LeftButton == ButtonState.Pressed)
            {

                if (MenuButtons[0].Bounds.Contains(Cursor))
                    LoadingScreen.Load(ScreenManager, true, PlayerIndex.One, new GameplayScreen(ScreenManager));

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


            ScreenManager.SpriteBatch.Begin();

            base.Draw(gameTime);

            DrawHud(ScreenManager.SpriteBatch);

            ScreenManager.SpriteBatch.Draw(Title, new Rectangle(150, 20, Title.Width,  Title.Height), Color.White);

            ScreenManager.SpriteBatch.Draw(MenuButtons[0].Image, MenuButtons[0].Bounds, Color.White);
            ScreenManager.SpriteBatch.Draw(MenuButtons[1].Image, MenuButtons[1].Bounds, Color.White);
            ScreenManager.SpriteBatch.Draw(MenuButtons[2].Image, MenuButtons[2].Bounds, Color.White);
            ScreenManager.SpriteBatch.Draw(MenuButtons[3].Image, MenuButtons[3].Bounds, Color.White);

            ScreenManager.SpriteBatch.End();

        }

        private void DrawHud(SpriteBatch spriteb)
        {
            
            Rectangle titleSafeArea = new Rectangle(100,550, 640, 200);
            Vector2 hudLocation = new Vector2(titleSafeArea.X, titleSafeArea.Y);
            Vector2 center = new Vector2(titleSafeArea.X + titleSafeArea.Width / 2f, titleSafeArea.Y + titleSafeArea.Height / 2f);

            Color timeColor = Color.DarkRed;
            //if (Maze.TimeRemaining > WarningTime || Maze.CurrentRoom.ReachedExit || Convert.ToInt32(Maze.TimeRemaining.TotalSeconds) % 2 == 0)
           // {
               // timeColor = Color.White;
            //}
           // else
            //{
               // timeColor = Color.Red;
            //}
            DrawShadowedString(menuFont, DebugInfo, hudLocation, timeColor);
            

           
            

        }

        private void DrawShadowedString(SpriteFont font, string value, Vector2 position, Color color__1)
        {
            ScreenManager.SpriteBatch.DrawString(font, value, position + new Vector2(1f, 1f), Color.Black);
            ScreenManager.SpriteBatch.DrawString(font, value, position, color__1);
        }
    }
}