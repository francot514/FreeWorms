using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FreeWorms.Environment
{
   public class Boundary
    {
        public int Width, Height;
        public int OWidth, OHeight;
        public int sideX, topY;
        public Fixture Fix;
        public Body Body, Bottom;

        public Boundary(World world, int width, int height)
        {

            Width = width;
            Height = height;

            topY = Height / 5;
            sideX = Width / 5;

            Fix = new Fixture();
            Fix.Friction = 1.0f;
            Fix.Restitution = 0;

            Vertices Ver = new Vertices(4);

            Ver.Add(new Vector2(Width, sideX * 2));
            Ver.Add(new Vector2(Width, - sideX * 2));
            Ver.Add(new Vector2(Height, topY));
            Ver.Add(new Vector2(Height, - topY));
            
            Fix.Shape = new PolygonShape(Ver, 0.5f);

            Body = new Body(world);
            Body.BodyType = BodyType.Static;
            Body.Position = new Vector2(-sideX, Height);

            Bottom = Body.CreateFixture(Fix.Shape).Body;
            Bottom.UserData = this;

        }
    }

}

