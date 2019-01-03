using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using TreeBasedWsnSimulator.Energy;
using TreeBasedWsnSimulator.Computations;
using TreeBasedWsnSimulator.DataPacket;
using TreeBasedWsnSimulator.Parameters;
using TreeBasedWsnSimulator.Logs;
using TreeBasedWsnSimulator.Roating;
using TreeBasedWsnSimulator.ui;
using TreeBasedWsnSimulator.Properties;

namespace TreeBasedWsnSimulator.Modules
{
    /// <summary>
    /// Interaction logic for Node.xaml
    /// </summary>
    public partial class Sensor : UserControl
    {

        public double SensingRangeRadius { get { return SR; } }
        public static double CR { get; set; }  // the radios of COMUNICATION range. double OF SENSING RANGE
        public double ComunicationRangeRadius { get { return CR; } }

        public int Level { get; set; }
        public Canvas Canvas;
        public MainWindow MainWindow { get; set; }
        public static double SR { get; set; }
        public double BatteryIntialEnergy; // jouls // value will not be changed
        private double _ResidualEnergy; //// jouls this value will be changed according to useage of battery

        public double ResidualEnergy // jouls this value will be changed according to useage of battery
        {
           get { return _ResidualEnergy; }
          set
            {
                _ResidualEnergy = value;
                Prog_batteryCapacityNotation.Value = _ResidualEnergy;
            }
        } //@unit(JOULS);




        #region Sensor
        // the nodes 
        public long NumberofPacketsGeneratedByMe { get; set; }
        public List<Datapacket> PacketsList = new List<Datapacket>();// for source nodes, the generated. for the sink is the packets that recived.
        public List<Sensor> OverlappingNodesList { get; set; } // overlapping sensors: called vector in grouping algorithm.
        public List<Sensor> Childern = new List<Sensor>(); // childern in the tree // in tree based.
        public Sensor TreeBasedParentSensor { get; set; } // parrnt in the tree.
        public List<Sensor> TreeBasedRoutingPath { get; set; } // routing path. // in treeBased
        FirstOrderRadioModel EnergyModel = new FirstOrderRadioModel();
        public List<SensorRoutingLog> RoutingOperationsLog = new List<SensorRoutingLog>();
       
        #endregion

        public int ID { get; set; }
        public Sensor( int nodeID)
        {
            InitializeComponent();
            BatteryIntialEnergy = PublicParamerters.BatteryIntialEnergy; // the value will not be change
            ResidualEnergy = BatteryIntialEnergy;// joules. intializing.
            Prog_batteryCapacityNotation.Value = BatteryIntialEnergy;
            Prog_batteryCapacityNotation.Maximum = BatteryIntialEnergy;
            lbl_Sensing_ID.Content = nodeID;
            ID = nodeID;
            NumberofPacketsGeneratedByMe = 0;
        }
       
        public double BatteryPercentage
        {
            get { return (ResidualEnergy/ BatteryIntialEnergy) *100;}
        }
        /// <summary>
        /// set the sensing range! Raduis
        /// </summary>
        public double SensingRange 
        {
            get { return Ellipse_Sensing_range.Width / 2; }
            set
            {
                // sensing range:
                Ellipse_Sensing_range.Height = value * 2; // heigh= sen rad*2;
                Ellipse_Sensing_range.Width = value * 2; // Width= sen rad*2;
                SR = SensingRange;
                CR = SR * 2; // comunication rad= sensing rad *2;

                // device:
                Device_Sensor.Width = value * 4; // device = sen rad*4;
                Device_Sensor.Height = value * 4;
                // communication range
                Ellipse_Sensing_range.Height = value * 4; // com rang= sen rad *4;
                Ellipse_Sensing_range.Width = value * 4;

                // battery:
                Prog_batteryCapacityNotation.Width = 8;
                Prog_batteryCapacityNotation.Height = 2;
            }
        }

        

        /// <summary>
        /// Real postion of object.
        /// </summary>
        public Point Position
        {
            get
            {
                double x = Device_Sensor.Margin.Left;
                double y = Device_Sensor.Margin.Top;
                Point p = new Point(x, y);
                return p;
            }
            set
            {
                Point p = value;
                Device_Sensor.Margin = new Thickness(p.X, p.Y, 0, 0);
            }
        }

