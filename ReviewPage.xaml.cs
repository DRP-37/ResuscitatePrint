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
    public sealed partial class ReviewPage : Page
    {
        public PatientData patientData;
        public Timing TimingCount;
        private StatusList StatusList;

        private StatusEvent SurvivedEvent;

        public ReviewPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Take value from previous screen
            var RDaT = (ReviewDataAndTiming)e.Parameter;
            patientData = RDaT.PatientData;
            TimingCount = RDaT.Timing;

            if (RDaT.StatusList != null)
            {
                StatusList = RDaT.StatusList;
            }

            PatientInfo.Background = MainPage.patienInformationComplete ? new SolidColorBrush(Colors.LightGreen) :
                new SolidColorBrush(Colors.Red);

            base.OnNavigatedTo(e);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Resuscitation), TimingCount);
        }

        private async void FinishButton_Click(object sender, RoutedEventArgs e)
        {
            TimingCount.Stop();

            if (patientData.Id != null)
            {
                patientData.sendToFirestore();

                // Send data to the firestore
                // This will be replaced with the actual patient data being passed around.
                //PatientData patientData = new PatientData();
                //patientData.sendToFirestore();

                // Clear cache stored locally
                var frame = Window.Current.Content as Frame;
                if (frame != null)
                {
                    var cacheSize = ((frame)).CacheSize;
                    ((frame)).CacheSize = 0;
                    ((frame)).CacheSize = cacheSize;
                }

                this.Frame.Navigate(typeof(MainPage));
            } else
            {
                var dialog = new MessageDialog("Patient ID must be filled out in the \"Patient Information\" menu");
                await dialog.ShowAsync();
            }
        }

        private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {
            // Nothing
        }

        private void TimeView_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Nothing
        }

        private void PatientInfo_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(PatientPage), new ReviewDataAndTiming(TimingCount, null, patientData));
        }

        private void StaffInfo_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(StaffPage), TimingCount);
        }

        private void SurvivedButton_Click(object sender, RoutedEventArgs e)
        {
            Button selected = sender as Button;
            SolidColorBrush colour = selected.Background as SolidColorBrush;

            if (colour.Color == Colors.LightGreen)
            {
                selected.Background = new SolidColorBrush(Colors.White);
                StatusList.Events.Remove(SurvivedEvent);
                SurvivedEvent = null;

            } else
            {
                if (SurvivedEvent != null)
                {
                    Button Opposite = selected == SurvivedYesButton ? SurvivedNoButton : SurvivedYesButton;
                    Opposite.Background = new SolidColorBrush(Colors.White);
                    StatusList.Events.Remove(SurvivedEvent);
                }

                selected.Background = new SolidColorBrush(Colors.LightGreen);
                TextBlock content = (TextBlock)selected.Content;
                SurvivedEvent = new StatusEvent("Survival", content.Text, TimingCount.Time, null);
                StatusList.Events.Add(SurvivedEvent);
            }
        }
    }
}
