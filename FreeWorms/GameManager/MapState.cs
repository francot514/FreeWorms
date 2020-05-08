using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FreeWorms.GameManager
{
   public static class MapState
    {

        public static Point[] SpawnPoints;
        public static int NextSpawn = 0;

        public static void Init()
        {

            SpawnPoints = new Point[8];
            SpawnPoints[0] = new Point(100, 300);
            SpawnPoints[1] = new Point(200, 300);
            SpawnPoints[2] = new Point(300, 300);
            SpawnPoints[3] = new Point(400, 300);
            SpawnPoints[4] = new Point(500, 300);
            SpawnPoints[5] = new Point(600, 300);
            SpawnPoints[6] = new Point(700, 300);
            SpawnPoints[7] = new Point(800, 300);


        }



        public static Point NextSpawnPoint()
        {
            
            Point point = new Point(100, 100);
            if (NextSpawn <= 7 )
            {
                point =  SpawnPoints[NextSpawn];

                NextSpawn+=1;
            }
            else
                NextSpawn = 0;

            return point;
            

        }

    }
}
