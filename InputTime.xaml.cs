using System;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI;
using Resuscitate.DataClasses;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Resuscitate
{

    public sealed partial class InputTime : Page
    {
        public PatientData PatientData;

        private const bool IS_SET = true;
        private const bool NOT_SET = false;

        private static readonly Color INCORRECT_INPUT_BACKGROUND_COLOUR = Colors.Red;

        private Timing TimingCount;

        public InputTime()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
            {
                if (e.Parameter.GetType() == typeof(ReviewDataAndTiming))
                {
                    var RDaT = (ReviewDataAndTiming)e.Parameter;
                    TimingCount = RDaT.Timing;
                    PatientData = RDaT.PatientData;
                }
                else if (e.Parameter.GetType() == typeof(PatientData))
                {
                    PatientData = (PatientData)e.Parameter;
                }
            }

            TimeHours.PlaceholderText = DateTime.Now.ToString("HH");
            TimeMinutes.PlaceholderText = DateTime.Now.ToString("mm");
        }

        private void InputLater_Click(object sender, RoutedEventArgs e)
        {
            // Set timer
            TimingCount = new Timing(NOT_SET);
            TimingCount.InitTiming();

            // Go to main page
            this.Frame.Navigate(typeof(Resuscitation), new ReviewDataAndTiming(TimingCount, null, PatientData));
        }

        private void Now_Click(object sender, RoutedEventArgs e)
        {
            // Set timer
            TimingCount = new Timing(IS_SET);
            TimingCount.InitTiming();

            // Initialise StatusList and first StatusEvent
            StatusList statusList = InitialiseStatusList(DateTime.Now.ToString("HH:mm"));

            // Go to main page
            this.Frame.Navigate(typeof(Resuscitation), new ReviewDataAndTiming(TimingCount, statusList, PatientData));
        }

        private void SetTime_Click(object sender, RoutedEventArgs e)
        {
            int? maybeOffsetMins = GetOffsetMinsFromInput();

            if (maybeOffsetMins == null)
            {
                // Change background of SetTime to red
                SetTime.Background = new SolidColorBrush(INCORRECT_INPUT_BACKGROUND_COLOUR);
                return;
            }

            int offsetMins = (int) maybeOffsetMins;

            // Set timer
            TimingCount = new Timing(IS_SET, offsetMins * 60);
            TimingCount.InitTiming();

            StatusList statusList = InitialiseStatusList($"{TimeHours.Text}:{TimeMinutes.Text}");

            // Go to main page
            this.Frame.Navigate(typeof(Resuscitation), new ReviewDataAndTiming(TimingCount, statusList, PatientData));
        }

        private void PatientInfo_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(PatientPage), new ReviewDataAndTiming(TimingCount, null, PatientData));
        }

        private void StaffInfo_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(StaffPage), PatientData);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame.CanGoBack)
            {
                rootFrame.GoBack();
            }
        }

        private void TimeMinutes_TextChanged(object sender, TextChangedEventArgs e)
        {
            TimeTextChanged((TextBox)sender);
        }

        private void TimeSeconds_TextChanged(object sender, TextChangedEventArgs e)
        {
            TimeTextChanged((TextBox) sender);
        }

        private StatusList InitialiseStatusList(string timeOfBirth)
        {
            StatusList statusList = new StatusList();
            StatusEvent timeOfBirthEvent = new StatusEvent("Time of Birth", timeOfBirth, "00:00");
            statusList.Events.Add(timeOfBirthEvent);

            PatientData.Tob = timeOfBirth;

            return statusList;
        }

        private int? GetOffsetMinsFromInput()
        {
            int hours, mins;

            TimeHours.Text = TimeHours.Text == "" ? TimeHours.PlaceholderText : TimeHours.Text;
            TimeMinutes.Text = TimeMinutes.Text == "" ? TimeMinutes.PlaceholderText : TimeMinutes.Text;

            // Parse minutes and seconds input
            bool isHoursParsable = Int32.TryParse(TimeHours.Text, out hours);

            if (TimeHours.Text == "")
            {
                hours = 0;
                isHoursParsable = true;
            }

            isHoursParsable &= hours < 25;

            bool isMinsParsable = Int32.TryParse(TimeMinutes.Text, out mins);
            if (TimeMinutes.Text == "")
            {
                mins = 0;
                isMinsParsable = true;
            }
            isHoursParsable &= mins < 60;

            // Incorrect input
            if (!isHoursParsable || !isMinsParsable)
            {
                return null;
            }

            int CurrentHours, CurrentMinutes;
            Int32.TryParse(DateTime.Now.ToString("HH"), out CurrentHours);
            Int32.TryParse(DateTime.Now.ToString("mm"), out CurrentMinutes);

            return (CurrentHours - hours) * 60 + CurrentMinutes - mins;
        }

        private void TimeTextChanged(TextBox timeBox)
        {
            if (timeBox.Text.Length > 2)
            {
                timeBox.Text = timeBox.Text.Substring(0, 2);
            }

            timeBox.Text = new String(timeBox.Text.Where(c => char.IsDigit(c)).ToArray());
        }

        private void TimeColon_Copy_SelectionChanged(object sender, RoutedEventArgs e)
        {
            // Nothing
        }

        private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {
            // Nothing
        }

        private void TextBlock_SelectionChanged_1(object sender, RoutedEventArgs e)
        {
            // Nothing
        }
    }
}
