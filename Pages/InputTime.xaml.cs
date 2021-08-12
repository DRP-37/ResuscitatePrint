using System;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI;
using Resuscitate.DataClasses;

namespace Resuscitate.Pages
{

    public sealed partial class InputTime : Page
    {
        private static readonly Color INCORRECT_INPUT_BACKGROUND_COLOUR = InputUtils.DEFAULT_INCORRECT_BUTTON_COLOUR;

        private const bool IS_SET = true;
        private const bool NOT_SET = false;

        // This ResuscitationData is never complete. It only contains a PatientData and StaffList
        private ResuscitationData MockData;

        public InputTime()
        {
            this.InitializeComponent();

            this.MockData = new ResuscitationData(new PatientData(), new StaffList());
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
            {
                MockData = (ResuscitationData)e.Parameter;

                MockData.SaveLocally();
            }

            TimeHours.PlaceholderText = DateTime.Now.ToString("HH");
            TimeMinutes.PlaceholderText = DateTime.Now.ToString("mm");
        }

        private void Now_Click(object sender, RoutedEventArgs e)
        {
            // Set timer
            Timing timingCount = new Timing(IS_SET);
            timingCount.InitTiming();

            string timeOfBirth = DateTime.Now.ToString("HH:mm");

            MockData.PatientData.Tob = timeOfBirth;

            // Go to main page
            this.Frame.Navigate(typeof(Resuscitation), 
                new ResuscitationData(timingCount, MockData.PatientData, MockData.StaffList, timeOfBirth));
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
            Timing timingCount = new Timing(IS_SET, offsetMins * 60);
            timingCount.InitTiming();

            string timeOfBirth = $"{TimeHours.Text}:{TimeMinutes.Text}";

            MockData.PatientData.Tob = timeOfBirth;

            // Go to main page
            this.Frame.Navigate(typeof(Resuscitation),
                new ResuscitationData(timingCount, MockData.PatientData, MockData.StaffList, timeOfBirth));
        }

        /* Currently collapsed */
        private void InputLater_Click(object sender, RoutedEventArgs e)
        {
            // Set timer
            Timing timingCount = new Timing(NOT_SET);
            timingCount.InitTiming();

            // Go to main page
            this.Frame.Navigate(typeof(Resuscitation),
                new ResuscitationData(timingCount, MockData.PatientData, MockData.StaffList, "Unknown"));
        }

        private void PatientInfo_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(PatientPage), MockData);
        }

        private void StaffInfo_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(StaffPage), MockData);
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
