using FreeWorms.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FreeWorms.GameManager
{
   public class WormState
    {

        public List<Worm> Worms;
        public List<Worm> DeadWorms;

        public WormState(Player[] players)
        {

            Worms = new List<Worm>();
            DeadWorms = new List<Worm>();

            for (int i = 0; i < players.Length; i++)
            {
                var worms = players[i].Team.Worms;
                for (int j = 0; j < worms.Length; j++)
                {
                    this.Worms.Add(worms[j]);
                }
            }


        }

    }
}
