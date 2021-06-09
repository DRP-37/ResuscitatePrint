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

        public string toString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("Timings of cardiac compressions: \n");
            for (int i = 0; i < start.Count; i++)
            {
                sb.Append(start[i].ToString() + " - " + end[i].ToString() + "\n");
            }

            return sb.ToString();
        }
    }
}
