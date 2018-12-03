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
        [DllImport("Gdi32.dll ")]
        public static extern int SetPixel(IntPtr hdc, int x, int y, int color);
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
