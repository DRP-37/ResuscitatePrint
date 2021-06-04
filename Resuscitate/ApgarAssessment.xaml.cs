using System;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media;

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
        
        ApgarScore score;
        public ApgarAssessment()
        {
            this.InitializeComponent();
            colours = new Button[] { colour0, colour1, colour2 };
            hrs = new Button[] { hr0, hr1, hr2 };
            respirations = new Button[] { resp0, resp1, resp2 };
            tones = new Button[] { tone0, tone1, tone2 };
            responses = new Button[] { response0, response1, response2 };

        }

        private void confirmButton_Click(object sender, RoutedEventArgs e)
        {
            score.HeartRate = hr;
            score.Respiration = respiration;
            score.Tone = tone;
            score.Response = response;
            score.Colour = colour;
            
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var timeString = (e.Parameter as string).Split(':');
            int minutes = timeString[0][0] == '0' ? (timeString[0][1] - '0') : int.Parse(timeString[0]);
            int seconds = timeString[1][0] == '0' ? (timeString[1][1] - '0') : int.Parse(timeString[1]);
            TimeSpan time = new TimeSpan(0, minutes, seconds);
            score = new ApgarScore(time);
        }

        private void colour_Click(object sender, RoutedEventArgs e)
        {
            Button selected = (sender as Button);
            changeColours(colours, selected);
            this.colour = selected.Name[selected.Name.Length - 1] - '0';
            Console.WriteLine(this.colour);
        }

        private void hr_Click(object sender, RoutedEventArgs e)
        {
            Button selected = (sender as Button);
            changeColours(hrs, selected);
            this.hr = selected.Name[selected.Name.Length - 1] - '0';
            Console.WriteLine(this.colour);
        }

        private void response_Click(object sender, RoutedEventArgs e)
        {
            Button selected = (sender as Button);
            changeColours(responses, selected);
            this.response = selected.Name[selected.Name.Length - 1] - '0';
        }

        private void tone_Click(object sender, RoutedEventArgs e)
        {
            Button selected = (sender as Button);
            changeColours(tones, selected);
            this.tone = selected.Name[selected.Name.Length - 1] - '0';
        }

        private void resp_Click(object sender, RoutedEventArgs e)
        {
            Button selected = (sender as Button);
            changeColours(respirations, selected);
            this.respiration = selected.Name[selected.Name.Length - 1] - '0';
        }

        private void changeColours(Button[] buttons, Button sender) {
            buttons[0].Background = new SolidColorBrush(Colors.White);
            buttons[1].Background = new SolidColorBrush(Colors.White);
            buttons[2].Background = new SolidColorBrush(Colors.White);
            sender.Background = new SolidColorBrush(Colors.LightGreen);
        }
    }
}