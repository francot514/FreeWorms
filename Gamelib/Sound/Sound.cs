using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Audio;

namespace Gamelib.Sound
{
    public class SoundEffect
    {
        public SoundEffectInstance Instance;
        public bool Loop, Finished;

        public SoundEffect(){

                Loop = false;
                Finished = false;

            }
            
    }
}
 
