using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using TreeBasedWsnSimulator.DataPacket;
using TreeBasedWsnSimulator.Modules;
using TreeBasedWsnSimulator.Parameters;
using TreeBasedWsnSimulator.ui;

namespace TreeBasedWsnSimulator.ExpermentsResults
{
    class ResultsObject 
    {
        public double AverageEnergyConsumption { get; set; }
        public double AverageHops { get; set; }
        public double AverageWaitingTime { get; set; }
        public double AverageRedundantTransmissions { get; set; }
        public double AverageRoutingDistance { get; set; }
        public double AverageTransmissionDistance { get; set; }
    }

    public class ValParPair
    {
        public string Par { get; set; }
        public string Val { get; set; }
    }

    /// <summary>
    /// Interaction logic for ExpReport.xaml
    /// </summary>
    public partial class ExpReport : Window
    {


        public ExpReport(MainWindow _mianWind)
        {
            InitializeComponent();

            List<ValParPair> List = new List<ValParPair>();
            ResultsObject res = new ResultsObject();
            double TotalResulaEnerg = 0;
            foreach(Sensor sen in _mianWind.myNetWork) 
            {
                if(sen.ID!=PublicParamerters.SinkNode.ID)
                {
                    TotalResulaEnerg += sen.ResidualEnergy;
                }
            }

            double hopsCoun = 0;
            double routingDisEf = 0;
            double avergTransDist = 0;
            foreach(Datapacket pk in PublicParamerters.SinkNode.PacketsList)
            {
                hopsCoun += pk.Hops;
                routingDisEf += pk.RoutingDistanceEfficiency;
                avergTransDist += pk.AverageTransDistrancePerHop;
            }
            double NumberOFpACKETSGENERATED = Convert.ToDouble(PublicParamerters.PackeTSequenceID);
           
           
            res.AverageEnergyConsumption = ((_mianWind.myNetWork.Count - 1) * PublicParamerters.BatteryIntialEnergy) - TotalResulaEnerg;

            res.AverageHops = hopsCoun / NumberOFpACKETSGENERATED;
            res.AverageRoutingDistance = routingDisEf / NumberOFpACKETSGENERATED;
            res.AverageTransmissionDistance = avergTransDist / NumberOFpACKETSGENERATED;

            List.Add(new ValParPair() {Par="Number of Nodes", Val= _mianWind.myNetWork.Count.ToString() } );
            List.Add(new ValParPair() { Par = "Density", Val = PublicParamerters.Density.ToString()});
            List.Add(new ValParPair() { Par = "Packet Rate", Val = _mianWind.PacketRate });
            List.Add(new ValParPair() { Par = "Simulation Time", Val = _mianWind.stopSimlationWhen.ToString()+" s" });
            List.Add(new ValParPair() { Par = "Total Energy Consumption", Val = res.AverageEnergyConsumption.ToString() });
            List.Add(new ValParPair() { Par = "Average Hops/path", Val = res.AverageHops.ToString() });
            List.Add(new ValParPair() { Par = "Average Redundant Transmissions/path", Val = res.AverageRedundantTransmissions.ToString() });
            List.Add(new ValParPair() { Par = "Average Routing Distance/path", Val = res.AverageRoutingDistance.ToString() });
            List.Add(new ValParPair() { Par = "Average Transmission Distance/Hop", Val = res.AverageTransmissionDistance.ToString() });
            List.Add(new ValParPair() { Par = "Average Waiting Time/path", Val = res.AverageWaitingTime.ToString() });

            List.Add(new ValParPair() { Par = "# gen pck", Val = NumberOFpACKETSGENERATED.ToString() });
            List.Add(new ValParPair() { Par = "Protocol", Val = "Tree" });
            dg_data.ItemsSource = List;
        }
    }
}
