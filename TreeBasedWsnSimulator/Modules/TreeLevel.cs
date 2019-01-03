using System.Collections.Generic;

namespace TreeBasedWsnSimulator.Modules
{
    public class TreeLevel
    {
       public int ID { get; set; }
       private List<Sensor> Sensors = new List<Sensor>();
       public int Count { get { return Sensors.Count; } }
       public string SensorsString { get; set; }
       /// <summary>
       /// retun [i]th sensor in the List Sensor Sensors
       /// </summary>
       /// <param name="i"></param>
       /// <returns></returns>
       public Sensor this[int i]
       {
           get
           {
               // This indexer is very simple, and just returns or sets 
               // the corresponding element from the internal array. 
               return Sensors[i];
           }
           set
           {
               Sensors[i] = value;
           }
       }
       private void GetString()
       {
           SensorsString = "";
           foreach(Sensor sen in Sensors)
           {
               SensorsString += sen.ID + ",";
           }
       }
       public void Remove(Sensor sensor) 
       { 
           Sensors.Remove(sensor);
           GetString();
       }
       public void Add(Sensor sensor) { Sensors.Add(sensor); GetString(); }
       public void Add(List<Sensor> sensors) { Sensors.AddRange(sensors); GetString(); }
       public List<Sensor> Nodes { get { return Sensors; } }
    }
}
