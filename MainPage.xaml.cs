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
using Resuscitate.DataClasses;
using Windows.Storage;
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Resuscitate
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        public static bool patienInformationComplete {get; set;}
        public static ApplicationDataContainer AppSettings { get; set; }

        public MainPage()
        {
            this.InitializeComponent();
            Frame mainFrame = Window.Current.Content as Frame;
            mainFrame.ContentTransitions = null;
            patienInformationComplete = false;
            AppSettings = ApplicationData.Current.LocalSettings;
        } 

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            // Debug testing the firestore
            /*
            database.Name = "Euan S-W";
            database.DOB = "30.10.2000";
            database.addInitialAssessment(new InitialAssessment(new Timing()));
            database.sendToFirestore();
            */

            // Go to main page
            this.Frame.Navigate(typeof(InputTime));
        }

        private void ReviewButton_Click(object sender, RoutedEventArgs e)
        {
            // Go to review page
            this.Frame.Navigate(typeof(SignInPage));
        }

        
        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SettingsPage));
        }
    }
}
