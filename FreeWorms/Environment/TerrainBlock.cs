
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;

namespace FreeWorms.Environment
{
    public class TerrainBlock
    {

        public Body Body;
        public Rectangle Rectangle;


        public TerrainBlock(Body body, Rectangle rect)
        {

            Body = body;
            Rectangle = rect;
            

        }

    }
}
