using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Resuscitate.DataClasses
{
    public class StatusList
    {
        private ObservableCollection<StatusEvent> _events = new ObservableCollection<StatusEvent>();

        public ObservableCollection<StatusEvent> Events
        {
            get { return _events; }
        }

        private StatusList() { }

        // Static constructor
        public static StatusList fromTimeOfBirth(string timeOfBirth)
        {
            StatusList statusList = new StatusList();
            StatusEvent timeOfBirthEvent = new StatusEvent("Time of Birth", timeOfBirth, "00:00");
            statusList.Events.Add(timeOfBirthEvent);

            return statusList;
        }

        public StatusEvent LastItem()
        {
            return Events.Last();
        }

        public void AddAll(List<StatusEvent> statusEvents)
        {
            statusEvents.Sort(new TimeAscending());

            foreach (StatusEvent statusEvent in statusEvents)
            {
                Events.Add(statusEvent);
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("List of Events:\n\n");

            foreach (StatusEvent statusEvent in Events)
            {
                sb.AppendLine(statusEvent.ToString());
            }

            return sb.ToString();
        }

        private class TimeAscending : Comparer<StatusEvent>
        {
            public override int Compare(StatusEvent x, StatusEvent y)
            {
                return x.Time.CompareTo(y.Time);
            }
        }
    }
}