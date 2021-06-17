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
        private bool isIntubation; 
        // True: Intubation
        // Flase: Suction
        private bool isSuccessful;
        // Confirmation:
        // 0: ET-CO2
        // 1: Equal Air Entry
        // 2: Unequal Air Entry
        private bool allowConfirmation = false;

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
                //intubationAndSuction.Confirmation = (IntubationConfirmation)confirmation;
                intubationAndSuction.IntubationSuccess = isSuccessful;

                List<Button> ConfirmationButtons = SelectedButtons(confirmations);
                if (ConfirmationButtons.Count == 0)
                {
                    // No confirmation selected
                    return;
                }

                statusEvent.Name = isIntubation ? "Intubation" : "Suction";
                //statusEvent.Data = isIntubation ? intubationString() : "Suction under direct vision";
                statusEvent.Time = intubationAndSuction.Time.ToString();
                statusEvent.Event = intubationAndSuction;

                if (isIntubation && isSuccessful)
                {
                    statusEvent.Data = "Successful: ";

                    foreach (Button button in ConfirmationButtons)
                    {
                        statusEvent.Data += button.Content.ToString() + ", ";
                    }

                    statusEvent.Data = statusEvent.Data.Substring(0, statusEvent.Data.Length - 2);

                } else if (isIntubation)
                {
                    statusEvent.Data = "Unsuccessful";
                } else
                {
                    statusEvent.Data = "Suction Under Direct Vision";
                }

                List<Event> Events = new List<Event>();
                Events.Add(intubationAndSuction);

                List<StatusEvent> StatusEvents = new List<StatusEvent>();
                StatusEvents.Add(statusEvent);

                Frame.Navigate(typeof(Resuscitation), new EventAndTiming(TimingCount, Events, StatusEvents));
            }
        }

        private List<Button> SelectedButtons(Button[] buttons)
        {
            List<Button> Selecteds = new List<Button>();

            foreach (Button button in buttons)
            {
                SolidColorBrush colour = button.Background as SolidColorBrush;

                if (colour.Color == Colors.LightGreen)
                {
                    Selecteds.Add(button);
                }
            }

            return Selecteds;
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
            // Nothing
        }

        private void Suction_Click(object sender, RoutedEventArgs e)
        {
            Button selected = sender as Button;
            SolidColorBrush colour = selected.Background as SolidColorBrush;

            if (colour.Color != Colors.White)
            {
                selected.Background = new SolidColorBrush(Colors.White);
                allowConfirmation = false;
                return;
            }

            isIntubation = false;
            allowConfirmation = true;
            Success.Visibility = Visibility.Collapsed;
            Unsuccessful.Visibility = Visibility.Collapsed;
            UpdateColours(new Button[] { Intubation, Suction }, selected);
        }

        private void Intubation_Click(object sender, RoutedEventArgs e)
        {
            Button selected = sender as Button;
            SolidColorBrush colour = selected.Background as SolidColorBrush;

            if (colour.Color != Colors.White)
            {
                selected.Background = new SolidColorBrush(Colors.White);
                Success.Visibility = Visibility.Collapsed;
                Unsuccessful.Visibility = Visibility.Collapsed;
                return;
            }

            isIntubation = true;
            Success.Visibility = Visibility.Visible;
            Unsuccessful.Visibility = Visibility.Visible;
            UpdateColours(new Button[] { Intubation, Suction }, selected);
        }

        private void Unsuccessful_Click(object sender, RoutedEventArgs e)
        {
            Button selected = sender as Button;
            SolidColorBrush colour = selected.Background as SolidColorBrush;

            if (colour.Color != Colors.White)
            {
                allowConfirmation = false;
                selected.Background = new SolidColorBrush(Colors.White);
                return;
            }

            isSuccessful = false;
            allowConfirmation = true;
            Confirmation.Visibility = Visibility.Collapsed;
            UpdateColours(new Button[] { Unsuccessful, Success }, selected);
        }

        private void Success_Click(object sender, RoutedEventArgs e)
        {
            Button selected = sender as Button;
            SolidColorBrush colour = selected.Background as SolidColorBrush;

            if (colour.Color != Colors.White)
            {
                allowConfirmation = false;
                Confirmation.Visibility = Visibility.Collapsed;
                selected.Background = new SolidColorBrush(Colors.White);
                return;
            }

            isSuccessful = true;
            allowConfirmation = false;
            Confirmation.Visibility = Visibility.Visible;
            UpdateColours(new Button[] { Unsuccessful, Success }, selected);
        }

        private void Confirmation_Click(object sender, RoutedEventArgs e)
        {
            Button selected = sender as Button;
            SolidColorBrush colour = selected.Background as SolidColorBrush;

            if (colour.Color == Colors.LightGreen)
            {
                selected.Background = new SolidColorBrush(Colors.White);
            }
            else
            {
                selected.Background = new SolidColorBrush(Colors.LightGreen);
            }
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
