using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resuscitate.DataClasses
{
    class Notes : Event
    {
        public Notes(string note)
        {
            this.Note = note;
        }

        public string Note { get; set; }

        override public String ToString()
        {
            return Note;
        }
    }
}
