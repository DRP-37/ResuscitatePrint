using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resuscitate.DataClasses
{
    public class BloodGas : Event
    {
        private string time;
        private float pH;
        private float lactate;
        private float glucose;
        private float pco2;
        private float excess;
        private float haemoglobin;
        public string Time { get => time; set => time = value; }
        public float PH { get => pH; set => pH = value; }
        public float Lactate { get => lactate; set => lactate = value; }
        public float Glucose { get => glucose; set => glucose = value; }
        public float PCO2 { get => pco2; set => pco2 = value; }
        public float Excess { get => excess; set => excess = value; }
        public float Haemoglobin { get => haemoglobin; set => haemoglobin = value; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"Timing: {Time}, pH: {pH.ToString()}, Lactate: {lactate.ToString()}, Glucose: {glucose.ToString()} mmol/l, PCO2: {PCO2.ToString()}, Excess: {Excess.ToString()}, Haemoglobin: {Haemoglobin.ToString()} g/l");

            return sb.ToString();
        }

    }
}
