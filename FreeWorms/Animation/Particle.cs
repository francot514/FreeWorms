using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;


namespace FreeWorms.Animation
{
   public class Particle: Animation
    {


        public Vector2 initialPos, initialVel, position, velocity;
        public int WorldHeight;
        public Random Random = new Random();

        public Particle(string name, SpriteImage sprite, bool loop):  base(name, sprite, loop)
            {
                var initialx = (int)(Global.OffsetX - 900);
                var initialy =  (int)(Global.OffsetY - 220);
                initialPos = new Vector2(Random.Next(Global.LevelWidth), Random.Next(initialx, initialy));
                initialVel = new Vector2((float)(Random.Next(3, 7) * 0.4), 0);
                Sprite = sprite;
                Loop = loop;

            }

        public void Startphysics()
        {
            var t = 0.016;
            var g = new Vector2((float)0, (float)9.81);

            var at = g;
            Vector2.Add(ref at, ref Velocity, out Velocity);

            var vt = Velocity;
            Vector2.Multiply(ref vt, (float)t, out vt);
            Vector2.Add(ref Sprite.Position, ref vt, out vt);

        }

       public void Update(GameTime gametime)
           {

               base.Update(gametime);

               Startphysics();

                if (this.getCurrentFrame() >= this.getTotalFrames()-1)
                {
                    this.setCurrentFrame(this.getTotalFrames()-1);
                    this.frameIncremeter *= -1;

                } 
                else if (this.getCurrentFrame() <= 0)
                {
                this.setCurrentFrame(0);
                this.frameIncremeter *= -1;
                }

            this.position.X += this.velocity.X;

            if (this.position.X > WorldHeight)
            {
                this.position.X = 0;
            }

        }


    }
}
