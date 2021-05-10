using System;


namespace Evolution
{
    public class Rabbit
    {
        private static readonly Random random = new Random();
        public int x;
        public int y;
        public int bornabylity;// ген способности к воспроизведению. (сколько надо травы для нового кролика).
        public int energy = 20;
        public int speed = 1;
        public int move;
        public int moving;
        public int overview; // ген способности смотреть вокруг.


        public Rabbit(int X, int Y, int overview, int bornabylity, int moving, int speed)
        {
            x = X;
            y = Y;

            speedViewCalc(overview, speed);
            bornMoveCalc(bornabylity, moving);
        }

        private void speedViewCalc(int overview, int speed)
        {
            this.overview = overview + random.Next(-1, 2);
            this.speed = speed + random.Next(-1, 2);
            if (this.overview + 2 * this.speed > 10)
                speedViewCalc(overview, speed);

        }

        private void bornMoveCalc(int bornabylity, int moving)
        {
            this.bornabylity = bornabylity + random.Next(-1, 2);
            this.moving = moving + random.Next(-1, 2);
            if (this.bornabylity + this.moving > 20)
            {
                bornMoveCalc(bornabylity, moving);
            }
            move = this.moving * 10;
        }

        public void CountMove(int cols, int rows)
        {
            for (int j = 0; j < speed; j++)
            {
                int road = 100;
                int minRoad = 100;
                int xMin = 0;
                int yMin = 0;

                for (int a = -(overview); a <= overview; a++)
                {
                    for (int b = -(overview); b <= overview; b++)
                    {
                        int col = (x + a + cols) % cols;
                        int row = (y + b + rows) % rows;

                        if (Form1.field[col, row] == 1)
                            road = Math.Abs(x - col) + Math.Abs(y - row);
                        if (minRoad > road)
                        {
                            minRoad = road;
                            xMin = col;
                            yMin = row;
                        }
                    }
                }


                Form1.field[x, y] = 0;
                int oldX = x;
                int oldY = y;

                if (minRoad == 100 || minRoad == 0)
                {
                    xMin = random.Next(0, cols);
                    if (xMin == x) xMin++;
                    yMin = random.Next(0, rows);
                    if (yMin == y) yMin++;
                }

                if (Math.Abs(xMin - x) > Math.Abs(yMin - y))
                    x += (xMin - x) / Math.Abs(xMin - x);
                else
                    y += (yMin - y) / Math.Abs(yMin - y);

                if (Form1.field[x, y] == 1)
                    energy -= 1;
                if (Form1.field[x, y] == Form1.RABBIT)
                {
                    x = oldX;
                    y = oldY;
                    move += 1;
                }



                Form1.field[x, y] = Form1.RABBIT;
            }
            move -= 1;
        }
    }
}
