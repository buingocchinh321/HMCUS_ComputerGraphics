using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Paint2D
{
    public abstract class Shape
    {
        public Color cl, fillColor = Color.White;
        public Point X, Y;
        public float thickness;
        public List<Point> Raster = new List<Point>();
        public List<Point> Highlight = new List<Point>();
        public List<Point> Fill = new List<Point>();
        public bool isHighlighted = false;
        public Shape() { }
        public Shape(Point X, Point Y, float thickness, Color cl)
        {
            this.X = X;
            this.Y = Y;
            this.thickness = thickness;
        }
        public virtual List<Point> Draw(SharpGL.OpenGL gl)
        {
            gl.PointSize(thickness);
            gl.Color(cl.R / 255.0, cl.G / 255.0, cl.B / 255.0);
            gl.Begin(SharpGL.OpenGL.GL_POINTS);
            for (int i = 0; i < Raster.Count; i++)
                gl.Vertex(Raster[i].X, gl.RenderContextProvider.Height - Raster[i].Y);
            gl.End();
            gl.Flush();

            // Highlight
            if (isHighlighted)
            {
                gl.PointSize(5);
                gl.Color(Color.Red.R / 255.0, 0 / 255.0, 0 / 255.0);
                gl.Begin(SharpGL.OpenGL.GL_POINTS);
                for (int i = 0; i < Highlight.Count; i++)
                {
                    gl.Vertex(Highlight[i].X, gl.RenderContextProvider.Height - Highlight[i].Y);
                }
                gl.End();
                gl.Flush();
            }
            // Fill
            if (fillColor != Color.Black)
            {
                //MessageBox.Show(Fill.Count.ToString());
                gl.PointSize(1);
                gl.Color(fillColor.R / 255.0, fillColor.G / 255.0, fillColor.B / 255.0);
                gl.Begin(SharpGL.OpenGL.GL_POINTS);
                for (int i = 0; i < Fill.Count; i++)
                {
                    gl.Vertex(Fill[i].X, gl.RenderContextProvider.Height - Fill[i].Y);
                }
                gl.End();
                gl.Flush();
                //MessageBox.Show("Done");
            }
            return Raster;
        }
        public double distance(Point A, Point B)
        {
            return Math.Sqrt((A.X - B.X) * (A.X - B.X) + (A.Y - B.Y) * (A.Y - B.Y));
        }
        public virtual void HighLight(SharpGL.OpenGL gl, Point Mouse)
        {
            double epsilon = 15;

            if (Raster.Count <= 0) return;
            for (int i = 0; i < Raster.Count; i++)
            {
                if (distance(Mouse, Raster[i]) <= epsilon)
                {
                    isHighlighted = true;
                    return;
                }
            }
            isHighlighted = false;
        }
        public virtual void FloodFill(SharpGL.OpenGL gl, Color fillColor)
        {
            this.fillColor = fillColor;
            Queue<Point> queue = new Queue<Point>();
            // tam duong tron: X
            queue.Enqueue(X);

            while (queue.Count > 0)
            {
                Point point = queue.Dequeue();

                if (Fill.Contains(point) || Raster.Contains(point) || Highlight.Contains(point)) continue;
                else
                {
                    //MessageBox.Show("Filling");
                    Fill.Add(point);
                    List<Point> neighborList = new List<Point>();
                    neighborList.Add(new Point(point.X + 1, point.Y));
                    neighborList.Add(new Point(point.X - 1, point.Y));
                    neighborList.Add(new Point(point.X, point.Y + 1));
                    neighborList.Add(new Point(point.X, point.Y - 1));

                    for (int i = 0; i < neighborList.Count; i++)
                        if (Fill.Contains(neighborList[i]) || Raster.Contains(neighborList[i]) || Highlight.Contains(neighborList[i])) continue;
                        else queue.Enqueue(neighborList[i]);
                }
            }
        }
        public virtual void ScanLine(SharpGL.OpenGL gl, Color fillColor) { }
        public virtual void Move(Point Start, Point End)
        {
            Fill.Clear();
            AffineTransform at = new AffineTransform();
            at.Translate(End.X - Start.X, End.Y - Start.Y);
            // Vòng lặp move từng pixel trên biên cạnh + control pixel
            for (int i = 0; i < Raster.Count; i++)
            {
                Raster[i] = at.Transform(Raster[i]);
                if (i < Highlight.Count) Highlight[i] = at.Transform(Highlight[i]);
            }
            // Move 2 điểm khởi thuỷ (Start, End) lúc mà bắt đầu vẽ
            X = at.Transform(X);
            Y = at.Transform(Y);
        }
        public virtual void Rotate(Point Start, Point End)
        {
            Fill.Clear();
            if (distance(X, Start) < distance(Y, Start))
            {
                Point temp = X;
                X = Y;
                Y = temp;
            }
            // cos(vectorOA, vectorOB) = tích vô hướng 2 vector / tích độ dài 2 vectors
            List<double> vectorA = new List<double> { Start.X - X.X, Start.Y - X.Y },
                vectorB = new List<double> { End.X - X.X, End.Y - X.Y };
            double lenA = distance(X, Start),
                lenB = distance(X, End);
            double costTheta = vectorA[0] * vectorB[0] + vectorA[1] * vectorB[1];
            costTheta /= lenA * lenB;

            // ArcCos để tính góc
            double theta = Math.Acos(costTheta);
            if (vectorA[0] * vectorB[1] - vectorA[1] * vectorB[0] < 0) theta = -theta;

            // Chuẩn bị ma trận để xoay hình
            AffineTransform at = new AffineTransform();
            at.Translate(-X.X, -X.Y);
            at.Rotate(theta);
            at.Translate(X.X, X.Y);

            // Vòng lặp xoay từng pixel trên biên cạnh + control pixel
            for (int i = 0; i < Raster.Count; i++)
            {
                Raster[i] = at.Transform(Raster[i]);
                if (i < Highlight.Count) Highlight[i] = at.Transform(Highlight[i]);
            }
            X = at.Transform(X);
            Y = at.Transform(Y);
        }
        public virtual void Scale(Point Start, Point End)
        {
            Fill.Clear();
            if (distance(X, Start) < distance(Y, Start))
            {
                Point temp = X;
                X = Y;
                Y = temp;
            }
            // sx = Start.X - X.X chia cho End.X - X.X, tương tự với Y
            List<double> vectorA = new List<double> { Start.X - X.X, Start.Y - X.Y },
                vectorB = new List<double> { End.X - X.X, End.Y - X.Y };
            double sx = vectorB[0] / vectorA[0],
                sy = vectorB[1] / vectorA[1],
                s = sx > sy ? sx : sy; // Chọn 1 trong 2 sx, sy để giữ đúng form của shape

            // Chuẩn bị ma trận scale
            AffineTransform at = new AffineTransform();
            at.Translate(-X.X, -X.Y);
            at.Scale(s, s);
            at.Translate(X.X, X.Y);

            // Vòng lặp scale từng pixel biên + control
            for (int i = 0; i < Raster.Count; i++)
            {
                Raster[i] = at.Transform(Raster[i]);
                if (i < Highlight.Count) Highlight[i] = at.Transform(Highlight[i]);
            }
            X = at.Transform(X);
            Y = at.Transform(Y);
        }
    }
    public partial class Line : Shape
    {
        public Line(Point X, Point Y, float thickness, Color cl)
        {
            this.X = X;
            this.Y = Y;
            this.thickness = thickness;
            this.cl = cl;
            int dx, dy, stepx, stepy, x, y;
            dx = -(X.X - Y.X);
            dy = -(X.Y - Y.Y);
            Highlight.Add(X);
            Highlight.Add(Y);
            if (dx < 0) { dx = -dx; stepx = -1; } else { stepx = 1; }
            if (dy < 0) { dy = -dy; stepy = -1; } else { stepy = 1; }
            x = X.X;
            y = X.Y;
            Raster.Add(new Point(x, y));

            if (dx > dy)
            {// |m| <= 1
                int p = dy + dy - dx;
                while (x != Y.X)
                {
                    if (p > 0)
                    {
                        y += stepy;
                        p += dy + dy - dx - dx;
                    }
                    else
                    {
                        p += dy + dy;
                    }
                    x += stepx;
                    Raster.Add(new Point(x, y));
                }
            }
            else
            {
                int p = dx + dx - dy;
                while (y != Y.Y)
                {
                    if (p > 0)
                    {
                        x += stepx;
                        p += dx + dx - dy - dy;
                    }
                    else
                    {
                        p += dx + dx;
                    }
                    y += stepy;
                    Raster.Add(new Point(x, y));
                }
            }
        }
    }
    public partial class Circle : Shape
    {
        int r = 0;
        public Circle(Point A, Point B, float thickness, Color cl)
        {
            this.X = A;
            this.Y = B;
            Highlight.Add(X);
            Highlight.Add(Y);

            this.thickness = thickness;
            this.cl = cl;
            r = (X.X - Y.X) * (X.X - Y.X) + (X.Y - Y.Y) * (X.Y - Y.Y);
            r = (int)Math.Sqrt(r) / 2;
            int x = 0, y = r;
            int p = 1 - r;
            this.X = new Point(X.X / 2 + Y.X / 2, X.Y / 2 + Y.Y / 2);
            Raster.Add(new Point(X.X + x, X.Y + y));
            Raster.Add(new Point(X.X + x, X.Y - y));
            Raster.Add(new Point(X.X - x, X.Y - y));
            Raster.Add(new Point(X.X - x, X.Y - y));
            while (x < y)
            {
                if (p < 0)
                {
                    x += 1;
                    p += 2 * x + 1;
                }
                else
                {
                    x += 1;
                    y -= 1;
                    p += 2 * x - 2 * y + 1;
                }
                // First 1/4
                Raster.Add(new Point(X.X + x, X.Y + y));
                Raster.Add(new Point(X.X + y, X.Y + x));

                // Second 1/4
                Raster.Add(new Point(X.X + x, X.Y - y));
                Raster.Add(new Point(X.X + y, X.Y - x));

                // Third 1/4
                Raster.Add(new Point(X.X - x, X.Y - y));
                Raster.Add(new Point(X.X - y, X.Y - x));


                // Fourth 1/4
                Raster.Add(new Point(X.X - x, X.Y + y));
                Raster.Add(new Point(X.X - y, X.Y + x));
            }
        }
        private double distance(Point A, Point B)
        {
            return Math.Sqrt((A.X - B.X) * (A.X - B.X) + (A.Y - B.Y) * (A.Y - B.Y));
        }
        public override void HighLight(SharpGL.OpenGL gl, Point Mouse)
        {
            double epsilon = 30;
            Highlight.Clear();
            if (Raster.Count <= 0) return;
            for (int i = 0; i < Raster.Count; i++)
            {
                if (distance(Mouse, Raster[i]) <= epsilon)
                {
                    isHighlighted = true;
                    if (Highlight.Count <= 2)
                    {
                        Highlight.Add(new Point(X.X - r, X.Y));
                        Highlight.Add(new Point(X.X - r, X.Y - r));
                        Highlight.Add(new Point(X.X - r, X.Y + r));
                        Highlight.Add(new Point(X.X, X.Y - r));
                        Highlight.Add(new Point(X.X, X.Y + r));
                        Highlight.Add(new Point(X.X + r, X.Y - r));
                        Highlight.Add(new Point(X.X + r, X.Y));
                        Highlight.Add(new Point(X.X + r, X.Y + r));
                    }
                    return;
                }
            }
            //isHighlighted = false;
        }

    }
    public partial class Rectangle : Shape
    {
        public Rectangle(Point X, Point Y, float thickness, Color cl)
        {
            this.X = X;
            this.Y = Y;
            this.thickness = thickness;
            this.cl = cl;

            Point A = new Point(X.X, X.Y);
            Point B = new Point(Y.X, X.Y);
            Point C = new Point(Y.X, Y.Y);
            Point D = new Point(X.X, Y.Y);

            Highlight.AddRange(new List<Point> { A, B, C, D, new Point(A.X/2 + B.X/2, A.Y/2+B.Y/2), new Point(B.X / 2 + C.X / 2, B.Y / 2 + C.Y / 2),
                new Point(C.X/2 + D.X/2, C.Y/2+D.Y/2), new Point(A.X/2 + D.X/2, A.Y/2+D.Y/2)});

            Line AB = new Line(A, B, thickness, cl);
            Line BC = new Line(B, C, thickness, cl);
            Line CD = new Line(C, D, thickness, cl);
            Line DA = new Line(D, A, thickness, cl);

            Raster.AddRange(AB.Raster);
            Raster.AddRange(BC.Raster);
            Raster.AddRange(CD.Raster);
            Raster.AddRange(DA.Raster);
        }
        public override void FloodFill(SharpGL.OpenGL gl, Color fillColor)
        {
            this.fillColor = fillColor;
            Queue<Point> queue = new Queue<Point>();
            // tam duong tron: X
            queue.Enqueue(new Point(X.X / 2 + Y.X / 2, X.Y / 2 + Y.Y / 2));

            while (queue.Count > 0)
            {
                Point point = queue.Dequeue();

                if (Fill.Contains(point) || Raster.Contains(point) || Highlight.Contains(point)) continue;
                else
                {
                    //MessageBox.Show("Filling");
                    Fill.Add(point);
                    List<Point> neighborList = new List<Point>();
                    neighborList.Add(new Point(point.X + 1, point.Y));
                    neighborList.Add(new Point(point.X - 1, point.Y));
                    neighborList.Add(new Point(point.X, point.Y + 1));
                    neighborList.Add(new Point(point.X, point.Y - 1));

                    for (int i = 0; i < neighborList.Count; i++)
                        if (Fill.Contains(neighborList[i]) || Raster.Contains(neighborList[i]) || Highlight.Contains(neighborList[i])) continue;
                        else queue.Enqueue(neighborList[i]);
                }
            }
        }
    }
    public partial class EquilateralRectangle : Shape
    {
        int r;
        public EquilateralRectangle(Point P1, Point P2, float thickness, Color cl)
        {
            this.X = P1;
            this.Y = P2;
            this.thickness = thickness;
            this.cl = cl;

            r = (X.X - Y.X) * (X.X - Y.X) + (X.Y - Y.Y) * (X.Y - Y.Y);
            r = (int)Math.Sqrt(r) / 2;
            this.X = new Point(X.X / 2 + Y.X / 2, X.Y / 2 + Y.Y / 2);
            Point A = new Point(X.X - r, X.Y + r);
            Point B = new Point(X.X + r, X.Y + r);
            Point C = new Point(X.X + r, X.Y - r);
            Point D = new Point(X.X - r, X.Y - r);

            Highlight.AddRange(new List<Point> { A, B, C, D, new Point(A.X/2 + B.X/2, A.Y/2+B.Y/2), new Point(B.X / 2 + C.X / 2, B.Y / 2 + C.Y / 2),
                new Point(C.X/2 + D.X/2, C.Y/2+D.Y/2), new Point(A.X/2 + D.X/2, A.Y/2+D.Y/2)});

            Line AB = new Line(A, B, thickness, cl);
            Line BC = new Line(B, C, thickness, cl);
            Line CD = new Line(C, D, thickness, cl);
            Line DA = new Line(D, A, thickness, cl);

            Raster.AddRange(AB.Raster);
            Raster.AddRange(BC.Raster);
            Raster.AddRange(CD.Raster);
            Raster.AddRange(DA.Raster);
        }
        public override void FloodFill(SharpGL.OpenGL gl, Color fillColor)
        {
            this.fillColor = fillColor;
            Queue<Point> queue = new Queue<Point>();
            // tam duong tron: X
            queue.Enqueue(new Point(X.X / 2 + Y.X / 2, X.Y / 2 + Y.Y / 2));

            while (queue.Count > 0)
            {
                Point point = queue.Dequeue();

                if (Fill.Contains(point) || Raster.Contains(point) || Highlight.Contains(point)) continue;
                else
                {
                    //MessageBox.Show("Filling");
                    Fill.Add(point);
                    List<Point> neighborList = new List<Point>();
                    neighborList.Add(new Point(point.X + 1, point.Y));
                    neighborList.Add(new Point(point.X - 1, point.Y));
                    neighborList.Add(new Point(point.X, point.Y + 1));
                    neighborList.Add(new Point(point.X, point.Y - 1));

                    for (int i = 0; i < neighborList.Count; i++)
                        if (Fill.Contains(neighborList[i]) || Raster.Contains(neighborList[i]) || Highlight.Contains(neighborList[i])) continue;
                        else queue.Enqueue(neighborList[i]);
                }
            }
        }
    }
    public partial class Ellipse : Shape
    {
        public Ellipse(Point A, Point B, float thickness, Color cl)
        {
            this.X = A;
            this.Y = B;
            this.thickness = thickness;
            this.cl = cl;

            int rx = Math.Abs(X.X - Y.X) / 2, ry = Math.Abs(X.Y - Y.Y) / 2;
            Point O = new Point((X.X + Y.X) / 2, (X.Y + Y.Y) / 2);
            int rx2 = rx * rx, ry2 = ry * ry, p1 = ry2 - rx2 * ry + rx2 / 4;
            int x = 0, y = ry;
            this.X = new Point(X.X / 2 + Y.X / 2, X.Y / 2 + Y.Y / 2);
            Raster.Add(new Point(X.X, X.Y + ry));
            Raster.Add(new Point(X.X, X.Y - ry));

            // region 1:
            while (ry2 * x < rx2 * y)
            {
                if (p1 < 0)
                {
                    x += 1;
                    p1 += 2 * ry2 * x + ry2;
                }
                else
                {
                    x += 1;
                    y -= 1;
                    p1 += 2 * ry2 * x - 2 * rx2 * y + ry2;
                }

                // First 1/4
                Raster.Add(new Point(X.X + x, X.Y - y));
                //gl.Vertex(X.X + y, gl.RenderContextProvider.Height - X.Y + x);

                // Second 1/4
                Raster.Add(new Point(X.X + x, X.Y + y));
                //gl.Vertex(X.X + y, gl.RenderContextProvider.Height - X.Y - x);

                // Third 1/4
                Raster.Add(new Point(X.X - x, X.Y + y));
                //gl.Vertex(X.X - y, gl.RenderContextProvider.Height - X.Y - x);

                // Fourth 1/4
                Raster.Add(new Point(X.X - x, X.Y - y));
                //gl.Vertex(X.X - y, gl.RenderContextProvider.Height - X.Y + x);
            }

            // region 2:
            double dp2 = ry2 * (x + 0.5) * (x + 0.5) + rx2 * (y - 1) * (y - 1) - rx2 * ry2;
            int p2 = (int)dp2;
            while (y != 0)
            {
                if (p2 > 0)
                {
                    y -= 1;
                    p2 += -2 * rx2 * y + rx2;
                }
                else
                {
                    x += 1;
                    y -= 1;
                    p2 += 2 * ry2 * x - 2 * rx2 * y + rx2;
                }

                // First 1/4
                Raster.Add(new Point(X.X + x, X.Y - y));

                // Second 1/4
                Raster.Add(new Point(X.X + x, X.Y + y));

                // Third 1/4
                Raster.Add(new Point(X.X - x, X.Y + y));

                // Fourth 1/4
                Raster.Add(new Point(X.X - x, X.Y - y));
            }
            Highlight.Add(new Point(X.X - rx, X.Y));
            Highlight.Add(new Point(X.X - rx, X.Y - ry));
            Highlight.Add(new Point(X.X - rx, X.Y + ry));

            Highlight.Add(new Point(X.X, X.Y - ry));
            Highlight.Add(new Point(X.X, X.Y + ry));

            Highlight.Add(new Point(X.X + rx, X.Y - ry));
            Highlight.Add(new Point(X.X + rx, X.Y));
            Highlight.Add(new Point(X.X + rx, X.Y + ry));
        }
        public override void FloodFill(SharpGL.OpenGL gl, Color fillColor)
        {
            this.fillColor = fillColor;
            Queue<Point> queue = new Queue<Point>();
            // tam duong tron: X
            queue.Enqueue(X);

            while (queue.Count > 0)
            {
                Point point = queue.Dequeue();

                if (Fill.Contains(point) || Raster.Contains(point) || Highlight.Contains(point)) continue;
                else
                {
                    //MessageBox.Show("Filling");
                    Fill.Add(point);
                    List<Point> neighborList = new List<Point>();
                    neighborList.Add(new Point(point.X + 1, point.Y));
                    neighborList.Add(new Point(point.X - 1, point.Y));
                    neighborList.Add(new Point(point.X, point.Y + 1));
                    neighborList.Add(new Point(point.X, point.Y - 1));

                    for (int i = 0; i < neighborList.Count; i++)
                        if (Fill.Contains(neighborList[i]) || Raster.Contains(neighborList[i]) || Highlight.Contains(neighborList[i])) continue;
                        else queue.Enqueue(neighborList[i]);
                }
            }
        }
    }
    public partial class Pentagon : Shape
    {
        public Pentagon(Point P1, Point P2, float thickness, Color cl)
        {
            this.X = P1;
            this.Y = P2;
            this.thickness = thickness;
            this.cl = cl;
            List<Point> List = new List<Point>();
            Point O = new Point(X.X / 2 + Y.Y / 2, X.Y / 2 + Y.Y / 2);
            int r = (O.X - Y.X) * (O.X - Y.X) + (O.Y - Y.Y) * (O.Y - Y.Y);
            r = (int)Math.Sqrt(r) / 2;
            for (int i = 0; i < 5; i++)
            {
                int degree = 18 + 72 * i;
                float x = (float)(r * cos(degree));
                float y = (float)(r * sin(degree));
                Point I = new Point((int)x + O.X, (int)y + O.Y);
                List.Add(I);
            }
            int length = List.Count;
            Line A = new Line(List[length - 1], List[0], thickness, cl);
            Raster.AddRange(A.Raster);
            for (int i = 0; i < length - 1; i++)
            {
                Line I = new Line(List[i], List[i + 1], thickness, cl);
                Raster.AddRange(I.Raster);
            }

            for (int i = 0; i < 5; i++)
            {
                Highlight.Add(List[i]);
            }
            List.Clear();
        }
        public double cos(int degres)
        {
            double radians = Math.PI * degres / 180.0;
            double cos = Math.Cos(radians);
            return cos;
        }
        public double sin(int degres)
        {
            double radians = Math.PI * degres / 180.0;
            double sin = Math.Round(Math.Sin(radians), 2);
            return sin;
        }
        public override void FloodFill(SharpGL.OpenGL gl, Color fillColor)
        {
            this.fillColor = fillColor;
            Queue<Point> queue = new Queue<Point>();
            // tam duong tron: X
            queue.Enqueue(X);

            while (queue.Count > 0)
            {
                Point point = queue.Dequeue();

                if (Fill.Contains(point) || Raster.Contains(point) || Highlight.Contains(point)) continue;
                else
                {
                    //MessageBox.Show("Filling");
                    Fill.Add(point);
                    List<Point> neighborList = new List<Point>();
                    neighborList.Add(new Point(point.X + 1, point.Y));
                    neighborList.Add(new Point(point.X - 1, point.Y));
                    neighborList.Add(new Point(point.X, point.Y + 1));
                    neighborList.Add(new Point(point.X, point.Y - 1));

                    for (int i = 0; i < neighborList.Count; i++)
                        if (Fill.Contains(neighborList[i]) || Raster.Contains(neighborList[i]) || Highlight.Contains(neighborList[i])) continue;
                        else queue.Enqueue(neighborList[i]);
                }
            }
        }
    }
    public partial class Hexagon : Shape
    {
        public Hexagon(Point X, Point Y, float thickness, Color cl)
        {
            this.X = X;
            this.Y = Y;
            this.thickness = thickness;
            this.cl = cl;

            Point O = new Point(X.X / 2 + Y.Y / 2, X.Y / 2 + Y.Y / 2);
            int r = (X.X - Y.X) * (X.X - Y.X) + (X.Y - Y.Y) * (X.Y - Y.Y);
            r = (int)Math.Sqrt(r);
            List<Point> List = new List<Point>();
            for (int i = 0; i < 6; i++)
            {
                int degree = 30 + 60 * i;
                float x = (float)(r * cos(degree));
                float y = (float)(r * sin(degree));
                Point I = new Point((int)x + O.X, (int)y + O.Y);
                List.Add(I);
            }
            int length = List.Count;
            Line A = new Line(List[length - 1], List[0], thickness, cl);
            Raster.AddRange(A.Raster);
            for (int i = 0; i < length - 1; i++)
            {
                Line I = new Line(List[i], List[i + 1], thickness, cl);
                Raster.AddRange(I.Raster);
            }

            for (int i = 0; i < 6; i++)
            {
                Highlight.Add(List[i]);
            }

            List.Clear();
        }
        public double cos(int degres)
        {
            double radians = Math.PI * degres / 180.0;
            double cos = Math.Cos(radians);
            return cos;
        }
        public double sin(int degres)
        {
            double radians = Math.PI * degres / 180.0;
            double sin = Math.Round(Math.Sin(radians), 2);
            return sin;
        }
    }
    public partial class Polygon : Shape
    {
        public List<Point> lP;
        public Polygon(List<Point> x, float thickness, Color cl)
        {
            lP = new List<Point>(x);
            this.thickness = thickness;
            this.cl = cl;
        }
        public override List<Point> Draw(SharpGL.OpenGL gl)
        {
            if (lP.Count >= 2)
            {
                for (int i = 0; i < lP.Count - 2; i++)
                {
                    Line I = new Line(lP[i], lP[i + 1], thickness, cl);
                    I.Draw(gl);
                    Raster.AddRange(I.Draw(gl));
                }
                Line I2 = new Line(lP[lP.Count - 2], lP[lP.Count - 1], thickness, cl);
                I2.Draw(gl);
                Raster.AddRange(I2.Draw(gl));
                Line I3 = new Line(lP[lP.Count - 1], lP[0], thickness, cl);
                I3.Draw(gl);
                Raster.AddRange(I3.Draw(gl));

                if (isHighlighted)
                {
                    gl.PointSize(5);
                    gl.Color(Color.Red.R / 255.0, 0 / 255.0, 0 / 255.0);
                    gl.Begin(SharpGL.OpenGL.GL_POINTS);
                    for (int i = 0; i < lP.Count; i++)
                    {
                        gl.Vertex(lP[i].X, gl.RenderContextProvider.Height - lP[i].Y);
                    }
                    gl.End();
                    gl.Flush();
                }
            }
            return Raster;
        }
        public override void Move(Point Start, Point End)
        {
            Fill.Clear();
            AffineTransform at = new AffineTransform();
            at.Translate(End.X - Start.X, End.Y - Start.Y);
            // Vòng lặp move từng pixel trên biên cạnh + control pixel
            for (int i = 0; i < lP.Count; i++)
            {
                lP[i] = at.Transform(lP[i]);
            }
            // Move 2 điểm khởi thuỷ (Start, End) lúc mà bắt đầu vẽ
            X = at.Transform(X);
            Y = at.Transform(Y);
        }
        public override void Rotate(Point Start, Point End)
        {
            Fill.Clear();
            double sumX = 0, sumY = 0;
            for (int i = 0; i < lP.Count; i++)
            {
                sumX += lP[i].X;
                sumY += lP[i].Y;
            }
            X = new Point((int)Math.Round(sumX / lP.Count), (int)Math.Round(sumY / lP.Count));
            // cos(vectorOA, vectorOB) = tích vô hướng 2 vector / tích độ dài 2 vectors
            List<double> vectorA = new List<double> { Start.X - X.X, Start.Y - X.Y },
                vectorB = new List<double> { End.X - X.X, End.Y - X.Y };
            double lenA = distance(X, Start),
                lenB = distance(X, End);
            double costTheta = vectorA[0] * vectorB[0] + vectorA[1] * vectorB[1];
            costTheta /= lenA * lenB;

            // ArcCos để tính góc
            double theta = Math.Acos(costTheta);
            if (vectorA[0] * vectorB[1] - vectorA[1] * vectorB[0] < 0) theta = -theta;

            // Chuẩn bị ma trận để xoay hình
            AffineTransform at = new AffineTransform();
            at.Translate(-X.X, -X.Y);
            at.Rotate(theta);
            at.Translate(X.X, X.Y);

            // Vòng lặp xoay từng pixel trên biên cạnh + control pixel
            for (int i = 0; i < lP.Count; i++)
            {
                lP[i] = at.Transform(lP[i]);
            }
            X = at.Transform(X);
            Y = at.Transform(Y);
        }
        public override void Scale(Point Start, Point End)
        {
            Fill.Clear();
            double sumX = 0, sumY = 0;
            for (int i = 0; i < lP.Count; i++)
            {
                sumX += lP[i].X;
                sumY += lP[i].Y;
            }
            X = new Point((int)Math.Round(sumX / lP.Count), (int)Math.Round(sumY / lP.Count));
            // sx = Start.X - X.X chia cho End.X - X.X, tương tự với Y
            List<double> vectorA = new List<double> { Start.X - X.X, Start.Y - X.Y },
                vectorB = new List<double> { End.X - X.X, End.Y - X.Y };
            double sx = vectorB[0] / vectorA[0],
                sy = vectorB[1] / vectorA[1],
                s = sx > sy ? sx : sy; // Chọn 1 trong 2 sx, sy để giữ đúng form của shape

            // Chuẩn bị ma trận scale
            AffineTransform at = new AffineTransform();
            at.Translate(-X.X, -X.Y);
            at.Scale(s, s);
            at.Translate(X.X, X.Y);

            // Vòng lặp scale từng pixel biên + control
            for (int i = 0; i < lP.Count; i++)
            {
                lP[i] = at.Transform(lP[i]);
            }
            X = at.Transform(X);
            Y = at.Transform(Y);
        }
    }
    public class AffineTransform
    {
        // Ma tran 3x3
        List<double> TransMatrix;
        public AffineTransform()
        {
            // Khởi tạo ma trận đơn vị 3x3
            TransMatrix = new List<double> { 1,0,0,
                                                 0,1,0,
                                                 0,0,1};
        }
        // Hàm nhân ma trận TransMatrix với Matrix
        public void Multiply(List<double> Matrix)
        {
            List<double> Result = new List<double> { 0,0,0,
                                                         0,0,0,
                                                         0,0,0};
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        Result[i * 3 + j] += Matrix[i * 3 + k] * TransMatrix[k * 3 + j];
                    } // for k
                }// for j
            } // for i
            TransMatrix = Result;
        }

        // Ham reset TransMatrix ve ma tran don vi 3x3
        public void ResetMatrix()
        {
            TransMatrix = new List<double> { 1,0,0,
                                                 0,1,0,
                                                 0,0,1};
        }

        public void Translate(double dx, double dy)
        {
            /*
            1, 0, dx,
            0, 1, dy,
            0, 0, 1
             */
            List<double> transformMatrix = new List<double> {   1, 0, dx,
                                                                    0, 1, dy,
                                                                    0, 0, 1 };
            Multiply(transformMatrix);
        }

        public void Scale(double sx, double sy)
        {
            /*
            sx, 0, 0,
            0, sy, 0,
            0, 0,  1
            */
            //Hàm tạo ma trận co giãn và nhân với ma trận hiện hành
            List<double> transformMatrix = new List<double> {   sx, 0, 0,
                                                                    0, sy, 0,
                                                                    0, 0, 1 };
            Multiply(transformMatrix);
        }

        public void Rotate(double theta)
        {
            //Hàm tạo ma trận xoay và nhân với mà trận hiện hành
            double cosTheta = Math.Cos(theta), sinTheta = Math.Sin(theta);
            List<double> rotateMatrix = new List<double> { cosTheta, -sinTheta, 0,
                                                               sinTheta, cosTheta, 0,
                                                                   0,       0,     1 };
            Multiply(rotateMatrix);
        }

        public Point Transform(Point p)
        {
            List<double> Location = new List<double> { p.X, p.Y, 1.0 },
                Result = new List<double> { 0, 0, 0 };
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    Result[i] += TransMatrix[i * 3 + j] * Location[j];
            return new Point((int)(Math.Round(Result[0])), (int)(Math.Round(Result[1])));
        }
    }
}
