using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FarseerPhysics.Collision;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics.Contacts;

namespace FreeWorms
{
    public static class Physics
    {
        public static int Scale;

        public static World World;

        public static AABB Collision;

        public static Vector2 Gravity;

        private static int Count = 7;
        //private static Body[] _bodies = new Body[Count];
        private static Fixture _sensor;
        private static bool[] _touching = new bool[Count];


        public static void Init()
        {
            Collision = new AABB();
            
            World = new World(new Vector2(0, 10));

            Scale = 30;
            Gravity = new Vector2(World.Gravity.X, World.Gravity.Y);


            World.ContactManager.BeginContact += BeginContact;
            World.ContactManager.EndContact += EndContact;

        }

        public static void Update(GameTime gametime)
        {

            World.Step(gametime.TotalGameTime.Seconds);

        }


        // Implement contact listener.
        private static bool BeginContact(Contact contact)
        {
            Fixture fixtureA = contact.FixtureA;
            Fixture fixtureB = contact.FixtureB;

            if (fixtureA == _sensor && fixtureB.Body.UserData != null)
            {
                _touching[(int)(fixtureB.Body.UserData)] = true;
            }

            if (fixtureB == _sensor && fixtureA.Body.UserData != null)
            {
                _touching[(int)(fixtureA.Body.UserData)] = true;
            }

            return true;
        }

        // Implement contact listener.
        private static void EndContact(Contact contact)
        {

            

            Fixture fixtureA = contact.FixtureA;
            Fixture fixtureB = contact.FixtureB;

            if (fixtureA == _sensor && fixtureB.Body.UserData != null)
            {
                _touching[(int)(fixtureB.Body.UserData)] = false;
            }

            if (fixtureB == _sensor && fixtureA.Body.UserData != null)
            {
                _touching[(int)(fixtureA.Body.UserData)] = false;
            }
        }

        

        public static bool isCollisionBetweenTypes(object objType1 , object objType2, Contact contact)
        {
            var obj1 = contact.FixtureA.Body.UserData;
            var obj2 = contact.FixtureB.Body.UserData;

            if ((obj1.GetType() == objType1.GetType() || obj1.GetType() == objType2.GetType()) &&
            (obj2.GetType() == objType1.GetType() || obj2.GetType() == objType2.GetType()))            
            {
                return true;
            } 
            else
            {

                return false;
            }
        }

        public static Vector2 shotRay(Vector2 startPointInMeters, Vector2 endPointInMeters)
        {


            var input = new RayCastInput();
            var output = new RayCastOutput();
            var intersectionPoint = new Vector2();
            var normalEnd = new Vector2();
            var intersectionNormal = new Vector2();



            Vector2.Multiply(ref endPointInMeters, 30, out endPointInMeters);
            Vector2.Add(startPointInMeters, endPointInMeters);

            input.Point1 = startPointInMeters;
            input.Point2 = endPointInMeters;
            input.MaxFraction = 1;
            var closestFraction = 1;
            var bodyFound = false;
            var b = new Body(World);
            var f = new Fixture();
            for (int i = 0; i < World.BodyList.Count; i++ )
            {
                b = World.BodyList[i];

                for (int j =0; j < b.FixtureList.Count; j++)
                {
                    f = b.FixtureList[j];

                    if (!f.RayCast(out output, ref input, j))
                        continue;
                    else if (output.Fraction < closestFraction && output.Fraction > 0)
                    {
                        //Fixes bug where I was getting extremely small e numbers
                        // in the lower sections of the physics world. It was causing the
                        // ray to shot only a small disntance from the orign of it.
                        if (output.Fraction > 0.001)
                        {
                            closestFraction = (int)output.Fraction;
                            intersectionNormal = output.Normal;
                            bodyFound = true;
                        }
                    }
                }

            }

            intersectionPoint.X = startPointInMeters.X + closestFraction * (endPointInMeters.X - startPointInMeters.Y);
            intersectionPoint.Y = startPointInMeters.Y + closestFraction * (endPointInMeters.Y - startPointInMeters.X);

            if (bodyFound)
            {
                return intersectionPoint;
            }

            return Vector2.Zero;


        }



    }
}
