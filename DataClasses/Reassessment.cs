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
    class Reassessment :  Event
    {
        HeartRate hr;
        ChestMovement Movement;
        RespiratoryEffort effort;
        TimeSpan time;

        public HeartRate Hr { get => Hr; set => Hr = value; }
        public ChestMovement Movement { get => Movement; set => Movement = value; }
        public RespiratoryEffort Effort { get => effort; set => effort = value; }
        public TimeSpan Time { get => time; set => time = value; }

        public override String ToString() {
            StringBuilder sb = new StringBuilder();
            sb.Append("Reassessment:" + '\n');
            sb.Append('\t' + "Heart rate (bpm): " + HeartRateToString() + '\n');
            sb.Append('\t' + "Chest movement: " + Movement.ToString() + '\n');
            sb.Append('\t' + )
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
