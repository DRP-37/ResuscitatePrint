using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resuscitate.DataClasses
{
    class TimingAndReview
    {
        public TimingAndReview(Timing t, StatusList s)
        {
            Timing = t;
            StatusList = s;
        }

        public Timing Timing { get; set; }
        internal StatusList StatusList { get; set; }
    }
}
