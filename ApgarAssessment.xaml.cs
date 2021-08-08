using System;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media;
using Resuscitate.DataClasses;
using Windows.UI.Popups;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.UI.Xaml.Controls.Primitives;
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Resuscitate
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ApgarAssessment : Page
    {
        private Timing TimingCount { get; set; }

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

            ScoreCount = 0;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Take value from previous screen
            TimingCount = (Timing)e.Parameter;

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

            List<Event> Events = new List<Event>();

            List<StatusEvent> StatusEvents = new List<StatusEvent>();
            StatusEvents.Add(new StatusEvent("Apgar Score", ScoreCount.ToString(), LastTime));

            // Set timer to check times between apgar score checks (Maybe move to new class if time)
            Resuscitation.apgarTimer = Stopwatch.StartNew();

            if (Resuscitation.apgarCounter < 4)
            {
                Resuscitation.apgarChecksCompleted[Resuscitation.apgarCounter] = true;
                Resuscitation.apgarCounter++;
            }

            Frame.Navigate(typeof(Resuscitation), new EventAndTiming(TimingCount, Events, StatusEvents));
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