        /// <summary>
        /// center location of node.
        /// </summary>
        public Point CenterLocation 
        {
            get
            {
                double x = Device_Sensor.Margin.Left;
                double y = Device_Sensor.Margin.Top;
                Point p = new Point(x+SensingRange, y+SensingRange);
                return p;
            }

        }


       /// <summary>
       /// 
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            
            if (this.Ellipse_Sensing_range.Fill != Brushes.Brown)
            {
                this.Ellipse_Sensing_range.Fill = Brushes.SkyBlue;
            }
            Sensor loopSensor = this.TreeBasedParentSensor;
            while (loopSensor != null)
            {
                if (loopSensor.Ellipse_Sensing_range.Fill != Brushes.Brown)
                {
                    loopSensor.Ellipse_Sensing_range.Fill = Brushes.SkyBlue;
                }
                loopSensor = loopSensor.TreeBasedParentSensor;
            }

            if (this.TreeBasedRoutingPath != null)
            {
                this.ToolTip = new Label() { Content ="Hops:"+ this.TreeBasedRoutingPath.Count +"\r\nBattery:"+BatteryPercentage+"%" };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            if (this.Ellipse_Sensing_range.Fill != Brushes.Brown)
            {
                this.Ellipse_Sensing_range.Fill = Brushes.Transparent;
            }

              
                Sensor loopSensor = this.TreeBasedParentSensor;
                while (loopSensor != null)
                {
                    if (loopSensor.Ellipse_Sensing_range.Fill != Brushes.Brown)
                    {
                        loopSensor.Ellipse_Sensing_range.Fill = Brushes.Transparent;
                    }
                    loopSensor = loopSensor.TreeBasedParentSensor;
                }
            
        }

       

       

        bool StartMove = false;
        private void Device_Sensor_MouseDown(object sender, MouseButtonEventArgs e)
        {

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                System.Windows.Point P = e.GetPosition(Canvas);
                P.X = P.X - SensingRange;
                P.Y = P.Y - SensingRange;
                this.Position = P;
                StartMove = true;
            }
        }

        private void Device_Sensor_MouseMove(object sender, MouseEventArgs e)
        {
            if (StartMove)
            {
                System.Windows.Point P = e.GetPosition(Canvas);
                P.X = P.X - SensingRange;
                P.Y = P.Y - SensingRange;
                this.Position = P;
            }
        }

        private void Device_Sensor_MouseUp(object sender, MouseButtonEventArgs e)
        {
            StartMove = false;
        }

