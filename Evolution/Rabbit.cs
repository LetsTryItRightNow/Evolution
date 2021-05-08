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
        public int bornabylity = 5;//способность к воспроизведению. (сколько надо травы для нового кролика).
        public int energy = 20;
        public int move;
        public int moving;
        public int overview = 6; // способность смотреть вокруг.

        public Rabbit(int X, int Y, int overview, int bornabylity, int moving)
        {

            x = X;
            y = Y;
            this.overview = overview + random.Next(-1,2);
            if (overview >= 16)
                this.overview = 16;
            this.bornabylity = bornabylity + random.Next(-1, 2);
            if (bornabylity >= 19)
                this.bornabylity = 19;
            this.moving = moving + random.Next(-1, 2);

            move = moving  * 10;


        }


     }

}
