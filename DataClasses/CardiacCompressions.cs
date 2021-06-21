using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resuscitate.DataClasses
{
    public class CardiacCompressions : Event
    {
        private Timing time;
        private string length;

        public Timing Time { get => time; set => time = value; }
        public string Length { get => length; set => length = value; }

        override public string ToString()
        {
            return $"Cardiac compression at: {time.ToString()} for {length} seconds\n";
        }
    }
}
