using Resuscitate.DataClasses;
using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Resuscitate
{
    public sealed partial class IntubationPage : Page
    {
        private static readonly Color SELECTED_COLOUR = InputUtils.DEFAULT_SELECTED_COLOUR;
        private static readonly Color UNSELECTED_COLOUR = InputUtils.DEFAULT_UNSELECTED_COLOUR;

        private Timing TimingCount;
        private Button[] IntubationSuction;
        private Button[] Successes;
        private Button[] Confirmations;

        private StatusEvent IntubationEvent = null;

        public IntubationPage()
        {
            this.InitializeComponent();

            IntubationSuction = new Button[] { Intubation, Suction };
            Successes = new Button[] { Success, Unsuccessful };
            Confirmations = new Button[] { ETCO2, EqualAir, UnequalAir };
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Take value from previous screen
            TimingCount = (Timing)e.Parameter;

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

            List<StatusEvent> StatusEvents = new List<StatusEvent>();
            StatusEvents.Add(IntubationEvent);

            Frame.Navigate(typeof(Resuscitation), new TimingAndEvents(TimingCount, StatusEvents));
        }

        private void Suction_Click(object sender, RoutedEventArgs e)
        {
            Button selected = InputUtils.ClickWithColours((Button) sender, IntubationSuction, SELECTED_COLOUR, UNSELECTED_COLOUR);

            if (selected == null)
            {
                IntubationEvent = null;
                return;
            }

            Success.Visibility = Visibility.Collapsed;
            Unsuccessful.Visibility = Visibility.Collapsed;
            Confirmation.Visibility = Visibility.Collapsed;

            IntubationEvent = new StatusEvent("Suction", (TextBlock) selected.Content, TimingCount.Time);
        }

        private void Intubation_Click(object sender, RoutedEventArgs e)
        {
            Button selected = InputUtils.ClickWithColours((Button) sender, IntubationSuction, SELECTED_COLOUR, UNSELECTED_COLOUR);

            if (selected == null)
            {
                IntubationEvent = null;

                Success.Visibility = Visibility.Collapsed;
                Unsuccessful.Visibility = Visibility.Collapsed;
                return;
            }

            Success.Visibility = Visibility.Visible;
            Unsuccessful.Visibility = Visibility.Visible;
        }

        private void Unsuccessful_Click(object sender, RoutedEventArgs e)
        {
            Button selected = InputUtils.ClickWithColours((Button)sender, Successes, SELECTED_COLOUR, UNSELECTED_COLOUR);

            if (selected == null)
            {
                IntubationEvent = null;

                return;
            }

            Confirmation.Visibility = Visibility.Collapsed;

            IntubationEvent = new StatusEvent("Intubation", "Unsuccessful", TimingCount.Time);
        }

        private void Success_Click(object sender, RoutedEventArgs e)
        {
            Button selected = InputUtils.ClickWithColours((Button) sender, Successes, SELECTED_COLOUR, UNSELECTED_COLOUR);

            if (selected == null)
            {
                IntubationEvent = null;

                Confirmation.Visibility = Visibility.Collapsed;
                return;
            }

            Confirmation.Visibility = Visibility.Visible;
        }

        private void Confirmation_Click(object sender, RoutedEventArgs e)
        {
            Button selected = sender as Button;
            SolidColorBrush colour = selected.Background as SolidColorBrush;

            if (colour.Color == SELECTED_COLOUR)
            {
                selected.Background = new SolidColorBrush(UNSELECTED_COLOUR);
            } else
            {
                selected.Background = new SolidColorBrush(SELECTED_COLOUR);

                // Flip colours of Equal/UnequalAir buttons
                if (selected.Equals(UnequalAir))
                {
                    EqualAir.Background = new SolidColorBrush(UNSELECTED_COLOUR);

                } else if (selected.Equals(EqualAir))
                {
                    UnequalAir.Background = new SolidColorBrush(UNSELECTED_COLOUR);
                }
            }

            IntubationEvent = GenerateSuccessfulStatusEvent();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Resuscitation), TimingCount);
        }

        private StatusEvent GenerateSuccessfulStatusEvent()
        {
            string confirmations = getConfirmationString();

            if (confirmations == null)
            {
                return null;
            }

            return new StatusEvent("Intubation", confirmations, TimingCount.Time);
        }

        private string getConfirmationString()
        {
            string data = "Successful: ";

            bool selectionMade = false;
            foreach (Button button in Confirmations)
            {
                SolidColorBrush confirmColour = button.Background as SolidColorBrush;

                if (confirmColour.Color == SELECTED_COLOUR)
                {
                    data += button.Content.ToString() + ", ";
                    selectionMade = true;
                }
            }

            if (!selectionMade)
            {
                return null;
            }

            // remove last comma before returning
            return data.Substring(0, data.Length - 2);
        }

        private void TimeView_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Nothing
        }

        private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {
            // Nothing
        }
    }
}
