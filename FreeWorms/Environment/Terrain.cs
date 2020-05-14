using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Factories;
using System.IO;

namespace FreeWorms.Environment
{
   public class Terrain
    {
       public List<TerrainBlock> Blocks;
       public Texture2D Canvas, Image, Buffer;
       public System.Drawing.Bitmap Bitmap;
       public int Scale;
       public Vector2 Offset;
       public Rectangle Bounds;
       public Boundary Boundary;
       public World World;
       private StreamWriter writer;


       public Terrain(World world, Texture2D image, System.Drawing.Bitmap bmp, int scale)
           {
               writer = new StreamWriter(File.Create("blocks.txt"));
               World = world;
               Image = image;
               Bitmap = bmp;
               Scale = scale;
               Offset = new Vector2(2300, 1300);
               Bounds = new Rectangle(0, 0, image.Width, image.Height);
              // Boundary = new Boundary(world, image.Width, image.Height);
               Blocks = new List<TerrainBlock>();

               CreateTerrainPhysics(0, 0, image.Width, image.Height, world, scale);

           }

        public void CreateTerrainPhysics(int x, int y, int width, int height, World world, int scale)
        {

            //width = width * 4;
            var data = Utils.GetPixels(Bitmap);
            var rectWidth = 0;
           // var rectHeight = 5;
            var Fixture = new Fixture();
            Fixture.Friction = 1.0f;
            Fixture.Shape = new PolygonShape(1.0f);
            

            for (int yPos = y; yPos < height; yPos+= 5)
            {
                rectWidth = 0;

                for (var xPos = x; xPos < width; xPos+= 1)
                {

                    //if (data[xPos + (yPos * width)].A == 255)
                    if (data[xPos * yPos].G != 0 && data[xPos * yPos].B != 0 && data[xPos * yPos].R != 0) //if not alpha pixel
                    {
                        rectWidth++;

                        if (rectWidth >= width)
                        {

                            CreateBlock(Fixture, rectWidth, 5, xPos, yPos);
                            rectWidth = 0;
                        }


                    }
                    else 
                    {
                        
                        if (rectWidth > 1)
                        {
                            CreateBlock(Fixture, rectWidth, 5, xPos, yPos);
                            rectWidth = 0; //reset rect
                        }
                    }

                }
            }

            //writer.Close();

        }


        private void CreateBlock(Fixture fix, int width, int height, int x, int y)
        {
            

            var body = BodyFactory.CreateRectangle(World, width, height, 1.0f);
            body.IsStatic = true;
            // body.Position = new Vector2(x - (width / 4), y - height);
            body.Position = new Vector2(((x / 4) - (width / 2)), y - height);
           
            
            body.Restitution = 1.0f;
            body.CollidesWith = Category.All;
            body.CollisionCategories = Category.All;

            Blocks.Add(new TerrainBlock(body, new Rectangle(x,y, width, height)));

            //writer.WriteLine(x + "," + y + " " + width);

            //body.CreateFixture(fix.Shape).Shape = new PolygonShape(1.0f);


        }


    }

}
