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

        public ApgarScore(Timing time, int col, int hr, int responce, int tone, int resp) {
            this.time = time;
            this.colour = col;
            this.hr = hr;
            this.response = responce;
            this.tone = tone;
            this.respiration = resp;
        }

        override public String ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("Time: " + time.ToString() + '\n');
            sb.Append("Colour: " + colour.ToString() + '\n');
            sb.Append("Heart Rate: " + hr.ToString() + '\n');
            sb.Append("Response to Stimulation: " + response.ToString() + '\n');
            sb.Append("Muscle Tone: " + tone.ToString() + '\n');
            sb.Append("Respiratory Effort: " + respiration.ToString() + '\n');

            return sb.ToString();
        }
    }
}
