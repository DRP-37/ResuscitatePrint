using Resuscitate.DataClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
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
    public sealed partial class AirwayPage : Page
    {
        public Timing TimingCount { get; set; }

        // Position:
        // 0: Neutral Head Position
        // 1: Recheck Head Position and Jaw Support
        // 2: Two-person Technique
        private int position = 0; // Set to 0 for now, will change when page is implemented
        private AirwayPositioning positioning;
        private StatusEvent statusEvent;

        public AirwayPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Take value from previous screen
            TimingCount = (Timing)e.Parameter;

            positioning = new AirwayPositioning();
            positioning.Time = TimingCount;

            statusEvent = new StatusEvent();

            base.OnNavigatedTo(e);
        }

        private async void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            positioning.Positioning = (Positioning)position;

            statusEvent.Name = "Positioning";
            statusEvent.Data = positioning.positionToString();
            statusEvent.Time = positioning.Time.ToString();
            statusEvent.Event = positioning;

            var dialog = new MessageDialog(positioning.ToString());
            await dialog.ShowAsync();

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

        }
    }
}
