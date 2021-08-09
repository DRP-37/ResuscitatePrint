using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;

namespace Resuscitate.DataClasses
{
    public class Timing : INotifyPropertyChanged
    {
        private const string ZERO_TIME = "00:00";

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

        /* Environment.TickCount of when the object was created */
        public int StartTime { get; }

        public Timing(bool isSet, int offset)
        {
            this.IsSet = isSet;
            this.Offset = offset;
            this.StartTime = Environment.TickCount;
            this.Time = ZERO_TIME;
        }

        public Timing(bool isSet) : this(isSet, 0) { }

        [JsonConstructor]
        private Timing(bool isSet, int offset, int startTime)
        {
            this.IsSet = isSet;
            this.Offset = offset;
            this.StartTime = startTime;
            this.Time = ZERO_TIME;

            InitTiming();
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            // Raise the PropertyChanged event, passing the name of the property whose value has changed.
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }

        public void InitTiming(int? startTime = null)
        {
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
            TimeSpan elapsed = TimeSpan.FromMilliseconds(Environment.TickCount - StartTime);

            if (elapsed.TotalMinutes < 10)
            {
                Time = '0' + string.Format("{00}:{1:00}", (int) elapsed.TotalMinutes, elapsed.Seconds);
            } else
            {
                Time = string.Format("{00}:{1:00}", (int) elapsed.TotalMinutes, elapsed.Seconds);
            }
        }

        public TimeSpan Elapsed()
        {
            return TimeSpan.FromMilliseconds(Environment.TickCount - StartTime);
        }

        public override string ToString()
        {
            return Time;
        }
    }
}
