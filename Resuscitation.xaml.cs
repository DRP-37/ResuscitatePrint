﻿using Resuscitate.DataClasses;
using System;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Resuscitate
{
    public sealed partial class Resuscitation : Page
    {
        // Global Variables
        public static Stopwatch reassessmentTimer;
        public static Stopwatch cprTimer;
        public static Stopwatch apgarTimer;
        public static int apgarCounter;
        public static bool[] apgarChecksCompleted;

        private PatientData data;
        private DispatcherTimer Timer = new DispatcherTimer();
        private StatusList StatusList = new StatusList();

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
            apgarCounter = 0;
            apgarChecksCompleted = new bool[] { false, false, false, false, false };

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            CurrTime = DateTime.Now.ToString("HH:mm");

            // If you want to specify you have come back from a page (eg StaffPage) use:
            //   Frame.ForwardStack.Count > 0 && Frame.ForwardStack.ElementAt(0).SourcePageType.Name == "StaffPage"
            if (Frame.ForwardStack.Count > 0)
            {
                return;
            }

            // Parameter checks
            if (e.Parameter.GetType() == typeof(EventAndTiming))
            {
                var EAndT = (EventAndTiming)e.Parameter;
                TimingCount = EAndT.Timing;

                foreach (Event Event in EAndT.MedicalEvents)
                {
                    data.addItem(Event);
                }

                StatusList.AddAll(EAndT.StatusEvents);
                StatusListView.ScrollIntoView(StatusList.LastItem());

            } else if (e.Parameter.GetType() == typeof(Timing))
            {
                TimingCount = (Timing)e.Parameter;

            } else if (e.Parameter.GetType() == typeof(ReviewDataAndTiming))
            {
                var RDaT = (ReviewDataAndTiming)e.Parameter;
                TimingCount = RDaT.Timing;
                data = RDaT.PatientData;

                if (data.DOB == "N/A")
                {
                    data.DOB = DateTime.Now.ToString("dd/MM/yyyy");
                }

                if (RDaT.StatusList != null)
                {
                    StatusList = RDaT.StatusList;
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
            return TimeSpan.Compare(cprTimer.Elapsed, new TimeSpan(0, 0, 30)) >= 0;
        }

        private bool displayReassessNotif()
        {
            return TimeSpan.Compare(reassessmentTimer.Elapsed, new TimeSpan(0, 2, 0)) >= 0;
        }

        private bool displayApgarNotif()
        {
            if (apgarCounter == 0)
            {
                return TimeView.Text.StartsWith("01:");
            }

            if (apgarCounter == 1)
            {
                return TimeView.Text.StartsWith("05:") || ((!apgarChecksCompleted[apgarCounter]) &&
                    (TimeSpan.Compare(apgarTimer.Elapsed, new TimeSpan(0, 4, 0)) >= 0));
            }

            if (!apgarChecksCompleted[apgarCounter])
            {
                return (TimeView.Text.StartsWith("" + apgarCounter * 5 + ":")) ||
                     (TimeSpan.Compare(apgarTimer.Elapsed, new TimeSpan(0, 5, 0)) >= 0);
            }

            return false;
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
            this.Frame.Navigate(typeof(NotesPage), new ReviewDataAndTiming(TimingCount, StatusList, data));
        }

        private void ReviewButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ReviewPage), new ReviewDataAndTiming(TimingCount, StatusList, data));
        }

        private void StaffInfoButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(StaffPage), data);
        }

        // Changes the image's width to fit the scroller screen
        private void AlgoScrollViewer_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Algorithm.Width = AlgoScrollViewer.ViewportWidth;
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
