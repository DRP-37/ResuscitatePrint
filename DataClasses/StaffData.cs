using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resuscitate.DataClasses
{
    public class StaffData
    {

        private string staffName;
        private string staffTitle;
        private string grade;
        private string timeOfEntry;

        public string StaffName { get => staffName; set => staffName = value; }
        public string StaffTitle { get => staffTitle; set => staffTitle = value; }
        public string Grade { get => grade; set => grade = value; }
        public string TimeOfEntry { get => timeOfEntry; set => timeOfEntry = value; }

        override public string ToString()
        {
            return $"Name: {staffTitle} {StaffName}, Grade: {grade}, Time of Entry: {timeOfEntry}";
        }
    }
}
