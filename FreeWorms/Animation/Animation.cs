using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FreeWorms.Animation
{
   public class Animation
    {
       public string Name;
       public SpriteImage Sprite;
       public float FrameTime;
       public int currentFrameY, frameIncremeter, frameHeight;
       public bool Finished, Loop, isSpriteLocked;
       public DateTime UpdateTime, accumulateDelta;
       public Vector2 Velocity;

       public Animation(string name, SpriteImage sprite, bool loop)
       {
           Name = name;
           frameIncremeter = 1;
           accumulateDelta = DateTime.Now;
           Sprite = sprite;
           Loop = loop;
           frameHeight = Sprite.Texture.Height / Sprite.Frames;

        }



        public int getCurrentFrame()
       {
           return this.currentFrameY;
       }

       public void setCurrentFrame(int frame)
       {
           if (frame >= 0 && frame < this.Sprite.Frames)
           {
               this.currentFrameY = frame;
           }

       }

       public int getTotalFrames()
       {
           return this.Sprite.Frames;
       }

        public void Update(GameTime gameTime)
        {

            float time = 0;
            DateTime startTime = new DateTime();
            TimeSpan elapsedTime = new TimeSpan();


            accumulateDelta = DateTime.Now + gameTime.ElapsedGameTime;

            // Process passing time.
            time += Convert.ToSingle(elapsedTime.Seconds);
            while (time > FrameTime)
            {
                time -= FrameTime;

                // Advance the frame index; looping or clamping as appropriate.
                if (Loop)
                {
                    currentFrameY = (currentFrameY + 1) % Sprite.Frames;

                    if (currentFrameY == Sprite.Frames - 1)
                        currentFrameY = 0;
                }
                else
                {
                    currentFrameY = Math.Min(currentFrameY + 1, currentFrameY - 1);

                    if (currentFrameY == Sprite.Frames - 1)
                        Finished = false;
                }
            }


        }


        public int  getFrameHeight()
    {       
        return this.frameHeight;
    }

    public int getFrameWidth()
    {       
       return this.Sprite.Texture.Width;
    }

     public void DrawOnCenter(SpriteBatch sbatch, int x, int y, SpriteImage sprite)
    {
        if (Finished == false)
        {
            sbatch.Begin();
            
            int centerx = (sprite.Texture.Width - this.getFrameWidth()) / 2;
            int centery = (sprite.Texture.Height - this.getFrameHeight()) / 2;
            
            sbatch.Draw(Sprite.Texture, new Rectangle(x, y, sprite.Texture.Width, sprite.Texture.Height), Color.White);

            Draw(sbatch, centerx, centery);

            sbatch.End();

        }
    }

    public void Draw(SpriteBatch sbatch, int x, int y) {

        var tmpCurrentFrameY = //Math.Floor((double)
                this.currentFrameY;

        sbatch.Draw(Sprite.Texture, new Rectangle(x, y, Sprite.Texture.Width, frameHeight), new Rectangle(0, (int)tmpCurrentFrameY * frameHeight,Sprite.Texture.Width, frameHeight), Color.White);

    }


    }
}
