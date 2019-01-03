using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeBasedWsnSimulator.Logs  
{
    public class SensorRoutingLog
    {
        public long PID { get; set; }
        public int RelaySequence { get; set; } // the sequnces of forwards the packet of the nodes.
        public int NodeID { get; set; }
      
        public string Operation { get; set; } // sent to/ recive form .. ID
        public double UsedEnergy_Nanojoule { get; set; } // the energy used for current operation
        public double UsedEnergy_Joule //the energy used for current operation
        {
            get
            {
                double _e9 = 1000000000; // 1*e^-9
                double _ONE = 1;
                double oNE_DIVIDE_e9 = _ONE / _e9;
                double re = UsedEnergy_Nanojoule * oNE_DIVIDE_e9;
                return re;
            }
        }
        public double RemaimBatteryEnergy_Joule { get; set; } // the remain energy of battery
        public double Distance_M { get; set; }
        public bool IsSend { get; set; } // is sending operation
        public bool IsReceive { get; set; }
        public DateTime Time { get; set; }


    }
}
