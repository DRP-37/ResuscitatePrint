using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resuscitate
{
    class ApgarScore
    {
        public ApgarScore(TimeSpan time)
        {
            Time = time;
        }

        public TimeSpan Time { get; set; }
        public int HeartRate { get; set; }
        public int Colour { get; set; }
        public int Response { get; set; }
        public int Tone { get; set; }
        public int Respiration { get; set; }

        override
        public String ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("Time: " + Time.ToString() + '\n');
            sb.Append("Colour: " + Colour.ToString() + '\n');
            sb.Append("Heart Rate: " + HeartRate.ToString() + '\n');
            sb.Append("Response to Stimulation: " + Response.ToString() + '\n');
            sb.Append("Muscle Tone: " + Tone.ToString() + '\n');
            sb.Append("Respiratory Effort: " + Respiration.ToString() + '\n');

            return sb.ToString();
        }
    }
}