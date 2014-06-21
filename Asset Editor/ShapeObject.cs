using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Asset_Editor
{
    class ShapeObject
    {
        public Point pos;
        public int width;
        public int height;

        public ShapeObject(Point p, int w, int h)
        {
            pos = p;
            width = w;
            height = h;
        }
    }
}
