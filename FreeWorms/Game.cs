using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using FreeWorms.Environment;
using FreeWorms.ContentManager;
using FarseerPhysics.Dynamics;
using System.IO;

namespace FreeWorms
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class WormsGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        ScreenManager screenManager;
        public GameContent GameContent;
        

        public WormsGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = Global.SCREEN_WIDTH;
            graphics.PreferredBackBufferHeight = Global.SCREEN_HEIGHT;


            IsMouseVisible = true;


            screenManager = new ScreenManager(this, graphics, Services, Content);
            Components.Add(screenManager);

            //Activate screen
            screenManager.AddScreen(new BackgroundScreen(), PlayerIndex.One);
            screenManager.AddScreen(new MainMenuScreen(), PlayerIndex.One);

            Global.Game = this;

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            

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


            GameContent = new GameContent(GraphicsDevice);

            GameContent.Init(Content);

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);




            base.Draw(gameTime);
        }
    }
}
