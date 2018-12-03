﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace myGraphics
{
    class Circle : Shape
    {
        private double radius;
        public Circle(Point s, double r)
        {
            type = 3;
            center = s;
            radius = r;
        }
        public override Shape shift(int x, int y)
        {
            //return new Circle(new Point(this.center.X + x, this.center.Y + y), this.radius);
            return new Circle(Transform.Shift(this.center, x, y), this.radius);
        }
        public override Shape rotate(Point end_point)
        {
            throw new NotImplementedException();
        }
        public override List<Point> highlight(ref Graphics g)
        {
            int convert_x = Form1.ConvertPoint(center).X;
            int convert_y = Form1.ConvertPoint(center).Y;
            g.DrawEllipse(new Pen(Color.Black, (float)3), (float)(convert_x + radius), (float)(convert_y), (float)1.5, (float)1.5);   //right
            g.DrawEllipse(new Pen(Color.Black, (float)3), (float)(convert_x - radius), (float)(convert_y), (float)1.5, (float)1.5);   //left
            g.DrawEllipse(new Pen(Color.Black, (float)3), (float)(convert_x), (float)(convert_y + radius), (float)1.5, (float)1.5);   //top
            g.DrawEllipse(new Pen(Color.Black, (float)3), (float)(convert_x), (float)(convert_y - radius), (float)1.5, (float)1.5);   //below
            g.DrawEllipse(new Pen(Color.Black, (float)3), (float)(convert_x), (float)(convert_y), (float)1.5, (float)1.5);  
            List<Point> p_list = new List<Point>();
            p_list.Add(new Point(center.X + (int)radius, center.Y));    //right
            p_list.Add(new Point(center.X - (int)radius, center.Y));    //left
            p_list.Add(new Point(center.X, center.Y + (int)radius));    //top
            p_list.Add(new Point(center.X, center.Y - (int)radius));    //below
            p_list.Add(center);
            return p_list;
        }
        public override Point other_point(Point selected_point)
        {
            return center;
        }
        public override void Draw(IntPtr hdc, ref Graphics g)
        {

            BresenhamCircle(hdc, ref g);

        }
        private void BresenhamCircle(IntPtr hdc, ref Graphics g)
        {
            int x, y, p;
            x = 0; y = Convert.ToInt32(radius);
            p = 3 - 2 * y;
            for (; x<= y; x++)
            {

                this.drawPoint(hdc, x, y, ref g);
                if (p >= 0)
                {
                    p += 4 * (x - y) + 10;
                    y--;
                }
                else
                {
                    p += 4 * x + 6;
                }
            }
        }
        private void drawPoint(IntPtr hdc, int x, int y , ref Graphics g)
        {
            /*
            Shape.drawPoint(hdc, new Point(x, y), ref g);
            Shape.drawPoint(hdc, new Point(-x, y), ref g);
            Shape.drawPoint(hdc, new Point(x, -y), ref g);
            Shape.drawPoint(hdc, new Point(-x, -y), ref g);

            Shape.drawPoint(hdc, new Point(y, x), ref g);
            Shape.drawPoint(hdc, new Point(-y, x), ref g);
            Shape.drawPoint(hdc, new Point(y, -x), ref g);
            Shape.drawPoint(hdc, new Point(-y, -x), ref g);
            */
            Shape.drawPoint(hdc, new Point(x + center.X, y + center.Y), ref g);
            Shape.drawPoint(hdc, new Point(-x + center.X, y + center.Y), ref g);
            Shape.drawPoint(hdc, new Point(x + center.X, -y + center.Y), ref g);
            Shape.drawPoint(hdc, new Point(-x + center.X, -y + center.Y), ref g);

            Shape.drawPoint(hdc, new Point(y + center.X, x + center.Y), ref g);
            Shape.drawPoint(hdc, new Point(-y + center.X, x + center.Y), ref g);
            Shape.drawPoint(hdc, new Point(y + center.X, -x + center.Y), ref g);
            Shape.drawPoint(hdc, new Point(-y + center.X, -x + center.Y), ref g);
            /*
            Shape.drawPoint(hdc, Transform.Shift(new Point(x, y), center.X, center.Y), ref g);
            Shape.drawPoint(hdc, Transform.Shift(new Point(-x, y), center.X, center.Y), ref g);
            Shape.drawPoint(hdc, Transform.Shift(new Point(x, -y), center.X, center.Y), ref g);
            Shape.drawPoint(hdc, Transform.Shift(new Point(-x, -y), center.X, center.Y), ref g);

            Shape.drawPoint(hdc, Transform.Shift(new Point(y, x), center.X, center.Y), ref g);
            Shape.drawPoint(hdc, Transform.Shift(new Point(-y, x), center.X, center.Y), ref g);
            Shape.drawPoint(hdc, Transform.Shift(new Point(y, -x), center.X, center.Y), ref g);
            Shape.drawPoint(hdc, Transform.Shift(new Point(-y, -x), center.X, center.Y), ref g);
            */

        }
    }
}