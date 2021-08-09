using Resuscitate.DataClasses;
using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Resuscitate
{
    public sealed partial class PatientPage : Page
    {
        private static readonly Color UNCHANGED_COLOR = InputUtils.DEFAULT_NAVIGATION_BUTTON_COLOUR;
        private static readonly Color CONFIRM_UPDATABLE_COLOR = InputUtils.DEFAULT_SELECTED_COLOUR;

        private ResuscitationData ResusData;

        private TextBox[] InfoViews;
        private string[] PreviousText;

        public PatientPage()
        {
            this.InitializeComponent();

            InfoViews = new TextBox[] { Surname, ID, DateOfBirth, TimeOfBirth, Sex,
                                        Gestation, EstimatedWeight, MedicalHistory };
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Can take a ResuscitationData parameter with PatientData
            if (e.Parameter.GetType() == typeof(ResuscitationData))
            {
                this.ResusData = (ResuscitationData)e.Parameter;
            }

            UpdateInfoViews();

            PreviousText = new string[] { Surname.Text, ID.Text, DateOfBirth.Text, TimeOfBirth.Text, Sex.Text,
                                                Gestation.Text, EstimatedWeight.Text, MedicalHistory.Text };

            ConfirmButton.Background = new SolidColorBrush(UNCHANGED_COLOR);

            base.OnNavigatedTo(e);
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            // Set patient information in database
            ResusData.PatientData.Surname = Surname.Text;
            ResusData.PatientData.Id = ID.Text;
            ResusData.PatientData.DOB = DateOfBirth.Text;
            ResusData.PatientData.Tob = TimeOfBirth.Text;
            ResusData.PatientData.Sex = Sex.Text;  // might be better to have a button 
            ResusData.PatientData.Gestation = Gestation.Text;
            ResusData.PatientData.Weight = EstimatedWeight.Text;
            ResusData.PatientData.History = MedicalHistory.Text;
            
            GoBack();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < InfoViews.Length; i++)
            {
                InfoViews[i].Text = PreviousText[i];
            }

            GoBack();
        }

        // All InfoViews TextBoxes use this
        private void MedicalHistory_TextChanged(object sender, TextChangedEventArgs e)
        {
            ConfirmButton.Background = new SolidColorBrush(CONFIRM_UPDATABLE_COLOR);
        }

        // Only call after PatientData has been set
        private void UpdateInfoViews()
        {
            Surname.Text = ResusData.PatientData.Surname;
            ID.Text = ResusData.PatientData.Id;
            DateOfBirth.Text = ResusData.PatientData.DOB;
            TimeOfBirth.Text = ResusData.PatientData.Tob;
            Sex.Text = ResusData.PatientData.Sex;
            Gestation.Text = ResusData.PatientData.Gestation;
            EstimatedWeight.Text = ResusData.PatientData.Weight;
            MedicalHistory.Text = ResusData.PatientData.History;
        }

        private void InformationComplete()
        {
            foreach (TextBox infoBox in InfoViews)
            {
                if (string.IsNullOrWhiteSpace(infoBox.Text))
                {
                    ResusData.PatientData.isComplete = false;
                    return;
                }
            }

            ResusData.PatientData.isComplete = true;
        }

        private void GoBack()
        {
            InformationComplete();

            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame.CanGoBack)
            {
                PageStackEntry prevStackEntry = rootFrame.BackStack[rootFrame.BackStackDepth - 1];
                this.Frame.Navigate(prevStackEntry.SourcePageType, ResusData);
            }
        }
    }
}
