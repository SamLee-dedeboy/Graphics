using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace myGraphics
{
    public partial class Form1 : Form
    {
        GraphicsManager manager = new GraphicsManager();
        static int SETTINGS_HEIGHT = 600;
        static int SETTINGS_WIDTH = 200;
        static int COOR_WIDTH = 300;
        static int COOR_HEIGHT = 600;
        Graphics g;
        int sel; //0 = line
                 //1 = rectangle
                 //2 = Polygon 
                 //3 = circle
                 //4 = ellipse ...
                 //5 = edit
                 //6 = move or rotate
        bool rotate = false;
        int last_sel;
        int select_point_range = 10;
        Point start_point, end_point;
        Point selected_point;
        bool selected;
        List<Point> point_list;
        List<Point> highlight_point;
        Shape temp_shape;
        bool start = false; // record the start of a drawing
        double sine = 0, cosine = 1;
        public Form1()
        {
         
            InitializeComponent();
            this.Paint += new PaintEventHandler(Form1_Paint);
            this.Paint += new PaintEventHandler(pictureBox1_Paint);
          //  this.Load += new EventHandler(Form1_Load);
            this.Width = 2 * COOR_WIDTH + SETTINGS_WIDTH;
            this.Height =  COOR_HEIGHT;
            this.MaximizeBox = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;

            sel = -1;
            selected = false;
            point_list = new List<Point>();
            highlight_point = new List<Point>();
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
           
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            g = e.Graphics;

            drawCoordinate(pictureBox1, ref g);
            //hdc = g.GetHdc();
            for (int i = 0; i < manager.shapeList.Count(); i++)
            {
                manager.shapeList[i].Draw(g.GetHdc(), ref g);
                g.ReleaseHdc();
            }
            if (sel == 2)
            {
                for (int i = 0; i < point_list.Count() - 1; i++)
                {
                    Line new_line = new Line(point_list[i], point_list[i + 1]);
                    new_line.Draw(g.GetHdc(), ref g);
                    g.ReleaseHdc();
                }
                if (point_list.Count == 0)
                    return;
                Line last_line = new Line(point_list.Last(), start_point);
                last_line.Draw(g.GetHdc(), ref g);
                g.ReleaseHdc();
            }
        }



        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            g = pictureBox1.CreateGraphics();
            
            if (start)
            {
                end_point = ReverseConvertPoint(new Point(e.X, e.Y));

                pictureBox1.Refresh();
                g = pictureBox1.CreateGraphics();
                switch (sel)
                {
                    case 0:
                        Line new_line = new Line(start_point, end_point);
                        //manager.addShape(new_line);
                        new_line.Draw(g.GetHdc(), ref g);
                        g.ReleaseHdc();
                        //  g.FillEllipse(Brushes.Black, 300,400, (float)15, (float)15);

                        break;
                    case 1:
                        Rectangle rectangle = new Rectangle(start_point, end_point,sine, cosine);
                        rectangle.Draw(g.GetHdc(), ref g);
                        g.ReleaseHdc();
                        break;
                    case 2:
                        if(point_list.Count!= 1)
                            point_list.RemoveAt(point_list.Count - 1);
                        point_list.Add(end_point);
                        Line new_polygon_line = new Line(point_list.Last(), end_point);
                        new_polygon_line.Draw(g.GetHdc(), ref g);

                        g.ReleaseHdc();
                        break;
                    case 3:
                        double r = Distance(start_point, end_point);
                        Circle circle = new Circle(start_point, r);
                        circle.Draw(g.GetHdc(), ref g);
                        g.ReleaseHdc();
                        break;
                    case 4:
                        Ellipse ellipse = new Ellipse(start_point, end_point,this.sine,this.cosine);
                        ellipse.Draw(g.GetHdc(), ref g);
                        g.ReleaseHdc();
                        break;
                    case 5:
                        if (selected)
                        {
                            
                            if (last_sel != 2)
                            {
                                temp_shape = manager.shapeList.Last();
                                this.sine = temp_shape.sine;
                                this.cosine = temp_shape.cosine;
                                start_point = temp_shape.other_point(selected_point);
                                manager.shapeList.RemoveAt(manager.shapeList.Count - 1);
                                sel = last_sel;
                            }
                            else    //edit polygon
                            {
                                point_list = ((Polygon)temp_shape).get_point_list();
                                for(int i = 0; i < point_list.Count; i++)
                                {
                                    if(point_list[i] == selected_point)
                                    {
                                        point_list[i] = end_point;
                                        break;
                                    }
                                }
                                Polygon edited_polygon = new Polygon(point_list);
                                edited_polygon.Draw(g.GetHdc(), ref g);
                                g.ReleaseHdc();
                                
                            }
                        }
                        else
                        {
                            sel = -1;
                        }
                        break;
                    case 6:
                        if (!rotate)
                        {    //shift
                            int delta_x = end_point.X - start_point.X;
                            int delta_y = end_point.Y - start_point.Y;
                            Shape shifted_shape = temp_shape.shift(delta_x, delta_y);
                            shifted_shape.Draw(g.GetHdc(), ref g);
                            g.ReleaseHdc();
                        }
                        else    //rotate
                        {
                            Shape rotate_shape = temp_shape.rotate(end_point);
                            rotate_shape.Draw(g.GetHdc(), ref g);
                            g.ReleaseHdc();
                        }
                        break;
                }
                g.Dispose();
            }
        }
       


        private void pictureBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                g = pictureBox1.CreateGraphics();
                if(sel == 2)    //Polygon
                {
                    point_list.RemoveAt(point_list.Count - 1);
                    Polygon polygon = new Polygon(point_list);
                    manager.addShape(polygon);
                    point_list.Clear();
                    polygon.Draw(g.GetHdc(), ref g);
                    g.ReleaseHdc();
                    start = false;
                    last_sel = sel;
                    sel = 5;
                    highlight_point = manager.shapeList.Last().highlight(ref g);
                }
            }
        }
        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (sel == -1)
                {
                    pictureBox1.Refresh();
                    return;
                }
                g = pictureBox1.CreateGraphics();
                if (!start)
                {
                    start_point = ReverseConvertPoint(new Point(e.X, e.Y));
                    start = true;
                    if (sel == 5)
                    {
                        for (int i = 0; i < highlight_point.Count; i++)
                        {
                            if (Math.Abs(start_point.X - highlight_point[i].X) < select_point_range &&
                                Math.Abs(start_point.Y - highlight_point[i].Y) < select_point_range)
                            {
                                selected_point = highlight_point[i];
                                selected = true;
                                if (selected_point == manager.shapeList.Last().center)
                                {
                                    sel = 6;
                                    temp_shape = manager.shapeList.Last();
                                    manager.shapeList.RemoveAt(manager.shapeList.Count - 1);

                                }
                                else if(selected_point == manager.shapeList.Last().rotate_point)
                                {
                                    sel = 6;
                                    temp_shape = manager.shapeList.Last();
                                    manager.shapeList.RemoveAt(manager.shapeList.Count - 1);
                                    rotate = true;
                                }
                                else
                                if (manager.shapeList.Last().type == 2)
                                {
                                    temp_shape = manager.shapeList.Last();
                                    manager.shapeList.RemoveAt(manager.shapeList.Count - 1);
                                }
                                return;
                            }
                        }
                        selected = false;
                        sel = last_sel;
                        pictureBox1.Refresh();
                    }
                    if (sel == 2)
                        point_list.Add(start_point);
                }
                else
                {
                    end_point = ReverseConvertPoint(new Point(e.X, e.Y));

                    switch (sel)
                    {
                        case 0:

                            Line new_line = new Line(start_point, end_point);
                            manager.addShape(new_line);
                            new_line.Draw(g.GetHdc(), ref g);
                            g.ReleaseHdc();
                            //  g.FillEllipse(Brushes.Black, 300,400, (float)15, (float)15);
                            start = false;
                            break;
                        case 1:
                            Rectangle rectangle = new Rectangle(start_point, end_point,sine,cosine);
                            manager.addShape(rectangle);
                            rectangle.Draw(g.GetHdc(), ref g);
                            g.ReleaseHdc();
                            start = false;
                            break;
                        case 2:

                            Line new_polygon_line = new Line(point_list.Last(), end_point);
                            new_polygon_line.Draw(g.GetHdc(), ref g);
                            g.ReleaseHdc();
                            point_list.Add(end_point);
                            break;
                        case 3:

                            double r = Distance(start_point, end_point);
                            Circle circle = new Circle(start_point, r);
                            manager.addShape(circle);
                            circle.Draw(g.GetHdc(), ref g);
                            g.ReleaseHdc();
                            start = false;
                            break;
                        case 4:
                            Ellipse ellipse = new Ellipse(start_point, end_point,this.sine,this.cosine);
                            manager.addShape(ellipse);
                            ellipse.Draw(g.GetHdc(), ref g);
                            g.ReleaseHdc();
                            start = false;
                            break;
                        case 5: //only enter this case when editting polygon
                            point_list = ((Polygon)temp_shape).get_point_list();
                            for (int i = 0; i < point_list.Count; i++)
                            {
                                if (point_list[i] == selected_point)
                                {
                                    point_list[i] = end_point;
                                    break;
                                }
                            }
                            Polygon edited_polygon = new Polygon(point_list);
                            manager.addShape(edited_polygon);
                            edited_polygon.Draw(g.GetHdc(), ref g);
                            g.ReleaseHdc();
                            point_list.Clear();
                            start = false;
                            selected = false;
                            break;
                        case 6:
                            if (!rotate)    //shift
                            {
                                int delta_x = end_point.X - start_point.X;
                                int delta_y = end_point.Y - start_point.Y;
                                Shape shifted_shape = temp_shape.shift(delta_x, delta_y);
                                manager.addShape(shifted_shape);
                                shifted_shape.Draw(g.GetHdc(), ref g);
                                g.ReleaseHdc();
                            }
                            else    //rotate
                            {
                                Shape rotate_shape = temp_shape.rotate(end_point);
                                manager.addShape(rotate_shape);
                                rotate_shape.Draw(g.GetHdc(), ref g);
                                rotate = false;
                                g.ReleaseHdc();
                            }
                            start = false;
                            selected = false;
                            break;

                    }
                    //edit part
                    if (!start)
                    {

                        last_sel = manager.shapeList.Last().type;
                        sel = 5;
                        highlight_point = manager.shapeList.Last().highlight(ref g);

                    }
                    g.Dispose();
                }
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            return;
            if (start)
            {
                if (sel == 5)
                {
                    start = false;
                    sel = -1;
                }
            }
        }
        
        private void pictureBox1_MouseDown_1(object sender, MouseEventArgs e)
        {
            /*
            if (e.Button == MouseButtons.Left)
            {
                g = pictureBox1.CreateGraphics();
                if (!start)
                {
                    start_point = ReverseConvertPoint(new Point(e.X, e.Y));
                    start = true;
                    if (sel == 2)
                        point_list.Add(start_point);
                    if(sel == 5)
                    {
                        for(int i = 0; i < highlight_point.Count; i++)
                        {
                            if(start_point.X - highlight_point[i].X < select_point_range &&
                                start_point.Y - highlight_point[i].Y < select_point_range)
                            {
                                selected_point = highlight_point[i];
                                break;
                            }
                        }
                    }
                }
                else
                {
                    end_point = ReverseConvertPoint(new Point(e.X, e.Y));

                    switch (sel)
                    {
                        case 0:
                          
                            Line new_line = new Line(start_point, end_point);
                            manager.addShape(new_line);
                            new_line.Draw(g.GetHdc(), ref g);
                            g.ReleaseHdc();
                            //  g.FillEllipse(Brushes.Black, 300,400, (float)15, (float)15);
                            start = false;
                            break;
                        case 1:
                            g = this.CreateGraphics();
                            Rectangle rectangle = new Rectangle(start_point, end_point);
                            manager.addShape(rectangle);
                            rectangle.Draw(g.GetHdc(), ref g);
                            g.ReleaseHdc();
                            start = false;
                            break;
                        case 2:
                            
                            Line new_polygon_line = new Line(point_list.Last(), end_point);
                            new_polygon_line.Draw(g.GetHdc(), ref g);
                            g.ReleaseHdc();
                            point_list.Add(end_point);
                            break;
                        case 3:
                            
                            double r = Distance(start_point, end_point);
                            Circle circle = new Circle(start_point, r);
                            manager.addShape(circle);
                            circle.Draw(g.GetHdc(), ref g);
                            g.ReleaseHdc();
                            start = false;
                            break;
                        case 4:
                            double ellipse_a = Distance(start_point, end_point);
                            Ellipse ellipse = new Ellipse(start_point, ellipse_a, ellipse_a / 2);
                            manager.addShape(ellipse);
                            ellipse.Draw(g.GetHdc(), ref g);
                            g.ReleaseHdc();
                            start = false;
                            break;
                    }
                    //edit part
                    sel = 5;
                    highlight_point =  manager.shapeList.Last().highlight(ref g);
                   

                    g.Dispose();
                }
            }
            */
        }
        

        private void button1_Click(object sender, EventArgs e)
        {
            sel = 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            manager.shapeList.Clear();
            pictureBox1.Refresh();

        }

        private void button3_Click(object sender, EventArgs e)
        {

            sel = 1;
        }


        private void button4_Click(object sender, EventArgs e)
        {
            sel = 2;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            sel = 3;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            sel = 4;
        }

        public static Point ConvertPoint(Point p)
        {
            Point res_p = new Point();
            res_p.X = p.X + COOR_WIDTH;
            res_p.Y = COOR_HEIGHT / 2 - p.Y;
            return res_p;
        }
        public static Point ReverseConvertPoint(Point p)
        {
            Point res_p = new Point();
            res_p.X = p.X - COOR_WIDTH;
            res_p.Y = COOR_HEIGHT / 2 - p.Y;
            return res_p;

        }

        public static void drawCoordinate(PictureBox picturebox1, ref Graphics g)
        {
            Pen p = new Pen(Color.Black, 1);//新建画笔

            Point p1 = ConvertPoint(new Point(-COOR_WIDTH, 0));
            Point p2 = ConvertPoint(new Point(COOR_WIDTH, 0));
            g.DrawLine(p, p1, p2); // x coordinate

            p1 = ConvertPoint(new Point(0, -picturebox1.Height / 2));
            p2 = ConvertPoint(new Point(0, picturebox1.Height / 2));
            g.DrawLine(p, p1, p2); // y coordinate

            p1 = ConvertPoint(new Point(COOR_WIDTH, -picturebox1.Height / 2));
            p2 = ConvertPoint(new Point(COOR_WIDTH, picturebox1.Height / 2));
            g.DrawLine(p, p1, p2);

            p1 = ConvertPoint(new Point(-COOR_WIDTH, -picturebox1.Height / 2));
            p2 = ConvertPoint(new Point(COOR_WIDTH, -picturebox1.Height / 2));
            g.DrawLine(p, p1, p2);

            p1 = ConvertPoint(new Point(-COOR_WIDTH, picturebox1.Height / 2));
            p2 = ConvertPoint(new Point(COOR_WIDTH, picturebox1.Height / 2));
            g.DrawLine(p, p1, p2);

            p1 = ConvertPoint(new Point(-COOR_WIDTH, -picturebox1.Height / 2));
            p2 = ConvertPoint(new Point(-COOR_WIDTH, picturebox1.Height / 2));
            g.DrawLine(p, p1, p2);
        }

        public static double Distance(Point p1, Point p2)
        {
           
            return Math.Pow(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2), 0.5);
        }
    }
}
