using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Evolution
{
    public partial class Form1 : Form
    {
        private List <Rabbit> item = new List<Rabbit>();
        private List<Rabbit> tempItem;
        public  int SIZE_SQUARE; //разрешение (иными словами масштаб поля).
        public static int RABBIT = 15; //не устанавливать меньше 15. индекс кролика на карте
        private Graphics graphics;
        public static int [,] field;
        private int cols;
        private int rows;
        Random random = new Random();
        private double genCount;
        

        public Form1()
        {
            InitializeComponent();
        }

        private void CreateField()
        {
            genCount = 0;
            SIZE_SQUARE = trackBar2.Value;
            button1.Enabled = true;
            item.Clear();
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graphics = Graphics.FromImage(pictureBox1.Image);
            cols = pictureBox1.Width / SIZE_SQUARE;
            rows = pictureBox1.Height / SIZE_SQUARE;
            field = new int [cols, rows];

            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    field[x, y] = random.Next(-20, RABBIT); //Первый параметр - плотность травы при создании поля. Неплохо бы вынести потом в отдельную переменную
                }
            }

            for (int i = 0; i < 1; i++)
            {
                int a = random.Next(0, cols);
                int b = random.Next(0, rows);

                Rabbit rabbits = new Rabbit(a, b, 4, 2, 6, 2);
                field[a, b] = RABBIT;

                item.Add(rabbits);
            }

            DrawMap();
        }

        private void DrawMap()
        {
            
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graphics = Graphics.FromImage(pictureBox1.Image);
            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    if (field[x, y] == 1) // значение ОДИН - трава 
                        graphics.FillRectangle(Brushes.Green, x * SIZE_SQUARE, y * SIZE_SQUARE, SIZE_SQUARE-1,
                            SIZE_SQUARE-1);
                    if (field[x, y] == RABBIT) // значение  кролика 

                        graphics.FillRectangle(Brushes.Red, x * SIZE_SQUARE, y * SIZE_SQUARE, SIZE_SQUARE,
                            SIZE_SQUARE);
                }
            }

            label1.Text = "Количество кроликов: " + item.Count.ToString();
            if (item.Count <= 0)
            {
                label2.Text = "ВСЕ КРОЛИКИ СДОХЛИ\nПРИРОДА ЖЕСТОКА";
            }

        }


        private void NextGeneration()
        {
            RabbitMove();
            CreateGrace();
            DrawMap();

            
        }

        private void CreateGrace()
        {
            int grace = trackBar1.Value;
            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    if (field[x, y] != RABBIT  && field[x, y] != 1) // значение ОДИН - трава 
                        field[x, y] = random.Next(-100*grace,RABBIT); //второй параметр - плотность роста травы во время эволюции
                                                            //связано с индексом кролика
                }
            }
        }

        private void RabbitMove()
        {
            
            foreach (var i in item)
            {
                i.CountMove (cols, rows);
            }


            tempItem = new List<Rabbit>(item);
            float bornabylityView = 0;
            float overviewView = 0;
            float movingView = 0;
            float speedView = 0;
            foreach (var j in tempItem)
            {
                if (j.move < 0)
                {
                    item.Remove(j);
                    field[j.x, j.y] = 0;
                }
                
                if (j.energy <= j.bornabylity)
                {
                    j.energy = 20;
                    item.Add(new Rabbit(j.x, j.y, j.overview, j.bornabylity, j.moving, j.speed));
                }

                bornabylityView += j.bornabylity;
                overviewView += j.overview;
                movingView += j.moving;
                speedView += j.speed;
            }

            label2.Text = "Средн. обзорность: " + (overviewView / item.Count).ToString("F") 
                      + "\nСредн. размножаемость: " + (bornabylityView / item.Count).ToString("F")
                      + "\nСредн. жизнь: " + (movingView  / item.Count ).ToString("F")
                      + "\nСредн. скорость: " + (speedView / item.Count).ToString("F");
        }

       

        private void bStart_Click(object sender, EventArgs e)
        {
            CreateField();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Text = $"Evolution [GENERATION {++genCount}]";
            NextGeneration();
            timer1.Interval = trackBar3.Value;
            bCreate.Enabled = false;
            trackBar2.Enabled = false;
            button2.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            bCreate.Enabled = true;
            trackBar2.Enabled = true;
        }

        

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            label5.Text = $"Скорость {trackBar3.Maximum*trackBar3.Minimum/trackBar3.Value}";
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            label6.Text = $"Рост травы {trackBar1.Maximum * trackBar1.Minimum/trackBar1.Value}";
        }

        
    }
}
