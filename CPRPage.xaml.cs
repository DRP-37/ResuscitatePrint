using Resuscitate.DataClasses;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Resuscitate
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CPRPage : Page
    {
        public Timing TimingCount { get; set; }

        private List<StatusEvent> StatusEvents;
        private CardiacCompressions compressions;
        private static bool START_EVENT = true;

        public CPRPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Enabled;

            StartButton.Background = new SolidColorBrush(Colors.LightGray);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Take value from previous screen
            TimingCount = (Timing)e.Parameter;

            compressions = new CardiacCompressions();
            StatusEvents = new List<StatusEvent>();

            SetStartStopButton();

            base.OnNavigatedTo(e);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            List<Event> Events = new List<Event>();
            Events.Add(compressions);
            

            Frame.Navigate(typeof(Resuscitation), new EventAndTiming(TimingCount, Events, StatusEvents));
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (StartButton.Content.Equals("Start"))
            {
                StatusEvents.Add(GenerateStatusEvent(START_EVENT, compressions));

                Resuscitation.cprTimer = Stopwatch.StartNew();
            }
            else
            {
                StatusEvents.Add(GenerateStatusEvent(!START_EVENT, compressions));

                Resuscitation.cprTimer.Stop();
                Resuscitation.cprTimer.Reset();
            }

            SetStartStopButton();
        }

        private void SetStartStopButton()
        {
            if (Resuscitation.cprTimer.IsRunning)
            {
                HeartBeating.Visibility = Visibility.Visible;
                StartButton.Content = "Stop";
                StartButton.Background = ConverHexColour("#FFDB4325");
            } else
            {
                HeartBeating.Visibility = Visibility.Collapsed;
                StartButton.Content = "Start";
                StartButton.Background = new SolidColorBrush(Colors.LightGray);
            }
        }

        // StartEvent is true when generating a CPR start event. If it is a CPR end event then it is false.
        private StatusEvent GenerateStatusEvent(bool StartEvent, CardiacCompressions Event)
        {
            string Seconds = "-1";

            if (Resuscitation.cprTimer.IsRunning) {
                string Milieconds = Resuscitation.cprTimer.ElapsedMilliseconds.ToString();
                Seconds = Milieconds.Substring(0, Milieconds.Length - 3);
                Event.Time = TimingCount;
                Event.Length = Seconds;
            }

            string Data = StartEvent ? "Started" : "Ended after " + Seconds + " seconds";
            return new StatusEvent("Cardiac Compressions", Data, TimingCount.Time, Event);
        }

        private void TimeView_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Nothing
        }

        private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {
            // Nothing
        }

        private SolidColorBrush ConverHexColour(string hexColour)
        {
            hexColour = hexColour.Replace("#", string.Empty);
            // from #RRGGBB string
            var s = (byte)System.Convert.ToUInt32(hexColour.Substring(0, 2), 16);
            var r = (byte)System.Convert.ToUInt32(hexColour.Substring(2, 2), 16);
            var g = (byte)System.Convert.ToUInt32(hexColour.Substring(4, 2), 16);
            var b = (byte)System.Convert.ToUInt32(hexColour.Substring(6, 2), 16);
            //get the color
            Color color = Color.FromArgb(s, r, g, b);
            // create the solidColorbrush
            return new SolidColorBrush(color);
        }
    }
}
