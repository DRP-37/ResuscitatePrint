using Resuscitate.DataClasses;
using System;
using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Resuscitate
{

    public sealed partial class DrainsPage : Page
    {
        private Timing TimingCount;
        private StatusEvent DrainEvent;

        private Button[] Drains;

        public DrainsPage()
        {
            this.InitializeComponent();

            Drains = new Button[] { ChestDrainLeft, ChestDrainRight, AbdominalDrain };
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Take value from previous screen
            TimingCount = (Timing)e.Parameter;

            DrainEvent = null;

            base.OnNavigatedTo(e);
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (DrainEvent == null)
            {
                return;
            }

            List<StatusEvent> StatusEvents = new List<StatusEvent>();

            StatusEvents.Add(DrainEvent);
            Frame.Navigate(typeof(Resuscitation), new TimingAndEvents(TimingCount, StatusEvents));
        }

        // Update colours and selection of procedure on click
        private void Drain_Click(object sender, RoutedEventArgs e)
        {
            Button selected = InputUtils.ClickWithDefaults((Button) sender, Drains);

            if (selected == null)
            {
                DrainEvent = null;
            } else
            {
                DrainEvent = new StatusEvent("Drain", (TextBlock)selected.Content, TimingCount.Time);
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

        private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {
            // Nothing
        }
    }
}
