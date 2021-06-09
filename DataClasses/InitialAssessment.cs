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
        ApgarScore Apgar;
        TemperatureReg TempReg;
        CordClamping Clamping;
        TimeSpan Time;

        internal ApgarScore Apgar { get => Apgar; set => Apgar = value; }
        public TemperatureReg TempReg { get => TempReg; set => TempReg = value; }
        public CordClamping Clamping { get => Clamping; set => Clamping = value; }
        public TimeSpan Time { get => Time; set => Time = value; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Initial Assessment at " +);
            sb.Append('\t' + Apgar.toString() + '\n');
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
                    break;
                case TemperatureReg.PretermBag:
                    return "Bag (Preterm)";
                    break;
                case TemperatureReg.TermBag:
                    return "Bag (Term)";
                    break;
                default: 
                    return "";
                    break;

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
