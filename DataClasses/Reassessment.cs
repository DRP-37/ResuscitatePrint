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

        public HeartRate Hr { get => Hr; set => Hr = value; }
        public ChestMovement Movement { get => Movement; set => Movement = value; }
        public RespiratoryEffort Effort { get => Effort; set => Effort = value; }
        public TimeSpan Time { get => Time; set => Time = value; }



        public override String ToString() {
            StringBuilder sb = new StringBuilder();
            sb.Append("Reassessment at " + Time.ToString() + " :\n");
            sb.Append('\t' + "Heart Rate (bpm): " + HeartRateToString() + '\n');
            sb.Append('\t' + "Chest Movement: " + Movement.ToString() + '\n');
            sb.Append('\t' + "Respiratory Effor: " + RespEffortToString() + "\n");
            sb.Append('\t' + "Timespan: " + Time.ToString());

            return sb.ToString();
        }

        private String HeartRateToString() {
            switch (Hr) {
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

        private String RespEffortToString()
        {
            switch (Effort)
            {
                case RespiratoryEffort.None:
                    return "No Effort";
                case RespiratoryEffort.Gasping:
                    return "Gasping";
                case RespiratoryEffort.Weak:
                    return "Weak Effort";
                case RespiratoryEffort.Regular:
                    return "Regular Respirations";
                default:
                    return "";
            }

        }
    }
}
