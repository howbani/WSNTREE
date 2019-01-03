using System;
using System.Collections.Generic;
using TreeBasedWsnSimulator.Computations;
using TreeBasedWsnSimulator.Modules;

namespace TreeBasedWsnSimulator.Lifetime
{
    public class NetworkLifeTime
    {
        /// <summary>
        /// how many sensor to be slected.
        /// how many packet each sensor will sent.
        /// </summary>
        /// <param name="NOS"></param>
        /// <param name="NOP"></param>
        public void RandimSelect(List<Sensor> Network, int NOS, int NOP)
        {
            // selecte The Nodes:
            List<Sensor> SelectedSn = new List<Modules.Sensor>(NOS);
            for (int i = 0; i < NOS; i++)
            {
                int ran = Convert.ToInt16(UnformRandomNumberGenerator.GetUniform(Network.Count - 1));
                if (ran > 0)
                {
                    SelectedSn.Add(Network[ran]);
                }
                else
                {
                    SelectedSn.Add(Network[1]);
                }
            }

            // each packet sendt NOP:
            for (int i = 0; i < NOP; i++)
            {
                foreach (Sensor sen in SelectedSn)
                {
                    sen.GeneratePacekts();
                }
            }

        } // end class random generated.


        /// <summary>
        /// how many sensor to be slected.
        /// how many packet each sensor will sent.
        /// </summary>
        /// <param name="NOS"></param>
        /// <param name="NOP"></param>
        public void FromAllNodes(List<Sensor> Network)
        {
            foreach (Sensor sen in Network)
            {
                sen.GeneratePacekts();
            }
        } // end class random generated.

    }
}
