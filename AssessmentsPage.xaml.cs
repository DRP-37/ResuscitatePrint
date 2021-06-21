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
    public sealed partial class AssessmentsPage : Page
    {
        public Timing TimingCount { get; set; }
        private Button[] colours;
        private Button[] hrs;
        private Button[] respirations;
        private Button[] tones;
        private Button[] tempRegulations;
        private Button[] cordClamping;
        private int cord = -1;
        private int temp = -1;
        private int hr = -1;
        private int respiration = -1;
        private int tone = -1;
        private int colour = -1;

        // New version of generating StatusEvents
        private StatusEvent CordClampingEvent;
        private StatusEvent[] TemperatureEvents;
        private StatusEvent EstimatedWeightEvent;
        private StatusEvent ColourEvent;
        private StatusEvent ToneEvent;
        private StatusEvent BreathingEvent;
        private StatusEvent HeartrateEvent;

        InitialAssessment initialAssessment;

        public AssessmentsPage()
        {
            this.InitializeComponent();
            colours = new Button[] { colour0, colour1, colour2 };
            hrs = new Button[] { hr0, hr1, hr2 };
            respirations = new Button[] { resp0, resp1, resp2 };
            tones = new Button[] { tone0, tone1, tone2 };
            tempRegulations = new Button[] { temp0, temp1, temp2 };
            cordClamping = new Button[] { cord0, cord1, cord2 };
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Take value from previous screen
            TimingCount = (Timing)e.Parameter;

            initialAssessment = new InitialAssessment(TimingCount);

            // Set all selections to null (new StatusEvent generation)
            CordClampingEvent = null;
            TemperatureEvents = new StatusEvent[3];
            EstimatedWeightEvent = null;
            ColourEvent = null;
            ToneEvent = null;
            BreathingEvent = null;
            HeartrateEvent = null;

            base.OnNavigatedTo(e);
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            // Set data structure
            initialAssessment.Colour = colour;
            initialAssessment.Tone = tone;
            initialAssessment.RespEffort = respiration;
            initialAssessment.HeartRate = hr;

            initialAssessment.Clamping = (CordClamping)cord;
            initialAssessment.TempReg = (TemperatureReg)temp;

            // Add check whether a proper selection has been made
            List<Event> Events = new List<Event>();
            Events.Add(initialAssessment);

            string Time = initialAssessment.Time.ToString();

            List<StatusEvent> StatusEvents = new List<StatusEvent>();

            // New StatusEvent generation
            AddIfNotNull(CordClampingEvent, StatusEvents);

            foreach (StatusEvent Event in TemperatureEvents)
            {
                AddIfNotNull(Event, StatusEvents);
            }

            AddIfNotNull(EstimatedWeightEvent, StatusEvents);
            AddIfNotNull(ColourEvent, StatusEvents);
            AddIfNotNull(HeartrateEvent, StatusEvents);
            AddIfNotNull(ToneEvent, StatusEvents);
            AddIfNotNull(BreathingEvent, StatusEvents);

            if (StatusEvents.Count < 1)
            {
                return;
            }

            Frame.Navigate(typeof(Resuscitation), new EventAndTiming(TimingCount, Events, StatusEvents));
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Resuscitation), TimingCount);
        }

        private void TimeView_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Nothing
        }

        private void ClickButton(Button button, Button[] buttons, string EventName, out int index, out StatusEvent statusEvent)
        {
            SolidColorBrush colour = button.Background as SolidColorBrush;

            if (colour.Color == Colors.LightGreen)
            {
                button.Background = new SolidColorBrush(Colors.White);
                index = -1;
                statusEvent = null;
                return;
            }

            changeColours(buttons, button);
            index = button.Name[button.Name.Length - 1] - '0';

            statusEvent = GenerateStatusEvent(EventName, buttons, TimingCount.Time, initialAssessment);
        }

        private void colour_Click(object sender, RoutedEventArgs e)
        {
            Button selected = sender as Button;

            ClickButton(selected, colours, "Colour", out colour, out ColourEvent);
            Console.WriteLine(this.colour);
        }

        private void hr_Click(object sender, RoutedEventArgs e)
        {
            Button selected = sender as Button;
            ClickButton(selected, hrs, "Heart Rate", out hr, out HeartrateEvent);
        }

        private void tone_Click(object sender, RoutedEventArgs e)
        {
            Button selected = sender as Button;
            ClickButton(selected, tones, "Muscle Tone", out tone, out ToneEvent);
        }

        private void resp_Click(object sender, RoutedEventArgs e)
        {
            Button selected = sender as Button;
            ClickButton(selected, respirations, "Breathing", out respiration, out BreathingEvent);
        }

        private void cord_Click(object sender, RoutedEventArgs e)
        {
            Button selected = sender as Button;
            ClickButton(selected, cordClamping, "Cord Clamping", out cord, out CordClampingEvent);
        }

        private void temp_Click(object sender, RoutedEventArgs e)
        {
            Button selected = sender as Button;
            int index = Array.IndexOf(tempRegulations, selected);
            SolidColorBrush colour = selected.Background as SolidColorBrush;

            if (colour.Color == Colors.LightGreen)
            {
                selected.Background = new SolidColorBrush(Colors.White);
                TemperatureEvents[index] = null;
                temp = -1;
            } else 
            {
                selected.Background = new SolidColorBrush(Colors.LightGreen);
                TemperatureEvents[index] = new StatusEvent("Temperature Regulation", selected.Content.ToString(), TimingCount.Time, initialAssessment);
                temp = index;
            }
        }

        private void changeColours(Button[] buttons, Button sender)
        {
            buttons[0].Background = new SolidColorBrush(Colors.White);
            buttons[1].Background = new SolidColorBrush(Colors.White);
            buttons[2].Background = new SolidColorBrush(Colors.White);
            sender.Background = new SolidColorBrush(Colors.LightGreen);
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

            return new StatusEvent(name, selected.Content.ToString(), time, Event); ;
        }

        private void AddIfNotNull(StatusEvent Event, List<StatusEvent> StatusEvents)
        {
            if (Event != null)
            {
                StatusEvents.Add(Event);
            }
        }

        private void EstimatedWeight_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            textBox.Text = new String(textBox.Text.Where(c => char.IsDigit(c) || c == '.').ToArray());
            double estimatedWeight;
            double.TryParse(textBox.Text, out estimatedWeight);

            TextValidity(estimatedWeight <= 9, textBox, "kg", "Estimated Weight", out EstimatedWeightEvent);
        }

        private void TextValidity(bool valid, TextBox textBox, string dataSuffix, string Name, out StatusEvent statusEvent)
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
                statusEvent = new StatusEvent(Name, textBox.Text + dataSuffix, TimingCount.Time, initialAssessment);
            }
        }
    }
}
