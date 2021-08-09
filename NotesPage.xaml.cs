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

        private ResuscitationData ResusData;
        private Timing TimingCount;
        private PatientData PatientData;

        private Notes note;
        //private AudioRecorder _audioRecorder;
        //private bool isRecording = false;

        public NotesPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Enabled;

            //this._audioRecorder = new AudioRecorder();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ResusData = (ResuscitationData)e.Parameter;

            TimingCount = ResusData.TimingCount;
            PatientData = ResusData.PatientData;
        }
        
        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            ConfirmButton.Background = new SolidColorBrush(CONFIRMATION_NOT_READY_COLOUR);
            note = new Notes(UserNotes.Text);
            PatientData.addNote(note);

            Frame.Navigate(typeof(Resuscitation), ResusData);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Resuscitation), ResusData);
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
