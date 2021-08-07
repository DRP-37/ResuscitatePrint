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
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PatientPage : Page
    {
        private static readonly Color CONFIRM_ENABLED_COLOR = new Color() { R = 242, G = 242, B = 242 };
        private static readonly Color CONFIRM_UPDATABLE_COLOR = Colors.LightGreen;

        private const int SURNAME_INDEX = 0;
        private const int ID_INDEX = 1;
        private const int DATE_BIRTH_INDEX = 2;
        private const int TIME_BIRTH_INDEX = 3;
        private const int SEX_INDEX = 4;
        private const int GESTATION_INDEX = 5;
        private const int EST_WEIGHT_INDEX = 6;
        private const int MED_HISTORY_INDEX = 7;

        private TextBox[] infoBoxes;
        private bool[] isWrittenTo;

        public PatientData patientData;
        public static double? UpdatedWeight { get; set; }

        public PatientPage()
        {
            this.InitializeComponent();
            infoBoxes = new TextBox[] { Surname, ID, DateOfBirth, TimeOfBirth, Sex,
                                        Gestation, EstimatedWeight, MedicalHistory };
            this.isWrittenTo = new bool[] { false, false, false, false, false, false, false, false };
            this.NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var PT = (ReviewDataAndTiming)e.Parameter;
            patientData = PT.PatientData;

            if (UpdatedWeight != null) {
                EstimatedWeight.Text = UpdatedWeight.ToString();
                UpdatedWeight = null;
            }

            isWrittenTo = new bool[] { false, false, false, false, false, false, false, false };

            addBirthTimestamp();

            base.OnNavigatedTo(e);
        }

        private void addBirthTimestamp()
        {
            isWrittenTo[DATE_BIRTH_INDEX] = true;
            infoBoxes[DATE_BIRTH_INDEX].Text = patientData.DOB;

            isWrittenTo[TIME_BIRTH_INDEX] = true;
            infoBoxes[TIME_BIRTH_INDEX].Text = patientData.Tob;
        }

        private void InformationComplete()
        {
            foreach(TextBox infoBox in infoBoxes)
            {
                if (string.IsNullOrWhiteSpace(infoBox.Text))
                {
                    MainPage.IsPatientDataComplete = false;
                    return;
                }
            }

            MainPage.IsPatientDataComplete = true;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            // Set patient information in database
            patientData.Surname = Surname.Text;
            patientData.Id = ID.Text;
            patientData.DOB = DateOfBirth.Text;
            patientData.Tob = TimeOfBirth.Text;
            patientData.Sex = Sex.Text;  // might be better to have a button 
            patientData.Gestation = Gestation.Text;
            patientData.Weight = EstimatedWeight.Text;
            patientData.History = MedicalHistory.Text;
            
            ConfirmButton.Background = new SolidColorBrush(CONFIRM_ENABLED_COLOR) ;
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

        private void MedicalHistory_TextChanged(object sender, TextChangedEventArgs e)
        {
            int index = Array.IndexOf(infoBoxes, sender as TextBox);
            isWrittenTo[index] = true;

            ConfirmButton.Background = new SolidColorBrush(CONFIRM_UPDATABLE_COLOR);
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
    }
}
