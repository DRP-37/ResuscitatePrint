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

        private BloodGas bloodGas;

        public BloodGasPage()
        {
            this.InitializeComponent();
            textBoxes = new TextBox[] { pH, pCO2, excess, lactate, glucose, haemoglobin };
            values = new double?[6];
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Take value from previous screen
            TimingCount = (Timing)e.Parameter;
            bloodGas = new BloodGas();

            base.OnNavigatedTo(e);
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            List<Event> Events = new List<Event>();
            List<StatusEvent> StatusEvents = new List<StatusEvent>();
            string Time = TimingCount.ToString();

            bloodGas.Time = TimingCount;

            if (values[0] != null)
            {
                bloodGas.PH = (float)values[0];
                StatusEvents.Add(new StatusEvent("pH", values[0] + "", Time));
            }
            
           /* if (values[1] != null)
            {
                bloodGas.PCO2 = (float)values[1];
                StatusEvents.Add(new StatusEvent("pCO2", values[1] + "", Time));
            }

            if (values[2] != null)
            {
                bloodGas.Excess = (float)values[2];
                StatusEvents.Add(new StatusEvent("Excess", values[2] + "", Time));
            }*/

            if (values[3] != null)
            {
                bloodGas.Lactate = (float)values[3];
                StatusEvents.Add(new StatusEvent("Lactate", values[3] + "", Time));
            }

            if (values[4] != null)
            {
                bloodGas.Glucose = (float)values[4];
                StatusEvents.Add(new StatusEvent("Glucose", values[4] + " mmol/l", Time));
            }

           /* if (values[5] != null)
            {
                bloodGas.Haemoglobin = (float)values[5];
                StatusEvents.Add(new StatusEvent("Haemogoblin", values[5] + " g/l", Time));
            }*/

            foreach (TextBox textBox in textBoxes)
            {
                SolidColorBrush colour = textBox.Background as SolidColorBrush;

                if (colour.Color == Colors.LightPink && !String.IsNullOrWhiteSpace(textBox.Text))
                {
                    return;
                }
            }

            Frame.Navigate(typeof(Resuscitation), new EventAndTiming(TimingCount, Events, StatusEvents));
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

            textBox.Text = new String(textBox.Text.Where(c => char.IsDigit(c) || c == '.').ToArray());

            double temp;
            bool valid = false;
            double.TryParse(textBox.Text, out temp);

            switch (index)
            {
                case 0:
                    // pH - between 6 and 7, has two decimal places
                    valid = (temp >= 6 && temp < 8) && LessThanXDecimalPlaces(2, temp);
                    break;
                case 1:
                    //pCO2 - one or two digits, has one decimal place
                    valid = (temp < 100) && LessThanXDecimalPlaces(1, temp);
                    break;
                case 2:
                    //excess - signed one or two digit integer part with one decimal place​
                    valid = (temp < 100 && temp > -100) && LessThanXDecimalPlaces(1, temp);
                    break;
                case 3:
                    //lactate - one or two digit integer part, one decimal place​
                    valid = (temp < 100) && LessThanXDecimalPlaces(1, temp);
                    break;
                case 4:
                    //glucose - one or two digit integer part and one digit decimal​
                    valid = (temp < 100) && LessThanXDecimalPlaces(1, temp);
                    break;
                case 5:
                    //haemoglobin - two or three digit integer ​
                    valid = (temp > 9) && (temp < 1000);
                    break;
                default:
                    break;
            }

            if (valid)
            {
                textBoxes[index].Background = new SolidColorBrush(Colors.White);
                textBoxes[index].BorderBrush = new SolidColorBrush(Colors.Black);
                values[index] = temp;
            }
            else
            {
                textBoxes[index].Background = new SolidColorBrush(Colors.LightPink);
                textBoxes[index].BorderBrush = new SolidColorBrush(Colors.PaleVioletRed);
                values[index] = null;
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
