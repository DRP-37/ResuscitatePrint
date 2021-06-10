using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
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
    public sealed partial class PatientPage : Page
    {
        public PatientPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        public bool InformationCompleted()
        {

            return true;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            /*     Set patient information in data base
             *     patient.name = Surname.Text;
             *     patient.id = ID.Text;
             *     patient.dob = DateOfBirth.Text;
             *     patient.tob = TimeOfBirth.Text;
             *     patient.sex = Sex.Text;  // might be better to have a button 
             *     patient.gestation = Gestation.Text;
             *     patient.weight = EstimatedWeight.Text;
             *     patient.history = MedicalHistory.Text;
            */
            BackButton_Click(sender, e);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame.CanGoBack)
            {
                rootFrame.GoBack();
            }
        }

        private void MedicalHistory_TextChanged(object sender, TextChangedEventArgs e)
        {
            ConfirmButton.Background = new SolidColorBrush(Colors.LightGreen);
        }
    }
}
