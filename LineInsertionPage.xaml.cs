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
    public sealed partial class LineInsertionPage : Page
    {
        public Timing TimingCount { get; set; }
        private Button[] insertion;
        // Airway Positioning:
        // 0: Umbilical
        // 1: Intraosseous
        private int lineInsertion;
        private bool isSuccessful;

        public LineInsertionPage()
        {
            this.InitializeComponent();
            insertion = new Button[] { Intraosseous, Umbilical };
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Take value from previous screen
            TimingCount = (Timing)e.Parameter;

            base.OnNavigatedTo(e);
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            // set data structure with line insertion, whether it was successful
            // and time stamp of selection
            // if a selection has not been made do not allow confirm
            if (SelectionMade(insertion) && 
                SelectionMade(new Button[] { Successful, Unsuccessful}))
            {
                Frame.Navigate(typeof(Resuscitation), TimingCount);
            }
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


        private void Successful_Click(object sender, RoutedEventArgs e)
        {
            Button curr = sender as Button;
            UpdateColours(new Button[] {Successful, Unsuccessful}, curr);
            isSuccessful = curr.Equals(Successful);
        }

        private void Insertion_Click(object sender, RoutedEventArgs e)
        {
            Button curr = sender as Button;
            UpdateColours(insertion, curr);
            lineInsertion = Array.IndexOf(insertion, curr);
        }

        private void UpdateColours(Button[] buttons, Button sender)
        {
            buttons[0].Background = new SolidColorBrush(Colors.White);
            buttons[1].Background = new SolidColorBrush(Colors.White);
            sender.Background = new SolidColorBrush(Colors.LightGreen);
        }

        private bool SelectionMade(Button[] buttons)
        {
            foreach (Button button in buttons)
            {
                SolidColorBrush colour = button.Background as SolidColorBrush;

                if (colour.Color == Colors.LightGreen)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
