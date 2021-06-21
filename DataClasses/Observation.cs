using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resuscitate.DataClasses
{
    public class Observation : Event
    {
        private Timing time;
        private string oximeterOxygen;
        private string hr;
        private string oxygenGiven;

        public Timing Time { get => time; set => time = value; }
        public string OximeterOxygen { get => oximeterOxygen; set => oximeterOxygen = value; }
        public string Hr { get => hr; set => hr = value; }
        public string OxygenGiven { get => oxygenGiven; set => oxygenGiven = value; }

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
