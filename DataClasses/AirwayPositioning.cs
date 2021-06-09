using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resuscitate.DataClasses
{
    class AirwayPositioning : Event
    {
        private Timing time;
        private Positioning pos;

        public AirwayPositioning(Timing time, Positioning pos)
        {
            this.time = time;
            this.pos = pos;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("Timing: " + time.ToString() + "\n");
            if (pos == Positioning.Neutral)
            {
                sb.Append("\tNeutral head position");
            } else if (pos == Positioning.RHP)
            {
                sb.Append("\tRecheck head position and jaw support");
            } else
            {
                sb.Append("\tTwo-person technique");
            }

            return sb.ToString();
        }
    }

    enum Positioning
    {
        Neutral,
        RHP,
        TPT
    }
}
