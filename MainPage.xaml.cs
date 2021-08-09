using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Resuscitate.DataClasses;
using Windows.Storage;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Resuscitate
{
    public sealed partial class MainPage : Page
    {
        public static ApplicationDataContainer AppSettings { get; set; }

        public MainPage()
        {
            this.InitializeComponent();

            Frame mainFrame = Window.Current.Content as Frame;
            mainFrame.ContentTransitions = null;

            AppSettings = ApplicationData.Current.LocalSettings;
        } 

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            // Go to main page
            this.Frame.Navigate(typeof(InputTime));
        }

        /* Currently collapsed */
        private void ReviewButton_Click(object sender, RoutedEventArgs e)
        {

        }
        
        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SettingsPage));
        }
    }
}
