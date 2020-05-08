using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace FreeWorms.Environment
{
    public class Map
    {
        public Texture2D Image;
        public Bitmap Bitmap;
        public Terrain Terrain;
        public string Name;
        public int SpawnPoint;

        public Map(Texture2D image, Bitmap bmp)
            {

                Name = "";
                SpawnPoint = 0;
                Image = image;
                Bitmap = bmp;   
            
            }



        public void SetTerrain(World world)
        {

            Terrain = new Terrain(world, Image, Bitmap, 1);
        }

    }
}
