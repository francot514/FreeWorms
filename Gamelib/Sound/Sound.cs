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
		public int Volume = 0;


        public SoundEffect(SoundEffectInstance instance)
		{

                Loop = false;
                Finished = false;
				Instance = instance;
				Volume = 10;
				

        }
			
		public void SetLoop(bool loop)
        {
            Loop = loop;
        }
            
    }
}
 