        private void UserControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TreeBasedRouting tree = new TreeBasedRouting(this, false);
            tree.Start();
        }



        public void GeneratePacekts()
        {
            PublicParamerters.PackeTSequenceID += 1;
            Datapacket datap = new Datapacket();
            datap.SourceNode = this;
            datap.SourceNodeID = ID;
            datap.Path = this.ID.ToString(); // start
            datap.Distance = Operations.DistanceBetweenTwoSensors(PublicParamerters.SinkNode, this);
            datap.PacektSequentID = PublicParamerters.PackeTSequenceID; // PID
            NumberofPacketsGeneratedByMe += 1;
            SendPacekt(this, datap);
        }

        /// <summary>
        /// send data
        /// </summary>
        public void SendPacekt(Sensor Sender, Datapacket datap)
        {
            // set sender and reciver.// each hop.
            Sensor RecieverSensor = Sender.TreeBasedParentSensor;
            datap.Sender = Sender;
            datap.Reciver = RecieverSensor;

            if (RecieverSensor != null)
            {
                if (RecieverSensor.ID != this.ID) // this is the sender.
                {
                    if (ResidualEnergy > 0)
                    {

                        this.Ellipse_Sensing_range.Fill = Brushes.Red;
                        SensorRoutingLog log = new SensorRoutingLog();
                        log.IsSend = true;
                        log.NodeID = this.ID;
                        log.Operation = "Send Data To:" + RecieverSensor.ID;
                        log.Time = DateTime.Now;
                        log.Distance_M = Operations.DistanceBetweenTwoSensors(Sender, RecieverSensor);
                        log.UsedEnergy_Nanojoule = EnergyModel.Transmit(PublicParamerters.RoutingDataLength, log.Distance_M);

                        // set the remain battery Energy:
                        double remainEnergy = ResidualEnergy - log.UsedEnergy_Joule;
                        ResidualEnergy = remainEnergy;
                        log.RemaimBatteryEnergy_Joule = ResidualEnergy;
                        log.PID = datap.PacektSequentID;

                        RoutingOperationsLog.Add(log);
                        log.RelaySequence = datap.Hops;

                        datap.UsedEnergy_Joule += log.UsedEnergy_Joule;
                        // send:
                        RecieverSensor.ReceivePacekt(Sender, datap);
                    }
                    else
                    {

                        this.Ellipse_Sensing_range.Fill = Brushes.Brown; // die out node.
                                                                         // MessageBox.Show("DeadNODE!");
                    }

                }
            }

        }

        /// <summary>
        /// reciev data
        /// </summary>
        public void ReceivePacekt(Sensor SenderSensor,Datapacket datap) 
        {
            if (SenderSensor != null)
            {
                if (SenderSensor.ID != this.ID)
                {
                    if (ResidualEnergy > 0)
                    {
                        SensorRoutingLog log = new SensorRoutingLog();
                        log.IsReceive = true;
                        log.NodeID = this.ID;
                        log.Operation = "Receive Data From:" + SenderSensor.ID;
                        log.Time = DateTime.Now;
                        log.Distance_M = Operations.DistanceBetweenTwoSensors(this, SenderSensor);
                        log.UsedEnergy_Nanojoule = EnergyModel.Receive(PublicParamerters.RoutingDataLength);

                        // set the remain battery Energy:
                        double remainEnergy = ResidualEnergy - log.UsedEnergy_Joule;
                        ResidualEnergy = remainEnergy;
                        log.RemaimBatteryEnergy_Joule = ResidualEnergy;

                        if (PublicParamerters.SaveNetworkRecords)
                        {
                            this.RoutingOperationsLog.Add(log);
                        }

                        datap.UsedEnergy_Joule += log.UsedEnergy_Joule;
                        // routing distance:
                        datap.Path += ">" + datap.Reciver.ID;
                        datap.RoutingDistance += log.Distance_M;
                        datap.Hops += 1;
                        datap.UsedEnergy_Joule += log.UsedEnergy_Joule;
                        datap.Delay += DelayModel.DelayModel.Delay(datap.Sender, datap.Reciver);
                        log.RelaySequence = datap.Hops;
                        log.PID = datap.PacektSequentID;
                        // Fowrward: colone
                        Datapacket forwardPacket = new Datapacket();
                        forwardPacket.SourceNodeID = datap.SourceNodeID;
                        forwardPacket.Distance = datap.Distance;
                        forwardPacket.RoutingDistance = datap.RoutingDistance;
                        forwardPacket.Path = datap.Path;
                        forwardPacket.Hops = datap.Hops;
                        forwardPacket.UsedEnergy_Joule = datap.UsedEnergy_Joule;
                        forwardPacket.Delay = datap.Delay;
                        forwardPacket.SourceNode = datap.SourceNode;
                        forwardPacket.PacektSequentID = datap.PacektSequentID;

                        Sensor RelayNode = datap.Reciver;
                        if (RelayNode.ID == PublicParamerters.SinkNode.ID)
                        {
                            PublicParamerters.SinkNode.PacketsList.Add(datap);
                        }
                        else
                        {
                            // FORWARD:
                            RelayNode.SendPacekt(datap.Reciver, forwardPacket);
                        }
                    }
                    else
                    {
                        this.Ellipse_Sensing_range.Fill = Brushes.Brown;// die out node
                                                                        // MessageBox.Show("DeadNODE!");
                    }
                }
            }
        }

      

        private void btn_send_packet_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Label lbl_title = sender as Label;
            switch (lbl_title.Name)
            {
                case "btn_send_1_packet":
                    {
                        GeneratePacekts();
                        break;
                    }
                case "btn_send_10_packet":
                    {
                        for (int j = 1; j <= 10; j++)
                        {
                            GeneratePacekts();
                        }
                        break;
                    }

                case "btn_send_100_packet":
                    {
                        for (int j = 1; j <= 100; j++)
                        {
                            GeneratePacekts();
                        }
                        break;
                    }

                case "btn_send_300_packet":
                    {
                        for (int j = 1; j <= 300; j++)
                        {
                            GeneratePacekts();
                        }
                        break;
                    }

                case "btn_send_1000_packet":
                    {
                        for (int j = 1; j <= 1000; j++)
                        {
                            GeneratePacekts();
                        }
                        break;
                    }

                case "btn_send_5000_packet":
                    {
                        for (int j = 1; j <= 5000; j++)
                        {
                            GeneratePacekts();
                        }
                        break;
                    }
            }   
        }

        private void Prog_batteryCapacityNotation_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double val = ResidualEnergyPercentage;
            if (val <= 0)
            {
                // dead certificate:
                Lifetime.DeadNodesRecord recod = new Lifetime.DeadNodesRecord();
                recod.DeadAfterPackets = PublicParamerters.PackeTSequenceID;
                recod.DeadOrder = PublicParamerters.DeadNodeList.Count + 1;
                recod.Rounds = PublicParamerters.Rounds + 1;
                recod.DeadNodeID = ID;
                recod.NOS = PublicParamerters.NOS;
                recod.NOP = PublicParamerters.NOP;
                PublicParamerters.DeadNodeList.Add(recod);

                Prog_batteryCapacityNotation.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Coloring.col0));
                Ellipse_center.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Coloring.col0));

                //
                if (Settings.Default.StopeWhenFirstNodeDeid)
                {
                    MainWindow.TimerCounter.Stop();
                    MainWindow.RandomSelectSourceNodesTimer.Stop();
                    MainWindow.stopSimlationWhen = PublicParamerters.SimulationTime;
                    MainWindow.top_menu.IsEnabled = true;
                }

            }
            if (val >= 1 && val <= 9)
            {
                Prog_batteryCapacityNotation.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Coloring.col1_9));
                Ellipse_center.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Coloring.col1_9));
            }

            if (val >= 10 && val <= 19)
            {
                Prog_batteryCapacityNotation.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Coloring.col10_19));
                Ellipse_center.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Coloring.col10_19));
            }

            if (val >= 20 && val <= 29)
            {
                Prog_batteryCapacityNotation.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Coloring.col20_29));
                Ellipse_center.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Coloring.col20_29));
            }

            // full:
            if (val >= 30 && val <= 39)
            {
                Prog_batteryCapacityNotation.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Coloring.col30_39));
                Ellipse_center.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Coloring.col30_39));
            }
            // full:
            if (val >= 40 && val <= 49)
            {
                Prog_batteryCapacityNotation.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Coloring.col40_49));
                Ellipse_center.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Coloring.col40_49));
            }
            // full:
            if (val >= 50 && val <= 59)
            {
                Prog_batteryCapacityNotation.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Coloring.col50_59));
                Ellipse_center.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Coloring.col50_59));
            }
            // full:
            if (val >= 60 && val <= 69)
            {
                Prog_batteryCapacityNotation.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Coloring.col60_69));
                Ellipse_center.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Coloring.col60_69));
            }
            // full:
            if (val >= 70 && val <= 79)
            {
                Prog_batteryCapacityNotation.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Coloring.col70_79));
                Ellipse_center.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Coloring.col70_79));
            }
            // full:
            if (val >= 80 && val <= 89)
            {
                Prog_batteryCapacityNotation.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Coloring.col80_89));
                Ellipse_center.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Coloring.col80_89));
            }
            // full:
            if (val >= 90 && val <= 100)
            {
                Prog_batteryCapacityNotation.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Coloring.col90_100));
                Ellipse_center.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Coloring.col90_100));
            }
        }

        public double ResidualEnergyPercentage
        {
            get { return (ResidualEnergy / BatteryIntialEnergy) * 100; }
        }
        private void lbl_Sensing_ID_MouseEnter(object sender, MouseEventArgs e)
        {
            this.ToolTip = new Label() { Content = ResidualEnergyPercentage.ToString("00.00") + "%" };
        }
    }
}
