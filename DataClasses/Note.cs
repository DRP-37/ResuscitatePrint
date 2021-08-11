using System;

namespace Resuscitate.DataClasses
{
    public class Note
    {
        public string Text { get; set; }

        public Note(string text)
        {
            this.Text = text;
        }

        override public string ToString()
        {
            return Text;
        }
    }
}
