using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreeBasedWsnSimulator.Computations; 

namespace TreeBasedWsnSimulator.Computations 
{
    /// <summary>
    /// generate anumber between 0- max:
    /// </summary>
    public static class UnformRandomNumberGenerator
    {
        public static double GetUniform(double max) 
        {
            return max * SimpleRNG.GetUniform();
        }
    }
}
