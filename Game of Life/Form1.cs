using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Game_of_Life
{
    public partial class Form1 : Form
    {

        Pen penGrid = new Pen(Color.ForestGreen, 2);
        SolidBrush brush = new SolidBrush(Color.DarkGreen);
        int[,] a = new int[56, 118];
        int[,] b = new int[56, 118];
        bool inProgress = false;
        
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Stop();
            
            DoubleBuffered = true;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            int x = 10;
            int y = 60;
            while (y <= 590)
            {
                x = 10;
                while (x <= 1160) 
                {
                    if (a[(int)((y - 50) / 10), (int)(x / 10)] == 1)
                        g.FillRectangle(brush, x, y, 10, 10);
                    x += 10;
                }
                y += 10;
            }
            g.DrawLine(penGrid, 10, 60, 10, 600);
            g.DrawLine(penGrid, 1170, 60, 1170, 600);
            g.DrawLine(penGrid, 10, 60, 1170, 60);
            g.DrawLine(penGrid, 10, 600, 1170, 600);
            if (inProgress == false)
            {
                x = 10;
                y = 60;
                while (x <= 1170)
                {
                    g.DrawLine(penGrid, x, 60, x, this.Height - 50);
                    x += 10;
                }
                while (y <= 600)
                {
                    g.DrawLine(penGrid, 10, y, this.Width - 28, y);
                    y += 10;
                }
            }
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            int x, y = 0;
            x = Control.MousePosition.X - this.Location.X - 10;
            y = Control.MousePosition.Y - this.Location.Y - 30;
            if ((x >= 10) && (x <= 1170) && (y >= 60) && (y <= 600))
            {
                if (a[(int)((y - 50) / 10), (int)(x / 10)] == 1)
                    a[(int)((y - 50) / 10), (int)(x / 10)] = 0;
                else
                    a[(int)((y - 50) / 10), (int)(x / 10)] = 1;
            Invalidate();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 56; i++)
                for (int j = 0; j < 118; j++)
                    a[i, j] = 0;
            Random random = new Random();
            for (int i = 0; i < 56; i++)
                for (int j = 0; j < 118; j++)
                    if (random.Next(0, 10) == 1)
                        a[i, j] = 1;
            Invalidate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (inProgress == false)
            {
                timer1.Start();
                timer1.Interval = (int)numAnimStep.Value;
                inProgress = true;
                btnStart.Text = "СТОП";
                btnStart.BackColor = Color.Red;
            }
            else
            {
                timer1.Stop();
                inProgress = false;
                btnStart.Text = "СТАРТ";
                btnStart.BackColor = Color.Green;
                Invalidate();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            for (int j = 0; j < 117; j++)
            {
                a[0, j] = 0;
                a[55, j] = 0;
            }
            for (int i = 0; i < 55; i++)
            {
                a[i, 0] = 0;
                a[i, 117] = 0;
            }


                for (int i = 1; i < 55; i++)
                for (int j = 1; j < 117; j++)
                    {
                        int k = 0;
                        if (a[i - 1, j - 1] == 1)
                            k++;
                        if (a[i - 1, j] == 1)
                            k++;
                        if (a[i - 1, j + 1] == 1)
                            k++;
                        if (a[i, j - 1] == 1)
                            k++;
                        if (a[i, j + 1] == 1)
                            k++;
                        if (a[i + 1, j - 1] == 1)
                            k++;
                        if (a[i + 1, j] == 1)
                            k++;
                        if (a[i + 1, j + 1] == 1)
                            k++;
                        if (a[i, j] == 1)
                        {
                            if ((k != numericUpDown3.Value) && (k != numericUpDown2.Value))
                                b[i, j] = 0;
                            else
                                b[i, j] = 1;
                        }
                        else
                        {
                            if (k == numericUpDown1.Value)
                                b[i, j] = 1;
                            else
                                b[i, j] = 0;
                        }

                    }
            for (int i = 1; i < 55; i++)
                for (int j = 1; j < 117; j++)
                     a[i, j] = b[i, j];
            Invalidate();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 56; i++)
                for (int j = 0; j < 118; j++)
                    a[i, j] = 0;
            Invalidate();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Title = "Сохранить клеточное поле";
            save.OverwritePrompt = true;
            save.CheckPathExists = true;
            save.Filter = "Data File(*.dat)|*.dat";
            if (save.ShowDialog() == DialogResult.OK)
            {
                string fileName = save.FileName;
                using (TextWriter sw = File.CreateText(fileName))
                {
                    for (int i = 1; i < 55; i++)
                        for (int j = 1; j < 117; j++)
                            sw.WriteLine(a[i, j].ToString());
                }
            }   
        }

        private void button5_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Data File(*.dat)|*.dat";
            if (open.ShowDialog() == DialogResult.OK)
            {
                string fileName = open.FileName;
                using (TextReader sr = File.OpenText(fileName))
                {
                    for (int i = 1; i < 55; i++)
                        for (int j = 1; j < 117; j++)
                        {
                            a[i, j] = Convert.ToInt32(sr.ReadLine());
                        }
                }
            }
            Invalidate();
            
        }
    }
}
