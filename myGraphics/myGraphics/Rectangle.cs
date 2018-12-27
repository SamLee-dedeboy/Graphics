using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace myGraphics
{
    class Rectangle : Shape
    {
        private Point p1, p2, p3, p4;

        public Rectangle(Point start_point, Point end_point, double sine = 0, double cosine = 1)
        {
            center = new Point((start_point.X + end_point.X) / 2, (start_point.Y + end_point.Y) / 2);
            this.sine = sine;
            this.cosine = cosine;
            type = SEL.RECTANGLE;
            start_point = Transform.Rotate(start_point,
                                            center,    //center
                                            -sine,    //sin(2pai - theta)
                                            cosine); //cosine(2pai-theta)
            end_point = Transform.Rotate(end_point,
                                            center,    //center
                                            -sine,   
                                            cosine);
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
            
            rotate_point = new Point((start_point.X + end_point.X) / 2, start_point.Y + rotate_point_offset);

            p1 = Transform.Rotate(p1, center, sine, cosine);
            p2 = Transform.Rotate(p2, center, sine, cosine);
            p3 = Transform.Rotate(p3, center, sine, cosine);
            p4 = Transform.Rotate(p4, center, sine, cosine);
            rotate_point = Transform.Rotate(rotate_point, center, sine, cosine);
        }
        public override void fill(Color color, ref Graphics g)
        {
            this.color = color;
            if (fill_flag == true)
            {
                if (p1.X < p3.X)
                {

                    Point start_point = Form1.ConvertPoint(p1);
                    g.FillRectangle(new SolidBrush(color), start_point.X, start_point.Y, p2.X - p1.X, p1.Y - p4.Y);
                }
                else
                {
                    Point start_point = Form1.ConvertPoint(p4);
                    g.FillRectangle(new SolidBrush(color), start_point.X, start_point.Y, p1.X - p4.X, p4.Y - p3.Y);
                }
            }
        }
       
        public override Shape shift(int x, int y)
        {
            // return new Rectangle(new Point(this.p1.X + x, this.p1.Y + y), new Point(this.p3.X + x, this.p3.Y + y));
            return new Rectangle(Transform.Shift(this.p1, x, y), Transform.Shift(this.p3, x, y), this.sine, this.cosine);
        }   
        public override Shape rotate(Point end_point)
        {
            
            double bevel = Math.Sqrt((end_point.X - center.X) * (end_point.X - center.X) + (end_point.Y - center.Y) * (end_point.Y - center.Y));
            double new_sine = Transform.formalize_sine(end_point, center);
            double new_cosine = Transform.formalize_cosine(end_point, center);
            Point formalize_p1 = Transform.Rotate(this.p1, center, -sine, cosine);
            Point formalize_p3 = Transform.Rotate(this.p3, center, -sine, cosine);
            Rectangle rotate_rectangle = new Rectangle(Transform.Rotate(formalize_p1, center, new_sine, new_cosine),
                                                       Transform.Rotate(formalize_p3, center, new_sine, new_cosine), 
                                                       new_sine, new_cosine);
            return rotate_rectangle;
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
        }
        public override Point other_point(Point selected_point)
        {
            if(selected_point == p1)
            {
                return p3;
            }
            else if(selected_point == p2)
            {
                return p4;
            }
            else if(selected_point == p3)
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
            Line line1 = new Line(p1, p2);
            Line line2 = new Line(p2, p3);
            Line line3 = new Line(p3, p4);
            Line line4 = new Line(p1, p4);

            line1.Draw(hdc, ref g);
            line2.Draw(hdc, ref g);
            line3.Draw(hdc, ref g);
            line4.Draw(hdc, ref g);

            this.border_list.AddRange(line1.border_list);
            this.border_list.AddRange(line2.border_list);
            this.border_list.AddRange(line3.border_list);
            this.border_list.AddRange(line4.border_list);
        }
    }
}
