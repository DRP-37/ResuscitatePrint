using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resuscitate.DataClasses
{
    public class PatientTiming
    {
        public PatientTiming(Timing t, PatientData pd)
        {
            Timing = t;
            PatientData = pd;
        }

        public Timing Timing { get; set; }
        public PatientData PatientData { get; set; }
    }
}
