using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace TreeBasedWsnSimulator.Computations 
{
    public class Line 
    {
        LineGeometry line;
        public Line(Point start, Point end, Canvas canvas)
        {
            line = new LineGeometry(start, end);
            Path myPath = new Path();
            myPath.Stroke = Brushes.Black;
            myPath.StrokeThickness = 1;
            myPath.Data = line;
            canvas.Children.Add(myPath);
        }
    }
}
