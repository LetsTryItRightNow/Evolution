using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evolution
{
     public class Rabbit
     {
         public Random random = new Random();
        public int x;
        public int y;
        public int bornabylity = 10;//способность к воспроизведению. (сколько надо травы для нового кролика).
        public int energy = 0;
        public int moving = 10 * 10;
        public int overview = 6; // способность смотреть вокруг.

        public Rabbit(int X, int Y, int overview, int bornabylity)
        {

            x = X;
            y = Y;
            this.overview = overview + random.Next(-1,2);
            this.bornabylity = bornabylity + random.Next(-1, 2);
        }


     }
}
