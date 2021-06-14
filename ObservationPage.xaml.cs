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
        private Button[] movements;
        private int response = -1;
        private int hr = -1;
        private int movement = -1;
        // int? is implicitly set to null if undeclared
        // using it instead of int to know whether any input has been added
        // Representing as a number instead of String to catch invalid input
        private int? oxygenLvl;
        private int? heartRate;
        private int? oxygenPercent;

        private Reassessment reassessment;
        private Observation observation;

        public ObservationPage()
        {
            this.InitializeComponent();
            responses = new Button[] { resp0, resp1, resp2, resp3 };
            hrs = new Button[] { hr0, hr1, hr2, hr3 };
            movements = new Button[] { absent, present };
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Take value from previous screen
            TimingCount = (Timing)e.Parameter;

            reassessment = new Reassessment();
            observation = new Observation();

            base.OnNavigatedTo(e);
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            // Set data structure
            bool invalidValues = false;
            List<Event> Events = new List<Event>();
            List<StatusEvent> StatusEvents = new List<StatusEvent>();
            string Time = TimingCount.ToString();

            reassessment.Time = TimingCount;
            observation.Time = TimingCount;

            if (heartRate != null) {
                observation.Hr = (float)heartRate;
                StatusEvents.Add(new StatusEvent("Heart Rate", heartRate + " bpm", Time));
            }

            if (oxygenLvl != null)
            {
                if (oxygenLvl > 100)
                {
                    invalidValues = true;
                    OxygenLevels.BorderBrush = new SolidColorBrush(Colors.Red);
                    OxygenLevels.Background = new SolidColorBrush(Colors.PaleVioletRed);
                } else
                {
                    observation.OximeterOxygen = (float)oxygenLvl;
                    StatusEvents.Add(new StatusEvent("Oxygen Level", oxygenLvl + "%", Time));
                }
            }

            if (oxygenPercent != null)
            {
                if (oxygenLvl > 100)
                {
                    invalidValues = true;
                    PercentOxygen.BorderBrush = new SolidColorBrush(Colors.PaleVioletRed);
                    PercentOxygen.Background = new SolidColorBrush(Colors.LightPink);
                } else
                {
                    observation.OxygenGiven = (float)oxygenPercent;
                    StatusEvents.Add(new StatusEvent("Oxygen Percent", oxygenPercent + "%", Time));
                }
            }

            if (invalidValues)
            {
                return;
            }

            reassessment.Hr = (HeartRate)hr;
            reassessment.Movement = (ChestMovement)movement;
            reassessment.Effort = (RespiratoryEffort)response;

            Events.Add(observation);
            Events.Add(reassessment);

            AddIfNotNull(GenerateStatusEvent("Heart Rate Range", hrs, Time), StatusEvents);
            AddIfNotNull(GenerateStatusEvent("Respiratory Effort", responses, Time), StatusEvents);
            AddIfNotNull(GenerateStatusEvent("Chest Movement", movements, Time), StatusEvents);

            Frame.Navigate(typeof(Resuscitation), new EventAndTiming(TimingCount, Events, StatusEvents));
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Resuscitation), TimingCount);
        }

        private void movement_Click(object sender, RoutedEventArgs e)
        {
            Button selected = (sender as Button);
            int index = Array.IndexOf(movements, selected);

            if (this.movement == index)
            {
                movements[index].Background = new SolidColorBrush(Colors.White);
                movement = -1;
            }
            else
            {
                UpdateColours(movements, selected);
                this.movement = index;
            }
        }

        private void hr_Click(object sender, RoutedEventArgs e)
        {
            Button selected = (sender as Button);
            int index = Array.IndexOf(hrs, selected);
            if (this.hr == index)
            {
                hrs[index].Background = new SolidColorBrush(Colors.White);
                hr = -1;
            }
            else
            {
                UpdateColours(hrs, selected);
                this.hr = selected.Name[selected.Name.Length - 1] - '0';
            }
        }

        private void resp_Click(object sender, RoutedEventArgs e)
        {
            Button selected = (sender as Button);
            int index = Array.IndexOf(responses, selected);
            if (this.response == index)
            {
                responses[index].Background = new SolidColorBrush(Colors.White);
                response = -1;
            }
            else
            {
                UpdateColours(responses, selected);
                this.response = selected.Name[selected.Name.Length - 1] - '0';
            }
        }

        private void OxygenLevels_TextChanged(object sender, TextChangedEventArgs e)
        {
            oxygenLvl = ParseInt(OxygenLevels);
        }

        private void PercentOxygen_TextChanged(object sender, TextChangedEventArgs e)
        {
            oxygenPercent = ParseInt(PercentOxygen);
        }

        private void HeartRate_TextChanged(object sender, TextChangedEventArgs e)
        {
            heartRate = ParseInt(HeartRate);
        }

        private void TimeView_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Nothing
        }

        private StatusEvent GenerateStatusEvent(string name, Button[] buttons, string time)
        {
            Button selected = null;

            foreach (Button button in buttons)
            {
                SolidColorBrush colour = button.Background as SolidColorBrush;

                if (colour.Color == Colors.LightGreen)
                {
                    selected = button;
                }
            }

            if (selected == null)
            {
                return null;
            }

            TextBlock Text = selected.Content as TextBlock;
            return new StatusEvent(name, Text.Text.Replace("\n", " "), time);
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

        private void AddIfNotNull(StatusEvent Event, List<StatusEvent> StatusEvents)
        {
            if (Event != null)
            {
                StatusEvents.Add(Event);
            }
        }

        private void UpdateColours(Button[] buttons, Button sender)
        {
            foreach (Button button in buttons)
            {
                button.Background = new SolidColorBrush(Colors.White);
            }
            sender.Background = new SolidColorBrush(Colors.LightGreen);
        }

        // Returns null if unsuccessful
        private int? ParseInt(TextBox textBox)
        {
            textBox.Text = new String(textBox.Text.Where(c => char.IsDigit(c) || c == '.').ToArray());
            int temp;
            if (!int.TryParse(textBox.Text, out temp))
            {
                // if parsing attempt wasn't successful
                // output message to enter only numbers
                return null;
            }
            else
            {
                return temp;
            }
        }

        private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {
            // Nothing
        }
    }
}
