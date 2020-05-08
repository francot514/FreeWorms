using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FreeWorms.Animation
{
   public class Effect
    {

       public int X, Y;
       public Vector2 Center;
       public Animation eclipse, circle;
       public bool Finished;
       public Particle[] Particles;
       public Random Random = new Random();

       public Effect(int x, int y)
           {

               X = x;
               Y = y;
               Particles = new Particle[10];
               Finished = false;

                for (var p = 9; p >= 0; p--)
                {
                    
                    Particles[p] = new Particle("",null,false);
                
                    Particles[p].initialPos =  new Vector2(x+Center.X,y+Center.Y);
                    Particles[p].initialVel =  new Vector2((float)Random.Next(-300,300), (float)Random.Next(-500,0));
                }
             

           }

            public void  onAnimationFinish(bool val)
            {
                this.Finished = val;
            }


            public void Update(GameTime gametime)
            {
                this.eclipse.Update(gametime);
                this.circle.Update(gametime);


                for (var p = this.Particles.Length - 1; p >= 0; p--)
                {
                    this.Particles[p].Update(gametime);
                }

                //Particles have the longest animation so once they are finished we can make the effect for deletion
                this.Finished = this.Particles[0].Finished;

                
            }




    }
}
