using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Resuscitate.DataClasses
{
    public class StaffList
    {
        private ObservableCollection<StaffMemberData> _members = new ObservableCollection<StaffMemberData>();

        public ObservableCollection<StaffMemberData> Members
        {
            get { return _members; }
        }

        override public string ToString()
        {
            StringBuilder sb = new StringBuilder("Staff Members Present:\n\n");

            foreach (StaffMemberData staffMember in Members)
            {
                sb.AppendLine(staffMember.ToString());
            }

            return sb.ToString();
        }
    }
}
