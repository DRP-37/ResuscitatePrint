using Resuscitate.DataClasses;
using System;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Resuscitate
{
    public sealed partial class ReviewPage : Page
    {
        private static readonly Color PATIENT_DATA_COMPLETE = InputUtils.DEFAULT_SELECTED_COLOUR;
        private static readonly Color PATIENT_DATA_INCOMPLETE = InputUtils.ConvertHexColour("#FFDB4325");

        private static readonly Color EXPORT_COMPLETE_COLOUR = InputUtils.DEFAULT_SELECTED_COLOUR;

        private PatientData PatientData;
        private Timing TimingCount;
        private StatusList StatusList;

        private bool exported = false;

        public ReviewPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Take value from previous screen
            var RDaT = (ReviewDataAndTiming)e.Parameter;
            PatientData = RDaT.PatientData;
            TimingCount = RDaT.Timing;

            if (RDaT.StatusList != null)
            {
                StatusList = RDaT.StatusList;
            }

            PatientInfo.Background = MainPage.IsPatientDataComplete ? new SolidColorBrush(PATIENT_DATA_COMPLETE) :
                new SolidColorBrush(PATIENT_DATA_INCOMPLETE);

            base.OnNavigatedTo(e);
        }

        private async void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            if (PatientData.Id == null)
            {
                var dialog = new MessageDialog("Patient ID must be filled out in the \"Patient Information\" menu");
                await dialog.ShowAsync();
                return;
            }

            StatusList.ExportAsTextFile(PatientData.Id, ExportButton, Notification);

            // bool exported cannot be set here, since ExportAsTextFile is async, the message popup and colour change can happen
            //    at any time, not necessarily before this line. It is updated when 'Finish' is clicked
        }

        private void FinishButton_Click(object sender, RoutedEventArgs e)
        {
            if (((SolidColorBrush) ExportButton.Background).Color != EXPORT_COMPLETE_COLOUR)
            {
                ShowCancelMessage();
                return;
            }

            FinishAndLeavePage();
        }

        private void FinishAndLeavePage()
        {
            TimingCount.Stop();

            // Clear cache stored locally
            var frame = Window.Current.Content as Frame;
            if (frame != null)
            {
                var cacheSize = ((frame)).CacheSize;
                ((frame)).CacheSize = 0;
                ((frame)).CacheSize = cacheSize;
            }

            this.Frame.Navigate(typeof(MainPage));
        }

        private async void ShowCancelMessage()
        {
            // Create the message dialog and set its content
            var messageDialog = new MessageDialog("You haven't exported this procedure.\nAre you sure you want to leave without exporting?");

            // Add commands and set their callbacks; both buttons use the same callback function instead of inline event handlers
            messageDialog.Commands.Add(new UICommand("Continue",
                new UICommandInvokedHandler(this.CommandInvokedHandler)));
            messageDialog.Commands.Add(new UICommand("Cancel",
                new UICommandInvokedHandler(this.CommandInvokedHandler)));

            // Set the command that will be invoked by default
            messageDialog.DefaultCommandIndex = 1;

            // Set the command to be invoked when escape is pressed
            messageDialog.CancelCommandIndex = 1;

            // Show the message dialog
            await messageDialog.ShowAsync();
        }

        private void CommandInvokedHandler(IUICommand command)
        {
            switch (command.Label)
            {
                case "Continue":
                    FinishAndLeavePage();
                    break;
                case "Cancel":
                    break;
            }
        }

        private void PatientInfo_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(PatientPage), new ReviewDataAndTiming(TimingCount, null, PatientData));
        }

        private void StaffInfo_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(StaffPage), PatientData);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Resuscitation), TimingCount);
        }

        private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {
            // Nothing
        }

        private void TimeView_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Nothing
        }
    }
}
