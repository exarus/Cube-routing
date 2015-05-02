using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SharpGL.SceneGraph;
using SharpGL;

namespace L4
{
    public partial class MainWindow : Window
    {
        private Point3f first = new Point3f(-1, -1, -1);
        private Point3f second = new Point3f(0.99f, 1, 1);
        private Point rotation = new Point();
        private readonly Cube CUBE = new Cube();
        private List<Line3f> lines = new List<Line3f>();

        private Point lastMousePos;

        public MainWindow()
        {
            InitializeComponent();
            coordinatesChanged();
        }

        private void openGLControl_OpenGLDraw(object sender, OpenGLEventArgs args)
        {
            OpenGL gl = openGLControl.OpenGL;

            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

            gl.LoadIdentity();

            gl.Rotate(rotation.X, 0.0f, 1.0f, 0.0f);
            gl.Rotate(rotation.Y, 1.0f, 0.0f, 0.0f);

            float size = 0.95f;

            if (isFilled.IsChecked.Value)
            {
                gl.Begin(OpenGL.GL_QUADS);

                gl.Color(0.1f, 0.1f, 0.8f);
                // Top face (y = size)
                gl.Vertex(size, size, -size);
                gl.Vertex(-size, size, -size);
                gl.Vertex(-size, size, size);
                gl.Vertex(size, size, size);

                // Bottom face (y = -size)  
                gl.Vertex(size, -size, size);
                gl.Vertex(-size, -size, size);
                gl.Vertex(-size, -size, -size);
                gl.Vertex(size, -size, -size);

                // Front face  (z = size)
                gl.Vertex(size, size, size);
                gl.Vertex(-size, size, size);
                gl.Vertex(-size, -size, size);
                gl.Vertex(size, -size, size);

                // Back face (z = -size)
                gl.Vertex(size, -size, -size);
                gl.Vertex(-size, -size, -size);
                gl.Vertex(-size, size, -size);
                gl.Vertex(size, size, -size);

                // Left face (x = -size)
                gl.Vertex(-size, size, size);
                gl.Vertex(-size, size, -size);
                gl.Vertex(-size, -size, -size);
                gl.Vertex(-size, -size, size);

                // Right face (x = size)
                gl.Vertex(size, size, -size);
                gl.Vertex(size, size, size);
                gl.Vertex(size, -size, size);
                gl.Vertex(size, -size, -size);

                gl.End();
            }

            size = 1.0f;
            gl.LineWidth(2.0f);
            gl.Color(1.0f, 0.0f, 0.0f);
            gl.Begin(OpenGL.GL_LINE_STRIP);

                gl.Vertex(-size, -size, -size);
                gl.Vertex(size, -size, -size);
                gl.Vertex(size, size, -size);
                gl.Vertex(-size, size, -size);
                gl.Vertex(-size, -size, -size);

                gl.Vertex(-size, -size, size);
                gl.Vertex(size, -size, size);
                gl.Vertex(size, size, size);
                gl.Vertex(-size, size, size);
                gl.Vertex(-size, -size, size);

                gl.Vertex(-size, size, size);
                gl.Vertex(-size, size, -size);

                gl.Vertex(size, size, -size);
                gl.Vertex(size, size, size);

                gl.Vertex(size, -size, size);
                gl.Vertex(size, -size, -size);

            gl.End();
            
            gl.PointSize(7.0f);
            gl.Begin(OpenGL.GL_POINTS);
            gl.Color(1.0f, 1.0f, 0.0f);
            gl.Vertex(first.get());
            gl.Vertex(second.get());
            gl.End();

            gl.LineWidth(5.0f);
            gl.Begin(OpenGL.GL_LINES);
            foreach (Line3f line in lines)
            {
                gl.Color(line.color.get());
                gl.Vertex(line.p1.get());
                gl.Vertex(line.p2.get());
            }
//            gl.Vertex(first.get());
//            gl.Vertex(second.get());
            gl.End();
        }

        private void openGLControl_OpenGLInitialized(object sender, OpenGLEventArgs args)
        {
            OpenGL gl = openGLControl.OpenGL;
            gl.ClearColor(0, 0, 0, 0);
        }

