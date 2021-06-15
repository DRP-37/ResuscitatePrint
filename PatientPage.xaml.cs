using Resuscitate.DataClasses;
using System;
using System.Collections.Generic;
using System.Drawing;
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
        private TextBox[] infoBoxes;
        private bool[] isWrittenTo;

        public PatientData patientData;

        public PatientPage()
        {
            this.InitializeComponent();
            infoBoxes = new TextBox[] { Surname, ID, DateOfBirth, TimeOfBirth, Sex,
                Gestation, EstimatedWeight, MedicalHistory };
            isWrittenTo = new bool[] { false, false, false, false, false, false, false, false };
            this.NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var PT = (PatientTiming)e.Parameter;
            patientData = PT.PatientData;

            isWrittenTo = new bool[] { false, false, false, false, false, false, false, false };
            base.OnNavigatedTo(e);
        }

        private void InformationComplete()
        {
            foreach(TextBox infoBox in infoBoxes)
            {
                if (String.IsNullOrWhiteSpace(infoBox.Text))
                {
                    MainPage.patienInformationComplete = false;
                    return;
                }
            }
            MainPage.patienInformationComplete = true;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            //     Set patient information in data base
            patientData.Surname = Surname.Text;
            patientData.Id = ID.Text;
            patientData.DOB = DateOfBirth.Text;
            patientData.Tob = TimeOfBirth.Text;
            patientData.Sex = Sex.Text;  // might be better to have a button 
            patientData.Gestation = Gestation.Text;
            patientData.Weight = EstimatedWeight.Text;
            patientData.History = MedicalHistory.Text;
            
            ConfirmButton.Background = new SolidColorBrush(new Windows.UI.Color() { R = 242, G = 242, B = 242}) ;
            GoBack();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < infoBoxes.Length; i++)
            {
                if (isWrittenTo[i])
                {
                    infoBoxes[i].Text = "";
                }
            }

            GoBack();
        }

        private void GoBack()
        {
            InformationComplete();
            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame.CanGoBack)
            {
                rootFrame.GoBack();
            }
        }

        private void MedicalHistory_TextChanged(object sender, TextChangedEventArgs e)
        {
            int index = Array.IndexOf(infoBoxes, sender as TextBox);
            isWrittenTo[index] = true;
            ConfirmButton.Background = new SolidColorBrush(Colors.LightGreen);
        }
    }
}
