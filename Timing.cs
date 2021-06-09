using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Windows.UI.Xaml;
using System.Runtime.CompilerServices;

namespace Resuscitate
{
    public class Timing : INotifyPropertyChanged
    {
        public readonly DispatcherTimer Timer = new DispatcherTimer();
        public bool IsSet { get; set; }
        public int Offset { get; set; }
        private int Count { get; set; }
        public string _time { get; set; }
        public string Time 
        {
            get { return _time; }
            set
            {
                _time = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            // Raise the PropertyChanged event, passing the name of the property whose value has changed.
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }

        public void InitTiming()
        {
            Count = 0;
            Time = ToString();

            // Start timer
            Timer.Tick += Timer_Tick;
            Timer.Interval = new TimeSpan(0, 0, 1);
            Timer.Start();
        }

        private void Timer_Tick(object sender, object e)
        {
            Count++;
            Time = ToString();
        }

        public int TotalTime()
        {
            return Offset + Count;
        }

        public override string ToString()
        {
            string minsStr, secsStr;

            int AllSeconds = Count + Offset;
            int mins = AllSeconds / 60;
            int secs = AllSeconds % 60;

            minsStr = mins < 10 ? "0" + mins.ToString() : mins.ToString();
            secsStr = secs < 10 ? "0" + secs.ToString() : secs.ToString();

            return minsStr + ":" + secsStr;
        }
    }
}
