using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resuscitate.DataClasses
{
    public enum RespiratoryEffort { 
        None,
        Gasping,
        Weak,
        Regular
    }

    public enum HeartRate  {
        LessSixty,
        SixtyToEighty,
        EightyToHundred,
        GreaterHundred
    }

    public enum ChestMovement { 
        Absent,
        Present
    }
    class Reassessment : Event
    {
        private HeartRate hr;
        private ChestMovement movement;
        private RespiratoryEffort effort;
        private Timing time;

        public Reassessment(HeartRate hr, ChestMovement cm, RespiratoryEffort e, Timing ts)
        {
            this.hr = hr;
            this.movement = cm;
            this.effort = e;
            this.time = ts;
        }

        public override String ToString() {
            StringBuilder sb = new StringBuilder();
            sb.Append("Reassessment:" + '\n');
            sb.Append('\t' + "Heart Rate (bpm): " + HeartRateToString() + '\n');
            sb.Append('\t' + "Chest Movement: " + movement.ToString() + '\n');
            sb.Append('\t' + "Respiratory Effor: " + effort.ToString() + "\n");
            sb.Append('\t' + "Timespan: " + time.ToString());

            return sb.ToString();
        }

        private String HeartRateToString() {
            switch (hr) {
                case HeartRate.LessSixty:
                    return "<60";
                case HeartRate.SixtyToEighty:
                    return "60 to 80";
                case HeartRate.EightyToHundred:
                    return "80 to 100";
                case HeartRate.GreaterHundred:
                    return ">100";                   
                default:
                    return "";

            }
        }

        private String 

    }
}
