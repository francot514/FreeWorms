using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using FreeWorms.ContentManager;
using FreeWorms.GameManager;

namespace FreeWorms.Data
{
    public class Player
    {

        public int ID;
        public string Name;
        public Team Team;

        public Player(int id)
        {
            ID = id;
            Team = new Team(id);


        }

        public void Update(GameTime gameTime)
        {

            if (Team != null)
                Team.Update(gameTime);



        }

        public void HandleInput(KeyboardState keystate, GamePadState gamepad)
        {


            if (Team != null)
                if (keystate.IsKeyDown(Keys.A) ||
                gamepad.IsButtonDown(Buttons.DPadLeft))
                {
                    Team.Worms[Team.Current].AnimState = AnimState.Walking;
                    Team.Worms[Team.Current].Direction = Direction.left;

                }
                else if (keystate.IsKeyDown(Keys.D) ||
                gamepad.IsButtonDown(Buttons.DPadRight))
                {

                    Team.Worms[Team.Current].AnimState = AnimState.Walking;
                    Team.Worms[Team.Current].Direction = Direction.right;


                }
                else if (keystate.IsKeyDown(Keys.Space) ||
                    gamepad.IsButtonDown(Buttons.A))
                {

                    Team.Worms[Team.Current].AnimState = AnimState.Jumping;


                }
                else
                {
                    Team.Worms[Team.Current].AnimState = AnimState.Idle;
                    Team.Worms[Team.Current].Direction = Direction.left;
                }
        }


        public void Draw(SpriteBatch sbatch)
        {

            if (Team != null)
                Team.Draw(sbatch);

        }

    }


}
