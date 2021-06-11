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
    public sealed partial class ObservationPage : Page
    {
        public Timing TimingCount { get; set; }
        private Button[] responses;
        private Button[] hrs;
        private int response;
        private int hr;
        private bool movement;
        // int? is implicitly set to null if undeclared
        // using it instead of int to know whether any input has been added
        // Representing as a number instead of String to catch invalid input
        private int? oxygenLvl;
        private int? heartRate;
        private int? oxygenPercent;

        private Reassessment reassessment;
        private Observation observation;
        private StatusEvent statusEvent;

        public ObservationPage()
        {
            this.InitializeComponent();
            responses = new Button[] { resp0, resp1, resp2, resp3 };
            hrs = new Button[] { hr0, hr1, hr2, hr3 };
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Take value from previous screen
            TimingCount = (Timing)e.Parameter;

            reassessment = new Reassessment();
            observation = new Observation();
            statusEvent = new StatusEvent();

            base.OnNavigatedTo(e);
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            // Set data structure
            //TODO: Not really sure if all data needs to be added for this to be valid
            // but a check will need to added

            reassessment.Time = TimingCount;
            observation.Time = TimingCount;

            reassessment.Hr = (HeartRate)hr;
            reassessment.Movement = (ChestMovement)(movement ? 1 : 0);
            reassessment.Effort = (RespiratoryEffort)response;

            if (heartRate != null) {
                observation.Hr = (float)heartRate;
            }
            if (oxygenLvl != null)
            {
                observation.OximeterOxygen = (float)oxygenLvl;
            }
            if (oxygenPercent != null)
            {
                observation.OxygenGiven = (float)oxygenPercent;
            }

            statusEvent.Name = "Reassessment and Observation";
            statusEvent.Data = "";
            statusEvent.Time = reassessment.Time.ToString();
            statusEvent.Event = reassessment;

            Frame.Navigate(typeof(Resuscitation), TimingCount);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame.CanGoBack)
            {
                rootFrame.GoBack();
            }
        }

        private void TimeView_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void movement_Click(object sender, RoutedEventArgs e)
        {
            movement = (sender as Button).Equals(absent);
            UpdateColours(new Button[] { absent, present}, sender as Button);
        }

        private void hr_Click(object sender, RoutedEventArgs e)
        {
            Button selected = (sender as Button);
            UpdateColours(hrs, selected);
            this.hr = selected.Name[selected.Name.Length - 1] - '0';
        }

        private void resp_Click(object sender, RoutedEventArgs e)
        {
            Button selected = (sender as Button);
            UpdateColours(responses, selected);
            this.response = selected.Name[selected.Name.Length - 1] - '0';
        }

        private bool SelectionMade(Button[] buttons)
        {
            foreach (Button button in buttons)
            {
                SolidColorBrush colour = button.Background as SolidColorBrush;

                if (colour.Color == Colors.LightGreen)
                {
                    return true;
                }
            }
            return false;
        }

        private void UpdateColours(Button[] buttons, Button sender)
        {
            foreach (Button button in buttons)
            {
                button.Background = new SolidColorBrush(Colors.White);
            }
            sender.Background = new SolidColorBrush(Colors.LightGreen);
        }

        private void ParseInput(TextBox textBox, int? input)
        {
            
            textBox.Text = new String(textBox.Text.Where(c => char.IsDigit(c) || c == '.').ToArray());
            int temp;
            if (!int.TryParse(textBox.Text, out temp))
            {
                // if parsing attempt wasn't successful
                // output message to enter only numbers
            }
            else
            {
                input = temp;
            }
        }

        private void OxygenLevels_TextChanged(object sender, TextChangedEventArgs e)
        {
            ParseInput(OxygenLevels, oxygenLvl);
        }

        private void PercentOxygen_TextChanged(object sender, TextChangedEventArgs e)
        {
            ParseInput(PercentOxygen, oxygenPercent);
        }

        private void HeartRate_TextChanged(object sender, TextChangedEventArgs e)
        {          
            ParseInput(HeartRate, heartRate);
        }
    }
}
