﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace myGraphics
{
    class Line : Shape
    {
        private double k, b;
        private Point startPoint, endPoint;
        private Point first_point, second_point;
        public Line(Point p1, Point p2)
        {
            first_point = p1;
            second_point = p2;
            this.sine = (p1.Y - p2.Y) / Transform.distance(p1, p2);
            this.cosine = (p1.X - p2.X) / Transform.distance(p1, p2);
            type = SEL.LINE;
            if (p1.X < p2.X)
            {
                startPoint = p1;
                endPoint = p2;
            }
            else
            {
                startPoint = p2;
                endPoint = p1;
            }
            k = (double)(endPoint.Y - startPoint.Y) / (endPoint.X - startPoint.X);

            if (k <= -1)
            {
                Point temp = startPoint;
                startPoint = endPoint;
                endPoint = temp;
            }
            center = new Point((startPoint.X + endPoint.X) / 2, (startPoint.Y + endPoint.Y) / 2);
            rotate_point = Transform.Rotate(new Point(center.X, center.Y - rotate_point_offset),
                                            center,
                                            sine,
                                            cosine);
            setBorderList();
        }
        public override void fill(Color color, ref Graphics g)
        {
            return;
        }
        public override Shape shift(int x, int y)
        {
            //return new Line(new Point(this.startPoint.X + x, this.startPoint.Y + y), new Point(this.endPoint.X + x, this.endPoint.Y + y));
            return new Line(Transform.Shift(this.startPoint, x, y), Transform.Shift(this.endPoint, x, y));
        }
        public override Shape rotate(Point end_point)
        {
            double new_sine = Transform.formalize_sine(end_point,center);
            double new_cosine = Transform.formalize_cosine(end_point, center);
            Point formalize_p1 = Transform.Rotate(this.first_point, center, -sine, cosine);
            Point formalize_p2 = Transform.Rotate(this.second_point, center, -sine, cosine);
            Line rotate_line = new Line(Transform.Rotate(formalize_p2, center, new_sine, new_cosine),
                                        Transform.Rotate(formalize_p1, center, new_sine, new_cosine));
            return rotate_line;
        }
        public override List<Point> highlight(ref Graphics g)
        {
            g.DrawEllipse(new Pen(Color.Black, (float)3), (float)(Form1.ConvertPoint(startPoint).X), (float)(Form1.ConvertPoint(startPoint).Y), (float)1.5, (float)1.5);
            g.DrawEllipse(new Pen(Color.Black, (float)3), (float)(Form1.ConvertPoint(endPoint).X), (float)(Form1.ConvertPoint(endPoint).Y), (float)1.5, (float)1.5);
            g.DrawEllipse(new Pen(Color.Black, (float)3), (float)(Form1.ConvertPoint(center).X), (float)(Form1.ConvertPoint(center).Y), (float)1.5, (float)1.5);
            g.DrawEllipse(new Pen(Color.Black, (float)3), (float)(Form1.ConvertPoint(rotate_point).X), (float)(Form1.ConvertPoint(rotate_point).Y), (float)1.5, (float)1.5);
            List<Point> p_list = new List<Point>();
            p_list.Add(startPoint);
            p_list.Add(endPoint);
            p_list.Add(center);
            p_list.Add(rotate_point);
            return p_list;
        }
        public override void Draw(IntPtr hdc, ref Graphics g)
        {
            //initialization
            int x, y, dx, dy, p;
            x = startPoint.X;
            y = startPoint.Y;
            if (k == 0)
            {
                for(; x <= endPoint.X; x++)
                {
                    Shape.drawPoint(hdc, new Point(x, y), ref g);
                    this.border_list.Add(new Point(x,y));
                }
                return;
            }
            // |k| < 1
            if (Math.Abs(k) < 1)   
            {

                dx = endPoint.X - x;
                dy = Math.Abs(endPoint.Y - y);
                p = 2 * dy - dx;

                for (; x <= endPoint.X; x++)
                {

                    Shape.drawPoint(hdc, new Point(x, y), ref g);
                    this.border_list.Add(new Point(x, y));
                    if (p >= 0)
                    {
                        if (k > 0)
                        {
                            y++;
                           
                        }
                        else
                        {
                            y--;
                            
                        }
                        p += 2 * (dy - dx);
                    }
                    else
                    {
                        p += 2 * dy;
                    }
                }
                return;
            }
            // |k| > 1
            dx = Math.Abs(endPoint.X - x);
            dy = Math.Abs(endPoint.Y - y);
            p = 2 * dx - dy;

            for (; y <= endPoint.Y; y++)
            {

                Shape.drawPoint(hdc, new Point(x, y), ref g);
                this.border_list.Add(new Point(x, y));
                if (p >= 0)
                {
                    if (k > 0)
                    {
                        x++;
                    }
                    else
                    {
                        x--;
                    }
                    p += 2 * (dx - dy);
                }
                else
                {
                    p += 2 * dx;
                }
            }
        }
       
        public override Point other_point(Point selected_point)
        {
            if(selected_point == this.startPoint)
            {
                return endPoint;
            }
            else
            {
                return startPoint;
            }
            
        }
        public Line cut(Rectangle rec)
        {
            if(inRec(rec, startPoint) && inRec(rec, endPoint))
            {
                return this;
            }
            //Cohen- sutherland 
            bool x1, x2, x3, x4, y1, y2, y3, y4;
            x1 = startPoint.X < rec.getP1().X;
            x2 = startPoint.X > rec.getP2().X;
            x3 = startPoint.Y < rec.getP3().Y;
            x4 = startPoint.Y > rec.getP1().Y;

            y1 = endPoint.X < rec.getP1().X;
            y2 = endPoint.X > rec.getP2().X;
            y3 = endPoint.Y < rec.getP3().Y;
            y4 = endPoint.Y > rec.getP1().Y;
            if ((x1 || x2 || x3 || x4 || y1 || y2 || y3 || y4) == false) //0000, 0000
            {
                return this;
            }
            else if ((x1 && y1) || (x2 && y2) || (x3 && y3) || (x4 && y4))   //x1x2x3x4 | y1y2y3y4 != 0000
            {
                return null;
            }
            else
            {
                if (!inRec(rec, startPoint) && !inRec(rec, endPoint))   // both ends are out of rectangle
                {
                    Point interSectPoint = findIntersection(rec);   //one end is in rectangle, the other is out of rectangle
                    if (!inRec(rec, startPoint))  //reserve startPoint - interSection
                    {
                        return new Line(endPoint, interSectPoint).cut(rec);
                    }
                    else
                        return new Line(startPoint, interSectPoint).cut(rec);
                }
                else
                {
                    Point interSectPoint = findIntersection(rec);   //one end is in rectangle, the other is out of rectangle
                    if (!inRec(rec, startPoint))  //reserve startPoint - interSection
                    {
                        return new Line(endPoint, interSectPoint);
                    }
                    else
                        return new Line(startPoint, interSectPoint);
                }
            }
        }
        private Boolean inRec(Rectangle rec, Point p)
        {
            return (p.X > rec.getP1().X && p.X < rec.getP2().X && p.Y > rec.getP3().Y && p.Y < rec.getP2().Y);
        }
        private Point findIntersection(Rectangle rec)
        {
            Point lastPoint = new Point(border_list[0].X, border_list[0].Y);
            
            Boolean inToOut = inRec(rec, border_list[0]);
            for (int i = 0; i < border_list.Count; i++)
            {
           
                if (inToOut)
                {
                    if (!inRec(rec, border_list[i]))
                    {
                        lastPoint = new Point(border_list[i].X, border_list[i].Y);
                        break;
                    }
                }
                else
                {
                    if (inRec(rec, border_list[i]))
                    {
                        lastPoint = new Point(border_list[i].X, border_list[i].Y);
                        break;
                    }
                }
                //this.border_list.Add(new Point(x, y));
            }
                return lastPoint;
        }
        private void setBorderList()
        {
            //initialization
            int x, y, dx, dy, p;
            x = startPoint.X;
            y = startPoint.Y;
            if (k == 0)
            {
                for (; x <= endPoint.X; x++)
                {
                    //Shape.drawPoint(hdc, new Point(x, y), ref g);
                    this.border_list.Add(new Point(x, y));
                }
                return;
            }
            // |k| < 1
            if (Math.Abs(k) < 1)
            {

                dx = endPoint.X - x;
                dy = Math.Abs(endPoint.Y - y);
                p = 2 * dy - dx;

                for (; x <= endPoint.X; x++)
                {

                    //Shape.drawPoint(hdc, new Point(x, y), ref g);
                    this.border_list.Add(new Point(x, y));
                    if (p >= 0)
                    {
                        if (k > 0)
                        {
                            y++;

                        }
                        else
                        {
                            y--;

                        }
                        p += 2 * (dy - dx);
                    }
                    else
                    {
                        p += 2 * dy;
                    }
                }
                return;
            }
            // |k| > 1
            dx = Math.Abs(endPoint.X - x);
            dy = Math.Abs(endPoint.Y - y);
            p = 2 * dx - dy;

            for (; y <= endPoint.Y; y++)
            {

                //Shape.drawPoint(hdc, new Point(x, y), ref g);
                this.border_list.Add(new Point(x, y));
                if (p >= 0)
                {
                    if (k > 0)
                    {
                        x++;
                    }
                    else
                    {
                        x--;
                    }
                    p += 2 * (dx - dy);
                }
                else
                {
                    p += 2 * dx;
                }
            }

        }
        private Point higher_point()
        {
            if (startPoint.Y >= endPoint.Y)
                return startPoint;
            else
                return endPoint;
        }
        private Point lower_point()
        {
            if (startPoint.Y < endPoint.Y)
                return startPoint;
            else
                return endPoint;
        }
    }
}
