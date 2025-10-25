
using Gamelib.Image;
using FreeWorms.Animation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FreeWorms.ContentManager
{
   public class SpriteManager
    {

        
        public List<Sprite> Sprites;
        public List<Frame> Frames;
        public List<Particle> Particles;

      public SpriteManager()
        {

            Sprites = new List<Sprite>();
            Frames = new List<Frame>();
            Particles = new List<Particle>();
        }



    }


}
