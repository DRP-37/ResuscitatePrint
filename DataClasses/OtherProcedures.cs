using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resuscitate.DataClasses
{
    class OtherProcedures : Event
    {
        private Timing time;
        private ProcedureType procedureType;

        public Timing Time { get => time; set => time = value; }
        public ProcedureType Procedure { get => procedureType; set => procedureType = value; }

        override public String ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("Time: " + time.ToString() + ", Procedure: " + procedureToString());

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

    enum ProcedureType
    {
        ChestLeft,
        ChestRight,
        Abdominal
    }
}
