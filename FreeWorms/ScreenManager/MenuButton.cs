using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FreeWorms
{
   public class MenuButton
    {

        public Texture2D Image;
        public Rectangle Bounds;
        public bool Pressed;


        public MenuButton(Rectangle rectangle)
        {

            Bounds = rectangle;
            Pressed = false;

        }


    }
}
