using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resuscitate.DataClasses
{
    class LineInsertion : Event
    {

        private Timing time;
        private InsertionType insertionType;
        private bool successful;

        public LineInsertion(Timing time, InsertionType insertionType, bool successful)
        {
            this.time = time;
            this.insertionType = insertionType;
            this.successful = successful;
        }

        override public string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("Time: " + time.ToString() + "\n");
            if (insertionType == InsertionType.UVC)
            {
                sb.Append("Umbilical vein catherter insertion\n");
            } else
            {
                sb.Append("Intraosseous line insertion\n");
            }

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
    }

    enum InsertionType
    {
        UVC,
        IL
    }
}
