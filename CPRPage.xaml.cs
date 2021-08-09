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
        private static readonly Color SELECTED_COLOUR = InputUtils.DEFAULT_CPR_SELECTED_COLOUR;
        private static readonly Color UNSELECTED_COLOUR = InputUtils.DEFAULT_CPR_UNSELECTED_COLOUR;

        private Timing TimingCount;
        private List<StatusEvent> StatusEvents;

        public CPRPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Enabled;

            StartButton.Background = new SolidColorBrush(UNSELECTED_COLOUR);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Take value from previous screen
            TimingCount = (Timing)e.Parameter;

            StatusEvents = new List<StatusEvent>();
            UpdateStartStopButton(Resuscitation.cprTimer.IsRunning);

            base.OnNavigatedTo(e);
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            bool IsStarting = StartButton.Content.Equals("Start");

            AddStatusEvent(IsStarting);
            UpdateStartStopButton(IsStarting);

            if (IsStarting)
            {
                Resuscitation.cprTimer = Stopwatch.StartNew();

            } else
            {
                Resuscitation.cprTimer.Stop();
                Resuscitation.cprTimer.Reset();
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Resuscitation), new TimingAndEvents(TimingCount, StatusEvents));
        }

        private void UpdateStartStopButton(bool HasStarted)
        {
            if (HasStarted)
            {
                // Set to be stopped when clicked
                HeartBeating.Visibility = Visibility.Visible;
                StartButton.Content = "Stop";
                StartButton.Background = new SolidColorBrush(SELECTED_COLOUR);
            } else
            {
                // Set to be started when clicked
                HeartBeating.Visibility = Visibility.Collapsed;
                StartButton.Content = "Start";
                StartButton.Background = new SolidColorBrush(UNSELECTED_COLOUR);
            }
        }

        // StartEvent is true when generating a CPR start event. If it is a CPR end event then it is false.
        private void AddStatusEvent(bool IsStartEvent)
        {
            string Data;

            if (IsStartEvent)
            {
                Data = "Started";

            }
            else
            {
                string Miliseconds = Resuscitation.cprTimer.ElapsedMilliseconds.ToString();
                string Seconds = "0";

                if (Miliseconds.Length > 3) { 
                    Seconds = Miliseconds.Substring(0, Miliseconds.Length - 3);
                }

                Data = "Ended after " + Seconds + " seconds";
            }

            StatusEvents.Add(new StatusEvent("Cardiac Compressions", Data, TimingCount.Time));
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
