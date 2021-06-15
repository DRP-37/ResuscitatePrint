using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resuscitate.DataClasses
{
    public class Medication : Event
    {
        private Timing time;
        private bool adrenaline1 = false;
        private bool adrenaline3 = false;
        private bool nahco = false;
        private bool dextrose = false;
        private bool redCell = false;
        private bool adrenaline = false;
        private bool surfactant120 = false;
        private bool surfactant240 = false;

        public Timing Time { get => time; set => time = value; }
        public bool Adrenaline1 { get => adrenaline1; set => adrenaline1 = value; }
        public bool Adrenaline3 { get => adrenaline3; set => adrenaline3 = value; }
        public bool Nahco { get => nahco; set => nahco = value; }
        public bool Dextrose { get => dextrose; set => dextrose = value; }
        public bool RedCell { get => redCell; set => redCell = value; }
        public bool Adrenaline { get => adrenaline; set => adrenaline = value; }
        public bool Surfactant120 { get => surfactant120; set => surfactant120 = value; }
        public bool Surfactant240 { get => surfactant240; set => surfactant240 = value; }

        public void setData(bool[] doses)
        {
            adrenaline1 = doses[0];
            adrenaline3 = doses[1];
            nahco = doses[2];
            dextrose = doses[3];
            redCell = doses[4];
            adrenaline = doses[5];
            surfactant120 = doses[6];
            surfactant240 = doses[7];
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"At time: {time}, medications administered:");

            if (adrenaline1)
            {
                sb.Append($"\tAdrenaline 1 in 10,000 (0.1 ml/kg) IV");
            }
            if (adrenaline3)
            {
                sb.Append($"\tAdrenaline 1 in 10,000 (0.3 ml/kg) IV");
            }
            if (nahco)
            {
                sb.Append($"\tSodium Bicarbonate 4.2% (2 to 4 mls/kg) IV");
            }
            if (dextrose)
            {
                sb.Append($"\tDextrose (2.5 mls/kg) IV");
            }
            if (redCell)
            {
                sb.Append($"\tRed cell transfusion");
            }
            if (adrenaline)
            {
                sb.Append($"\tAdrenaline via ETT");
            }
            if (surfactant120)
            {
                sb.Append($"\tSurfactant via ETT 120mg");
            }
            if (surfactant240)
            {
                sb.Append($"\tSurfactant via ETT 240mg");
            }
            return sb.ToString();
        }
    }
}
