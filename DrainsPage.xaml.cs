using Resuscitate.DataClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
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
    public sealed partial class DrainsPage : Page
    {
        public Timing TimingCount { get; set; }
        private Button[] drains;
        // Drain Procedure:
        // 0: Chest Drain: Left
        // 1: Chest Drain: Right
        // 2: Abdominal Drain
        private int drainProcedure;
        private OtherProcedures procedures;
        private StatusEvent DrainEvent;

        public DrainsPage()
        {
            this.InitializeComponent();

            drains = new Button[] { ChestDrainLeft, ChestDrainRight, AbdominalDrain };
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Take value from previous screen
            TimingCount = (Timing)e.Parameter;

            procedures = new OtherProcedures();
            procedures.Time = TimingCount.Time;

            DrainEvent = null;

            base.OnNavigatedTo(e);
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            // set data structure with drainProcedure and time stamp of selection

            procedures.Procedure = (ProcedureType)drainProcedure;

            List<Event> Events = new List<Event>();
            Events.Add(procedures);

            List<StatusEvent> StatusEvents = new List<StatusEvent>();

            if (DrainEvent != null)
            {
                StatusEvents.Add(DrainEvent);
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
            // Nothing
        }

        // Update colours and selection of procedure on click
        private void Drain_Click(object sender, RoutedEventArgs e)
        {
            Button selected = sender as Button;
            SolidColorBrush colour = (SolidColorBrush)selected.Background;

            if (colour.Color == Colors.LightGreen)
            {
                selected.Background = new SolidColorBrush(Colors.White);
                DrainEvent = null;
                drainProcedure = -1;
                return;
            }

            UpdateColours(drains, sender as Button);
            drainProcedure = Array.IndexOf(drains, sender as Button);

            string Data = ((TextBlock)selected.Content).Text.Replace("\n", " ");
            DrainEvent = new StatusEvent("Drain", Data, TimingCount.Time, procedures);
        }

        private void UpdateColours(Button[] buttons, Button sender)
        {
            buttons[0].Background = new SolidColorBrush(Colors.White);
            buttons[1].Background = new SolidColorBrush(Colors.White);
            buttons[2].Background = new SolidColorBrush(Colors.White);
            sender.Background = new SolidColorBrush(Colors.LightGreen);
        }

        private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {
            // Nothing
        }
    }
}
