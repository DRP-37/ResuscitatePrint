using Resuscitate.DataClasses;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Navigation;

namespace Resuscitate.Pages
{
    public sealed partial class ApgarAssessment : Page
    {
        private readonly Button[] Colours;
        private readonly Button[] HeartRates;
        private readonly Button[] Responses;
        private readonly Button[] Tones;
        private readonly Button[] Respirations;

        private ResuscitationData ResusData;
        private Timing TimingCount;

        /* ScoreCount holds the current total score
         * LastTime holds the time at which the last button was selected */
        private int ScoreCount;
        private string LastTime;

        public ApgarAssessment()
        {
            this.InitializeComponent();

            Colours = new Button[] { Colour0Button, Colour1Button, Colour2Button };
            HeartRates = new Button[] { HeartRate0Button, HeartRate1Button, HeartRate2Button };
            Responses = new Button[] { Response0Button, Response1Button, Response2Button };
            Tones = new Button[] { Tone0Button, Tone1Button, Tone2Button };
            Respirations = new Button[] { Respiration0Button, Respiration1Button, Respiration2Button };
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Take value from previous screen
            ResusData = (ResuscitationData)e.Parameter;
            TimingCount = ResusData.TimingCount;

            ScoreCount = 0;

            base.OnNavigatedTo(e);
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            // If not all options are pressed
            if (!AllSectonsSelected())
            {
                FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
                return;
            }

            // Set timer to check times between apgar score checks (Maybe move to new class if time)
            ResusData.StartNewApgarTimer();

            if (Resuscitation.apgarCounter < 4)
            {
                Resuscitation.apgarChecksCompleted[Resuscitation.apgarCounter] = true;
                Resuscitation.apgarCounter++;
            }

            List<StatusEvent> statusEvents = new List<StatusEvent>();
            statusEvents.Add(new StatusEvent("Apgar Score", ScoreCount.ToString(), LastTime));

            ResusData.StatusList.AddAll(statusEvents);

            Frame.Navigate(typeof(Resuscitation), ResusData);
        }

        private bool AllSectonsSelected()
        {
            return InputUtils.SelectionMade(Colours) != null &&
                InputUtils.SelectionMade(HeartRates) != null &&
                InputUtils.SelectionMade(Responses) != null &&
                InputUtils.SelectionMade(Tones) != null &&
                InputUtils.SelectionMade(Respirations) != null;
        }

        private void ApgarDefaultClick(Button sender, Button[] buttons)
        {
            Button selected = InputUtils.SelectionMade(buttons);
            InputUtils.ClickWithDefaults(sender, buttons);

            int newSelectedIndex = Array.IndexOf(buttons, sender);

            if (selected == null)
            {
                ScoreCount += newSelectedIndex;

            } else if (selected.Equals(sender)) {
                ScoreCount -= newSelectedIndex;

            } else
            {
                int prevIndex = Array.IndexOf(buttons, selected);

                ScoreCount += newSelectedIndex - prevIndex;
            }

            LastTime = TimingCount.Time;
            Score.Text = ScoreCount.ToString();
        }

        private void ColourButton_Click(object sender, RoutedEventArgs e)
        {
            ApgarDefaultClick((Button) sender, Colours);
        }

        private void HeartRateButton_Click(object sender, RoutedEventArgs e)
        {
            ApgarDefaultClick((Button)sender, HeartRates);
        }

        private void ResponseButton_Click(object sender, RoutedEventArgs e)
        {
            ApgarDefaultClick((Button)sender, Responses);
        }

        private void ToneButton_Click(object sender, RoutedEventArgs e)
        {
            ApgarDefaultClick((Button)sender, Tones);
        }

        private void RespirationButton_Click(object sender, RoutedEventArgs e)
        {
            ApgarDefaultClick((Button)sender, Respirations);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Resuscitation), TimingCount);
        }

    }
}