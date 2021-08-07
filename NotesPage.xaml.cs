using Resuscitate.DataClasses;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace Resuscitate
{
    public sealed partial class NotesPage : Page
    {
        private static readonly Color CONFIRMATION_READY_COLOUR = InputUtils.DEFAULT_SELECTED_COLOUR;
        private static readonly Color CONFIRMATION_NOT_READY_COLOUR = new Color() { R = 242, G = 242, B = 242 };

        private Timing TimingCount;
        private PatientData patientData;
        private StatusList statusList;

        private Notes userInput;
        private AudioRecorder _audioRecorder;
        private bool isRecording = false;

        public NotesPage()
        {
            this.InitializeComponent();
            this._audioRecorder = new AudioRecorder();
            this.NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Take value from previous screen
            ReviewDataAndTiming RDaT = (ReviewDataAndTiming)e.Parameter;
            TimingCount = RDaT.Timing;
            patientData = RDaT.PatientData;
            statusList = RDaT.StatusList;
        }
        
        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            ConfirmButton.Background = new SolidColorBrush(CONFIRMATION_NOT_READY_COLOUR);
            userInput = new Notes(UserNotes.Text);
            patientData.addNote(userInput);

            Frame.Navigate(typeof(Resuscitation), new ReviewDataAndTiming(TimingCount, statusList, patientData));
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Resuscitation), TimingCount);
        }

        private void UserNotes_TextChanged(object sender, TextChangedEventArgs e)
        {
            ConfirmButton.Background = new SolidColorBrush(CONFIRMATION_READY_COLOUR);
        }

        private void TimeView_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Nothing
        }
    }
}
