using Gamelib.Image;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace FreeWorms.ContentManager
{
    public class ImageManager
    {
        public Stream CurrentFile;
        public List<IMG> Images;
        public List<Bitmap> Bitmaps;
        public string ImagesFolder;

        public ImageManager(GraphicsDevice graphics)
        {
            Images = new List<IMG>();
            Bitmaps = new List<Bitmap>();
            ImagesFolder = "DATA/Image";
            DirectoryInfo DIR = new DirectoryInfo(ImagesFolder);
            foreach (FileInfo Img in DIR.GetFiles())
                if (Img.Extension == ".IMG")
                    Images.Add(new IMG(Img.FullName));

            foreach (IMG Im in Images)
                Bitmaps.Add(Im.Bitmap);

        }

        public Stream GetBitmap(int index)
        {
            Bitmap Bmp = Bitmaps[index];


            MemoryStream Stream = new MemoryStream();
            //var Stream = File.OpenWrite("image.bmp");
            Bmp.Save(Stream, System.Drawing.Imaging.ImageFormat.Bmp);

            return Stream;


        }

        public Texture2D GetImage(GraphicsDevice graphics, int index)
        {
            Texture2D Image = new Texture2D(graphics, 1, 1);

            Image = Texture2D.FromStream(graphics, GetBitmap(index));

            return Image;
        }

    }

}
