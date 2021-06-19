using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resuscitate
{
    class ExportData
    {
        public string ID { get; set; }
        public string TimeOfBirth { get; set; }

        public ExportData(string ID, string TimeOfBirth)
        {
            this.ID = ID;
            this.TimeOfBirth = TimeOfBirth;
        }

        public override bool Equals(object obj)
        {
            var other = (ExportData)obj;
            return ID == other.ID && TimeOfBirth == other.TimeOfBirth;
        }
    }
}