        private void openGLControl_Resized(object sender, OpenGLEventArgs args)
        {
            OpenGL gl = openGLControl.OpenGL;

            gl.MatrixMode(OpenGL.GL_PROJECTION);

            gl.LoadIdentity();

            gl.Perspective(45.0f, (double)Width / (double)Height, 0.01, 100.0);

            gl.LookAt(0, 0, 5, 0, 0, 0, 0, 1, 0);

            gl.MatrixMode(OpenGL.GL_MODELVIEW);
        }

        public struct Line3f
        {
            public Line3f(Point3f p1, Point3f p2) :
                this(p1, p2, new Point3f(1.0f, 1.0f, 0.0f))
            {}

            public Line3f(Point3f p1, Point3f p2, Point3f color)
            {
                this.p1 = p1;
                this.p2 = p2;
                this.color = color;
            }

            public readonly Point3f p1;
            public readonly Point3f p2;
            public readonly Point3f color;
        }

        private void openGLControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Point d = (Point)(e.GetPosition(this) - lastMousePos);
                if (lastMousePos.X != 0)
                {
                    rotation.X += d.X;
                }
                if (lastMousePos.Y != 0)
                {
                    rotation.Y += d.Y;
                }
                
                lastMousePos = e.GetPosition(this);
            }
            else
            {
                lastMousePos.X = 0;
                lastMousePos.Y = 0;
            }
        }

        /*
         * 0 - front    ( x,  y,  1)
         * 1 - left     (-1,  y,  x)
         * 2 - right    ( 1,  y,  x)
         * 3 - bottom   ( x, -1,  y)
         * 4 - top      ( x,  1,  y)
         * 5 - back     ( x,  y, -1)
         */
        private Point3f indexToPoint(byte index, Point p) 
        {
            float x = (float)p.X,
                  y = -(float)p.Y,
                  z = 1;

            switch (index)
            {
                case 0:
                    break;
                case 1:
                    z = x;
                    x = -1;
                    break;
                case 2:
                    z = -x;
                    x = 1;
                    break;
                case 3:
                    z = y;
                    y = -1;
                    break;
                case 4:
                    z = -y;
                    y = 1;
                    break;
                case 5:
                    y = -y;
                    z = -1;
                    break;
                default:
                    throw new Exception();
                    break;
            }

            return new Point3f(x, y, z);
        }

        private byte indexFromObject(object sender)
        {
            object[] a = {front, left, right, bottom, top, back};
            for (byte i = 0; i < 6; i++)
                if (sender == a[i]) return i;
            throw new Exception();
        }

        private void Border_MouseMove(object sender, MouseEventArgs e)
        {
            Point pos = e.GetPosition(sender as IInputElement);
            pos.X = (pos.X / (sender as Shape).Width ) * 2 - 1;
            pos.Y = (pos.Y / (sender as Shape).Height) * 2 - 1;

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                first = indexToPoint(indexFromObject(sender), pos);
                coordinatesChanged();
            }
            if (e.RightButton == MouseButtonState.Pressed)
            {
                second = indexToPoint(indexFromObject(sender), pos);
                coordinatesChanged();
            }
        }
        
        private void Button_MouseDown(object sender, MouseButtonEventArgs e)
        {
            updatePath();
        }

        private void updatePath()
        {
            lines.Clear();

            var paths = CUBE.getPath(first, second);

            foreach(List<Point3f> path in paths) {
                if (path.Count >= 2)
                {
                    Line3f line = new Line3f(path.First(), path.Last(), new Point3f(0, 0, 1));
                    lines.Add(line);
                    // TODO: iterator
                    for (int i = 1; i < path.Count; i++)
                    {
                        line = new Line3f(path[i - 1], path[i]);
                        lines.Add(line);
                    }
                }
            }
        }

        private void coordinatesChanged()
        {
            firstPoint.Content = first;
            secondPoint.Content = second;
            lines.Clear();
            updatePath();
        }
    }
}
