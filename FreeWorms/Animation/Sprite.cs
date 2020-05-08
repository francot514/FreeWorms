using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Gamelib.Image;

namespace FreeWorms.Animation
{
    public class SpriteImage
    {

        public string Name;
        public int Frames, MsPerFrame, frameIncremeter;
        public Texture2D Texture;
        public Vector2 Position;

        public SpriteImage(Texture2D texture, int frames)
        {

            Frames = frames;
            Texture = texture;
            Position = new Vector2(0, 0);
        }
    }

}
