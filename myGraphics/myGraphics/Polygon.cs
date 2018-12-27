using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myGraphics
{
    class Polygon : Shape
    {
        private List<Point> point_list;
        private Point internal_center;
        public Polygon(List<Point> p_list, double sine = 0, double cosine = 1)
        {
            this.sine = sine;
            this.cosine = cosine;
            type = SEL.POLYGON;
            int center_x = 0, center_y = 0;
            point_list = new List<Point>();
            point_list.AddRange(p_list);
            for(int i = 0; i < point_list.Count; i++)
            {
                center_x += point_list[i].X;
                center_y += point_list[i].Y;
            }
            center_x /= point_list.Count;
            center_y /= point_list.Count;
            center = new Point(center_x, center_y);
            rotate_point = Transform.Rotate(new Point(center.X, center.Y + rotate_point_offset),center, this.sine,this.cosine);
            
            //internal center
            int delta_x = 0, delta_y = 0;
            for(int i = 0; i < point_list.Count; i++)
            {
                delta_x += (point_list[i].X - center_x) * (point_list[i].X - center_x) * (point_list[i].X - center_x);
                delta_y += (point_list[i].Y - center_y) * (point_list[i].Y - center_y) * (point_list[i].Y - center_y);
            }
            delta_x = (int)Math.Pow(delta_x, 1 / 3);
            delta_y = (int)Math.Pow(delta_y, 1 / 3);
            delta_x /= point_list.Count;
            delta_y /= point_list.Count;
            internal_center = new Point(center_x + delta_x, center_y + delta_y);
        }
        public override void fill(Color color, ref Graphics g)
        {
            this.color = color;
            List<Point> converted_point_list = new List<Point>();
            for(int i = 0; i < point_list.Count; i++)
            {
                converted_point_list.Add(Form1.ConvertPoint(point_list[i]));
            }
            g.FillPolygon(new SolidBrush(color), converted_point_list.ToArray());
        }
    
        public override Shape shift(int x, int y)
        {
            List<Point> shifted_point_list = new List<Point>();
            for (int i = 0; i < this.point_list.Count; i++)
                //shifted_point_list.Add(new Point(this.point_list[i].X + x, this.point_list[i].Y + y));
                shifted_point_list.Add(Transform.Shift(this.point_list[i], x, y));
            return new Polygon(shifted_point_list, this.sine,this.cosine);
        }
        public override Shape rotate(Point end_point)
        {
            double bevel = Math.Sqrt((end_point.X - center.X) * (end_point.X - center.X) + (end_point.Y - center.Y) * (end_point.Y - center.Y));
            double new_sine = Transform.formalize_sine(end_point, center);
            double new_cosine = Transform.formalize_cosine(end_point, center);
            List<Point> formalize_point_list = new List<Point>();
            for(int i = 0; i < this.point_list.Count; i++)
            {
                formalize_point_list.Add(Transform.Rotate(Transform.Rotate(point_list[i], center, -sine, cosine),center,new_sine,new_cosine));
            }
            return new Polygon(formalize_point_list, new_sine,new_cosine);
        }
        public override List<Point> highlight(ref Graphics g)
        {
           for(int i = 0; i < point_list.Count; i++)
            {
                g.DrawEllipse(new Pen(Color.Black, (float)3), 
                    (float)(Form1.ConvertPoint(point_list[i]).X), (float)(Form1.ConvertPoint(point_list[i]).Y), (float)1.5, (float)1.5);
            }
            g.DrawEllipse(new Pen(Color.Black, (float)3), (float)(Form1.ConvertPoint(center).X), (float)(Form1.ConvertPoint(center).Y), (float)1.5, (float)1.5);
            g.DrawEllipse(new Pen(Color.Black, (float)3), (float)(Form1.ConvertPoint(rotate_point).X), (float)(Form1.ConvertPoint(rotate_point).Y), (float)1.5, (float)1.5);
            g.DrawEllipse(new Pen(Color.Red, (float)3), (float)(Form1.ConvertPoint(internal_center).X), (float)(Form1.ConvertPoint(internal_center).Y), (float)1.5, (float)1.5);
    
            List<Point> p_list = new List<Point>();
            p_list.AddRange(point_list);
            p_list.Add(center);
            p_list.Add(rotate_point);
            return p_list;
        }
        public override Point other_point(Point selected_point)
        {
            throw new NotImplementedException();
        }
        public override void Draw(IntPtr hdc, ref Graphics g)
        {
            for(int i = 0; i < point_list.Count() - 1; i++)
            {
                Line line = new Line(point_list[i], point_list[i + 1]);
                line.Draw(hdc, ref g);
            }
            Line last_line = new Line(point_list[point_list.Count - 1], point_list[0]);
            last_line.Draw(hdc, ref g);
        }
        public void drawBezier(IntPtr hdc, ref Graphics g)
        {
            float step = 0.0001F;
            List<Point> bezier_curves_points = new List<Point>();
            float t = 0F;
            do
            {
                Point temp_point = bezier_interpolation_func(t, this.point_list, this.point_list.Count);    // 计算插值点
                t += step;
                //bezier_curves_points.Add(temp_point);
                Shape.drawPoint(hdc, temp_point, ref g);
            }
            while (t <= 1 && this.point_list.Count > 1);    // 一个点的情况直接跳出.
            return;
   
        }
        private Point bezier_interpolation_func(float t, List<Point> points, int count)
        {
          
            float[] part = new float[count];
            float sum_x = 0, sum_y = 0;
            for (int i = 0; i < count; i++)
            {
                int tmp;
                int n_order = count - 1;    // 阶数
                tmp = calc_combination_number(n_order, i);
                sum_x += (float)(tmp * points[i].X * Math.Pow((1 - t), n_order - i) * Math.Pow(t, i));
                sum_y += (float)(tmp * points[i].Y * Math.Pow((1 - t), n_order - i) * Math.Pow(t, i));
            }
            return new Point((int)sum_x, (int)sum_y);
        }
        private int calc_combination_number(int n, int k)
        {
            int[] result = new int[n + 1];
            for (int i = 1; i <= n; i++)
            {
                result[i] = 1;
                for (int j = i - 1; j >= 1; j--)
                    result[j] += result[j - 1];
                result[0] = 1;
            }
            return result[k];

        }
        public List<Point> get_point_list()
        {
            List<Point> res = new List<Point>();
            res.AddRange(point_list);
            return res;
        }
    }
}
