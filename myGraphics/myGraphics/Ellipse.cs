using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace myGraphics
{
    class Ellipse : Shape
    {
        private double a;
        private double b;
        private Point p1, p2,p3,p4;
        /*
        public Ellipse(Point center, double a, double b, double sine = 0, double cosine = 1)
        {
            type = 4;
            this.sine = sine;
            this.cosine = cosine;
            this.a = a;
            this.b = b;
            this.center = center;
            rotate_point = Transform.Rotate(new Point(center.X, center.Y + rotate_point_offset), center, this.sine, this.cosine);
        }
        */
        public Ellipse(Point start_point, Point end_point, double sine = 0, double cosine = 1)
        {
            type = 4;
            this.sine = sine;
            this.cosine = cosine;
            this.center = new Point((start_point.X + end_point.X) / 2, (start_point.Y + end_point.Y) / 2);
            //formalize
            start_point = Transform.Rotate(start_point,
                                            center,    //center
                                            -sine,    //sin(2pai - theta)
                                            cosine); //cosine(2pai-theta)
            end_point = Transform.Rotate(end_point,
                                            center,    //center
                                            -sine,
                                            cosine);

            this.a = Math.Abs((start_point.X - end_point.X) / 2);
            this.b = Math.Abs((start_point.Y - end_point.Y) / 2);
            if (start_point.Y < end_point.Y)
            {
                Point temp = start_point;
                start_point = end_point;
                end_point = temp;
            }
            this.p1 = start_point;
            this.p3 = end_point;
            if (start_point.X < end_point.X)
            {
                this.p2 = new Point(end_point.X, start_point.Y);
                this.p4 = new Point(start_point.X, end_point.Y);
            }
            else
            {
                this.p2 = new Point(start_point.X, end_point.Y);
                this.p4 = new Point(end_point.X, start_point.Y);
            }
            //rotate back
            p1 = Transform.Rotate(p1, center, sine, cosine);
            p2 = Transform.Rotate(p2, center, sine, cosine);
            p3 = Transform.Rotate(p3, center, sine, cosine);
            p4 = Transform.Rotate(p4, center, sine, cosine);
            rotate_point = Transform.Rotate(new Point(center.X, center.Y + rotate_point_offset), center, this.sine, this.cosine);
        }
        public override Shape shift(int x, int y)
        {
            // return new Ellipse(new Point(this.center.X + x, this.center.Y + y), this.a, this.b);
            return new Ellipse(Transform.Shift(this.p1, x, y),Transform.Shift(this.p3, x,y), this.sine, this.cosine);
        }
        public override Shape rotate(Point end_point)
        {
            double bevel = Math.Sqrt((end_point.X - center.X) * (end_point.X - center.X) + (end_point.Y - center.Y) * (end_point.Y - center.Y));
            double new_sine = Transform.formalize_sine(end_point, center);
            double new_cosine = Transform.formalize_cosine(end_point, center);
            Point formalize_p1 = Transform.Rotate(this.p1, center, -sine, cosine);
            Point formalize_p3 = Transform.Rotate(this.p3, center, -sine, cosine);
            Ellipse rotate_ellipse = new Ellipse(Transform.Rotate(formalize_p1, center, new_sine, new_cosine),
                                                       Transform.Rotate(formalize_p3, center, new_sine, new_cosine),
                                                       new_sine, new_cosine);
            return rotate_ellipse;
        }
        public override List<Point> highlight(ref Graphics g)
        {
            g.DrawEllipse(new Pen(Color.Black, (float)3), (float)(Form1.ConvertPoint(p1).X), (float)(Form1.ConvertPoint(p1).Y), (float)1, (float)1);
            g.DrawEllipse(new Pen(Color.Black, (float)3), (float)(Form1.ConvertPoint(p2).X), (float)(Form1.ConvertPoint(p2).Y), (float)1, (float)1);
            g.DrawEllipse(new Pen(Color.Black, (float)3), (float)(Form1.ConvertPoint(p3).X), (float)(Form1.ConvertPoint(p3).Y), (float)1, (float)1);
            g.DrawEllipse(new Pen(Color.Black, (float)3), (float)(Form1.ConvertPoint(p4).X), (float)(Form1.ConvertPoint(p4).Y), (float)1, (float)1);
            g.DrawEllipse(new Pen(Color.Black, (float)3), (float)(Form1.ConvertPoint(center).X), (float)(Form1.ConvertPoint(center).Y), (float)1, (float)1);
            g.DrawEllipse(new Pen(Color.Black, (float)3), (float)(Form1.ConvertPoint(rotate_point).X), (float)(Form1.ConvertPoint(rotate_point).Y), (float)1, (float)1);
            List<Point> p_list = new List<Point>();
            p_list.Add(p1);
            p_list.Add(p2);
            p_list.Add(p3);
            p_list.Add(p4);
            p_list.Add(center);
            p_list.Add(rotate_point);
            return p_list;
            // Point right = Transform.Rotate(new Point(center.X + (int)a, center.Y), center, this.sine, this.cosine);
            // Point left = Transform.Rotate(new Point(center.X - (int)a, center.Y), center, this.sine, this.cosine);
            // Point top = Transform.Rotate(new Point(center.X, center.Y + (int)b), center, this.sine, this.cosine);
            // Point below = Transform.Rotate(new Point(center.X, center.Y - (int)b), center, this.sine, this.cosine);
            /*
            Point top_left = Transform.Rotate(new Point(center.X - (int)a, center.Y - (int)b), center, this.sine, this.cosine);
            Point top_right = Transform.Rotate(new Point(center.X + (int)a, center.Y + (int)b), center, this.sine, this.cosine);
            Point below_left = Transform.Rotate(new Point(center.X - (int)a, center.Y - (int)b), center, this.sine, this.cosine);
            Point below_right = Transform.Rotate(new Point(center.X + (int)a, center.Y - (int)b), center, this.sine, this.cosine);
            g.DrawEllipse(new Pen(Color.Black, (float)3), Form1.ConvertPoint(top_right).X, Form1.ConvertPoint(top_right).Y, (float)1.5, (float)1.5);
            g.DrawEllipse(new Pen(Color.Black, (float)3), Form1.ConvertPoint(top_left).X, Form1.ConvertPoint(top_left).Y, (float)1.5, (float)1.5);
            g.DrawEllipse(new Pen(Color.Black, (float)3), Form1.ConvertPoint(below_left).X, Form1.ConvertPoint(below_left).Y, (float)1.5, (float)1.5);
            g.DrawEllipse(new Pen(Color.Black, (float)3), Form1.ConvertPoint(below_right).X, Form1.ConvertPoint(below_right).Y, (float)1.5, (float)1.5);
            g.DrawEllipse(new Pen(Color.Black, (float)3), Form1.ConvertPoint(center).X, Form1.ConvertPoint(center).Y, (float)1.5, (float)1.5);
            g.DrawEllipse(new Pen(Color.Black, (float)3), Form1.ConvertPoint(rotate_point).X, Form1.ConvertPoint(rotate_point).Y, (float)1.5, (float)1.5);
            List<Point> p_list = new List<Point>();
            p_list.Add(top_right);     //right
            p_list.Add(top_left);     //left
            p_list.Add(below_right);     //top
            p_list.Add(below_left);     //below
            p_list.Add(center);
            p_list.Add(rotate_point);
            return p_list;
            */
        }
        public override Point other_point(Point selected_point)
        {
            if (selected_point == p1)
            {
                return p3;
            }
            else if (selected_point == p2)
            {
                return p4;
            }
            else if (selected_point == p3)
            {
                return p1;
            }
            else
            {
                return p2;
            }
        }
        public override void Draw(IntPtr hdc, ref Graphics g)
        {
            int x, y;
            double p;
            if (a <= 10 || b <= 10)
                return; 
            //area 1
            x = 0; y = Convert.ToInt32(b);
            p = b * b - a * a * b + a * a / 4;
            for (; b*b*(x+1) <= a*a*(y-0.5); x++)
            {

                this.drawPoint(hdc, x, y, ref g);
                if (p >= 0)
                {
                    p += b * b * (2 * x + 3) + a * a * (-2 * y + 2);
                    y--;
                }
                else
                {
                    p += b * b * (2 * x + 3);
                }
            }

            //area 2
            // p = b * (x + 0.5) * 2 + a * (y - 1) * 2 - a * b * 2;
            p = b * b * (x + 0.5) * (x + 0.5) + a * a * (y - 1) * (y - 1) - a * a * b * b;
            for (; x <= a && y>=0; y--)
            {
                this.drawPoint(hdc, x, y, ref g);
                if (p >= 0)
                {
                    //should be :p += a * a * (-2 * y + 3);
                    p += a * a * (-2 * y + 3);
                }
                else
                {
                    //  should be : p += 2 * b * b * (x + 1) - a * a * (-2 * y + 3);
                    p += 2 * b * b * (x + 1) + a * a * (-2 * y + 3);
                    x++;
                }
            }
            if (x <= a)
            {
                for (; x <= a; x++)
                    this.drawPoint(hdc, x, y, ref g);
            }
            if(y >= 0)
            {
                for (; y >= 0; y--)
                    this.drawPoint(hdc, x, y, ref g);
            }
        }
        private void drawPoint(IntPtr hdc, int x, int y, ref Graphics g)
        {
            Shape.drawPoint(hdc, Transform.Rotate(new Point(x + center.X, y + center.Y), center, this.sine ,this.cosine), ref g);
            Shape.drawPoint(hdc, Transform.Rotate(new Point(-x + center.X, y + center.Y), center, this.sine, this.cosine), ref g);
            Shape.drawPoint(hdc, Transform.Rotate(new Point(x + center.X, -y + center.Y), center, this.sine, this.cosine), ref g);
            Shape.drawPoint(hdc, Transform.Rotate(new Point(-x + center.X, -y + center.Y), center, this.sine, this.cosine), ref g);
        }
    }
}
