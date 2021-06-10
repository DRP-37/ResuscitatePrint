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
        private String userInput;
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
            TimingCount = (Timing)e.Parameter;

            base.OnNavigatedTo(e);
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            ConfirmButton.Background = new SolidColorBrush(new Windows.UI.Color() { R = 242, G = 242, B = 242 });
            userInput = UserNotes.Text;
            Frame.Navigate(typeof(Resuscitation), TimingCount);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame.CanGoBack)
            {
                rootFrame.GoBack();
            }
        }

        private void TimeView_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void UserNotes_TextChanged(object sender, TextChangedEventArgs e)
        {
            ConfirmButton.Background = new SolidColorBrush(Colors.LightGreen);
        }

        private async void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            // Temporary button to showcase it works - we could have the audio recording 
            // Moved to the review page
            await this._audioRecorder.PlayFromDisk(Dispatcher);
        }

        private void RecordButton_Click(object sender, RoutedEventArgs e)
        {
            if (!isRecording)
            {
                RecordImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/Microphone.png"));
                this._audioRecorder.Record();
                isRecording = true;
            } else
            {
                RecordImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/MicrophoneOff.png"));
                this._audioRecorder.StopRecording();
                isRecording = false;
            }
        }
    }
}
