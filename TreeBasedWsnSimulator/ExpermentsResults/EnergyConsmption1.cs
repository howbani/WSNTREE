using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreeBasedWsnSimulator.Modules;
using TreeBasedWsnSimulator.Parameters;

namespace TreeBasedWsnSimulator.ExpermentsResults
{
 /*
Distance =50:
Sources: 10, 592, 575, 46,236
Distance 100:
Sources: 91,511,151,538,530
Distance: 150:
Sources: 28, 418, 99,310,115
Distance 200:
Sources: 369, 505, 422, 591, 326
Distance 250:
Sources: 222, 569, 395, 344, 588 
 */
    public class EnergyConsmption1 
    {
        public int Distance { get; set; }
        public int Number_of_Packets_tobe_Sent { get; set; }
        public double EnergyConsumption { get; set; }
        public double SumRoutingEfficiency { get; set; }
        public double AvaerageRoutingEfficiency { get { return SumRoutingEfficiency / Number_of_RecievedPackets; } }
        public int Number_of_RecievedPackets { get; set; }
        public List<Sensor> Nodes = new List<Sensor>();
    }


    public class DoEnergyConsmption1Experment
    {
        private int NumPackets=0; 
        private string d50Str = "50-10,592,575,46,236";
        private string d100Str = "100-91,511,151,538,530";
        private string d150Str = "150-28,418,99,310,115";
        private string d200Str = "200-369,505,422,591,326";
        private string d250Str = "250-222,569,395,344,588";
        List<string> Distances = new List<string>();
        List<EnergyConsmption1> Results = new List<EnergyConsmption1>();
        private List<Sensor> NETWORK;
        public DoEnergyConsmption1Experment(List<Sensor> nET,int _NumPackets)
        {
            NETWORK = nET;
            NumPackets = _NumPackets; 
            Distances.Add(d50Str);
            Distances.Add(d100Str);
            Distances.Add(d150Str);
            Distances.Add(d200Str);
            Distances.Add(d250Str);


            foreach(string distance in Distances)
            {
                EnergyConsmption1 x = new EnergyConsmption1();
                x.Number_of_Packets_tobe_Sent = _NumPackets;
                x.Distance = Convert.ToInt32(distance.Split('-')[0]); // distance.
                string nodesStr = distance.Split('-')[1];
                string[] nodes = nodesStr.Split(',');
                foreach(string node in nodes)
                {
                    if(node.ToString()!="")
                    {
                        int id = Convert.ToInt16(node);
                        x.Nodes.Add(NETWORK[id]);
                    }
                }
                Results.Add(x);
            }

        }
         
        private void SumEffeciancyAndConsumption(EnergyConsmption1 exp)
        {
            
            foreach(DataPacket.Datapacket pa in PublicParamerters.SinkNode.PacketsList)
            {
                exp.EnergyConsumption += pa.UsedEnergy_Joule;
                exp.SumRoutingEfficiency += pa.RoutingEfficiency;
            }
            
        }


        public List<EnergyConsmption1> Perform()
        {
            foreach (EnergyConsmption1 exp in Results)
            {
                for (int i = 1; i <= NumPackets; i++)
                {
                    foreach (Sensor node in exp.Nodes)
                    {
                        node.GeneratePacekts();
                    }
                }

                // collect:
                exp.Number_of_RecievedPackets = PublicParamerters.SinkNode.PacketsList.Count;
                SumEffeciancyAndConsumption(exp);
                PublicParamerters.SinkNode.PacketsList.Clear();// clear.
            }
            return Results;
        }
    }
}
