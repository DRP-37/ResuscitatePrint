using Resuscitate.DataClasses;
using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Resuscitate.Pages
{
    public sealed partial class CPRPage : Page
    {
        private static readonly Color SELECTED_COLOUR = InputUtils.DEFAULT_CPR_SELECTED_COLOUR;
        private static readonly Color UNSELECTED_COLOUR = InputUtils.DEFAULT_CPR_UNSELECTED_COLOUR;

        private ResuscitationData ResusData;
        private Timing TimingCount;

        private List<StatusEvent> CPREvents;

        public CPRPage()
        {
            this.InitializeComponent();

            StartButton.Background = new SolidColorBrush(UNSELECTED_COLOUR);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Take value from previous screen
            ResusData = (ResuscitationData)e.Parameter;
            TimingCount = ResusData.TimingCount;

            CPREvents = new List<StatusEvent>();
            UpdateStartStopButton(ResusData.CPRIsRunning());

            base.OnNavigatedTo(e);
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            bool IsStarting = StartButton.Content.Equals("Start");

            AddStatusEvent(IsStarting);
            UpdateStartStopButton(IsStarting);

            if (IsStarting)
            {
                ResusData.StartNewCPRTimer();

            } else
            {
                ResusData.StopCPRTimer();
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            ResusData.StatusList.AddAll(CPREvents);

            Frame.Navigate(typeof(Resuscitation), ResusData);
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

            } else
            {
                long? miliseconds = ResusData.CPRElapsedMiliseconds();

                if (miliseconds == null)
                {
                    throw new System.ArithmeticException();
                }

                string MilisecondsStr = miliseconds.ToString();
                string SecondsStr = "0";

                if (MilisecondsStr.Length > 3) { 
                    SecondsStr = MilisecondsStr.Substring(0, MilisecondsStr.Length - 3);
                }

                Data = "Ended after " + SecondsStr + " seconds";
            }

            CPREvents.Add(new StatusEvent("Cardiac Compressions", Data, TimingCount.Time));
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
