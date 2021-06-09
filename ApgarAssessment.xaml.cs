﻿using System;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media;
using Resuscitate.DataClasses;
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Resuscitate
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ApgarAssessment : Page
    {
        int hr;
        int respiration;
        int tone;
        int response;
        int colour;
        Button[] colours;
        Button[] hrs;
        Button[] respirations;
        Button[] tones;
        Button[] responses;
        Timing TimingCount;
        TextBox displayScore;

        int scoreCount;
        ApgarScore score;
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
            TimingCount = (Timing)e.Parameter;

            score = new ApgarScore();
            score.Time = TimingCount;
            base.OnNavigatedTo(e);
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            score.Hr = hr;
            score.Respiration = respiration;
            score.Tone = tone;
            score.Response = response;
            score.Colour = colour;

            Timing timing = new Timing();
            timing.Offset = 0;
            timing.Count = 0;
            Frame.Navigate(typeof(Resuscitation), timing);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame.CanGoBack)
            {
                rootFrame.GoBack();
            }
        }

        private void colour_Click(object sender, RoutedEventArgs e)
        {
            Button selected = (sender as Button);
            int prevIndex = FindSelected(colours);
            changeColours(colours, selected);
            this.colour = selected.Name[selected.Name.Length - 1] - '0';

            UpdateScore(prevIndex, colour);
            Console.WriteLine(this.colour);
        }

        private void hr_Click(object sender, RoutedEventArgs e)
        {
            Button selected = (sender as Button);
            int prevIndex = FindSelected(hrs);
            changeColours(hrs, selected);
            this.hr = selected.Name[selected.Name.Length - 1] - '0';

            UpdateScore(prevIndex, hr);
            Console.WriteLine(this.colour);
        }

        private void response_Click(object sender, RoutedEventArgs e)
        {
            Button selected = (sender as Button);
            int prevIndex = FindSelected(responses);
            changeColours(responses, selected);
            this.response = selected.Name[selected.Name.Length - 1] - '0';

            UpdateScore(prevIndex, response);
        }

        private void tone_Click(object sender, RoutedEventArgs e)
        {
            Button selected = (sender as Button);
            int prevIndex = FindSelected(tones);
            changeColours(tones, selected);
            this.tone = selected.Name[selected.Name.Length - 1] - '0';

            UpdateScore(prevIndex, tone);
        }

        private void resp_Click(object sender, RoutedEventArgs e)
        {
            Button selected = (sender as Button);
            int prevIndex = FindSelected(respirations);
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
            displayScore.Text = "" + scoreCount;
        }


    }
}