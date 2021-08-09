using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;

namespace Resuscitate.DataClasses
{
    public class Timing : INotifyPropertyChanged
    {
        public readonly DispatcherTimer Timer = new DispatcherTimer();
        public bool IsSet { get; set; }
        public int Offset { get; set; }
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

        private int Count { get; set; }
        private int startTime;

        public Timing(bool isSet, int offset)
        {
            IsSet = isSet;
            Offset = offset;
        }

        public Timing(bool isSet) : this(isSet, 0) { }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            // Raise the PropertyChanged event, passing the name of the property whose value has changed.
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }

        public void InitTiming()
        {
            Count = 0;
            startTime = Environment.TickCount;
            Time = ToString();

            // Start timer
            Timer.Tick += Timer_Tick;
            Timer.Interval = TimeSpan.FromSeconds(1);
            Timer.Start();
        }

        internal void Stop()
        {
            Timer.Stop();
        }

        private void Timer_Tick(object sender, object e)
        {
            //Count++;
            //Time = ToString();

            var elapsed = TimeSpan.FromMilliseconds(Environment.TickCount - startTime);

            if (elapsed.TotalMinutes < 10)
            {
                Time = '0' + string.Format("{00}:{1:00}", (int)elapsed.TotalMinutes, elapsed.Seconds);
            }
            else
            {
                Time = string.Format("{00}:{1:00}", (int)elapsed.TotalMinutes, elapsed.Seconds);
            }
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
