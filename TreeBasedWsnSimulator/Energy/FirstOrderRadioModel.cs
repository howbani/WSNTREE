using System;
using TreeBasedWsnSimulator.Parameters;

namespace TreeBasedWsnSimulator.Energy
{
    public class FirstOrderRadioModel
    {
        public double E_elec;
        public double Efs;
        public double Emp;

        public FirstOrderRadioModel()
        {
            E_elec = PublicParamerters.E_elec;
            Efs = PublicParamerters.Efs;
            Emp = PublicParamerters.Emp;
        }
      


        public double d0  //Distance threshold ( unit m)
        {
            get { return Math.Sqrt(Efs / Emp); }
        }
        /// <summary>
        /// Each sensor node will consume the following ETx amount of energy to transmit a (L=DataLength)bits message 
        /// over distance d:
        /// </summary>
        /// <returns></returns>
        public double Transmit(double k,double d) 
        {
            double E_tx = 0; 
            if(d<=d0)
            {
                E_tx = (k * E_elec) + 
                    (k * Efs * d * d);
            }
            else if(d>d0)
            {
                E_tx = (k * E_elec) +
                    (k * Emp * d * d * d * d);
            }
            return E_tx;
        }
        /// <summary>
        /// ERx amount of energy to receive this message:
        /// </summary>
        /// <param name="k"></param>
        /// <returns></returns>
        public double Receive(double k)
        {
            double ERx = k * E_elec;
            return ERx;
        }

       



        
    }
}
