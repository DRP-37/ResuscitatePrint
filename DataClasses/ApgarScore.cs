using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resuscitate.DataClasses
{
    class ApgarScore : Event
    {
        private Timing time;
        private int colour;
        private int hr;
        private int response;
        private int tone;
        private int respiration;

        internal Timing Time { get => time; set => time = value; }
        public int Colour { get => colour; set => colour = value; }
        public int Hr { get => hr; set => hr = value; }
        public int Response { get => response; set => response = value; }
        public int Tone { get => tone; set => tone = value; }
        public int Respiration { get => respiration; set => respiration = value; }

        override public String ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("Time: " + Time.ToString() + '\n');
            sb.Append("Colour: " + Colour.ToString() + '\n');
            sb.Append("Heart Rate: " + Hr.ToString() + '\n');
            sb.Append("Response to Stimulation: " + Response.ToString() + '\n');
            sb.Append("Muscle Tone: " + Tone.ToString() + '\n');
            sb.Append("Respiratory Effort: " + Respiration.ToString() + '\n');
            sb.Append("Total score: " + totalScore().ToString());

            return sb.ToString();
        }

        public int totalScore()
        {
            return Colour + Hr + Response + Tone + Respiration;
        }
    }
}
