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

        // New StatusEvent Generation
        StatusEvent HeartrateButtonEvent;
        StatusEvent MovementEvent;
        StatusEvent BreathingEvent;
        StatusEvent OxySaturationEvent;
        StatusEvent HeartrateBpmEvent;
        StatusEvent OxyPercentEvent;

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

            HeartrateButtonEvent = null;
            MovementEvent = null;
            BreathingEvent = null;
            OxySaturationEvent = null;
            HeartrateBpmEvent = null;
            OxyPercentEvent = null;

            base.OnNavigatedTo(e);
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            // Set data structure
            List<Event> Events = new List<Event>();
            List<StatusEvent> StatusEvents = new List<StatusEvent>();

            reassessment.Time = TimingCount;
            observation.Time = TimingCount;

            reassessment.Hr = (HeartRate)hr;
            reassessment.Movement = (ChestMovement)movement;
            reassessment.Effort = (RespiratoryEffort)response;

            Events.Add(observation);
            Events.Add(reassessment);

            AddIfNotNull(HeartrateButtonEvent, StatusEvents);
            AddIfNotNull(BreathingEvent, StatusEvents);
            AddIfNotNull(MovementEvent, StatusEvents);

            AddIfNotNull(OxySaturationEvent, StatusEvents);
            AddIfNotNull(HeartrateBpmEvent, StatusEvents);
            AddIfNotNull(OxyPercentEvent, StatusEvents);

            if (StatusEvents.Count > 0)
            {
                Resuscitation.reassessmentTimer = Stopwatch.StartNew();
                if (Resuscitation.cprTimer.IsRunning)
                {
                    Resuscitation.cprTimer.Restart();
                }
                Frame.Navigate(typeof(Resuscitation), new EventAndTiming(TimingCount, Events, StatusEvents));
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Resuscitation), TimingCount);
        }

        private void ClickReassessmentButton(Button button, Button[] buttons, string EventName, out int index, out StatusEvent statusEvent)
        {
            SolidColorBrush colour = button.Background as SolidColorBrush;

            if (colour.Color == Colors.LightGreen)
            {
                button.Background = new SolidColorBrush(Colors.White);
                index = -1;
                statusEvent = null;
                return;
            }

            UpdateColours(buttons, button);
            index = button.Name[button.Name.Length - 1] - '0';

            statusEvent = GenerateStatusEvent(EventName, buttons, TimingCount.Time, reassessment);
        }

        private void movement_Click(object sender, RoutedEventArgs e)
        {
            Button selected = (sender as Button);
            ClickReassessmentButton(selected, movements, "Chest Movement", out movement, out MovementEvent);
        }

        private void hr_Click(object sender, RoutedEventArgs e)
        {
            Button selected = (sender as Button);
            ClickReassessmentButton(selected, hrs, "Heartrate Range", out hr, out HeartrateButtonEvent);
        }

        private void resp_Click(object sender, RoutedEventArgs e)
        {
            Button selected = (sender as Button);
            ClickReassessmentButton(selected, responses, "Breathing", out response, out BreathingEvent);
        }

        private void ObservationTextValidity(bool valid, TextBox textBox, string dataSuffix, string Name, out StatusEvent statusEvent)
        {
            if (!valid)
            {
                textBox.Background = new SolidColorBrush(Colors.LightPink);
                textBox.BorderBrush = new SolidColorBrush(Colors.PaleVioletRed);
                statusEvent = null;
            }
            else
            {
                textBox.Background = new SolidColorBrush(Colors.White);
                textBox.BorderBrush = new SolidColorBrush(Colors.Black);
                statusEvent = new StatusEvent(Name, textBox.Text + dataSuffix, TimingCount.Time, observation);
            }
        }

        private void OxygenLevels_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            textBox.Text = new String(textBox.Text.Where(c => char.IsDigit(c)).ToArray());
            int oxygenLevel;
            Int32.TryParse(textBox.Text, out oxygenLevel);

            ObservationTextValidity(oxygenLevel <= 100, textBox, "%", "Oxygen Saturation", out OxySaturationEvent);
        }

        private void PercentOxygen_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            textBox.Text = new String(textBox.Text.Where(c => char.IsDigit(c)).ToArray());
            int oxygenPercent;
            Int32.TryParse(textBox.Text, out oxygenPercent);

            ObservationTextValidity(oxygenPercent <= 100, textBox, "%", "Oxygen Percentage", out OxyPercentEvent);
        }

        private void HeartRate_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            textBox.Text = new String(textBox.Text.Where(c => char.IsDigit(c)).ToArray());
            int heartrateBpm;
            Int32.TryParse(textBox.Text, out heartrateBpm);

            ObservationTextValidity(heartrateBpm <= 300, textBox, " bpm", "Heartrate", out HeartrateBpmEvent);
        }

        private void TimeView_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Nothing
        }

        private StatusEvent GenerateStatusEvent(string name, Button[] buttons, string time, Event Event)
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
            return new StatusEvent(name, Text.Text.Replace("\n", " "), time, Event);
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

        private bool AddIfNotNull(StatusEvent Event, List<StatusEvent> StatusEvents)
        {
            if (Event != null)
            {
                StatusEvents.Add(Event);
                return true;
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

        private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {
            // Nothing
        }

        private void TextBlock_SelectionChanged_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
