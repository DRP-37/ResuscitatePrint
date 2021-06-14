using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resuscitate.DataClasses
{
    class CardiacCompressions : Event
    {
        private List<Timing> start = new List<Timing>();
        private List<Timing> end = new List<Timing>();

        public void startedCompressions(Timing time)
        {
            start.Add(time);
        }

        public void stopCompressions(Timing time)
        {
            end.Add(time);
        }

        override public string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("Timings of cardiac compressions: \n");

            if (end.Count > start.Count)
            {
                sb.Append("- " + end[end.Count - 1]);
            }

            for (int i = 0; i < start.Count; i++)
            {
                if (i < end.Count)
                {
                    sb.Append(start[i].ToString() + " - " + end[i].ToString() + "\n");
                }
            }

            if (start.Count > end.Count)
            {
                sb.Append(start[start.Count - 1] + " -");
            }

            return sb.ToString();
        }
    }
}
