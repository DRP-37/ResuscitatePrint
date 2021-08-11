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
        private ResuscitationData ResusData;
        private Timing TimingCount;

        private Button[] Colours;
        private Button[] HeartRates;
        private Button[] Responses;
        private Button[] Tones;
        private Button[] Respirations;

        /* scoreCount holds the current total score
         * lastTime holds the time at which the last button was selected */
        private int ScoreCount;
        private string LastTime;

        public ApgarAssessment()
        {
            this.InitializeComponent();

            Colours = new Button[] { colour0, colour1, colour2 };
            HeartRates = new Button[] { hr0, hr1, hr2 };
            Responses = new Button[] { response0, response1, response2 };
            Tones = new Button[] { tone0, tone1, tone2 };
            Respirations = new Button[] { resp0, resp1, resp2 };
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

        private void colour_Click(object sender, RoutedEventArgs e)
        {
            ApgarDefaultClick((Button) sender, Colours);
        }

        private void hr_Click(object sender, RoutedEventArgs e)
        {
            ApgarDefaultClick((Button)sender, HeartRates);
        }

        private void response_Click(object sender, RoutedEventArgs e)
        {
            ApgarDefaultClick((Button)sender, Responses);
        }

        private void tone_Click(object sender, RoutedEventArgs e)
        {
            ApgarDefaultClick((Button)sender, Tones);
        }

        private void resp_Click(object sender, RoutedEventArgs e)
        {
            ApgarDefaultClick((Button)sender, Respirations);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Resuscitation), TimingCount);
        }

    }
}