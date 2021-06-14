using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resuscitate.DataClasses
{
    public class EventAndTiming
    {
        public EventAndTiming(Timing t, List<Event> e, List<StatusEvent> s) {
            Timing = t;
            MedicalEvents = e;
            StatusEvents = s;
        }

        public Timing Timing { get; set; }
        public List<Event> MedicalEvents { get; set; }
        public List<StatusEvent> StatusEvents { get; set; }
    }
}
