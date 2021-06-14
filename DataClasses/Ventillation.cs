using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resuscitate.DataClasses
{
    public class Ventillation : Event
    {
        private Timing time;
        private VentillationType ventType;
        private float oxygen;
        public Timing Time { get => time; set => time = value; }
        public VentillationType VentType { get => ventType; set => ventType = value; }
        public float Oxygen { get => oxygen; set => oxygen = value; }

        override public String ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"Ventillation at: {time.ToString()}\n");
            sb.Append($"\tVentillation type: {ventToString()}\n");
            sb.Append($"Air/Oxygen given: {oxygen}%\n");

            return sb.ToString();
        }


        public string ventToString()
        {
            if (ventType == VentillationType.InflationMask)
            {
                return "Inflation via Mask";
            }
            else if (ventType == VentillationType.InflationETT)
            {
                return "Inflation via ETT";
            }
            else if (ventType == VentillationType.VentMask)
            {
                return "Ventillation via Mask";
            }
            else if (ventType == VentillationType.VentETT)
            {
                return "Ventillation via ETT";
            }
            else if (ventType == VentillationType.MaskCPAP)
            {
                return "Mask CPAP";
            }
            else
            {
                return "";
            }
        }
    }

    public enum VentillationType
    {
        InflationMask,
        InflationETT,
        VentMask,
        VentETT,
        MaskCPAP
    }
}
