using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace myGraphics
{
    class Transform
    {
        public static Point Shift(Point p, int x, int y)
        {
            Point res = new Point(p.X + x, p.Y + y);
            return res;
        }
        public static Point Rotate(Point p, Point center, double sin, double cos)
        {
            Point res = new Point((int)(center.X + (p.X - center.X) * cos - (p.Y - center.Y) * sin),
                             (int)(center.Y + (p.X - center.X) * sin + (p.Y - center.Y) * cos));
            return res;

        }


        public static double sine(Point start_point, Point center, Point end_point)
        {
            double a = distance(start_point, center);
            double b = distance(center, end_point);
            double c = distance(start_point, end_point);
            return cos_C(a, b, c);
        }
        public static double cosine(Point start_point, Point center, Point end_point)
        {
            double temp_sin = sine(start_point, center, end_point);
            return 1 - temp_sin * temp_sin;
        }
        public static double formalize_sine(Point end_point, Point center)
        {
            double bevel = Math.Sqrt((end_point.X - center.X) * (end_point.X - center.X) + (end_point.Y - center.Y) * (end_point.Y - center.Y));
            return (center.X - end_point.X) / bevel;
        }
        public static double formalize_cosine(Point end_point, Point center)
        {
            double bevel = Math.Sqrt((end_point.X - center.X) * (end_point.X - center.X) + (end_point.Y - center.Y) * (end_point.Y - center.Y));
            return (end_point.Y - center.Y) / bevel;
        }
        public static double distance(Point p1, Point p2)
        {
            return Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));
        }
        public static double cos_C(double a, double b, double c)
        {
            return (a * a + b * b - c * c) / (2 * a * b);
        }
    }
}
