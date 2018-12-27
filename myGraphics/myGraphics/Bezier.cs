using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace myGraphics
{
    class Bezier : Shape
    {
        Polygon outerPolygon;
        public Bezier(List<Point> p_list, double sine = 0, double cosine = 1)
        {
            type = SEL.BEZIER;
            outerPolygon = new Polygon(p_list, sine, cosine);
            center = outerPolygon.center;
            rotate_point = outerPolygon.rotate_point;
        }
        public Bezier(Polygon outerPolygon)
        {
            type = SEL.BEZIER;

            this.outerPolygon = outerPolygon;
            center = outerPolygon.center;
            rotate_point = outerPolygon.rotate_point;
        }
        public override void fill(Color color, ref Graphics g)
        {
            //do nothing
        }
        public override Shape shift(int x, int y)
        {
            return new Bezier((Polygon)outerPolygon.shift(x, y));
        }
        public override Shape rotate(Point end_point)
        {
            return new Bezier((Polygon)outerPolygon.rotate(end_point));
        }
        public override List<Point> highlight(ref Graphics g)
        {
            return outerPolygon.highlight(ref g);
        }
        public override Point other_point(Point selected_point)
        {
            return outerPolygon.other_point(selected_point);
        }
        public override void Draw(IntPtr hdc, ref Graphics g)
        {
            outerPolygon.drawBezier(hdc, ref g);
        }
        public List<Point> get_point_list()
        {
            return outerPolygon.get_point_list();
        }
    }
}
