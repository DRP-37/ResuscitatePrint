using Amazon.Runtime.Internal.Util;
using Resuscitate.DataClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Resuscitate.DataClasses;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Resuscitate
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Resuscitation : Page
    {
        PatientData data = new PatientData();
        private Timing TimingCount;

        public Resuscitation()
        {
            // Initialise
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter.GetType() == typeof(EventAndTiming))
            {
                var EAndT = (EventAndTiming)e.Parameter;
                TimingCount = EAndT.Timing;
                var medicalEvent = EAndT.MedicalEvent;
                data.addItem(medicalEvent);
            }
            else {
                TimingCount = (Timing)e.Parameter;
            }
            

            base.OnNavigatedTo(e);
        }

        private void TimeView_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Nothing
        }

        private void InitAssessmentButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AssessmentsPage), TimingCount);
        }

        private void ApgarButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ApgarAssessment), TimingCount);
        }

        private void ReassessmentButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ObservationPage), TimingCount);
        }

        private void AirwayButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AirwayPage), TimingCount);
        }

        private void VentilationButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(VentilationPage), TimingCount);
        }

        private void IntubationButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(IntubationPage), TimingCount);
        }

        private void CPRButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(CPRPage), TimingCount);
        }

        private void LineInsertionButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(LineInsertionPage), TimingCount);
        }

        private void BloodButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(BloodGasPage), TimingCount);
        }

        private void MedicationButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MedicationPage), TimingCount);
        }

        private void OtherProceduresButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(DrainsPage), TimingCount);
        }

        private void NotesButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(NotesPage), TimingCount);
        }

        private void ReviewButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ReviewPage));
        }

        private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {
            // Nothing
        }

        private void TextBlock_SelectionChanged_1(object sender, RoutedEventArgs e)
        {
            // Nothing
        }
    }
}
