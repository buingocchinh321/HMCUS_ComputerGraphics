using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpGL;
using Timer = System.Windows.Forms.Timer;
using System.Diagnostics;

namespace Paint2D
{
    public partial class Form1 : Form
    {
        Color clUser; // color to draw
        Point pStart, pEnd, pMouse;
        List<Point> lP = new List<Point>();
        int shape;
        bool drawSignal = false;
        Timer timer;
        Stopwatch stopWatch;
        List<Shape> lS = new List<Shape>();
        bool movingObj = false, rotatingObj = false, scalingObj = false;
        public Form1()
        {
            StartTime();
            InitializeComponent();
            clUser = Color.White; // default = white
        }
        private void openGLControl_Resized(object sender, EventArgs e)
        {
            // Get the OpenGL object.
            SharpGL.OpenGL gl = openGLControl.OpenGL;
            // Set the projection matrix.
            gl.MatrixMode(SharpGL.OpenGL.GL_PROJECTION);
            // Load the identity.
            gl.LoadIdentity();
            // Create a perspective transformation.
            gl.Viewport(0, 0, openGLControl.Width, openGLControl.Height);
            gl.Ortho2D(0, openGLControl.Width, 0, openGLControl.Height);

        }
        private void openGLControl_OpenGLInitialized(object sender, EventArgs e)
        {
            SharpGL.OpenGL gl = openGLControl.OpenGL;
            // Set the clear color.
            gl.ClearColor(0, 0, 0, 0);
            // Set the projection matrix.
            gl.MatrixMode(SharpGL.OpenGL.GL_PROJECTION);
            // Load the identity.
            gl.LoadIdentity();
        }
        private void openGLControl_OpenGLDraw(object sender, SharpGL.RenderEventArgs args)
        {
            SharpGL.OpenGL gl = openGLControl.OpenGL;
            gl.Clear(SharpGL.OpenGL.GL_COLOR_BUFFER_BIT | SharpGL.OpenGL.GL_DEPTH_BUFFER_BIT);
            // Draw list Shape
            for (int i = 0; i < lS.Count; i++) lS[i].Draw(gl);

            // line
            if (shape == 1 && drawSignal == true)
            {
                Shape x = new Line(pStart, pEnd, float.Parse(numericUpDown1.Value.ToString()), clUser);
                lS.Add(x);
                shape = 0;
            }
            if (shape == 2 && drawSignal == true)
            {
                Circle x = new Circle(pStart, pEnd, float.Parse(numericUpDown1.Value.ToString()), clUser);
                lS.Add(x);
                shape = 0;
            }
            if (shape == 3 && drawSignal == true)
            {
                Rectangle x = new Rectangle(pStart, pEnd, float.Parse(numericUpDown1.Value.ToString()), clUser);
                lS.Add(x);
                shape = 0;
            }
            if (shape == 4 && drawSignal == true)
            {
                EquilateralRectangle x = new EquilateralRectangle(pStart, pEnd, float.Parse(numericUpDown1.Value.ToString()), clUser);
                lS.Add(x);
                shape = 0;
            }
            if (shape == 5 && drawSignal == true)
            {
                Ellipse x = new Ellipse(pStart, pEnd, float.Parse(numericUpDown1.Value.ToString()), clUser);
                lS.Add(x);
                shape = 0;
            }
            if (shape == 6 && drawSignal == true)
            {
                Pentagon x = new Pentagon(pStart, pEnd, float.Parse(numericUpDown1.Value.ToString()), clUser);
                lS.Add(x);
                shape = 0;
            }
            if (shape == 7 && drawSignal == true)
            {
                Hexagon x = new Hexagon(pStart, pEnd, float.Parse(numericUpDown1.Value.ToString()), clUser);
                lS.Add(x);
                shape = 0;
            }
            if (shape == 8 && drawSignal == true)
            {
                if (lP.Count == 2)
                {
                    Polygon x = new Polygon(lP, float.Parse(numericUpDown1.Value.ToString()), clUser);
                    lS.Add(x);
                }
                else if (lP.Count > 2)
                {
                    lS.RemoveAt(lS.Count - 1);
                    Polygon x = new Polygon(lP, float.Parse(numericUpDown1.Value.ToString()), clUser);
                    lS.Add(x);
                }
            }
            drawSignal = false;
        }
        private void openGLControl_MouseDown(object sender, MouseEventArgs e)
        {
            drawSignal = false;
            if (e.Button == MouseButtons.Left)
            {
                pStart = e.Location;
                pEnd = pStart;
            }
            //movingObj = false;

        }
        private void openGLControl_MouseUp(object sender, MouseEventArgs e)
        {
            drawSignal = true;
            if (e.Button == MouseButtons.Left) pEnd = e.Location;
            if (movingObj)
            {
                for (int i = 0; i < lS.Count; i++)
                {
                    if (lS[i].isHighlighted)
                    {
                        lS[i].Move(pStart, pEnd);
                    }
                }
                movingObj = false;
            }
            if (rotatingObj)
            {
                for (int i = 0; i < lS.Count; i++)
                {
                    if (lS[i].isHighlighted)
                    {
                        lS[i].Rotate(pStart, pEnd);
                    }
                }
                rotatingObj = false;
            }
            if (scalingObj)
            {
                for (int i = 0; i < lS.Count; i++)
                {
                    if (lS[i].isHighlighted)
                    {
                        lS[i].Scale(pStart, pEnd);
                    }
                }
                scalingObj = false;
            }
        }
        private void openGLControl_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                pMouse = e.Location;
                if (shape == 8) lP.Add(e.Location);
                if (shape == 0 && !movingObj && !rotatingObj && !scalingObj)
                {
                    for (int i = 0; i < lS.Count; i++)
                    {
                        lS[i].HighLight(openGLControl.OpenGL, pMouse);
                    }
                }
            }
            if (e.Button == MouseButtons.Right && shape == 8)
            {
                lP.Clear();
                drawSignal = false;
                shape = 0;
            }
            if (e.Button == MouseButtons.Right)
            {
                contextMenuStrip1.Show(new Point(e.Location.X + 50, e.Location.Y + 50));
            }
        }
        private void openGLControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) pEnd = e.Location;
        }
        private void bt_Color_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                clUser = colorDialog1.Color;
            }
        }
        private void bt_Line_Click(object sender, EventArgs e)
        {
            shape = 1;
            drawSignal = false;
        }
        private void bt_Circle_Click(object sender, EventArgs e)
        {
            shape = 2;
            drawSignal = false;
        }
        private void bt_Rectangle_Click(object sender, EventArgs e)
        {
            shape = 3;
            drawSignal = false;
        }
        private void bt_Ellipse_Click(object sender, EventArgs e)
        {
            shape = 5;
            drawSignal = false;
        }
        private void bt_Pen_Click(object sender, EventArgs e)
        {
            shape = 6;
            drawSignal = false;
        }
        private void bt_Hex_Click(object sender, EventArgs e)
        {
            shape = 7;
            drawSignal = false;
        }
        private void Square_Click(object sender, EventArgs e)
        {
            shape = 4;
            drawSignal = false;
        }
        private void StartTime()
        {
            stopWatch = new Stopwatch();
            timer = new Timer();
            timer.Interval = 1;
            timer.Tick += timer1_Tick;
            timer.Start();
            stopWatch.Start();
        }
        private void bt_Clear_Click(object sender, EventArgs e)
        {
            lS.Clear();
            lP.Clear();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            timeBox.Text = stopWatch.Elapsed.ToString();
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // flood fill
            if (comboBox1.SelectedIndex == 1)
            {
                for (int i = 0; i < lS.Count; i++)
                {
                    //if(lS[i].isHighlighted)
                    lS[i].FloodFill(openGLControl.OpenGL, colorDialog1.Color);
                }
                comboBox1.SelectedIndex = -1;
            }
        }
        private void bt_poly_Click(object sender, EventArgs e)
        {
            shape = 8;
            drawSignal = true;
        }
        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem == contextMenuStrip1.Items[0])
            {
                lP.Clear();
                lS.Clear();
            }
            if (e.ClickedItem == contextMenuStrip1.Items[1])
            {
                movingObj = true;
            }
            else if (e.ClickedItem == contextMenuStrip1.Items[2])
            {
                rotatingObj = true;
            }
            else if (e.ClickedItem == contextMenuStrip1.Items[3])
            {
                scalingObj = true;
            }
        }
        
    }
}
