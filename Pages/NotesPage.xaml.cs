using Resuscitate.DataClasses;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Resuscitate.Pages
{
    public sealed partial class NotesPage : Page
    {
        private static readonly Color CONFIRMATION_READY_COLOUR = InputUtils.DEFAULT_SELECTED_COLOUR;
        private static readonly Color CONFIRMATION_NOT_READY_COLOUR = InputUtils.DEFAULT_NAVIGATION_BUTTON_COLOUR;

        private ResuscitationData ResusData;
        private Timing TimingCount;
        private PatientData PatientData;

        //private AudioRecorder _audioRecorder;
        //private bool isRecording = false;

        public NotesPage()
        {
            this.InitializeComponent();

            //this._audioRecorder = new AudioRecorder();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ResusData = (ResuscitationData)e.Parameter;

            TimingCount = ResusData.TimingCount;
            PatientData = ResusData.PatientData;
        }
        
        private void AddNoteButton_Click(object sender, RoutedEventArgs e)
        {
            PatientData.addNote(new Note(UserNotes.Text));

            UserNotes.Text = "";

            FlyoutBase.ShowAttachedFlyout((FrameworkElement) sender);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Resuscitation), ResusData);
        }

        private void UserNotes_TextChanged(object sender, TextChangedEventArgs e)
        {
            /* If a note is able to be submitted, enable button */
            if (string.IsNullOrWhiteSpace(((TextBox) sender).Text))
            {
                AddNoteButton.IsEnabled = false;
                AddNoteButton.Background = new SolidColorBrush(CONFIRMATION_NOT_READY_COLOUR);
            } else
            {
                AddNoteButton.IsEnabled = true;
                AddNoteButton.Background = new SolidColorBrush(CONFIRMATION_READY_COLOUR);
            }
        }

        private void TimeView_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Nothing
        }
    }
}
