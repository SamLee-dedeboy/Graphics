using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace myGraphics
{
    class GraphicsManager
    {
        public GraphicsManager()
        {
             shapeList = new List<Shape>();
        }
        public List<Shape> shapeList;
        public void addShape(Shape shape)
        {
            shapeList.Add(shape);
        }
        public void removeShape(Shape shape)
        {
            foreach(var p in shapeList)
            {
                if(p == shape)
                {
                    shapeList.Remove(p);
                }
            }
        }

    }
}
