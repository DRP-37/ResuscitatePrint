using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resuscitate.DataClasses
{
    public class Ventilation : Event
    {
        private Timing time;
        private VentilationType ventType;
        private float oxygen;
        public Timing Time { get => time; set => time = value; }
        public VentilationType VentType { get => ventType; set => ventType = value; }
        public float Oxygen { get => oxygen; set => oxygen = value; }

        override public String ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"Ventilation at: {time.ToString()}\n");
            sb.Append($"\tVentilation type: {ventToString()}\n");
            sb.Append($"Air/Oxygen given: {oxygen}%\n");

            return sb.ToString();
        }


        public string ventToString()
        {
            if (ventType == VentilationType.InflationMask)
            {
                return "Inflation via Mask";
            }
            else if (ventType == VentilationType.InflationETT)
            {
                return "Inflation via ETT";
            }
            else if (ventType == VentilationType.VentMask)
            {
                return "Ventilation via Mask";
            }
            else if (ventType == VentilationType.VentETT)
            {
                return "Ventilation via ETT";
            }
            else if (ventType == VentilationType.MaskCPAP)
            {
                return "Mask CPAP";
            }
            else
            {
                return "";
            }
        }
    }

    public enum VentilationType
    {
        InflationMask,
        InflationETT,
        VentMask,
        VentETT,
        MaskCPAP
    }
}
