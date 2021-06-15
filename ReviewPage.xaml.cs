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

        public ReviewPage()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {

            var PT = (PatientTiming)e.Parameter;
            patientData = PT.PatientData;
            TimingCount = PT.Timing;

            //var dialog = new MessageDialog(patientData.ToString());
            //await dialog.ShowAsync();

            // Take value from previous screen


            PatientInfo.Background = MainPage.patienInformationComplete ? new SolidColorBrush(Colors.LightGreen) :
                new SolidColorBrush(Colors.Red);
            base.OnNavigatedTo(e);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame.CanGoBack)
            {
                rootFrame.GoBack();
            }
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {

        }

        private void FinishButton_Click(object sender, RoutedEventArgs e)
        {
            TimingCount.Stop();

            patientData.sendToFirestore();

            // Send data to the firestore
            // This will be replaced with the actual patient data being passed around.
            //PatientData patientData = new PatientData();
            //patientData.sendToFirestore();

            this.Frame.Navigate(typeof(MainPage));
        }

        private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        private void TimeView_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void PatientInfo_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(PatientPage), new PatientTiming(TimingCount, patientData));
        }
    }
}
