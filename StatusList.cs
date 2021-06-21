
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resuscitate
{
    public class StatusList
    {
        private ObservableCollection<StatusEvent> _events = new ObservableCollection<StatusEvent>();

        public ObservableCollection<StatusEvent> Events
        {
            get { return this._events; }
        }
    }
}