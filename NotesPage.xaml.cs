using Resuscitate.DataClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;


namespace Resuscitate
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NotesPage : Page
    {
        public Timing TimingCount { get; set; }
        public ReviewDataAndTiming RDaT;
        public PatientData patientData;
        public StatusList statusList;
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
            RDaT = (ReviewDataAndTiming)e.Parameter;
            TimingCount = RDaT.Timing;
            patientData = RDaT.PatientData;
            statusList = RDaT.StatusList;
        }
        
        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            ConfirmButton.Background = new SolidColorBrush(new Color() { R = 242, G = 242, B = 242 });
            userInput = new Notes(UserNotes.Text);
            patientData.addNote(userInput);
            Frame.Navigate(typeof(Resuscitation), new ReviewDataAndTiming(TimingCount, statusList, patientData));
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Resuscitation), TimingCount);
        }

        private void TimeView_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void UserNotes_TextChanged(object sender, TextChangedEventArgs e)
        {
            ConfirmButton.Background = new SolidColorBrush(Colors.LightGreen);
        }

    

        
    }
}
