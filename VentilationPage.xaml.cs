using Resuscitate.DataClasses;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Popups;
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
    public sealed partial class VentilationPage : Page
    {
        public Timing TimingCount { get; set; }
        private Button[] procedures;
        // Ventilation Support:
        // 0: Inflation Breaths: Via Mask
        // 1: Inflation Breaths: Via ETT
        // 2: Ventilation Breaths: Via Mask
        // 3: Ventilation Breaths: Via ETT
        // 4: Mask CPAP
        private int ventilationProcedure = -1;

        private Ventilation ventilation;
        List<Event> EventList = new List<Event>();
        List<StatusEvent> StatusList = new List<StatusEvent>();

        private Button[] positions;

        // Airway Positioning:
        // 0: Neutral Head Position
        // 1: Recheck position
        // 2: Two-person Technique
        private int airwayProcedure = -1;

        // Position:
        // 0: Neutral Head Position
        // 1: Recheck Head Position and Jaw Support
        // 2: Two-person Technique
        private AirwayPositioning positioning;

        // New StatusEvent generation
        StatusEvent AirwayEvent;
        StatusEvent VentilationEvent;
        private bool invalidAirGiven = true;

        public VentilationPage()
        {
            this.InitializeComponent();
            // Airway buttons
            positions = new Button[] { NeutralPosition, RecheckPosition, TwoPerson };
            // Ventilation buttons
            procedures = new Button[] { InflationMask, InflationETT, VentilationMask, VentilationETT, MaskCPAP };
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Take value from previous screen
            TimingCount = (Timing)e.Parameter;

            ventilation = new Ventilation();
            positioning = new AirwayPositioning();

            AirwayEvent = null;
            VentilationEvent = null;

            base.OnNavigatedTo(e);
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectionMade(procedures) != null && invalidAirGiven)
            {
                FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
                AirGiven.Background = new SolidColorBrush(Colors.LightPink);
                AirGiven.BorderBrush = new SolidColorBrush(Colors.Red);
                return;
            }

            AddIfNotNull(AirwayEvent, StatusList, EventList);
            AddIfNotNull(VentilationEvent, StatusList, EventList);

            if (StatusList.Count > 0 && (AirGiven.Background as SolidColorBrush).Color != Colors.LightPink) { 
                Frame.Navigate(typeof(Resuscitation), new EventAndTiming(TimingCount, EventList, StatusList));
            } 
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            AddIfNotNull(AirwayEvent, StatusList, EventList);
            AddIfNotNull(VentilationEvent, StatusList, EventList);

            bool airwayEventAdded = false;

            if (AirwayEvent != null)
            {
                ResetButtons(positions);
                airwayProcedure = -1;

                positioning = new AirwayPositioning();

                Notification.Text = "The timeline has been updated. ";
                FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
                airwayEventAdded = true;
            }

            if (VentilationEvent != null)
            {
                ResetButtons(procedures);
                ventilationProcedure = -1;

                AirGiven.Background = new SolidColorBrush(Colors.White);
                AirGiven.BorderBrush = new SolidColorBrush(Colors.Black);

                ventilation = new Ventilation();

                Notification.Text = "The timeline has been updated. ";
                FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
            } else
            {

                if (SelectionMade(procedures) == null)
                {
                    if (airwayEventAdded)
                    {
                        if (!String.IsNullOrWhiteSpace(AirGiven.Text))
                        {
                            Notification.Text = "The timeline has been updated with the selected airway procedure. \n" +
                                "Please select ventilation procedure according to O2% given. ";
                        }
                    }
                    else
                    {
                        Notification.Text = "Please select ventilation procedure according to O2% given. ";
                    }

                    FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
                }
                else if (SelectionMade(procedures) != null && invalidAirGiven)
                {
                    if (airwayEventAdded)
                    {
                        Notification.Text = "The timeline has been updated with the selected airway procedure. \n" +
                            "Please enter a valid percentage for the ventilation procedure. ";
                    }
                    else
                    {
                        Notification.Text = "Please enter a valid percentage for the ventilation procedure. ";
                    }

                    FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
                    AirGiven.Background = new SolidColorBrush(Colors.LightPink);
                    AirGiven.BorderBrush = new SolidColorBrush(Colors.Red);
                }
            }

            VentilationEvent = null;
            AirwayEvent = null;
        }

        private void ResetButtons(Button[] buttons)
        {
            foreach (Button button in buttons)
            {
                button.Background = new SolidColorBrush(Colors.White);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Resuscitation), new EventAndTiming(TimingCount, EventList, StatusList));
        }

        private void NeutralPosition_Click(object sender, RoutedEventArgs e)
        {
            Button selected = sender as Button;
            SolidColorBrush colour = selected.Background as SolidColorBrush;

            if (colour.Color == Colors.LightGreen)
            {
                selected.Background = new SolidColorBrush(Colors.White);
                airwayProcedure = -1;
                AirwayEvent = null;
                return;
            }

            UpdateColours(positions, selected);
            airwayProcedure = selected.Name[selected.Name.Length - 1] - '0';

            string Data = ((TextBlock)selected.Content).Text.Replace("\n", " ");
            positioning.Positioning = (Positioning)airwayProcedure;
            positioning.Time = TimingCount;
            AirwayEvent = new StatusEvent("Airway Positioning", Data, TimingCount.Time, positioning);
        }

        private void Ventilation_Click(object sender, RoutedEventArgs e)
        {
            Button selected = sender as Button;
            SolidColorBrush colour = selected.Background as SolidColorBrush;

            if (colour.Color == Colors.LightGreen)
            {
                selected.Background = new SolidColorBrush(Colors.White);
                ventilationProcedure = -1;
                VentilationEvent = null;
                return;
            }

            UpdateColours(procedures, selected);
            ventilationProcedure = selected.Name[selected.Name.Length - 1] - '0';
            int? airGiven = CheckPercentage(AirGiven);

            if (airGiven == null)
            {
                VentilationEvent = null;
                return;
            }

            string Name = ((TextBlock)selected.Content).Text.Replace("\n", " ");
            ventilation.VentType = (VentilationType)ventilationProcedure;
            VentilationEvent = new StatusEvent(Name, $"{airGiven}% Air/Oxygen Given", TimingCount.Time, ventilation);
        }

        private void TimeView_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Nothing
        }

        private async void AirGiven_TextChanged(object sender, TextChangedEventArgs e)
        {
       
            TextBox textBox = (TextBox)sender;
            textBox.Text = new String(textBox.Text.Where(c => char.IsDigit(c) || c == '.').ToArray());
            Button selected = SelectionMade(procedures);
            int? airGiven = CheckPercentage(textBox);

            if (airGiven == null)
            {
                AirGiven.Background = new SolidColorBrush(Colors.LightPink);
                AirGiven.BorderBrush = new SolidColorBrush(Colors.Red);
                invalidAirGiven = true;
            }

            if (airGiven == null || selected == null)
            {
                VentilationEvent = null;
                return;
            }

            invalidAirGiven = false;
            string Name = ((TextBlock)selected.Content).Text.Replace("\n", " ");
            ventilation.Oxygen = (float)airGiven;
            ventilation.Time = TimingCount;
            VentilationEvent = new StatusEvent(Name, $"{airGiven}% Air/Oxygen Given", TimingCount.Time, ventilation);
        }

        private int? CheckPercentage(TextBox textBox)
        {
            if (textBox.Text == "")
            {
                textBox.BorderBrush = new SolidColorBrush(Colors.Black);
                textBox.Background = new SolidColorBrush(Colors.White);
                return null;
            }

            int airGiven;
            bool parsed = Int32.TryParse(textBox.Text, out airGiven);

            if (!parsed || airGiven > 100)
            {
                textBox.BorderBrush = new SolidColorBrush(Colors.Red);
                textBox.Background = new SolidColorBrush(Colors.LightPink);
                return null;
            }

            textBox.BorderBrush = new SolidColorBrush(Colors.Black);
            textBox.Background = new SolidColorBrush(Colors.White);
            return airGiven;
        }

        private Button SelectionMade(Button[] buttons)
        {
            for (int i=0; i < buttons.Length; i++) 
            {
                Button button = buttons[i];
                SolidColorBrush colour = button.Background as SolidColorBrush;

                if (colour.Color == Colors.LightGreen)
                {
                    return button;
                }
            }
            return null;
        }

        private void UpdateColours(Button[] buttons, Button sender)
        {
            foreach (Button button in buttons)
            {
                button.Background = new SolidColorBrush(Colors.White);
            }
            sender.Background = new SolidColorBrush(Colors.LightGreen);
        }

        private void AddIfNotNull(StatusEvent Event, List<StatusEvent> StatusEvents, List<Event> Events)
        {
            if (Event != null)
            {
                if (Event.Event != null)
                {
                    Events.Add(Event.Event);
                }
                StatusEvents.Add(Event);
            }
        }

        private void textBlock_SelectionChanged_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
