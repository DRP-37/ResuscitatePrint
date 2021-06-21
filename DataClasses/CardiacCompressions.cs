using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resuscitate.DataClasses
{
    public class CardiacCompressions : Event
    {
        private Timing time;
        private string length;

        public Timing Time { get => time; set => time = value; }
        public string Length { get => length; set => length = value; }

        override public string ToString()
        {
<<<<<<< HEAD
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
                    sb.Append(start[i].Time + " - " + end[i].Time + "\n");
                }
            }

            if (start.Count > end.Count)
            {
                sb.Append(start[start.Count - 1] + " -");
            }

            return sb.ToString();
=======
            return $"Cardiac compression at: {time.ToString()} for {length} seconds\n";
>>>>>>> 83df2222bb19982320f4ebf496725d7c36392872
        }
    }
}
