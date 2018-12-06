using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using myGraphics;
using System.Runtime.InteropServices;
namespace myGraphics
{
    public abstract class Shape
    {
        public int type;
        public double sine = 0;
        public double cosine = 1;
        public Point center;
        public Point rotate_point;
        public const int rotate_point_offset = 20;
        public List<Point> border_list = new List<Point>();
        [DllImport("Gdi32.dll ")]
        public static extern int SetPixel(IntPtr hdc, int x, int y, int color);

        public abstract void fill(Color color, ref Graphics g);
        /*
        {
            Queue<Point> fill_queue = new Queue<Point>();
            fill_queue.Enqueue(this.center);
            Point central_point, top, left, right, below;
            List<Point> filled_list = new List<Point>();
            while (fill_queue.Any())
            {
                
                central_point = fill_queue.Dequeue();
                top = new Point(central_point.X, central_point.Y + 1);
                left = new Point(central_point.X - 1, central_point.Y);
                right = new Point(central_point.X + 1, central_point.Y);
                below = new Point(central_point.X, central_point.Y - 1);
                if (!border_list.Contains(top) && !filled_list.Contains(top) && !fill_queue.Contains(top))
                    fill_queue.Enqueue(top);
                if (!border_list.Contains(left) && !filled_list.Contains(left) && !fill_queue.Contains(left))
                    fill_queue.Enqueue(left);
                if (!border_list.Contains(right) && !filled_list.Contains(right) && !fill_queue.Contains(right))
                    fill_queue.Enqueue(right);
                if (!border_list.Contains(below) && !filled_list.Contains(below) && !fill_queue.Contains(below))
                    fill_queue.Enqueue(below);
                filled_list.Add(central_point);
                central_point = Form1.ConvertPoint(central_point);
                SetPixel(hdc, central_point.X, central_point.Y,100);
            }
        }*/
        public abstract void Draw(IntPtr hdc, ref Graphics g);
        public abstract Shape shift(int x, int y);
        public abstract Shape rotate(Point end_point);
        public abstract List<Point> highlight(ref Graphics g);
        public abstract Point other_point(Point selected_point);
       // public Shape edit(Point selected_point, Point end_point);
        public static void drawPoint(IntPtr hdc, Point p, ref Graphics g)
        {
            p = Form1.ConvertPoint(p);
            SetPixel(hdc, p.X, p.Y, 0);
            // g.FillEllipse(Brushes.Black, p.X,p.Y, (float)1.5, (float)1.5);
        }
        
        

    }

}
