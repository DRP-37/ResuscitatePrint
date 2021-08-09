using System;

namespace Resuscitate.DataClasses
{
    public class Notes
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
