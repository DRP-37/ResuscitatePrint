using Resuscitate.DataClasses;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Navigation;

namespace Resuscitate.Pages
{

    public sealed partial class LineInsertionPage : Page
    {
        private Timing TimingCount;
        private Button[] InsertionButtons;
        private Button[] Successes;

        private StatusEvent LineInsertionEvent;

        public LineInsertionPage()
        {
            this.InitializeComponent();

            InsertionButtons = new Button[] { Intraosseous, Umbilical };
            Successes = new Button[] { Successful, Unsuccessful };
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Take value from previous screen
            TimingCount = (Timing)e.Parameter;

            LineInsertionEvent = null;

            base.OnNavigatedTo(e);
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            List<StatusEvent> StatusEvents = new List<StatusEvent>();

            if (LineInsertionEvent != null)
            {
                StatusEvents.Add(LineInsertionEvent);
            }

            if (StatusEvents.Count > 0)
            {
                Frame.Navigate(typeof(Resuscitation), new TimingAndEvents(TimingCount, StatusEvents));
            }
        }

        private void Insertion_Click(object sender, RoutedEventArgs e)
        {
            Button selected = InputUtils.ClickWithDefaults((Button) sender, InsertionButtons);

            if (selected == null)
            {
                LineInsertionEvent = null;
                return;
            }

            LineInsertionEvent = GenerateStatusEvent();
        }

        private void Successful_Click(object sender, RoutedEventArgs e)
        {
            Button selected = InputUtils.ClickWithDefaults((Button)sender, Successes);

            if (selected == null)
            {
                LineInsertionEvent = null;
                return;
            }

            LineInsertionEvent = GenerateStatusEvent();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Resuscitation), TimingCount);
        }

        private StatusEvent GenerateStatusEvent()
        {
            Button insertionSelection = InputUtils.SelectionMade(InsertionButtons);
            Button successSelection = InputUtils.SelectionMade(Successes);

            if (insertionSelection == null || successSelection == null)
            {
                return null;
            }

            string insertion = ((TextBlock) insertionSelection.Content).Text.Replace("\n", " ");
            string data = insertion + ": " + successSelection.Content;

            return new StatusEvent("Line Insertion", data, TimingCount.Time);
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
