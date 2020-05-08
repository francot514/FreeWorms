using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Content;

namespace FreeWorms
{

public class ScreenManager : DrawableGameComponent
{
    #region "Fields"

    private List<GameScreen> screens = new List<GameScreen>();

    private List<GameScreen> screensToUpdate = new List<GameScreen>();

    private InputState input = new InputState();
    private SpriteBatch m_spriteBatch;
    private SpriteFont m_font;

    private Texture2D blankTexture;

    private bool isInitialized;
    private bool m_traceEnabled;
    public readonly GameServiceContainer services;
    public readonly Microsoft.Xna.Framework.Content.ContentManager content;
    #endregion
    public readonly GraphicsDeviceManager graphics;

    public SpriteBatch SpriteBatch
    {
        get { return m_spriteBatch; }
    }


    /// <summary>
    /// A default font shared by all the screens. This saves
    /// each screen having to bother loading their own local copy.
    /// </summary>
    public SpriteFont Font
    {
        get { return m_font; }
    }


    /// <summary>
    /// If true, the manager prints out a list of all the screens
    /// each time it is updated. This can be useful for making sure
    /// everything is being added and removed at the right times.
    /// </summary>
    public bool TraceEnabled
    {
        get { return m_traceEnabled; }
        set { m_traceEnabled = value; }
    }

    public ScreenManager(Game game, GraphicsDeviceManager graphicsdevice, GameServiceContainer service, Microsoft.Xna.Framework.Content.ContentManager m_content)
        : base(game)
    {

        graphics = graphicsdevice;
        services = service;
        content = m_content;
        TouchPanel.EnabledGestures = GestureType.None;
        // we must set EnabledGestures before we can query for them, but
        // we don't assume the game wants to read them.

    }


    public override void Initialize()
    {
        base.Initialize();

        isInitialized = true;
    }

    protected override void LoadContent()
    {
       // Load content belonging to the screen manager.
       Microsoft.Xna.Framework.Content.ContentManager content = Game.Content;

        m_spriteBatch = new SpriteBatch(GraphicsDevice);
        m_font = content.Load<SpriteFont>("Fonts\\Menu");
        blankTexture = content.Load<Texture2D>("Interface\\blank");

        // Tell each of the screens to load their content.
        foreach (GameScreen screen in screens)
        {
            screen.LoadContent();
        }
    }

    protected override void UnloadContent()
    {
        // Tell each of the screens to unload their content.
        foreach (GameScreen screen in screens)
        {
            screen.UnloadContent();
        }
    }

    public override void Update(GameTime gameTime)
    {
        // Read the keyboard and gamepad.
        input.Update();

        // Make a copy of the master screen list, to avoid confusion if
        // the process of updating one screen adds or removes others.
        screensToUpdate.Clear();

        foreach (GameScreen screen in screens)
        {
            screensToUpdate.Add(screen);
        }

        bool otherScreenHasFocus = !Game.IsActive;
        bool coveredByOtherScreen = false;

        // Loop as long as there are screens waiting to be updated.
        while (screensToUpdate.Count > 0)
        {
            // Pop the topmost screen off the waiting list.
            GameScreen screen = screensToUpdate[screensToUpdate.Count - 1];

            screensToUpdate.RemoveAt(screensToUpdate.Count - 1);

            // Update the screen.
            screen.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            if (screen.ScreenState == ScreenState.TransitionOn || screen.ScreenState == ScreenState.Active)
            {
                // If this is the first active screen we came across,
                // give it a chance to handle input.
                if (!otherScreenHasFocus)
                {
                    screen.HandleInput(input);

                    otherScreenHasFocus = true;
                }

                // If this is an active non-popup, inform any subsequent
                // screens that they are covered by it.
                if (!screen.IsPopup)
                {
                    coveredByOtherScreen = true;
                }
            }
        }

        // Print debug trace?
        if (m_traceEnabled)
        {
            TraceScreens();
        }
    }

    private void TraceScreens()
    {
        List<string> screenNames = new List<string>();

        foreach (GameScreen screen in screens)
        {
            screenNames.Add(screen.GetType().Name);
        }

        Debug.WriteLine(string.Join(", ", screenNames.ToArray()));
    }

    public override void Draw(GameTime gameTime)
    {
        foreach (GameScreen screen in screens)
        {
            if (screen.ScreenState == ScreenState.Hidden)
            {
                continue;
            }

            screen.Draw(gameTime);
        }
    }


    /// <summary>
    /// Adds a new screen to the screen manager.
    /// </summary>
    public void AddScreen(GameScreen screen, System.Nullable<PlayerIndex> controllingPlayer)
    {
        screen.ControllingPlayer = controllingPlayer;
        screen.ScreenManager = this;
        screen.IsExiting = false;

        // If we have a graphics device, tell the screen to load content.
        if (isInitialized)
        {
            screen.LoadContent();
        }

        screens.Add(screen);

        // update the TouchPanel to respond to gestures this screen is interested in
        TouchPanel.EnabledGestures = screen.EnabledGestures;
    }

    public void RemoveScreen(GameScreen screen)
    {
        // If we have a graphics device, tell the screen to unload content.
        if (isInitialized)
        {
            screen.UnloadContent();
        }

        screens.Remove(screen);
        screensToUpdate.Remove(screen);

        // if there is a screen still in the manager, update TouchPanel
        // to respond to gestures that screen is interested in.

        if (screens.Count > 0)
        {
            TouchPanel.EnabledGestures = screens[screens.Count - 1].EnabledGestures;
        }

    }


    public GameScreen[] GetScreens()
    {
        return screens.ToArray();
    }

    public void FadeBackBufferToBlack(float alpha)
    {
        Viewport viewport = GraphicsDevice.Viewport;

        m_spriteBatch.Begin();

        m_spriteBatch.Draw(blankTexture, new Rectangle(0, 0, viewport.Width, viewport.Height), Color.Black * alpha);

        m_spriteBatch.End();
    }
 }
}