using FreeWorms.Animation;
using Gamelib.Image;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FreeWorms.ContentManager
{
    public static class AnimManager
    {

        public static Animation.Animation[] Animations;
        public static DIRManager DirManager;


        public static void Config(DIRManager dirManager)
        {

            DirManager = dirManager;

            Animations = new Animation.Animation[10];


        }

        public static void Init(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            var WormIdle = content.Load<Texture2D>("Sprites\\widle");
            Animations[0] = new Animation.Animation("Idle",new SpriteImage(WormIdle, 1), false);

            var WormWalk = content.Load<Texture2D>("Sprites\\wwalk");
            Animations[1] = new Animation.Animation("Walk", new SpriteImage(WormWalk, 14), true);

        }
    }

}
