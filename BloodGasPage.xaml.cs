using Resuscitate.DataClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Resuscitate
{
    public sealed partial class BloodGasPage : Page
    {
        // Default TextBox colours
        private static readonly Color INCORRECT_INPUT_BACKGROUND_COLOR = InputUtils.DEFAULT_INCORRECT_INPUT_BACKGROUND;

        private const int PH_INDEX = 0;
        private const int PCO2_INDEX = 1;
        private const int EXCESS_INDEX = 2;
        private const int LACTATE_INDEX = 3;
        private const int GLUCOSE_INDEX = 4;
        private const int HAEMOGLOBIN_INDEX = 5;

        private Timing TimingCount;
        private StatusEvent[] StatusEvents;
        private string[] Suffixes;

        private TextBox[] BloodGasViews;

        public BloodGasPage()
        {
            this.InitializeComponent();
            BloodGasViews = new TextBox[] { pH, pCO2, excess, lactate, glucose, haemoglobin };
            Suffixes = new string[] { "", "", "", "", " mmol/l", " g/l" };
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Take value from previous screen
            TimingCount = (Timing)e.Parameter;

            StatusEvents = new StatusEvent[6];

            base.OnNavigatedTo(e);
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            List<StatusEvent> statusEvents = new List<StatusEvent>();

            foreach (StatusEvent statusEvent in StatusEvents)
            {
                if (statusEvent != null)
                {
                    statusEvents.Add(statusEvent);
                }
            }

            if (statusEvents.Count <= 0)
            {
                return;
            }

            foreach (TextBox textBox in BloodGasViews)
            {
                SolidColorBrush colour = textBox.Background as SolidColorBrush;

                if (colour.Color == INCORRECT_INPUT_BACKGROUND_COLOR && !String.IsNullOrWhiteSpace(textBox.Text))
                {
                    // TODO: flyout here?
                    return;
                }
            }

            Frame.Navigate(typeof(Resuscitation), new TimingAndEvents(TimingCount, statusEvents));
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            int index = Array.IndexOf(BloodGasViews, textBox);

            // Disallow input of incorrect characters
            if (index == EXCESS_INDEX)
            {
                // Excess also enables minus character
                textBox.Text = new String(textBox.Text.Where(c => char.IsDigit(c) || c == '.' || c == '-').ToArray());
            } else
            {
                textBox.Text = new String(textBox.Text.Where(c => char.IsDigit(c) || c == '.').ToArray());
            }

            double value = -1;
            bool valid = !string.IsNullOrWhiteSpace(textBox.Text) 
                && double.TryParse(textBox.Text, out value) && CheckRangeValidity(value, index);

            StatusEvents[index] = valid ? new StatusEvent(textBox.Tag.ToString(), value + Suffixes[index], TimingCount.Time) : null;

            InputUtils.UpdateValidColours(textBox, valid);
        }

        private bool CheckRangeValidity(double value, int index)
        {
            switch (index)
            {
                case PH_INDEX:
                    // pH - between 6 and 7, has two decimal places
                    return (value >= 6 && value < 8) && LessThanXDecimalPlaces(2, value);
                case PCO2_INDEX:
                    //pCO2 - one or two digits, has one decimal place
                    return (value < 100) && LessThanXDecimalPlaces(1, value);
                case EXCESS_INDEX:
                    //excess - signed one or two digit integer part with one decimal place​
                    return (value < 100 && value > -100) && LessThanXDecimalPlaces(1, value);
                case LACTATE_INDEX:
                    //lactate - one or two digit integer part, one decimal place​
                    return (value < 100) && LessThanXDecimalPlaces(1, value);
                case GLUCOSE_INDEX:
                    //glucose - one or two digit integer part and one digit decimal​
                    return (value < 100) && LessThanXDecimalPlaces(1, value);
                case HAEMOGLOBIN_INDEX:
                    //haemoglobin - two or three digit integer ​
                    return (value > 9) && (value < 1000);
            }

            return false;
        }

        private bool LessThanXDecimalPlaces(int x, double number)
        { 
            double decimalPlaces = Math.Pow(10, x);
            double value = number * decimalPlaces;
            return value == (int) value;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Resuscitation), TimingCount);
        }

        private void TimeView_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Nothing
        }
    }
}
