using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resuscitate.DataClasses
{
    public enum IntubationConfirmation { 
        ETCO2,
        EqualAirEntry,
        UnequalAirEntry
    }
    class IntubationAndSuction
    {
        private bool suction;
        private bool intubation;
        private bool intubationSuccess;
        private IntubationConfirmation confirmation;
        private Timing time;

        public bool Suction { get => suction; set => suction = value; }
        public bool Intubation { get => intubation; set => intubation = value; }
        public bool IntubationSuccess { get => intubationSuccess; set => intubationSuccess = value; }
        public IntubationConfirmation Confirmation { get => confirmation; set => confirmation = value; }
        internal Timing Time { get => time; set => time = value; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Intubation and Suction at " + Time.ToString() + ":\n");
            if (Suction){
                sb.Append("\tSuction under direct vision\n");
            }
            if (Intubation) {
                sb.Append(intubationSuccess ? "Successful" : "Unsuccessful" + "intubation");
                if (intubationSuccess)
                {
                    sb.Append(" with" + ConfirmationToString() + " confirmation\n");
                }
                else {
                    sb.Append('\n');
                } 
            }
            return sb.ToString();

        }

        private String ConfirmationToString() {
            switch (Confirmation) {
                case IntubationConfirmation.ETCO2:
                    return "ETC02";
                case IntubationConfirmation.EqualAirEntry:
                    return "Equal Air Entry";
                case IntubationConfirmation.UnequalAirEntry:
                    return "Unequal Air Entry";
                default:
                    return "";
            }
        }
    }
}
