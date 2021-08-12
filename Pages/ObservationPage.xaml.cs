using Resuscitate.DataClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Resuscitate.Pages
{
    public sealed partial class ObservationPage : Page
    {
        private const int MAX_PERCENTAGE = 100;
        private const int MAX_BPM = 300;

        // Button colours
        private static readonly Color CPR_SELECTED_COLOUR = InputUtils.DEFAULT_CPR_SELECTED_COLOUR;
        private static readonly Color CPR_UNSELECTED_COLOUR = Colors.White;

        private readonly Button[] Responses;
        private readonly Button[] HeartRates;
        private readonly Button[] Movements;
        private readonly Button[] Airways;
        private readonly Button[] Breathings;
        private readonly Button[] Compressions;

        private ResuscitationData ResusData;
        private Timing TimingCount;

        // Observation events
        private StatusEvent HeartrateButtonEvent;
        private StatusEvent MovementEvent;
        private StatusEvent RespirationEvent;
        private StatusEvent OxySaturationEvent;
        private StatusEvent HeartrateBpmEvent;
        private StatusEvent OxyPercentEvent;

        // Ventilation shortcut events
        private StatusEvent AirwayEvent;
        private StatusEvent BreathingEvent;
        private StatusEvent CirculationEvent;
        private List<StatusEvent> CPREvents = new List<StatusEvent>();

        public ObservationPage()
        {
            this.InitializeComponent();

            HeartRates = new Button[] { HeartRate0Button, HeartRate1Button, HeartRate2Button, HeartRate3Button };
            Movements = new Button[] { AbsentButton, PresentButton };
            Responses = new Button[] { Response0Button, Response1Button, Response2Button, Response3Button };
            Airways = new Button[] { MaskButton, ETTButton };
            Breathings = new Button[] { VentilationButton, InflationButton };
            Compressions = new Button[] { CPRButton };
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Take value from previous screen
            ResusData = (ResuscitationData)e.Parameter;
            TimingCount = ResusData.TimingCount;

            ResetEvents();
            SetCPRStopButton(ResusData.CPRIsRunning());

            base.OnNavigatedTo(e);
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            bool eventAdded = UpdateStatusList();

            if (eventAdded)
            {
                ResusData.StartNewReassessmentTimer();
                Frame.Navigate(typeof(Resuscitation), ResusData);
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            bool eventAdded = UpdateStatusList();

            if (eventAdded)
            {
                FlyoutBase.ShowAttachedFlyout((FrameworkElement) sender);
                ResusData.StartNewReassessmentTimer();
            }

            ResusData.SaveLocally();

            ResetEvents();
            ResetAllButtonsAndFields();
        }

        private void MovementButton_Click(object sender, RoutedEventArgs e)
        {
            MovementEvent = ClickReassessmentButton((Button) sender, Movements, "Chest Movement");
        }

        private void HeartRateButton_Click(object sender, RoutedEventArgs e)
        {
            HeartrateButtonEvent = ClickReassessmentButton((Button )sender, HeartRates, "Heart Rate Range");
        }

        private void ResponseButton_Click(object sender, RoutedEventArgs e)
        {
            RespirationEvent = ClickReassessmentButton((Button) sender, Responses, "Breathing");
        }

        private void AirwayManagement_Click(object sender, RoutedEventArgs e)
        {
            AirwayEvent = ClickReassessmentButton((Button) sender, Airways, "Airway Management");
        }

        private void Breathing_Click(object sender, RoutedEventArgs e)
        {
            BreathingEvent = ClickReassessmentButton((Button )sender, Breathings, "Breathing Management");
        }

        private void OngoingCirculation_Click(object sender, RoutedEventArgs e)
        {
            Button selected = InputUtils.ClickWithDefaults((Button) sender, new Button[] { (Button)sender });

            if (selected == null)
            {
                CirculationEvent = null;
                return;
            }

            CirculationEvent = new StatusEvent("Cardiac Compressions", (TextBlock)selected.Content, TimingCount.Time);
        }

        private void StopCirculation_Click(object sender, RoutedEventArgs e)
        {
            bool hasStarted = ResusData.CPRIsRunning();

            if (hasStarted)
            {
                // Stop timer
                string Milieconds = ResusData.CPRElapsedMiliseconds().ToString();
                string Seconds = "0";

                if (Milieconds.Length > 3)
                {
                    Seconds = Milieconds.Substring(0, Milieconds.Length - 3);
                }

                CPREvents.Add(new StatusEvent("Cardiac Compressions", "Ended after " + Seconds + " seconds", TimingCount.Time));

                ResusData.StopCPRTimer();

            } else
            {
                // Start new timer
                ResusData.StartNewCPRTimer();
                CPREvents.Add(new StatusEvent("Cardiac Compressions", "Started", TimingCount.Time));
            }

            SetCPRStopButton(!hasStarted);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            ResusData.StatusList.AddAll(CPREvents);

            Frame.Navigate(typeof(Resuscitation), ResusData);
        }

        private void OxygenLevels_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox) sender;

            textBox.Text = new string(textBox.Text.Where(c => char.IsDigit(c)).ToArray());
            Int32.TryParse(textBox.Text, out int oxygenLevel);

            bool valid = !string.IsNullOrWhiteSpace(textBox.Text) && oxygenLevel <= MAX_PERCENTAGE;

            InputUtils.UpdateValidColours(textBox, valid);

            OxySaturationEvent = valid ? new StatusEvent("Oxygen Saturation", textBox.Text + "%", TimingCount.Time) : null;
        }

        private void OxygenPercent_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox) sender;

            textBox.Text = new String(textBox.Text.Where(c => char.IsDigit(c)).ToArray());
            Int32.TryParse(textBox.Text, out int oxygenPercent);

            bool valid = !string.IsNullOrWhiteSpace(textBox.Text) && oxygenPercent <= MAX_PERCENTAGE;

            InputUtils.UpdateValidColours(textBox, valid);

            OxyPercentEvent = valid ? new StatusEvent("Oxygen Percentage", textBox.Text + "%", TimingCount.Time) : null;
        }

        private void HeartRate_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox) sender;

            textBox.Text = new String(textBox.Text.Where(c => char.IsDigit(c)).ToArray());
            int heartrateBpm;
            Int32.TryParse(textBox.Text, out heartrateBpm);

            bool valid = !string.IsNullOrWhiteSpace(textBox.Text) && heartrateBpm <= MAX_BPM;

            InputUtils.UpdateValidColours(textBox, valid);

            HeartrateBpmEvent = valid ? new StatusEvent("Heart Rate", textBox.Text + " bpm", TimingCount.Time) : null;
        }

        private void SetCPRStopButton(bool hasStarted)
        {
            if (hasStarted)
            {
                // Change to stop button
                StopCPRButton.Background = new SolidColorBrush(CPR_SELECTED_COLOUR);
                ((TextBlock)StopCPRButton.Content).Text = "Stop";
            } else
            {
                // Change to start button
                StopCPRButton.Background = new SolidColorBrush(CPR_UNSELECTED_COLOUR);
                ((TextBlock)StopCPRButton.Content).Text = "Start";
            }
        }

        // Returns true if one or more StatusEvents were added
        private bool UpdateStatusList()
        {
            List<StatusEvent> statusEvents = new List<StatusEvent>();

            StatusEvent.MaybeAdd(HeartrateButtonEvent, statusEvents);
            StatusEvent.MaybeAdd(RespirationEvent, statusEvents);
            StatusEvent.MaybeAdd(MovementEvent, statusEvents);
            StatusEvent.MaybeAdd(OxySaturationEvent, statusEvents);
            StatusEvent.MaybeAdd(HeartrateBpmEvent, statusEvents);
            StatusEvent.MaybeAdd(OxyPercentEvent, statusEvents);

            StatusEvent.MaybeAdd(AirwayEvent, statusEvents);
            StatusEvent.MaybeAdd(BreathingEvent, statusEvents);
            StatusEvent.MaybeAdd(CirculationEvent, statusEvents);

            foreach (StatusEvent statusEvent in CPREvents)
            {
                statusEvents.Add(statusEvent);
            }

            ResusData.StatusList.AddAll(statusEvents);

            return statusEvents.Count > 0;
        }

        private void ResetEvents()
        {
            HeartrateButtonEvent = null;
            MovementEvent = null;
            RespirationEvent = null;
            OxySaturationEvent = null;
            HeartrateBpmEvent = null;
            OxyPercentEvent = null;

            AirwayEvent = null;
            BreathingEvent = null;
            CirculationEvent = null;

            CPREvents = new List<StatusEvent>();
        }

        private void ResetAllButtonsAndFields()
        {
            InputUtils.ResetButtons(Responses);
            InputUtils.ResetButtons(HeartRates);
            InputUtils.ResetButtons(Movements);
            InputUtils.ResetButtons(Airways);
            InputUtils.ResetButtons(Breathings);
            InputUtils.ResetButtons(Compressions);

            OxygenLevels.Text = "";
            HeartRate.Text = "";
            PercentOxygen.Text = "";

            // CPR button unaffected
        }

        private StatusEvent ClickReassessmentButton(Button sender, Button[] buttons, string EventName)
        {
            Button selected = InputUtils.ClickWithDefaults(sender, buttons);

            if (selected == null) {

                return null;
            }

            return StatusEvent.FromTextBlockButtons(EventName, buttons, TimingCount.Time);
        }

        private void TimeView_TextChanged(object sender, TextChangedEventArgs e)
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