using Resuscitate.DataClasses;
using System;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Resuscitate.Pages
{
    public sealed partial class ReviewPage : Page
    {
        public static readonly Color COMPLETE_COLOUR = InputUtils.DEFAULT_SELECTED_COLOUR;

        private static readonly Color PATIENT_DATA_COMPLETE = InputUtils.DEFAULT_SELECTED_COLOUR;
        private static readonly Color PATIENT_DATA_INCOMPLETE = InputUtils.ConvertHexColour("#FFDB4325");

        private static bool Exported = false;

        private ResuscitationData ResusData;
        private Timing TimingCount;
        private PatientData PatientData;
        private StatusList StatusList;

        public ReviewPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Take value from previous screen
            ResusData = (ResuscitationData)e.Parameter;
            TimingCount = ResusData.TimingCount;
            PatientData = ResusData.PatientData;
            StatusList = ResusData.StatusList;

            PatientInfo.Background = PatientData.isComplete ? new SolidColorBrush(PATIENT_DATA_COMPLETE) :
                new SolidColorBrush(PATIENT_DATA_INCOMPLETE);

            if (Exported)
            {
                ExportButton.Background = new SolidColorBrush(COMPLETE_COLOUR);
            }

            ResusData.SaveLocally();

            base.OnNavigatedTo(e);
        }

        private async void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            if (PatientData.Id == null || string.IsNullOrWhiteSpace(PatientData.Id))
            {
                var dialog = new MessageDialog("Patient ID must be filled out in the \"Patient Information\" menu");
                await dialog.ShowAsync();
                return;
            }

            new ExportData(PatientData, ResusData.StaffList, StatusList).ExportAsTextFile(ExportButton, Notification);
        }

        private void FinishButton_Click(object sender, RoutedEventArgs e)
        {
            if (!Exported)
            {
                ShowCancelMessage();
                return;
            }

            FinishAndLeavePage();
        }

        internal static void SetSuccessfulExported(Button exportButton)
        {
            Exported = true;

            exportButton.Background = new SolidColorBrush(COMPLETE_COLOUR);
        }

        private void FinishAndLeavePage()
        {
            TimingCount.Stop();

            ApplicationDataContainer AppSettings = ApplicationData.Current.LocalSettings;
            AppSettings.Values[ResuscitationData.HAS_STORE_KEY] = false;

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
            this.Frame.Navigate(typeof(PatientPage), ResusData);
        }

        private void StaffInfo_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(StaffPage), ResusData);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Resuscitation), ResusData);
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
