using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FreeWorms.GameManager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FreeWorms.Data
{
    public class Team
    {

        public string Name, ColorVal;
        public Color Color;
        public int ID, Current;
        public Worm[] Worms;

        public Team(int id)
        {

            ID = id;
            Color = Color.Red;
            Current = 0;

            this.Worms = new Worm[4];
            for (var i = 0; i < 4; i++)
            {
                var tmp = MapState.NextSpawnPoint();
                Worms[i] = (new Worm(this, i, tmp.X, tmp.Y));

            }


        }

        public void Update(GameTime gameTime)
        {

            if (Worms != null)
                for (int i = 0; i < Worms.Length; i++)
                    if (Worms[i] != null)
                        Worms[i].Update(gameTime);

        }

        public void Draw(SpriteBatch sbatch)
        {

            if (Worms != null)
                for (int i = 0; i < Worms.Length; i++)
                    if (Worms[i] != null)
                        Worms[i].Draw(sbatch);


        }

    }
}
