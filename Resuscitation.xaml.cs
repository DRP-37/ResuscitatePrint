using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Resuscitate
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Resuscitation : Page
    {
        readonly DispatcherTimer Timer = new DispatcherTimer();
        private int Count_Seconds = 0;

        public Resuscitation()
        {
            // Initialise
            this.InitializeComponent();

            // Start timer
            Timer.Tick += Timer_Tick;
            Timer.Interval = new TimeSpan(0, 0, 1);
            Timer.Start();
        }

        private void Timer_Tick(object sender, object e)
        {
            // Update Time
            Count_Seconds ++;

            // Update View with Time
            String minsStr, secsStr;
            int mins = Count_Seconds / 60;
            int secs = Count_Seconds % 60;

            minsStr = mins < 10 ? "0" + mins.ToString() : mins.ToString();
            secsStr = secs < 10 ? "0" + secs.ToString() : secs.ToString();

            TimeView.Text = minsStr + ":" + secsStr;
        }

        private void TimeView_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void InitAssessmentButton_Click(object sender, RoutedEventArgs e)
        {
            //this.Frame.Navigate(InitialAssessment);
        }

        private void ApgarButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ApgarAssessment));
        }

        private void ReassessmentButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AirwayButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void VentilationButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void IntubationButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CPRButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LineInsertionButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BloodButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void initialAssessment_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MedicationButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void OtherProceduresButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void NotesButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }
    }
}
