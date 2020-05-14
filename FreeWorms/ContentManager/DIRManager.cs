using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gamelib.Common;
using Gamelib.Image;
using System.IO;

namespace FreeWorms.ContentManager
{
    public class DIRManager
    {

        public List<DIR> GameDirs;
        public List<SPR> SPR;
        public List<DIR.DirEntry> Entries;
        public string[] Directories;
        public SpriteManager SpriteManager;

        public DIRManager()
            {

                GameDirs = new List<DIR>();
                SPR = new List<SPR>();
                Entries = new List<DIR.DirEntry>();
                Directories = new string[] 
                
                 {
                     "DATA/Gfx/Gfx.DIR",
                    "DATA/Gfx/Gfx0.DIR",
                    "DATA/Gfx/Gfx1.DIR"
                };

                SpriteManager = new SpriteManager();

            }


        public void Init()
        {



            foreach (string Dir in Directories)
            {
                var DIR = new DIR(Dir);

                GameDirs.Add(DIR);


            }

            for (int i = 0; i < GameDirs.Count; i++)
            {

                Entries.AddRange(GameDirs[i].Entries);
            }



        
        }

    }
}
