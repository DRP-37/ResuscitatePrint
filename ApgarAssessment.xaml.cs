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
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Resuscitate
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ApgarAssessment : Page
    {
        private Timing TimingCount { get; set; }

        private int hr;
        private int respiration;
        private int tone;
        private int response;
        private int colour;
        private Button[] colours;
        private Button[] hrs;
        private Button[] respirations;
        private Button[] tones;
        private Button[] responses;
        private TextBox displayScore;

        private int scoreCount;
        private ApgarScore score;
        private StatusEvent statusEvent;

        public ApgarAssessment()
        {
            this.InitializeComponent();

            colours = new Button[] { colour0, colour1, colour2 };
            hrs = new Button[] { hr0, hr1, hr2 };
            respirations = new Button[] { resp0, resp1, resp2 };
            tones = new Button[] { tone0, tone1, tone2 };
            responses = new Button[] { response0, response1, response2 };

            displayScore = Score;
            scoreCount = 0;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Take value from previous screen
            TimingCount = (Timing)e.Parameter;

            score = new ApgarScore();
            score.Time = TimingCount;

            statusEvent = new StatusEvent();

            base.OnNavigatedTo(e);
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            // If not all options are pressed
            if (FindSelected(tones) == -1 || FindSelected(responses) == -1 || FindSelected(hrs) == -1 
                || FindSelected(colours) == -1 || FindSelected(respirations) == -1)
            {
                return;
            }

            score.Hr = hr;
            score.Respiration = respiration;
            score.Tone = tone;
            score.Response = response;
            score.Colour = colour;

            statusEvent.Name = "Apgar Score";
            statusEvent.Data = score.totalScore().ToString();
            statusEvent.Time = score.Time.ToString();
            statusEvent.Event = score;

            List<Event> Events = new List<Event>();
            Events.Add(score);

            List<StatusEvent> StatusEvents = new List<StatusEvent>();
            StatusEvents.Add(statusEvent);

            Resuscitation.apgarTimer = Stopwatch.StartNew();
            if (Resuscitation.apgarCounter < 4)
            {
                Resuscitation.apgarScoresCompleted[Resuscitation.apgarCounter] = true;
                Resuscitation.apgarCounter++;
            }
            Frame.Navigate(typeof(Resuscitation), new EventAndTiming(TimingCount, Events, StatusEvents));
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Resuscitation), TimingCount);
        }

        private void colour_Click(object sender, RoutedEventArgs e)
        {
            Button selected = sender as Button;
            int prevIndex = FindSelected(colours);
            int selectedIndex = Array.IndexOf(colours, selected);

            if (prevIndex == selectedIndex)
            {
                selected.Background = new SolidColorBrush(Colors.White);
                scoreCount -= selectedIndex;
                displayScore.Text = scoreCount.ToString();
                return;
            }

            changeColours(colours, selected);
            this.colour = selected.Name[selected.Name.Length - 1] - '0';

            UpdateScore(prevIndex, colour);
        }

        private void hr_Click(object sender, RoutedEventArgs e)
        {
            Button selected = sender as Button;
            int prevIndex = FindSelected(hrs);
            int selectedIndex = Array.IndexOf(hrs, selected);

            if (prevIndex == selectedIndex)
            {
                selected.Background = new SolidColorBrush(Colors.White);
                scoreCount -= selectedIndex;
                displayScore.Text = scoreCount.ToString();
                return;
            }

            changeColours(hrs, selected);
            this.hr = selected.Name[selected.Name.Length - 1] - '0';

            UpdateScore(prevIndex, hr);
        }

        private void response_Click(object sender, RoutedEventArgs e)
        {
            Button selected = sender as Button;
            int prevIndex = FindSelected(responses);
            int selectedIndex = Array.IndexOf(responses, selected);

            if (prevIndex == selectedIndex)
            {
                selected.Background = new SolidColorBrush(Colors.White);
                scoreCount -= selectedIndex;
                displayScore.Text = scoreCount.ToString();
                return;
            }

            changeColours(responses, selected);
            this.response = selected.Name[selected.Name.Length - 1] - '0';

            UpdateScore(prevIndex, response);
        }

        private void tone_Click(object sender, RoutedEventArgs e)
        {
            Button selected = sender as Button;
            int prevIndex = FindSelected(tones);
            int selectedIndex = Array.IndexOf(tones, selected);

            if (prevIndex == selectedIndex)
            {
                selected.Background = new SolidColorBrush(Colors.White);
                scoreCount -= selectedIndex;
                displayScore.Text = scoreCount.ToString();
                return;
            }

            changeColours(tones, selected);
            this.tone = selected.Name[selected.Name.Length - 1] - '0';

            UpdateScore(prevIndex, tone);
        }

        private void resp_Click(object sender, RoutedEventArgs e)
        {
            Button selected = sender as Button;
            int prevIndex = FindSelected(respirations);
            int selectedIndex = Array.IndexOf(respirations, selected);

            if (prevIndex == selectedIndex)
            {
                selected.Background = new SolidColorBrush(Colors.White);
                scoreCount -= selectedIndex;
                displayScore.Text = scoreCount.ToString();
                return;
            }

            changeColours(respirations, selected);
            this.respiration = selected.Name[selected.Name.Length - 1] - '0';
            UpdateScore(prevIndex, respiration);
        }

        private void changeColours(Button[] buttons, Button sender) {
            buttons[0].Background = new SolidColorBrush(Colors.White);
            buttons[1].Background = new SolidColorBrush(Colors.White);
            buttons[2].Background = new SolidColorBrush(Colors.White);
            sender.Background = new SolidColorBrush(Colors.LightGreen);
        }

        // Returns the index of the currently selected button;
        // Returns -1 if a button has not been selected
        private int FindSelected(Button[] buttons)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                Button curr = buttons[i];
                SolidColorBrush colour = curr.Background as SolidColorBrush;

                if (colour.Color == Colors.LightGreen)
                {
                    return i;
                }
            }

            return -1;
        }

        // Updates the Current Score displayed on screen
        private void UpdateScore(int prev, int curr) 
        {
            scoreCount = prev == -1 ? (scoreCount + curr) : (scoreCount + curr - prev);
            displayScore.Text = scoreCount.ToString();
        }

    }
}