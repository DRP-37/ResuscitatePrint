using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resuscitate.DataClasses
{
    class Observation : Event
    {
        

        public float OximeterOxygen { get => OximeterOxygen; set => OximeterOxygen = value; }
        public float Hr { get => Hr; set => Hr = value; }
        public float OxygenGiven { get => OxygenGiven; set => OxygenGiven = value; }
        public TimeSpan Time { get => Time; set => Time = value; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Observation at " + Time.ToString() + ":\n");
            sb.Append("\tHeart Rate (bpm): " + Hr.ToString() + '\n');
            sb.Append("\tOximeter Oxygen Level: " + OximeterOxygen.ToString() + "%\n");
            sb.Append("\tPercentage Oxygen Given: " + OxygenGiven.ToString() + "%\n");
            return sb.ToString();
        }
    }
}
