using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resuscitate.DataClasses
{
    public class Observation : Event
    {
<<<<<<< HEAD
        private string time;
        private float oximeterOxygen;
        private float hr;
        private float oxygenGiven;

        public string Time { get => time; set => time = value; }
        public float OximeterOxygen { get => oximeterOxygen; set => oximeterOxygen = value; }
        public float Hr { get => hr; set => hr = value; }
        public float OxygenGiven { get => oxygenGiven; set => oxygenGiven = value; }
=======
        private Timing time;
        private string oximeterOxygen;
        private string hr;
        private string oxygenGiven;

        public Timing Time { get => time; set => time = value; }
        public string OximeterOxygen { get => oximeterOxygen; set => oximeterOxygen = value; }
        public string Hr { get => hr; set => hr = value; }
        public string OxygenGiven { get => oxygenGiven; set => oxygenGiven = value; }
>>>>>>> 83df2222bb19982320f4ebf496725d7c36392872

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Observation at " + Time + ":\n");
            sb.Append("\tHeart Rate (bpm): " + Hr.ToString() + '\n');
            sb.Append("\tOximeter Oxygen Level: " + OximeterOxygen.ToString() + "%\n");
            sb.Append("\tPercentage Oxygen Given: " + OxygenGiven.ToString() + "%\n");
            return sb.ToString();
        }
    }
}
