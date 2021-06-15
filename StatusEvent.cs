using Resuscitate.DataClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resuscitate
{
    public class StatusEvent
    {
        public string Name { get; set; }
        public string Data { get; set; }
        public string Time { get; set; }

        public Event Event { get; set; }

        public StatusEvent() { }

        // TODO: add Event to constructor
        public StatusEvent(string Name, string Data, string Time)
        {
            this.Name = Name;
            this.Data = Data;
            this.Time = Time;
        }

    }
}
