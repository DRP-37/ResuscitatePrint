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
    public sealed partial class VentilationPage : Page
    {
        public Timing TimingCount { get; set; }
        private Button[] procedures;
        private int? airGiven = null;
        // Ventilation Support:
        // 0: Inflation Breaths: Via Mask
        // 1: Inflation Breaths: Via ETT
        // 2: Ventilation Breaths: Via Mask
        // 3: Ventilation Breaths: Via ETT
        // 4: Mask CPAP
        private int ventilationProcedure = -1;

        private Ventillation ventilation;
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

            ventilation = new Ventillation();
            positioning = new AirwayPositioning();

            base.OnNavigatedTo(e);
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            AddVentilationEvents();

            AddAirwayEvents();

            if (StatusList.Count > 0) { 
                Frame.Navigate(typeof(Resuscitation), new EventAndTiming(TimingCount, EventList, StatusList));
            } 
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            AddVentilationEvents();
            AddAirwayEvents();

            ResetButtons(positions);
            airwayProcedure = -1;

            ResetButtons(procedures);
            ventilationProcedure = -1;
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

        private void Ventilation_Click(object sender, RoutedEventArgs e)
        {
            Button selected = (sender as Button);
            int index = Array.IndexOf(procedures, selected);
            if (this.ventilationProcedure == index)
            {
                procedures[index].Background = new SolidColorBrush(Colors.White);
                ventilationProcedure = -1;
            }
            else
            {
                UpdateColours(procedures, selected);
                ventilationProcedure = index;
            }
        }

        private void TimeView_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Nothing
        }

        private void AirGiven_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            textBox.Text = new String(textBox.Text.Where(c => char.IsDigit(c) || c == '.').ToArray());
            int temp;
            if (!int.TryParse(AirGiven.Text, out temp))
            {
                // if parsing attempt wasn't successful
                // output message to enter only numbers
            } else
            {
                airGiven = temp;
            }

        }

        private Button SelectionMade(Button[] buttons)
        {
            foreach (Button button in buttons)
            {
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

        private void NeutralPosition_Click(object sender, RoutedEventArgs e)
        {
            int index = Array.IndexOf(positions, sender as Button);
            if (airwayProcedure == index)
            {
                positions[index].Background = new SolidColorBrush(Colors.White);
                airwayProcedure = -1;
            }
            else
            {
                UpdateColours(positions, sender as Button);
                airwayProcedure = index;
            }
        }

        private bool AddVentilationEvents()
        {
            Button ventSelection = SelectionMade(procedures);

            if (ventSelection == null)
            {
                return false;
            }

            bool hasAirGiven = airGiven != null;

            if (hasAirGiven && airGiven > 100)
            {
                AirGiven.BorderBrush = new SolidColorBrush(Colors.Red);
                AirGiven.Background = new SolidColorBrush(Colors.LightPink);
                return false;
            }

            // set data structure with ventilation procedure and time stamp of selection
            ventilation.Time = TimingCount;
            ventilation.Oxygen = hasAirGiven ? (float)airGiven : -1;
            ventilation.VentType = (VentillationType)ventilationProcedure;
            EventList.Add(ventilation);

            StatusEvent statusEvent = new StatusEvent();
            statusEvent.Name = ventilation.ventToString();
            statusEvent.Data = hasAirGiven ? $"{airGiven}% Air/Oxygen Given" : "(Air/Oxygen Given Not Indicated)";
            statusEvent.Time = ventilation.Time.ToString();
            statusEvent.Event = ventilation;
            StatusList.Add(statusEvent);

            return true;
        }

        private bool AddAirwayEvents()
        {
            Button airwaySelection = SelectionMade(positions);

            if (airwaySelection == null)
            {
                return false;
            }

            string Time = TimingCount.ToString();

            positioning.Positioning = (Positioning)airwayProcedure;
            positioning.Time = TimingCount;

            // var dialog = new MessageDialog(positioning.ToString());
            // await dialog.ShowAsync();
            EventList.Add(positioning);

            TextBlock Text = airwaySelection.Content as TextBlock;
            StatusList.Add(new StatusEvent("Airway Positioning", Text.Text.Replace("\n", " "), Time, positioning));

            return true;
        }

        private void textBlock_SelectionChanged_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
