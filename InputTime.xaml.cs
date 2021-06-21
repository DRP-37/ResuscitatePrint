using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Diagnostics;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI;
using Resuscitate.DataClasses;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Resuscitate
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class InputTime : Page
    {

        public PatientData patientData;
        public Timing TimingCount { get; set; }

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
                    patientData = RDaT.PatientData;
                }
                else if (e.Parameter.GetType() == typeof(PatientData))
                {
                    patientData = (PatientData)e.Parameter;
                }
            }

            TimeHours.PlaceholderText = DateTime.Now.ToString("HH");
            TimeMinutes.PlaceholderText = DateTime.Now.ToString("mm");
        }

        private void InputLater_Click(object sender, RoutedEventArgs e)
        {
            // Set timer
            TimingCount = new Timing { IsSet = false, Offset = 0 };
            TimingCount.InitTiming();

            // Go to main page
            this.Frame.Navigate(typeof(Resuscitation), new ReviewDataAndTiming(TimingCount, null, patientData));
        }

        private void Now_Click(object sender, RoutedEventArgs e)
        {
            // Set timer
            TimingCount = new Timing { IsSet = true, Offset = 0 };
            TimingCount.InitTiming();

            StatusEvent timeOfBirthEvent = new StatusEvent("Time of Birth", DateTime.Now.ToString("HH:mm"), "00:00", null);
            StatusList statusList = new StatusList();
            statusList.Events.Add(timeOfBirthEvent);
            patientData.Tob = DateTime.Now.ToString("HH:mm");

            // Go to main page
            this.Frame.Navigate(typeof(Resuscitation), new ReviewDataAndTiming(TimingCount, statusList, patientData));
        }

        private void SetTime_Click(object sender, RoutedEventArgs e)
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
                // Change background of SetTime to red
                SetTime.Background = new SolidColorBrush(Colors.Red);
                return;
            }

            int CurrentHours, CurrentMinutes;
            Int32.TryParse(DateTime.Now.ToString("HH"), out CurrentHours);
            Int32.TryParse(DateTime.Now.ToString("mm"), out CurrentMinutes);

            int OffsetMins = (CurrentHours - hours) * 60 + CurrentMinutes - mins;

            // Set timer
            TimingCount = new Timing { IsSet = true, Offset = OffsetMins * 60 };
            TimingCount.InitTiming();

            StatusEvent timeOfBirthEvent = new StatusEvent("Time of Birth", TimeHours.Text + ":" + TimeMinutes.Text, "00:00", null);
            StatusList statusList = new StatusList();
            patientData.Tob = $"{TimeHours.Text}:{TimeMinutes.Text}";
            statusList.Events.Add(timeOfBirthEvent);

            // Go to main page
            this.Frame.Navigate(typeof(Resuscitation), new ReviewDataAndTiming(TimingCount, statusList, patientData));
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
            TextBox textBox = (TextBox)sender;

            if (textBox.Text.Length > 2)
            {
                textBox.Text = textBox.Text.Substring(0, 2);
            }

            textBox.Text = new String(textBox.Text.Where(c => char.IsDigit(c)).ToArray());
        }

        private void TimeSeconds_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (textBox.Text.Length > 2)
            {
                textBox.Text = textBox.Text.Substring(0, 2);
            }

            textBox.Text = new String(textBox.Text.Where(c => char.IsDigit(c)).ToArray());
        }

        private void TimeColon_Copy_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        private void PatientInfo_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(PatientPage), new ReviewDataAndTiming(TimingCount, null, patientData));
        }

        private void StaffInfo_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(StaffPage), patientData);
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
