using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
    public sealed partial class AssessmentsPage : Page
    {
        public Timing TimingCount { get; set; }
        private Button[] colours;
        private Button[] hrs;
        private Button[] respirations;
        private Button[] tones;
        private Button[] tempRegulations;
        private Button[] cordClamping;
        private int cord;
        private int temp;
        private int hr;
        private int respiration;
        private int tone;
        private int colour;

        public AssessmentsPage()
        {
            this.InitializeComponent();
            colours = new Button[] { colour0, colour1, colour2 };
            hrs = new Button[] { hr0, hr1, hr2 };
            respirations = new Button[] { resp0, resp1, resp2 };
            tones = new Button[] { tone0, tone1, tone2 };
            tempRegulations = new Button[] { temp0, temp1, temp2 };
            cordClamping = new Button[] { cord0, cord1, cord2 };
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Take value from previous screen
            TimingCount = (Timing)e.Parameter;

            base.OnNavigatedTo(e);
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            // Set data structure
            // Add check whether a proper selection has been made
            Frame.Navigate(typeof(Resuscitation), TimingCount);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame.CanGoBack)
            {
                rootFrame.GoBack();
            }
        }

        private void TimeView_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Nothing
        }

        private void colour_Click(object sender, RoutedEventArgs e)
        {
            Button selected = (sender as Button);
            changeColours(colours, selected);
            this.colour = selected.Name[selected.Name.Length - 1] - '0';
        }

        private void hr_Click(object sender, RoutedEventArgs e)
        {
            Button selected = (sender as Button);
            changeColours(hrs, selected);
            this.hr = selected.Name[selected.Name.Length - 1] - '0';
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

        private void cord_Click(object sender, RoutedEventArgs e)
        {
            Button selected = (sender as Button);
            changeColours(cordClamping, selected);
            this.cord = selected.Name[selected.Name.Length - 1] - '0';
        }

        private void temp_Click(object sender, RoutedEventArgs e)
        {
            Button selected = (sender as Button);
            changeColours(tempRegulations, selected);
            this.temp = selected.Name[selected.Name.Length - 1] - '0';
        }

        private void changeColours(Button[] buttons, Button sender)
        {
            buttons[0].Background = new SolidColorBrush(Colors.White);
            buttons[1].Background = new SolidColorBrush(Colors.White);
            buttons[2].Background = new SolidColorBrush(Colors.White);
            sender.Background = new SolidColorBrush(Colors.LightGreen);
        }
    }
}
