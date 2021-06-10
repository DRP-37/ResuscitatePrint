using Resuscitate.DataClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resuscitate
{
    class StatusEvent
    {
        public string Name { get; }
        public string Data { get; }
        public string Time { get; }

        public Event Event { get; }

        // TODO: add Event to constructor
        public StatusEvent(string Name, string Data, string Time)
        {
            this.Name = Name;
            this.Data = Data;
            this.Time = Time;
        }

    }
}
