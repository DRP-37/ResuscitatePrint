using Resuscitate.DataClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Resuscitate
{
    public sealed partial class VentilationPage : Page
    {
        private const int MAX_PERCENTAGE = 100;

        private const bool IS_RETURNING = true;
        private const bool IS_NOT_RETURNING = false;

        private Timing TimingCount;

        private Button[] AirwayPositions;
        private Button[] Ventilations;

        List<StatusEvent> StatusList = new List<StatusEvent>();

        StatusEvent AirwayEvent;
        StatusEvent VentilationEvent;

        public VentilationPage()
        {
            this.InitializeComponent();

            AirwayPositions = new Button[] { NeutralPosition, RecheckPosition, TwoPerson };
            Ventilations = new Button[] { InflationMask, InflationETT, VentilationMask, VentilationETT, MaskCPAP };
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Take value from previous screen
            TimingCount = (Timing)e.Parameter;

            AirwayEvent = null;
            VentilationEvent = null;

            base.OnNavigatedTo(e);
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            GenerateUpdateFlyout((FrameworkElement) sender, ConfirmFlyout, IS_RETURNING);

            bool canReturn = UpdateStatusEventsAndColours();

            if (StatusList.Count > 0 && canReturn) { 
                Frame.Navigate(typeof(Resuscitation), new TimingAndEvents(TimingCount, StatusList));
            } 
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            GenerateUpdateFlyout((FrameworkElement) sender, UpdateFlyout, IS_NOT_RETURNING);

            UpdateStatusEventsAndColours();
        }

        /* Returns true if returning to Resuscitate page is possible */
        private bool UpdateStatusEventsAndColours()
        {
            if (AirwayEvent != null)
            {
                InputUtils.ResetButtons(AirwayPositions);

                StatusList.Add(AirwayEvent);
                AirwayEvent = null;
            }

            if (VentilationEvent != null)
            {
                InputUtils.ResetButtons(Ventilations);
                InputUtils.ResetTextBoxColour(AirGiven);

                StatusList.Add(VentilationEvent);
                VentilationEvent = null;
            }

            if (InputUtils.SelectionMade(Ventilations) != null && !IsValidAirGiven())
            {
                InputUtils.SetInvalidTextBoxColour(AirGiven);
                return false;
            }

            return true;
        }

        private void NeutralPosition_Click(object sender, RoutedEventArgs e)
        {
            Button selected = InputUtils.ClickWithDefaults((Button)sender, AirwayPositions);

            if (selected == null)
            {
                AirwayEvent = null;
                return;
            }

            AirwayEvent = new StatusEvent("Airway Positioning", (TextBlock)selected.Content, TimingCount.Time);
        }

        private void Ventilation_Click(object sender, RoutedEventArgs e)
        {
            Button selected = InputUtils.ClickWithDefaults((Button)sender, Ventilations);

            if (selected == null)
            {
                VentilationEvent = null;
                return;
            }

            int? airGiven = ParsePercentage(AirGiven);

            if (airGiven == null)
            {
                VentilationEvent = null;
                return;
            }

            VentilationEvent = new StatusEvent((TextBlock)selected.Content, $"{airGiven}% Air/Oxygen Given", TimingCount.Time);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Resuscitation), new TimingAndEvents(TimingCount, StatusList));
        }

        private void AirGiven_TextChanged(object sender, TextChangedEventArgs e)
        {

            TextBox textBox = (TextBox) sender;

            // Only accept valid characters
            textBox.Text = new String(textBox.Text.Where(c => char.IsDigit(c) || c == '.').ToArray());

            int? airGiven = ParsePercentage(textBox);

            InputUtils.UpdateValidColours(textBox, IsValidOrEmptyAirGiven(airGiven));

            // Check if a ventilation button is pressed and airGiven is valid,
            //   If so, make a VentilationEvent
            Button selected = InputUtils.SelectionMade(Ventilations);

            if (airGiven == null || selected == null)
            {
                VentilationEvent = null;
                return;
            }

            VentilationEvent = new StatusEvent((TextBlock)selected.Content, $"{airGiven}% Air/Oxygen Given", TimingCount.Time);
        }

        private bool IsValidOrEmptyAirGiven(int? airGiven)
        {
            if (AirGiven.Text == "")
            {
                return true;
            }

            return airGiven != null ? airGiven <= MAX_PERCENTAGE : false;
        }

        private bool IsValidAirGiven()
        {
            return ParsePercentage(AirGiven) != null;
        }

        private int? ParsePercentage(TextBox textBox)
        {
            if (textBox.Text == "")
            {
                return null;
            }

            int airGiven;
            bool parsed = Int32.TryParse(textBox.Text, out airGiven);

            if (!parsed || airGiven > MAX_PERCENTAGE)
            {
                return null;
            }

            return airGiven;
        }

        // Returns true if Successful Flyout was generated or none was generated
        // Returns false if Unsuccessful Flyout was generated
        private void GenerateUpdateFlyout(FrameworkElement sender, TextBlock flyout, bool isReturning)
        {
            if (VentilationEvent != null && AirwayEvent != null && !isReturning)
            {
                flyout.Text = "The timeline has been updated.";
                FlyoutBase.ShowAttachedFlyout(sender);
                return;
            }

            bool isVentilationSelected = InputUtils.SelectionMade(Ventilations) != null;
            bool isValidAirGiven = IsValidAirGiven();

            bool showFlyout = false;
            flyout.Text = "";

            if (AirwayEvent != null)
            {
                flyout.Text = "The timeline has been updated with the selected airway procedure.\n";
            }

            if (!isVentilationSelected && isValidAirGiven && !isReturning)
            {
                flyout.Text += "Please select ventilation procedure according to O2% given.";
                showFlyout = true;
            }

            if (isVentilationSelected && !isValidAirGiven)
            {
                flyout.Text += "Please enter a valid percentage for the ventilation procedure.";
                showFlyout = true;
            }

            if (showFlyout)
            {
                FlyoutBase.ShowAttachedFlyout(sender);
            }
        }

        private void TimeView_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Nothing
        }

        private void textBlock_SelectionChanged_1(object sender, RoutedEventArgs e)
        {
            // Nothing
        }
    }
}
