using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace TreeBasedWsnSimulator.Modules
{
    public class Edge
    {
        public Sensor StartVertex { get; set; }
        public Sensor EndVertex { get; set; }
        Line edg = new Line();
        public void DrawEdge(Canvas myCanvas)
        {
            edg.X1 = StartVertex.CenterLocation.X;
            edg.Y1 = StartVertex.CenterLocation.Y;
            edg.X2 = EndVertex.CenterLocation.X;
            edg.Y2 = EndVertex.CenterLocation.Y;
            edg.Stroke = Brushes.Black;
            edg.StrokeThickness = 1;
            myCanvas.Children.Add(edg);

        }
        


    }
}
