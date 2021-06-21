using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resuscitate.DataClasses
{
    public class ReviewDataAndTiming
    {
        public ReviewDataAndTiming(Timing t, StatusList s, PatientData pd)
        {
            Timing = t;
            StatusList = s;
            PatientData = pd;
        }

        public Timing Timing { get; set; }
        public StatusList StatusList { get; set; }
        public PatientData PatientData { get; set; }
    }
}
