using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resuscitate.DataClasses
{
    public class OtherProcedures : Event
    {
        private string time;
        private ProcedureType procedureType;

        public string Time { get => time; set => time = value; }
        public ProcedureType Procedure { get => procedureType; set => procedureType = value; }

        override public String ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("Time: " + Time + ", Procedure: " + procedureToString());

            return sb.ToString();
        }

        public String procedureToString()
        {
            if (procedureType == ProcedureType.ChestLeft)
            {
                return "Left Chest Drain";
            } else if (procedureType == ProcedureType.ChestRight)
            {
                return "Right Chest Drain";
            } else
            {
                return "Abdominal Drain";
            }
        }
    }

    public enum ProcedureType
    {
        ChestLeft,
        ChestRight,
        Abdominal
    }
}
