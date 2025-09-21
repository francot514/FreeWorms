using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Collision.Shapes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FreeWorms.Animation;
using FreeWorms.GameManager;
using FreeWorms.ContentManager;
using FarseerPhysics.Factories;

namespace FreeWorms.Data
{
    public class Worm
    {

        public int ID, Health;
        public string Name;
        public static float Density = 10.0f;
        public Direction Direction;
        public Body Body;
        public Fixture Fixture;
        public Point Start, Current;
        public Team Team;
        public Animation.Animation Animation;
        public AnimState AnimState;
        public bool IsDead;

        public Worm(Team team, int id, int x, int y)
        {

            Name = "Worm" + id;
            Team = team;
            Health = 100;
            Start = new Point(x, y);
            Current = Start;
            IsDead = false;

            var Fixture = new Fixture();
            Fixture.Friction = 1.0f;
            Fixture.Restitution = 0.1f;
            Fixture.Shape = new CircleShape(24, 10);

            Body = BodyFactory.CreateRectangle(Physics.World, 31, 31, 1.0f);
            Body.BodyType = BodyType.Dynamic;
            Body.CollidesWith = Category.All;
            Body.CollisionCategories = Category.All;
            Body.FixedRotation = true;

          

            AnimState = AnimState.Idle;

            Animation = AnimManager.Animations[0];

        }

        public void Update(GameTime gameTime)
        {

            Animation.Update(gameTime);

           
            if (AnimState == AnimState.Walking)
            {
                Animation = AnimManager.Animations[1];

                if (Direction == Direction.left)
                    Body.Position = new Vector2(Body.Position.X - 1, Body.Position.Y);

                else if (Direction == Direction.right)
                    Body.Position = new Vector2(Body.Position.X + 1, Body.Position.Y);
            }
            else if (AnimState == AnimState.Jumping)
            {
                var forces = new Vector2((int)Direction, -2);

                Body.ApplyLinearImpulse(forces);
            }
            else
                Animation = AnimManager.Animations[0];

            if (Body.Position.X > 0 && Body.Position.Y > 0)
            {
                Current = new Point((int)Body.Position.X, (int)Body.Position.Y);

                if (Body.Position.Y > 696)
                    Body.Position = new Vector2(Start.X + 1, Start.Y);

            }
          

        }

 



        public void Draw(SpriteBatch sbatch)
        {
            if (Animation != null)
                Animation.Draw(sbatch, (int)Body.Position.X, (int)Body.Position.Y);
       

        }
    }

}
