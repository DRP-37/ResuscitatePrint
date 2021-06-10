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
    public sealed partial class LineInsertionPage : Page
    {
        public Timing TimingCount { get; set; }
        private string tick = "\u2713";
        private string cross = "\u2A09";

        private LineInsertion insertion;
        private StatusEvent statusEvent;

        public LineInsertionPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Take value from previous screen
            TimingCount = (Timing)e.Parameter;

            insertion = new LineInsertion();
            insertion.Time = TimingCount;

            statusEvent = new StatusEvent();

            base.OnNavigatedTo(e);
        }

        private async void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            statusEvent.Name = "Line Insertion";
            statusEvent.Data = insertion.insertionToString() + (insertion.Successful ? tick : cross);
            statusEvent.Time = insertion.Time.ToString();
            statusEvent.Event = insertion;

            var dialog = new MessageDialog(insertion.ToString());
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
