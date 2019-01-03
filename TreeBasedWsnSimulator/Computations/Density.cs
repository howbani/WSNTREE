using System;
using System.Collections.Generic;
using System.Linq;
using TreeBasedWsnSimulator.Modules;

namespace TreeBasedWsnSimulator.Computations
{
    public  class Density
    {
        private static double miu(List<Sensor> net)
        {
            double sum = 0;
            double n = net.Count;
            foreach ( Sensor s in net)
            {
                if (s.OverlappingNodesList  != null)
                {
                    double x = s.OverlappingNodesList.Count;
                    sum += x;
                }
            }
            return sum / n;
        }

        /// <summary>
        /// standared deviasion.
        /// </summary>
        /// <param name="net"></param>
        /// <returns></returns>
        public static double GetDensity(List<Sensor> net)
        {
            double n = net.Count;
            double mean = miu(net);
            double sum = 0;
            foreach(Sensor s in net)
            {
                if (s.OverlappingNodesList != null)
                {
                    double x = s.OverlappingNodesList.Count;
                    double va = (x - mean) * (x - mean);
                    double cas = x * va;
                    sum += cas;
                }
            }
            return (Math.Sqrt((1 / n) * sum)) / (2 * Math.PI);
        }

    }
}
