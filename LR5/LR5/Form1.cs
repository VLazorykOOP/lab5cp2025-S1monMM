using System;
using System.Drawing;
using System.Windows.Forms;

namespace LR5
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                float x1 = float.Parse(textBox1.Text);
                float y1 = float.Parse(textBox2.Text);
                float x2 = float.Parse(textBox3.Text);
                float y2 = float.Parse(textBox4.Text);
                float vx1 = float.Parse(textBox5.Text);
                float vy1 = float.Parse(textBox6.Text);
                float vx2 = float.Parse(textBox7.Text);
                float vy2 = float.Parse(textBox8.Text);

                panel1.Refresh();
                DrawHermiteCurve(x1, y1, x2, y2, vx1, vy1, vx2, vy2);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void DrawHermiteCurve(float x1, float y1, float x2, float y2, float vx1, float vy1, float vx2, float vy2)
        {
            Graphics g = panel1.CreateGraphics();
            g.Clear(Color.White);
            Pen pen = new Pen(Color.Blue, 2);

            g.DrawRectangle(pen, x1 - 2, y1 - 2, 4, 4);
            g.DrawRectangle(pen, x2 - 2, y2 - 2, 4, 4);

            int steps = 100;
            for (int i = 0; i <= steps; i++)
            {
                float t = (float)i / steps;
                float t2 = t * t;
                float t3 = t2 * t;

                float h1 = 2 * t3 - 3 * t2 + 1;
                float h2 = -2 * t3 + 3 * t2;
                float h3 = t3 - 2 * t2 + t;
                float h4 = t3 - t2;

                float x = h1 * x1 + h2 * x2 + h3 * vx1 + h4 * vx2;
                float y = h1 * y1 + h2 * y2 + h3 * vy1 + h4 * vy2;

                if (i > 0)
                {
                    g.DrawLine(pen, lastX, lastY, x, y);
                }
                lastX = x;
                lastY = y;
            }

            pen.Dispose();
            g.Dispose();
        }

        private float lastX, lastY;

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                float sideLength = float.Parse(textBox9.Text);
                int order = int.Parse(textBox10.Text);        

                panel2.Refresh();
                DrawKochOnSquare(sideLength, order);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Fractal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DrawKochOnSquare(float D, int order)
        {
            Graphics g = panel2.CreateGraphics();
            g.Clear(Color.White);

            float startX = 250;
            float startY = 90;

            PointF p1 = new PointF(startX, startY);
            PointF p2 = new PointF(startX + D, startY);
            PointF p3 = new PointF(startX + D, startY + D);
            PointF p4 = new PointF(startX, startY + D);

            Pen pen = new Pen(Color.Blue, 1.5f);

            DrawKochLine(g, pen, p1, p2, order);
            DrawKochLine(g, pen, p2, p3, order); 
            DrawKochLine(g, pen, p3, p4, order); 
            DrawKochLine(g, pen, p4, p1, order); 

            pen.Dispose();
            g.Dispose();
        }

        private void DrawKochLine(Graphics g, Pen pen, PointF a, PointF b, int order)
        {
            if (order == 0)
            {
                g.DrawLine(pen, a, b);
                return;
            }

            PointF ab1 = new PointF(
                a.X + (b.X - a.X) / 3f,
                a.Y + (b.Y - a.Y) / 3f);

            PointF ab2 = new PointF(
                a.X + 2f * (b.X - a.X) / 3f,
                a.Y + 2f * (b.Y - a.Y) / 3f);


            double angle = Math.Atan2(b.Y - a.Y, b.X - a.X) - Math.PI / 3;
            float dist = Distance(ab1, ab2);
            PointF apex = new PointF(
                (float)(ab1.X + Math.Cos(angle) * dist),
                (float)(ab1.Y + Math.Sin(angle) * dist));


            DrawKochLine(g, pen, a, ab1, order - 1);
            DrawKochLine(g, pen, ab1, apex, order - 1);
            DrawKochLine(g, pen, apex, ab2, order - 1);
            DrawKochLine(g, pen, ab2, b, order - 1);
        }
        private float Distance(PointF p1, PointF p2)
        {
            return (float)Math.Sqrt((p2.X - p1.X) * (p2.X - p1.X) + (p2.Y - p1.Y) * (p2.Y - p1.Y));
        }
    }
}