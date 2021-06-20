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

        private StatusEvent IntubationEvent = null;

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
            IntubationEvent = null;

            base.OnNavigatedTo(e);
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (IntubationEvent == null)
            {
                FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
                return;
            }

            List<Event> Events = new List<Event>();
            List<StatusEvent> StatusEvents = new List<StatusEvent>();

            intubationAndSuction.Time = TimingCount;
            intubationAndSuction.Intubation = isIntubation;
            intubationAndSuction.Suction = !isIntubation;
            //intubationAndSuction.Confirmation = (IntubationConfirmation)confirmation;
            intubationAndSuction.IntubationSuccess = isSuccessful;

            Events.Add(intubationAndSuction);
            StatusEvents.Add(IntubationEvent);

            Frame.Navigate(typeof(Resuscitation), new EventAndTiming(TimingCount, Events, StatusEvents));
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
                IntubationEvent = null;
                return;
            }

            isIntubation = false;
            allowConfirmation = true;
            Success.Visibility = Visibility.Collapsed;
            Unsuccessful.Visibility = Visibility.Collapsed;
            UpdateColours(new Button[] { Intubation, Suction }, selected);

            string Data = ((TextBlock)selected.Content).Text.Replace("\n", " ");
            IntubationEvent = new StatusEvent("Suction", Data, TimingCount.Time, intubationAndSuction);
        }

        private void Intubation_Click(object sender, RoutedEventArgs e)
        {
            Button selected = sender as Button;
            SolidColorBrush colour = selected.Background as SolidColorBrush;

            if (colour.Color != Colors.White)
            {
                selected.Background = new SolidColorBrush(Colors.White);
                IntubationEvent = null;
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
                IntubationEvent = null;
                selected.Background = new SolidColorBrush(Colors.White);
                IntubationEvent = null;
                return;
            }

            isSuccessful = false;
            allowConfirmation = true;
            Confirmation.Visibility = Visibility.Collapsed;
            UpdateColours(new Button[] { Unsuccessful, Success }, selected);

            IntubationEvent = new StatusEvent("Intubation", "Unsuccessful", TimingCount.Time, intubationAndSuction);
        }

        private void Success_Click(object sender, RoutedEventArgs e)
        {
            Button selected = sender as Button;
            SolidColorBrush colour = selected.Background as SolidColorBrush;

            if (colour.Color != Colors.White)
            {
                IntubationEvent = null;
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
                IntubationEvent = null;
                return;
            }
            
            selected.Background = new SolidColorBrush(Colors.LightGreen);

            string Data = "Successful: ";

            foreach (Button button in confirmations)
            {
                Data += button.Content.ToString() + ", ";
            }

            Data = Data.Substring(0, Data.Length - 2);

            IntubationEvent = new StatusEvent("Intubation", Data, TimingCount.Time, intubationAndSuction);
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
