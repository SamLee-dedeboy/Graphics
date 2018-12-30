using System;
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
            type = SEL.CIRCLE;
            center = s;
            radius = r;
        }
        public override void fill(Color color, ref Graphics g)
        {
            this.color = color;
            Point start_point = Form1.ConvertPoint(new Point((int)(center.X - radius), (int)(center.Y + radius)));
            for (int i = 0; i < border_list.Count; i++)
            {
                int x = border_list[i].X;
                int y = border_list[i].Y;
                for (int j = 0; j < x; j += 2)
                {
                    Shape.drawPoint(g.GetHdc(), Transform.Rotate(new Point(j + center.X, y + center.Y), center, sine, cosine), ref g);
                    g.ReleaseHdc();

                    Shape.drawPoint(g.GetHdc(), Transform.Rotate(new Point(-j + center.X, y + center.Y), center, sine, cosine), ref g);
                    g.ReleaseHdc();

                    Shape.drawPoint(g.GetHdc(), Transform.Rotate(new Point(j + center.X, -y + center.Y), center, sine, cosine), ref g);
                    g.ReleaseHdc();

                    Shape.drawPoint(g.GetHdc(), Transform.Rotate(new Point(-j + center.X, -y + center.Y), center, sine, cosine), ref g);

                    g.ReleaseHdc();

                }
            }
            g.FillEllipse(new SolidBrush(color), start_point.X, start_point.Y, 2*(float)radius, 2*(float)radius);
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
            Shape.drawPoint(hdc, new Point(x + center.X, y + center.Y), ref g);
            Shape.drawPoint(hdc, new Point(-x + center.X, y + center.Y), ref g);
            Shape.drawPoint(hdc, new Point(x + center.X, -y + center.Y), ref g);
            Shape.drawPoint(hdc, new Point(-x + center.X, -y + center.Y), ref g);

            Shape.drawPoint(hdc, new Point(y + center.X, x + center.Y), ref g);
            Shape.drawPoint(hdc, new Point(-y + center.X, x + center.Y), ref g);
            Shape.drawPoint(hdc, new Point(y + center.X, -x + center.Y), ref g);
            Shape.drawPoint(hdc, new Point(-y + center.X, -x + center.Y), ref g);
        

        }
    }
}
