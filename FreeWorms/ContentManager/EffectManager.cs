using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FreeWorms.Animation;
using Microsoft.Xna.Framework;

namespace FreeWorms.ContentManager
{
    public class EffectManager
    {
        public List<Effect> GameEffects;

        public EffectManager()
        { 
        
            GameEffects = new List<Effect>();
        
        
        }

        public void Add(Effect effect)
        {
            this.GameEffects.Add(effect);
        }


        public void stopAll()
        {
            for (var i = this.GameEffects.Count - 1; i >= 0; i--)
            {
                this.GameEffects[i].Finished = true;
            }
        }


        
        public void Update(GameTime gametime)
        {
            for (var i = this.GameEffects.Count - 1; i >= 0; i--)
            {
                this.GameEffects[i].Update(gametime);

                //TODO deleting while looping??
                if (this.GameEffects[i].Finished == true)
                {
                    GameEffects.RemoveAt(i);
                }

            }


        }

    }
}
