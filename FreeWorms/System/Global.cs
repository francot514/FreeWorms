
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace FreeWorms
{

   public enum Direction
    {
        left = 0,
        right = 1
    }

   public static class Global
    {

        public static int SCREEN_WIDTH = 800;
        public static int SCREEN_HEIGHT = 600;
        public static int LevelWidth, LevelHeight, OffsetX, OffsetY;
        public static GraphicsDevice GraphicsDevice;
        public static Microsoft.Xna.Framework.Content.ContentManager Content;
        public static SpriteBatch SpriteBatch;
        public static WormsGame Game;
        public static double ScreenWidth, ScaleWidth;
        public static double ScreenHeight, ScaleHeight;
        public static bool DebugMode;

    }
}
