using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resuscitate.DataClasses
{
    class EventAndTiming
    {
        public EventAndTiming(Timing t, Event e) {
            Timing = t;
            MedicalEvent = e;
        }

        public Timing Timing { get; set; }
        internal Event MedicalEvent { get; set; }
    }
}
