using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resuscitate.DataClasses
{
    class Observation : Event
    {
        private float oximeterOxygen;
        private float hr;
        private float oxygenGiven;
        private Timing time;

        public float OximeterOxygen { get => oximeterOxygen; set => oximeterOxygen = value; }
        public float Hr { get => hr; set => hr = value; }
        public float OxygenGiven { get => oxygenGiven; set => oxygenGiven = value; }
        public Timing Time { get => time; set => time = value; }

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
