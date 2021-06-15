using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resuscitate
{
    class StaffList
    {
        private ObservableCollection<StaffMemberData> _members = new ObservableCollection<StaffMemberData>();

        public ObservableCollection<StaffMemberData> Members
        {
            get { return this._members; }
        }
    }
}
