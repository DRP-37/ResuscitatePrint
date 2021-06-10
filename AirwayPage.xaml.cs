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
        private int airwayProcedure; 

        // Position:
        // 0: Neutral Head Position
        // 1: Recheck Head Position and Jaw Support
        // 2: Two-person Technique
        private int position = 0; // Set to 0 for now, will change when page is implemented
        private AirwayPositioning positioning;
        private StatusEvent statusEvent;

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
            positioning.Time = TimingCount;

            statusEvent = new StatusEvent();

            base.OnNavigatedTo(e);
        }

        private async void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            positioning.Positioning = (Positioning)position;

            statusEvent.Name = "Positioning";
            statusEvent.Data = positioning.positionToString();
            statusEvent.Time = positioning.Time.ToString();
            statusEvent.Event = positioning;

            var dialog = new MessageDialog(positioning.ToString());
            await dialog.ShowAsync();

            // set data structure with airway procedure and time stamp of selection
            // if a selection has not been made do not allow confirm
            if (SelectionMade(positions))
            {
                Frame.Navigate(typeof(Resuscitation), TimingCount);
            }
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

        private void NeutralPosition_Click(object sender, RoutedEventArgs e)
        {
            UpdateColours(positions, sender as Button);
            airwayProcedure = Array.IndexOf(positions, sender as Button);
        }

        // Move to its own class later on - needed it many classes
        private bool SelectionMade(Button[] buttons)
        {
            foreach(Button button in buttons)
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
            buttons[0].Background = new SolidColorBrush(Colors.White);
            buttons[1].Background = new SolidColorBrush(Colors.White);
            buttons[2].Background = new SolidColorBrush(Colors.White);
            sender.Background = new SolidColorBrush(Colors.LightGreen);
        }
    }

}
