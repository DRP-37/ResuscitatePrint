using System;
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
    public class InitialAssessment : Event
    {
        private int colour;
        private int tone;
        private int respEffort;
        private int heartRate;
        private TemperatureReg tempreg;
        private CordClamping clamping;
        private string time;

        
        public TemperatureReg TempReg { get => tempreg; set => tempreg = value; }
        public CordClamping Clamping { get => clamping; set => clamping = value; }
        public string Time { get => time; set => time = value; }
        public int Colour { get => colour; set => colour = value; }
        public int Tone { get => tone; set => tone = value; }
        public int RespEffort { get => respEffort; set => respEffort = value; }
        public int HeartRate { get => heartRate; set => heartRate = value; }

        public InitialAssessment(string t)
        {
            this.Time = t;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("Initial Assessment at " + Time + "\n");
            try
            {
                sb.Append('\t' + "Colour: "+ Colour.ToString() + '\n');
                sb.Append('\t' + "Muscle Tone: " + Tone.ToString() + '\n');
                sb.Append('\t' + "Respiratory Effort: " + respEffort.ToString() + '\n');
                sb.Append('\t' + "Heart Rate: " + heartRate.ToString() + '\n');
                sb.Append('\t' + TempToString(TempReg) + '\n');
                sb.Append('\t' + CordClampToString(Clamping));
                if (Clamping == CordClamping.Now)
                {
                    sb.Append(" " + Time + '\n');
                }
                else
                {
                    sb.Append('\n');
                }
            }
            catch
            {
                sb.Append("N/A");
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
