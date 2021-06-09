using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resuscitate.DataClasses
{
    public enum TemperatureReg { 
        Dry,
        PretermBag,
        TermBag
    }
    public enum CordClamping { 
        Immediate,
        Now,
        Delayed
    }
    internal class InitialAssessment : Event
    {
        private ApgarScore Apgar;
        private TemperatureReg TempReg;
        private CordClamping Clamping;
        private Timing Time;

        public InitialAssessment(ApgarScore apgar, TemperatureReg temp, CordClamping cc, Timing ts)
        {
            this.Apgar = apgar;
            this.TempReg = temp;
            this.Clamping = cc;
            this.Time = ts;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Initial Assessment at " +);
            sb.Append('\t' + Apgar.ToString() + '\n');
            sb.Append('\t' + TempToString(TempReg) + '\n');
            sb.Append('\t' + CordClampToString(Clamping));
            if (Clamping == CordClamping.Now)
            {
                sb.Append(" " + Time.ToString() + '\n');
            }
            else {
                sb.Append('\n');
            }

            return sb.ToString();
        }
        private String TempToString(TemperatureReg temp) {
            switch (temp)
            {
                case TemperatureReg.Dry:
                    return "Dry and Wrap";
                case TemperatureReg.PretermBag:
                    return "Bag (Preterm)";
                case TemperatureReg.TermBag:
                    return "Bag (Term)";
                default: 
                    return "";

            }
        }

        private String CordClampToString(CordClamping clamping) {
            switch (clamping)
            {
                case CordClamping.Immediate:
                    return "Immediate";
                case CordClamping.Delayed:
                    return "Delayed";
                case CordClamping.Now:
                    return "Delayed";
                default:
                    return "";
            }
        }
    }
}
