using Resuscitate.DataClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
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
        //private string tick = "\u2713";
        //private string cross = "\u2A09";

        private LineInsertion insertion;
        private StatusEvent LineInsertionEvent;

        private Button[] insertionButtons;
        // Airway Positioning:
        // 0: Umbilical
        // 1: Intraosseous
        private int? lineInsertion;
        private bool? isSuccessful;

        public LineInsertionPage()
        {
            this.InitializeComponent();
            insertionButtons = new Button[] { Intraosseous, Umbilical };
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Take value from previous screen
            TimingCount = (Timing)e.Parameter;

            insertion = new LineInsertion();
            insertion.Time = TimingCount.Time;

            isSuccessful = null;
            lineInsertion = null;

            LineInsertionEvent = new StatusEvent();

            base.OnNavigatedTo(e);
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            // set data structure with line insertion, whether it was successful
            // and time stamp of selection
            // if a selection has not been made do not allow confirm
            List<Event> Events = new List<Event>();
            Events.Add(insertion);

            List<StatusEvent> StatusEvents = new List<StatusEvent>();
            //StatusEvents.Add(new StatusEvent(insertion.insertionToString(), insertion.Successful ? "Successful" : "Unsuccessful", insertion.Time, insertion));

            if (LineInsertionEvent != null)
            {
                StatusEvents.Add(LineInsertionEvent);
            }

            if (StatusEvents.Count > 0)
            {
                Frame.Navigate(typeof(Resuscitation), new EventAndTiming(TimingCount, Events, StatusEvents));
            }
            
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Resuscitation), TimingCount);
        }

        private void TimeView_TextChanged(object sender, TextChangedEventArgs e)
        {

        }


        private void Successful_Click(object sender, RoutedEventArgs e)
        {
            Button curr = sender as Button;
            SolidColorBrush colour = (SolidColorBrush)curr.Background;

            if (colour.Color == Colors.LightGreen)
            {
                curr.Background = new SolidColorBrush(Colors.White);
                isSuccessful = null;
                LineInsertionEvent = null;
                return;
            }

            UpdateColours(new Button[] {Successful, Unsuccessful}, curr);
            isSuccessful = curr.Equals(Successful);

            LineInsertionEvent = GenerateStatusEvent();
        }

        private void Insertion_Click(object sender, RoutedEventArgs e)
        {
            Button curr = sender as Button;
            SolidColorBrush colour = (SolidColorBrush)curr.Background;

            if (colour.Color == Colors.LightGreen)
            {
                curr.Background = new SolidColorBrush(Colors.White);
                lineInsertion = null;
                LineInsertionEvent = null;
                return;
            }

            UpdateColours(insertionButtons, curr);
            lineInsertion = Array.IndexOf(insertionButtons, curr);

            LineInsertionEvent = GenerateStatusEvent();
        }

        private void UpdateColours(Button[] buttons, Button sender)
        {
            buttons[0].Background = new SolidColorBrush(Colors.White);
            buttons[1].Background = new SolidColorBrush(Colors.White);
            sender.Background = new SolidColorBrush(Colors.LightGreen);
        }

        private StatusEvent GenerateStatusEvent()
        {
            if (lineInsertion == null || isSuccessful == null)
            {
                return null;
            }

            string Insertion = ((TextBlock)insertionButtons[(int)lineInsertion].Content).Text.Replace("\n", " ");
            string Data = Insertion + ": " + ((bool)isSuccessful ? "Successful" : "Unsuccessful");

            return new StatusEvent("Line Insertion", Data, TimingCount.Time, insertion);
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

        private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {
            // Nothing
        }
    }
}
