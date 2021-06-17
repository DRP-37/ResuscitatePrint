using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resuscitate.DataClasses
{
    public class LineInsertion : Event
    {

        private Timing time;
        private InsertionType insertionType;
        private bool successful;

        public Timing Time { get => time; set => time = value; }
        public InsertionType Insertion { get => insertionType; set => insertionType = value; }
        public bool Successful { get => successful; set => successful = value; }

        override public string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("Time: " + time.ToString() + " ");
            sb.Append(insertionToString() + " ");

            if (successful)
            {
                sb.Append("Successful");
            }
            else
            {
                sb.Append("Unsuccessful");
            }
            return sb.ToString();
        }

        public string insertionToString()
        {
            if (insertionType == InsertionType.UVC)
            {
                return "Umbilical vein catherter insertion";
            }
            else
            {
                return "Intraosseous line insertion";
            }
        }
    }

    public enum InsertionType
    {
        UVC,
        IL
    }
}
