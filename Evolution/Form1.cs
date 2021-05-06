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
        public const int SIZE_SQUARE = 10; //разрешение (иными словами масштаб поля).
        private const int RABBIT = 15; //не устанавливать меньше 15. индекс кролика на карте
        private Graphics graphics;
        private int [,] field;
        private int cols;
        private int rows;
        Random random = new Random();


        public Form1()
        {
            InitializeComponent();
            
        }

        private void CreateField()
        {
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
                    field[x, y] = random.Next(-20, 15); //Второй параметр - плотность травы при создании поля. Неплохо бы вынести потом в отдельную переменную
                }
            }

            for (int i = 0; i < 1; i++)
            {
                int a = random.Next(0, cols);
                int b = random.Next(0, rows);

                Rabbit rabbits = new Rabbit(a, b, 4, 10);
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

        }


        private void NextGeneration()
        {
            RabbitMove();
            CreateGrace();
            DrawMap();
        }

        private void CreateGrace()
        {
           for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    if (field[x, y] != RABBIT  && field[x, y] != 1) // значение ОДИН - трава 
                        field[x, y] = random.Next(-4000,RABBIT); //второй параметр - плотность роста травы во время эволюции
                                                            //связано с индексом кролика

                }
            }
        }

        private void RabbitMove()
        {
            foreach (var i in item)
            {

                int road = 100;
                int minRoad = 100;
                int xMin = 0;
                int yMin = 0;
                
                for ( int x = -(i.overview); x <= i.overview; x++)
                {
                    for (int y = -(i.overview); y <= i.overview; y++)
                    {
                            int col = (i.x + x + cols) % cols;
                            int row = (i.y + y + rows) % rows;

                       // if (col < 0 || row < 0 || col >= cols || row >= rows) break;

                            if (field[col, row] == 1)
                                road = Math.Abs(i.x - col) + Math.Abs(i.y - row);
                            if (minRoad > road )
                            {
                                minRoad = road;
                                xMin = col;
                                yMin = row;
                            }
                    }
                }


                field[i.x, i.y] = 0;
                int oldX = i.x;
                int oldY = i.y;

                if (minRoad == 100)
                { 
                    xMin =  random.Next(cols);
                    yMin = random.Next(rows);
                }

                if (Math.Abs(xMin-i.x)>Math.Abs(yMin - i.y))
                {
                    
                    i.x += (xMin - i.x) / Math.Abs(xMin-i.x);   // пытается делить на нуль !!!! исправить.
                } 
                else
                {
                    
                    i.y += (yMin - i.y) / Math.Abs(yMin - i.y); // пытается делить на нуль !!!! исправить.
                }

                if (field[i.x, i.y] == 1)
                    i.energy += 1;
                if (field[i.x, i.y] == RABBIT)
                {
                    i.x = oldX;
                    i.y = oldY;
                    i.moving += 1;
                }
                    
                i.moving -= 1;
                field[i.x, i.y] = RABBIT;
            }

            tempItem = new List<Rabbit>(item);
            float bornabylityView = 0;
            float overviewView = 0;
            foreach (var j in tempItem)
            {
                if (j.moving < 0)
                {
                    item.Remove(j);
                    field[j.x, j.y] = 0;
                }
                //j.energy += 1;
                if (j.energy >= j.bornabylity)
                {
                    j.energy = 0;
                    item.Add(new Rabbit(j.x, j.y, j.overview, j.bornabylity));
                }

                bornabylityView += j.bornabylity;
                overviewView += j.overview;
            }

            label2.Text = "Средн. обзорность: " + (overviewView / item.Count).ToString();
            label3.Text = "Средн. размножаемость: " + (bornabylityView / item.Count).ToString();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void bStart_Click(object sender, EventArgs e)
        {
            CreateField();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            NextGeneration();
            bCreate.Enabled = false;
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
        }
    }
}
