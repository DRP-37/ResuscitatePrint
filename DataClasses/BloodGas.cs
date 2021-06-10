using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resuscitate.DataClasses
{
    class BloodGas : Event
    {
        private Timing time;
        private float pH;
        private float lactate;
        private float glucose;
        public Timing Time { get => time; set => time = value; }
        public float PH { get => pH; set => pH = value; }
        public float Lactate { get => lactate; set => lactate = value; }
        public float Glucose { get => glucose; set => glucose = value; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"Timing: {time.ToString()}, pH: {pH.ToString()}, Lactate: {lactate.ToString()}, Glucose: {glucose.ToString()}");

            return sb.ToString();
        }

    }
}
