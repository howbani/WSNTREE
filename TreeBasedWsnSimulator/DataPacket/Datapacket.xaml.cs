using System;
using System.Windows.Controls;
using System.Windows.Input;
using TreeBasedWsnSimulator.Modules;
using TreeBasedWsnSimulator.Parameters;

namespace TreeBasedWsnSimulator.DataPacket
{
    /// <summary>
    /// Interaction logic for Datapacket.xaml
    /// </summary>
    public partial class Datapacket : UserControl
    {
        public long PacektSequentID { get; set; }
        public double Delay { get; set; }
        public double UsedEnergy_Joule { get; set; }// the aucla consumed Energy
        public int Hops { get; set; } // the hops from the source to the sink.
        public int SourceNodeID { set; get; } 
        public Sensor SourceNode { get; set; } 
        public string Path { set; get; }
        public double Distance { set; get; } // the distance from source to the sink, or cluster head.
        public double RoutingDistance { set; get; }// The Routing Distance, denoted by〖 d〗_j^i (p_k ), of a data packet p_k traveled in the path P_j^i (p_k ) is the sum of distances between any two consecutive nodes in〖 P〗_j^i (p_k ) as modeled in (1). 

        public Sensor Sender { get; set; }
        public Sensor Reciver { get; set; } 



        public Datapacket()
        {
            InitializeComponent();
           // Edg_start_point.StartPoint = sender.CenterLocation;
         //   Edg_end_point.Point = reciver.CenterLocation;
        }

        public void cAnimate()
        {
          

        }

        private void lbl_lable_node_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        /// <summary>
        /// RoutingDistanceEfficiency
        /// </summary>
        public double RoutingDistanceEfficiency
        {
            get
            {
                return 100 * (Distance / RoutingDistance);
            }
        }

        /*
        /// <summary>
        /// TransDistanceEfficiency
        /// </summary>
        public double TransDistanceEfficiency
        {
            get
            {
                if (Hops > 1)
                {
                    return 100 * (1 - (AverageTransDistrancePerHop / Distance));
                }
                else
                {
                    return 100;
                }


            }
        }*/
       
        /// <summary>
        /// Average Transmission Distance (ATD): for〖 P〗_b^s (g_k ), we define average transmission distance per hop as shown in (28).
        /// </summary>
        public double AverageTransDistrancePerHop
        {
            get
            {
                return (RoutingDistance / Hops);
            }
        }

         
        public double TransDistanceEfficiency
        {
            get
            {
                return 100 * (1 - (RoutingDistance / (PublicParamerters.SensingRange * Hops * (Hops + 1))));
            }
        }

        /// <summary>
        /// RoutingEfficiency
        /// </summary>
        public double RoutingEfficiency
        {
            get
            {
                return (RoutingDistanceEfficiency + TransDistanceEfficiency) / 2;
            }
        }

       
    }
}
