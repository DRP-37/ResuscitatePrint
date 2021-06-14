using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resuscitate.DataClasses
{
    public class AirwayPositioning : Event
    {
        private Timing time;
        private Positioning pos;

        public Timing Time { get => time; set => time = value; }
        public Positioning Positioning { get => pos; set => pos = value; }


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("Timing: " + time.ToString() + ", Position: " + positionToString());

            return sb.ToString();
        }

        public string positionToString()
        {
            if (pos == Positioning.Neutral)
            {
                return "Neutral head position";
            }
            else if (pos == Positioning.RHP)
            {
                return "Recheck head position and jaw support";
            }
            else
            {
                return "Two-person technique";
            }
        }
    }

    public enum Positioning
    {
        Neutral,
        RHP,
        TPT
    }
}
