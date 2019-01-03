using System;
using System.Windows;
using TreeBasedWsnSimulator.Modules;

namespace TreeBasedWsnSimulator.Computations
{
    public class Operations
    {

        public static double DistanceBetweenTwoSensors(Sensor sensor1, Sensor sensor2)
        {
            double dx = (sensor1.CenterLocation.X - sensor2.CenterLocation.X);
            dx *= dx;
            double dy = (sensor1.CenterLocation.Y - sensor2.CenterLocation.Y);
            dy *= dy;
            return Math.Sqrt(dx + dy);
        }

        public static double DistanceBetweenTwoPoints(Point p1, Point p2) 
        {
            double dx = (p1.X - p2.X);
            dx *= dx;
            double dy = (p1.Y - p2.Y);
            dy *= dy;
            return Math.Sqrt(dx + dy);
        }

        /// <summary>
        /// Overlapped= true == overllapped
        /// 
        /// </summary>
        /// <param name="sensor1"></param>
        /// <param name="sensor2"></param>
        /// <returns></returns>
        public static bool isOverlapped(Sensor sensor1, Sensor sensor2)
        {
            bool re = false;
            double disttance = DistanceBetweenTwoSensors(sensor1, sensor2);
            if (disttance < (sensor1.ComunicationRangeRadius + sensor2.ComunicationRangeRadius))
            {
                re = true;
            }
            return re;
        }

        /// <summary>
        /// check if j is within the range of i.
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        public static bool isInMyRange(Sensor i, Sensor j)
        {
            bool re = false;
            double disttance = DistanceBetweenTwoSensors(i, j);
            if (disttance <= (i.ComunicationRangeRadius))
            {
                re = true;
            }
            return re;
        }

    }
}
