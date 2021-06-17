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
        DispatcherTimer Timer = new DispatcherTimer();
        private int Count = 0;

        private List<StatusEvent> StatusEvents;
        private CardiacCompressions compressions;
        private static bool START_EVENT = true;

        public CPRPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Enabled;

            // Initialise timer
            Timer.Tick += Timer_Tick;
            Timer.Interval = new TimeSpan(0, 0, 1);

            StartButton.Background = new SolidColorBrush(Colors.LightGray);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Take value from previous screen
            TimingCount = (Timing)e.Parameter;

            compressions = new CardiacCompressions();
            StatusEvents = new List<StatusEvent>();

            base.OnNavigatedTo(e);
        }

        private void Timer_Tick(object sender, object e)
        {
            Count++;
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
                Timer.Start();
                StatusEvents.Add(GenerateStatusEvent(START_EVENT, compressions));

                HeartBeating.Visibility = Visibility.Visible;
                StartButton.Content = "Stop";
                StartButton.Background = new SolidColorBrush(Colors.MediumVioletRed);

                Resuscitation.cprTimer = Stopwatch.StartNew();
            }
            else
            {
                Timer.Stop();
                StatusEvents.Add(GenerateStatusEvent(!START_EVENT, compressions));

                Count = 0;
                HeartBeating.Visibility = Visibility.Collapsed;
                StartButton.Content = "Start";
                StartButton.Background = new SolidColorBrush(Colors.LightGray);

                Resuscitation.cprTimer.Reset();
            }
        }

        // StartEvent is true when generating a CPR start event. If it is a CPR end event then it is false.
        private StatusEvent GenerateStatusEvent(bool StartEvent, Event Event)
        {
            string Data = StartEvent ? "Started" : "Ended after " + Count + " seconds";
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
    }
}
