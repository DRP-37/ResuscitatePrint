using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resuscitate.DataClasses
{
    class OtherProcedures : Event
    {
        List<(Timing, ProcedureType)> otherProcedures = new List<(Timing, ProcedureType)>();

        public void addProcedure(Timing time, ProcedureType procedure)
        {
            otherProcedures.Add((time, procedure));
        }

        override public String ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var p in otherProcedures)
            {
                sb.Append("Time: " + p.Item1.ToString() + ", Procedure: " + p.Item2.ToString() + "\n");
            }

            return sb.ToString();
        }
    }

    enum ProcedureType
    {
        ChestLeft,
        ChestRight,
        Abdominal
    }
}
