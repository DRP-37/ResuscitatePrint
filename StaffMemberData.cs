using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resuscitate
{
    class StaffMemberData
    {
        public string Name { get; set; }
        public string Position { get; set; }
        public string Grade { get; set; }
        public string TimeOfArrival { get; set; }

        public StaffMemberData(string Name, string Position, string Grade, string TimeOfArrival)
        {
            this.Name = Name;
            this.Position = Position;
            this.Grade = Grade;
            this.TimeOfArrival = TimeOfArrival;
        }
    }
}
