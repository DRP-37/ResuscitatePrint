using Resuscitate.DataClasses;
using System;
using System.Collections.Generic;
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
    public sealed partial class BloodGasPage : Page
    {
        public Timing TimingCount { get; set; }
        private TextBox[] textBoxes;
        private double?[] values;
        private StatusEvent[] StatusEvents;
        private string[] Suffixes;

        private BloodGas bloodGas;

        public BloodGasPage()
        {
            this.InitializeComponent();
            textBoxes = new TextBox[] { pH, pCO2, excess, lactate, glucose, haemoglobin };
            Suffixes = new string[] { "", "", "", "", " mmol/l", " g/l" };
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Take value from previous screen
            TimingCount = (Timing)e.Parameter;
            bloodGas = new BloodGas();

            values = new double?[6];
            StatusEvents = new StatusEvent[6];

            base.OnNavigatedTo(e);
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            List<Event> Events = new List<Event>();
            List<StatusEvent> statusEvents = new List<StatusEvent>();

            foreach (StatusEvent StatusEvent in StatusEvents)
            {
                if (StatusEvent != null)
                {
                    statusEvents.Add(StatusEvent);
                }
            }

            if (statusEvents.Count <= 0)
            {
                return;
            }

            foreach (TextBox textBox in textBoxes)
            {
                SolidColorBrush colour = textBox.Background as SolidColorBrush;

                if (colour.Color == Colors.LightPink && !String.IsNullOrWhiteSpace(textBox.Text))
                {
                    return;
                }
            }

            Frame.Navigate(typeof(Resuscitation), new EventAndTiming(TimingCount, Events, statusEvents));
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Resuscitation), TimingCount);
        }

        private void TimeView_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            int index = Array.IndexOf(textBoxes, textBox);

            if (index == 2)
            {
                // Excess also enables minus character
                textBox.Text = new String(textBox.Text.Where(c => char.IsDigit(c) || c == '.' || c == '-').ToArray());
            }
            else
            {
                textBox.Text = new String(textBox.Text.Where(c => char.IsDigit(c) || c == '.').ToArray());
            }

            double value;
            bool valid = false;
            double.TryParse(textBox.Text, out value);

            switch (index)
            {
                case 0:
                    // pH - between 6 and 7, has two decimal places
                    valid = (value >= 6 && value < 8) && LessThanXDecimalPlaces(2, value);
                    break;
                case 1:
                    //pCO2 - one or two digits, has one decimal place
                    valid = (value < 100) && LessThanXDecimalPlaces(1, value);
                    break;
                case 2:
                    //excess - signed one or two digit integer part with one decimal place​
                    valid = (value < 100 && value > -100) && LessThanXDecimalPlaces(1, value);
                    break;
                case 3:
                    //lactate - one or two digit integer part, one decimal place​
                    valid = (value < 100) && LessThanXDecimalPlaces(1, value);
                    break;
                case 4:
                    //glucose - one or two digit integer part and one digit decimal​
                    valid = (value < 100) && LessThanXDecimalPlaces(1, value);
                    break;
                case 5:
                    //haemoglobin - two or three digit integer ​
                    valid = (value > 9) && (value < 1000);
                    break;
                default:
                    break;
            }

            if (valid)
            {
                textBox.Background = new SolidColorBrush(Colors.White);
                textBox.BorderBrush = new SolidColorBrush(Colors.Black);
                values[index] = value;
                StatusEvents[index] = new StatusEvent(textBox.Tag.ToString(), value + Suffixes[index], TimingCount.Time, bloodGas);
            }
            else
            {
                textBox.Background = new SolidColorBrush(Colors.LightPink);
                textBox.BorderBrush = new SolidColorBrush(Colors.PaleVioletRed);
                values[index] = null;
                StatusEvents[index] = null;
            }
        }

        private bool LessThanXDecimalPlaces(int x, double number)
        { 
            double decimalPlaces = Math.Pow(10, x);
            double value = number * decimalPlaces;
            return value == (int) value;
        }
    }
}
