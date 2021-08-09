using System.Collections.Generic;

namespace Resuscitate.DataClasses
{
    public class TimingAndEvents
    {
        public TimingAndEvents(Timing t, List<StatusEvent> s) {
            Timing = t;
            StatusEvents = s;
        }

        public Timing Timing { get; set; }
        public List<StatusEvent> StatusEvents { get; set; }
    }
}
