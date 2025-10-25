using Gamelib.Image;
using Gamelib.Sound;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeWorms.ContentManager
{
    public class SoundManager
    {
        public Stream CurrentFile;
        public List<SoundEffect> Sounds;
        public string SoundsFolder;

        public SoundManager()
        {

            Sounds = new List<SoundEffect>();
            SoundsFolder = "DATA/Sound";

        }

    }
}
