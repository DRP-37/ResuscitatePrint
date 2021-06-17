using Amazon.Runtime.Internal.Util;
using Resuscitate.DataClasses;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Timers;
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
        private PatientData data;
        private DispatcherTimer Timer = new DispatcherTimer();
        private StatusList StatusList = new StatusList();

        public static Stopwatch apgarTimer;
        public static Stopwatch reassessmentTimer;
        public static Stopwatch cprTimer;

        private Timing TimingCount;
        private string CurrTime;

        public Resuscitation()
        {
            // Initialise
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Enabled;
            Timer.Tick += Timer_Tick;
            Timer.Interval = new TimeSpan(0, 0, 1);
            Timer.Start();

            apgarTimer = Stopwatch.StartNew();
            reassessmentTimer = Stopwatch.StartNew();
            cprTimer = new Stopwatch();

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            CurrTime = DateTime.Now.ToString("HH:mm");

            if (e.Parameter.GetType() == typeof(EventAndTiming))
            {
                var EAndT = (EventAndTiming)e.Parameter;
                TimingCount = EAndT.Timing;

                foreach (Event Event in EAndT.MedicalEvents)
                {
                    data.addItem(Event);
                }

                foreach (StatusEvent Event in EAndT.StatusEvents)
                {
                    StatusList.Events.Add(Event);
                }

                int lastItem = EAndT.StatusEvents.Count - 1;

                if (lastItem > -1)
                {
                    StatusListView.ScrollIntoView(EAndT.StatusEvents[lastItem]);
                }
            }
            else if (e.Parameter.GetType() == typeof(Timing))
            {
                TimingCount = (Timing)e.Parameter;
            }
            else
            {
                var RDaT = (ReviewDataAndTiming)e.Parameter;
                TimingCount = RDaT.Timing;
                data = RDaT.PatientData;

                if (RDaT.StatusList != null)
                {
                    this.StatusList = RDaT.StatusList;
                }
            }

            base.OnNavigatedTo(e);
        }
        private void Timer_Tick(object sender, object e)
        {
            CurrTimeView.Text = DateTime.Now.ToString("HH:mm");
        }

        private void TimeView_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Nothing
            if (displayApgarNotif()) {
                Notification.Text = "Calculate next Apgar Score.";
            }
            else if (displayReassessNotif())
            {
                Notification.Text = reassessmentTimer.Elapsed.ToString(@"mm\:ss") 
                    + " minutes have passed since the last reassessment.";
            } else if (cprNotif())
            {
                Notification.Text = cprTimer.Elapsed.ToString(@"mm\:ss")
                    + " seconds of CPR have passed. Please reassess.";
            } else
            {
                Notification.Text = "";
            }

        }

        private bool cprNotif()
        {
            return (TimeSpan.Compare(cprTimer.Elapsed, new TimeSpan(0, 0, 30)) >= 0);
        }

        private bool displayReassessNotif()
        {
            return (TimeSpan.Compare(reassessmentTimer.Elapsed, new TimeSpan(0, 2, 0)) >= 0);
        }

        private bool displayApgarNotif()
        {
            return ((TimeView.Text.StartsWith("10:")) || 
                   (TimeView.Text.StartsWith("15:")) ||
                   (TimeView.Text.StartsWith("20:"))  ||
                   (TimeSpan.Compare(apgarTimer.Elapsed, new TimeSpan(0, 5, 0)) >= 0)) ||
                   (TimeView.Text.StartsWith("01:")) || 
                   ((TimeView.Text.StartsWith("05:")) ||
                   (TimeSpan.Compare(apgarTimer.Elapsed, new TimeSpan(0, 4, 0)) >= 0));
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
            this.Frame.Navigate(typeof(ReviewPage), new ReviewDataAndTiming(TimingCount, StatusList, data));
        }

        private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {
            // Nothing
        }

        private void TextBlock_SelectionChanged_1(object sender, RoutedEventArgs e)
        {
            // Nothing
        }

        // Changes the image's width to fit the scroller screen
        private void AlgoScrollViewer_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Algorithm.Width = AlgoScrollViewer.ViewportWidth;
        }

        private void StaffInfoButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(StaffPage), TimingCount);
        }
    }
}
