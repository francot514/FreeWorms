using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace FreeWorms
{
    public static class Utils
    {

        public static System.Drawing.Color Transparent = System.Drawing.Color.FromArgb(255, 0, 0, 0);

        public static Texture2D GetTexture(GraphicsDevice graphic, string file)
        {
            Texture2D Texture = new Texture2D(graphic, 0, 0);

            if (File.Exists(file))
                Texture = Texture2D.FromStream(graphic, File.Open(file, FileMode.Open));

            return Texture;


        }

        public static Color[] GetPixels(Bitmap bitmap)
        {
            int cols = bitmap.Width;
            int rows = bitmap.Height;

            StreamWriter writer = new StreamWriter(File.Create("pixels.txt"));

            Color[] pixels = new Color[(bitmap.Width * bitmap.Height  )- 1];

            for (int i = 0; i < rows; i++)
            {
                writer.WriteLine();

                for (int j = 0; j < cols; j++)
                {
                    
                    pixels[i*j] = bitmap.GetPixel(j, i);


                    if (pixels[i * j].B == 0
                        && pixels[i * j].R == 0 && pixels[i * j].G == 0)
                        writer.Write(0);
                    else
                        writer.Write(1);
                }
            }



            return pixels;

        }


    }
}
