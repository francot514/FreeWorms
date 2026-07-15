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
        public bool Loop = false;
        public int Volume = 0;

        public SoundEffect(SoundEffectInstance instance)
        {
            Instance = instance;
            Volume = 10;
        }

        public void SetLoop(bool loop)
        {
            Loop = loop;
        }

    }
}
 