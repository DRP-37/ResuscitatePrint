using Resuscitate.DataClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Resuscitate.Pages
{
    public sealed partial class AssessmentsPage : Page
    {
        private const int MAX_ALLOWED_EST_WEIGHT = 9;

        private ResuscitationData ResusData;
        private Timing TimingCount;

        private Button[] Colours;
        private Button[] HeartRates;
        private Button[] Respirations;
        private Button[] Tones;
        private Button[] Temperatures;
        private Button[] CordClampings;

        // StatusEvents
        private StatusEvent CordClampingEvent;
        private StatusEvent[] TemperatureEvents;
        private StatusEvent ColourEvent;
        private StatusEvent ToneEvent;
        private StatusEvent BreathingEvent;
        private StatusEvent HeartrateEvent;

        public AssessmentsPage()
        {
            this.InitializeComponent();

            Colours = new Button[] { colour0, colour1, colour2 };
            HeartRates = new Button[] { hr0, hr1, hr2 };
            Respirations = new Button[] { resp0, resp1, resp2 };
            Tones = new Button[] { tone0, tone1, tone2 };
            Temperatures = new Button[] { temp0, temp1, temp2 };
            CordClampings = new Button[] { cord0, cord1, cord2 };
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Take value from previous screen
            ResusData = (ResuscitationData)e.Parameter;
            TimingCount = ResusData.TimingCount;

            // Set all selections to null (new StatusEvent generation)
            CordClampingEvent = null;
            TemperatureEvents = new StatusEvent[3];
            ColourEvent = null;
            ToneEvent = null;
            BreathingEvent = null;
            HeartrateEvent = null;

            base.OnNavigatedTo(e);
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            List<StatusEvent> statusEvents = new List<StatusEvent>();

            // Add all generated StatusEvents
            StatusEvent.MaybeAdd(CordClampingEvent, statusEvents);

            foreach (StatusEvent Event in TemperatureEvents)
            {
                StatusEvent.MaybeAdd(Event, statusEvents);
            }

            StatusEvent.MaybeAdd(ColourEvent, statusEvents);
            StatusEvent.MaybeAdd(HeartrateEvent, statusEvents);
            StatusEvent.MaybeAdd(ToneEvent, statusEvents);
            StatusEvent.MaybeAdd(BreathingEvent, statusEvents);

            bool validWeight = IsValidEstWeight();

            if (validWeight)
            {
                ResusData.PatientData.Weight = EstimatedWeight.Text;
            }

            if (statusEvents.Count <= 0 && !validWeight)
            {
                return;
            }

            ResusData.StatusList.AddAll(statusEvents);

            Frame.Navigate(typeof(Resuscitation), ResusData);
        }

        private void colour_Click(object sender, RoutedEventArgs e)
        {
            ColourEvent = ClickAndGenerateEvent((Button) sender, Colours, "Colour");
        }

        private void hr_Click(object sender, RoutedEventArgs e)
        {
            HeartrateEvent = ClickAndGenerateEvent((Button) sender, HeartRates, "Heart Rate");
        }

        private void tone_Click(object sender, RoutedEventArgs e)
        {
            ToneEvent = ClickAndGenerateEvent((Button) sender, Tones, "Muscle Tone");
        }

        private void resp_Click(object sender, RoutedEventArgs e)
        {
            BreathingEvent = ClickAndGenerateEvent((Button) sender, Respirations, "Breathing");
        }

        private void cord_Click(object sender, RoutedEventArgs e)
        {
            CordClampingEvent = ClickAndGenerateEvent((Button) sender, CordClampings, "Cord Clamping");
        }

        private void temp_Click(object sender, RoutedEventArgs e)
        {
            Button selected = InputUtils.ClickAnyWithDefaults((Button) sender, Temperatures);

            if (selected == null)
            {
                int index = Array.IndexOf(Temperatures, (Button) sender);

                TemperatureEvents[index] = null;
            } else 
            {
                int index = Array.IndexOf(Temperatures, selected);

                TemperatureEvents[index] = new StatusEvent("Temperature Regulation", selected.Content.ToString(), TimingCount.Time);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Resuscitation), TimingCount);
        }

        private void EstimatedWeight_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox estWeightView = (TextBox) sender;

            estWeightView.Text = new String(estWeightView.Text.Where(c => char.IsDigit(c) || c == '.').ToArray());

            InputUtils.UpdateValidColours(estWeightView, IsValidEstWeight());
        }

        private bool IsValidEstWeight()
        {
            double estimatedWeight;
            double.TryParse(EstimatedWeight.Text, out estimatedWeight);

            return !string.IsNullOrWhiteSpace(EstimatedWeight.Text)
                && estimatedWeight <= MAX_ALLOWED_EST_WEIGHT;
        }

        private StatusEvent ClickAndGenerateEvent(Button button, Button[] buttons, string EventName)
        {
            Button selected = InputUtils.ClickWithDefaults(button, buttons);

            if (selected == null)
            {
                return null;
            }

            return new StatusEvent(EventName, selected.Content.ToString(), TimingCount.Time);
        }

        private void TimeView_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Nothing
        }
    }
}
