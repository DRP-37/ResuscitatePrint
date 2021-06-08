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
        Timing TimingCount;

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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            TimingCount = (Timing)e.Parameter;

            score = new ApgarScore(TimingCount.ToString());

            base.OnNavigatedTo(e);
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            score.HeartRate = hr;
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