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
    public sealed partial class CPRPage : Page
    {
        public Timing TimingCount { get; set; }
        DispatcherTimer Timer = new DispatcherTimer();
        private int Count = 0;

        private List<int> Timings = new List<int>();

        public CPRPage()
        {
            this.InitializeComponent();

            // Initialise timer
            Timer.Tick += Timer_Tick;
            Timer.Interval = new TimeSpan(0, 0, 1);

            StartButton.Background = new SolidColorBrush(Colors.LightGray);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Take value from previous screen
            TimingCount = (Timing)e.Parameter;

            base.OnNavigatedTo(e);
        }

        private void Timer_Tick(object sender, object e)
        {
            Count++;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            Timer.Stop();
            Timings.Add(Count);

            // TODO: Use list of seconds to make events
            Frame.Navigate(typeof(Resuscitation), TimingCount);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Timer.Stop();
            Timings.Add(Count);

            Frame.Navigate(typeof(Resuscitation), TimingCount);
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (StartButton.Content.Equals("Start"))
            {
                Timer.Start();
                HeartBeating.Visibility = Visibility.Visible;
                StartButton.Content = "Stop";
                StartButton.Background = new SolidColorBrush(Colors.MediumVioletRed);

            } else
            {
                Timer.Stop();
                Timings.Add(Count);
                Count = 0;
                HeartBeating.Visibility = Visibility.Collapsed;
                StartButton.Content = "Start";
                StartButton.Background = new SolidColorBrush(Colors.LightGray);
            }
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
