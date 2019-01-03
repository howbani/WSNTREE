using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace TreeBasedWsnSimulator.Modules
{
    /// <summary>
    /// Interaction logic for TreeNode.xaml
    /// </summary>
    public partial class TreeNode : UserControl
    {
        public TreeNode ParentNode { get; set; }
        public List<TreeNode> Children = new List<TreeNode>();
        public int ID { get; set; }
        public int Level { get; set; }

        public TreeNode(Sensor sensor)
        {
            InitializeComponent();
            ID = sensor.ID;
            lbl_node_id.Content = ID;
            if(sensor.TreeBasedParentSensor!=null)
            {
                lbl_parrentID.Content = sensor.TreeBasedParentSensor.ID;
            }
            else
            {
                lbl_parrentID.Content = "Nil";
            }
        }

        public Point Position
        {
            get
            {
                double x = Tree_node_holder.Margin.Left;
                double y = Tree_node_holder.Margin.Top;
                Point p = new Point(x, y);
                return p;
            }
            set
            {
                Point p = value;
                Tree_node_holder.Margin = new Thickness(p.X, p.Y, 0, 0);
            }
        }

        public double Width { get { return Tree_node_holder.Width; } }
        public double Height { get { return Tree_node_holder.Height; } }

    }
}
