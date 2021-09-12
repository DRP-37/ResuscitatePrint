using Resuscitate.DataClasses;
using System;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Resuscitate.Pages
{
    public sealed partial class Resuscitation : Page
    {
        // Notification Timers
        public static int apgarCounter;
        public static bool[] apgarChecksCompleted;

        /* Private Variables */
        private string CurrTime;

        private ResuscitationData ResusData;

        private Timing TimingCount;
        private StatusList StatusList;

        public Resuscitation()
        {
            // Initialise
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Enabled;

            // Apgar Notification Handling
            apgarCounter = 0;
            apgarChecksCompleted = new bool[] { false, false, false, false, false };
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            CurrTime = DateTime.Now.ToString("HH:mm");

            // If you want to specify you have come back from a page (eg StaffPage) use:
            //   Frame.ForwardStack.Count > 0 && Frame.ForwardStack.ElementAt(0).SourcePageType.Name == "StaffPage"

            // TODO: This is a quickfix to say if something hasn't specifically navigated here (GoBack was used) the
            //    parameter it gives should not be used
            if (Frame.ForwardStack.Count > 0)
            {
                return;
            }

            // Should only take ResuscitateData or TimingAndEvents
            if (e.Parameter.GetType() == typeof(ResuscitationData))
            {
                ResusData = (ResuscitationData)e.Parameter;
                TimingCount = ResusData.TimingCount;
                StatusList = ResusData.StatusList;

            } else if (e.Parameter.GetType() == typeof(TimingAndEvents))
            {
                TimingAndEvents data = (TimingAndEvents)e.Parameter;
                TimingCount = data.Timing;
                StatusList.AddAll(data.StatusEvents);
            }

            ResusData.SaveLocally();

            StatusListView.ScrollIntoView(StatusList.LastItem());
        }

        /* Runs every second */
        private void TimeView_TextChanged(object sender, TextChangedEventArgs e)
        {
            CurrTimeView.Text = DateTime.Now.ToString("HH:mm");

            if (displayApgarNotif()) {
                Notification.Text = "Calculate next Apgar Score.";
            } else if (displayReassessNotif())
            {
                Notification.Text = ResusData.ReassessmentElapsed().ToString(@"mm\:ss") 
                    + " minutes have passed since the last reassessment.";
            } else if (cprNotif())
            {
                TimeSpan elapsed = ResusData.LastReassessmentTime < ResusData.CPRStartTime ?
                    (TimeSpan) ResusData.CPRElapsed() : ResusData.ReassessmentElapsed();

                Notification.Text = elapsed.ToString(@"mm\:ss")
                    + " seconds of CPR have passed since the last reasessment. Please reassess.";
            } else
            {
                Notification.Text = "";
            }

        }

        private bool cprNotif()
        {
            if (!ResusData.CPRIsRunning())
            {
                return false;
            }

            long? CPRStartTime = ResusData.CPRStartTime;
            long ReassessStartTime = ResusData.LastReassessmentTime;

            if (CPRStartTime != null && ReassessStartTime < CPRStartTime)
            {
                return TimeSpan.Compare((TimeSpan) ResusData.CPRElapsed(), new TimeSpan(0, 0, 30)) >= 0;
            } else
            {
                return TimeSpan.Compare(ResusData.ReassessmentElapsed(), new TimeSpan(0, 0, 30)) >= 0;
            }
        }

        private bool displayReassessNotif()
        {
            if (ResusData.LastReassessmentTime <= 0)
            {
                return false;
            }

            return TimeSpan.Compare(ResusData.ReassessmentElapsed(), new TimeSpan(0, 2, 0)) >= 0;
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
                    (TimeSpan.Compare(ResusData.ApgarElapsed(), new TimeSpan(0, 4, 0)) >= 0));
            }

            if (!apgarChecksCompleted[apgarCounter])
            {
                return TimeView.Text.StartsWith("" + apgarCounter * 5 + ":") ||
                     (TimeSpan.Compare(ResusData.ApgarElapsed(), new TimeSpan(0, 5, 0)) >= 0);
            }

            return false;
        }

        private void InitAssessmentButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AssessmentsPage), ResusData);
        }

        private void ApgarButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ApgarAssessment), ResusData);
        }

        private void ReassessmentButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ObservationPage), ResusData);
        }

        private void VentilationButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(VentilationPage), ResusData);
        }

        private void IntubationButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(IntubationPage), TimingCount);
        }

        private void CPRButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(CPRPage), ResusData);
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
            this.Frame.Navigate(typeof(MedicationPage), ResusData);
        }

        private void OtherProceduresButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(DrainsPage), TimingCount);
        }

        private void NotesButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(NotesPage), ResusData);
        }

        private void ReviewButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ReviewPage), ResusData);
        }

        private void StaffInfoButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(StaffPage), ResusData);
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
