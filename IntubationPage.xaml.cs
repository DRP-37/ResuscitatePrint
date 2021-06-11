using Resuscitate.DataClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
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
    public sealed partial class IntubationPage : Page
    {
        public Timing TimingCount { get; set; }
        private Button[] confirmations;
        private int confirmation;
        // Confirmation:
        // 0: ET-CO2
        // 1: Equal Air Entry
        // 2: Unequal Air Entry
        private bool isIntubation; 
        // True: Intubation
        // Flase: Suction
        private bool isSuccessful;
        private bool allowConfirmation;

        private IntubationAndSuction intubationAndSuction;
        private StatusEvent statusEvent;

        public IntubationPage()
        {
            this.InitializeComponent();
            confirmations = new Button[] { ETCO2, EqualAir, UnequalAir };
            allowConfirmation = false;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Take value from previous screen
            TimingCount = (Timing)e.Parameter;

            intubationAndSuction = new IntubationAndSuction();
            statusEvent = new StatusEvent();

            base.OnNavigatedTo(e);
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (allowConfirmation ||
                (SelectionMade(confirmations) && Confirmation.Visibility == Visibility.Visible)) {
                // set data structure 

                intubationAndSuction.Time = TimingCount;
                intubationAndSuction.Intubation = isIntubation;
                intubationAndSuction.Suction = !isIntubation;
                intubationAndSuction.Confirmation = (IntubationConfirmation)confirmation;
                intubationAndSuction.IntubationSuccess = isSuccessful;

                statusEvent.Name = isIntubation ? "Intubation" : "Suction";
                statusEvent.Data = isIntubation ? intubationString() : "Suction under direct vision";
                statusEvent.Time = intubationAndSuction.Time.ToString();
                statusEvent.Event = intubationAndSuction;

                List<Event> Events = new List<Event>();
                Events.Add(intubationAndSuction);

                List<StatusEvent> StatusEvents = new List<StatusEvent>();
                StatusEvents.Add(statusEvent);

                Frame.Navigate(typeof(Resuscitation), new EventAndTiming(TimingCount, Events, StatusEvents));
            }
        }

        private string intubationString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(isSuccessful ? "Successful" : "Unsuccessful");
            sb.Append(" intubation");
            if (isSuccessful)
            {
                sb.Append($", confirmation: {intubationAndSuction.ConfirmationToString()}");
            }
            return sb.ToString();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Resuscitation), TimingCount);
        }

        private void TimeView_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Unsuccessful_Click(object sender, RoutedEventArgs e)
        {
            isSuccessful = false;
            allowConfirmation = true;
            Confirmation.Visibility = Visibility.Collapsed;
            UpdateColours(new Button[] { Unsuccessful, Success }, sender as Button);
        }

        private void Success_Click(object sender, RoutedEventArgs e)
        {
            isSuccessful = true;
            allowConfirmation = false;
            Confirmation.Visibility = Visibility.Visible;
            UpdateColours(new Button[] { Unsuccessful, Success }, sender as Button);
        }

        private void Suction_Click(object sender, RoutedEventArgs e)
        {
            isIntubation = false;
            allowConfirmation = true;
            Success.Visibility = Visibility.Collapsed;
            Unsuccessful.Visibility = Visibility.Collapsed;
            UpdateColours(new Button[] { Intubation, Suction }, sender as Button);
        }

        private void Intubation_Click(object sender, RoutedEventArgs e)
        {
            allowConfirmation = false;
            isIntubation = true;
            Success.Visibility = Visibility.Visible;
            Unsuccessful.Visibility = Visibility.Visible;
            UpdateColours(new Button[] { Intubation, Suction }, sender as Button);
        }

        private void Confirmation_Click(object sender, RoutedEventArgs e)
        {
            UpdateColours(confirmations, sender as Button);
            confirmation = Array.IndexOf(confirmations, sender as Button);
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

        }
    }
}
