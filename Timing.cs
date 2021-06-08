using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Resuscitate
{
    class Timing
    {
        public bool IsSet { get; set; }
        public int Offset { get; set; }
        public int Count { get; set; }

        public int TotalTime()
        {
            return Offset + Count;
        }

        public override string ToString()
        {
            string minsStr, secsStr;

            int Time = Count + Offset;
            int mins = Time / 60;
            int secs = Time % 60;

            minsStr = mins < 10 ? "0" + mins.ToString() : mins.ToString();
            secsStr = secs < 10 ? "0" + secs.ToString() : secs.ToString();

            return minsStr + ":" + secsStr;
        }
    }
}
