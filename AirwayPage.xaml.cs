using Resuscitate.DataClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Text.RegularExpressions;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Resuscitate
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AirwayPage : Page
    {
        public Timing TimingCount { get; set; }
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

        public AirwayPage()
        {
            this.InitializeComponent();
            positions = new Button[] { NeutralPosition, RecheckPosition, TwoPerson };
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Take value from previous screen
            TimingCount = (Timing)e.Parameter;

            positioning = new AirwayPositioning();

            base.OnNavigatedTo(e);
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            Button selection = SelectionMade(positions);

            // set data structure with airway procedure and time stamp of selection
            // if a selection has not been made do not allow confirm
            if (selection != null)
            {
                string Time = TimingCount.ToString();

                positioning.Positioning = (Positioning)airwayProcedure;
                positioning.Time = TimingCount.Time;

                // var dialog = new MessageDialog(positioning.ToString());
                // await dialog.ShowAsync();

                List<Event> Events = new List<Event>();
                Events.Add(positioning);

                List<StatusEvent> StatusEvents = new List<StatusEvent>();
                TextBlock Text = selection.Content as TextBlock;
                StatusEvents.Add(new StatusEvent("Airway Positioning", Text.Text.Replace("\n", " "), Time, positioning));

                Frame.Navigate(typeof(Resuscitation), new EventAndTiming(TimingCount, Events, StatusEvents));
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Resuscitation), TimingCount);
        }

        private void TimeView_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Nothing
        }

        private void NeutralPosition_Click(object sender, RoutedEventArgs e)
        {
            int index = Array.IndexOf(positions, sender as Button);
            if (airwayProcedure == index)
            {
                positions[index].Background = new SolidColorBrush(Colors.White);
                airwayProcedure = -1;
            } else
            {
                UpdateColours(positions, sender as Button);
                airwayProcedure = index;
            }
        }

        // Move to its own class later on - needed it many classes
        private Button SelectionMade(Button[] buttons)
        {
            foreach(Button button in buttons)
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
            buttons[0].Background = new SolidColorBrush(Colors.White);
            buttons[1].Background = new SolidColorBrush(Colors.White);
            buttons[2].Background = new SolidColorBrush(Colors.White);
            sender.Background = new SolidColorBrush(Colors.LightGreen);
        }

        private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {
            // Nothing
        }
    }

}
