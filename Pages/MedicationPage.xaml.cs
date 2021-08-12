using Resuscitate.DataClasses;
using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Resuscitate.Pages
{
    public sealed partial class MedicationPage : Page
    {
        private static readonly Color SELECTED_COLOUR = InputUtils.DEFAULT_SELECTED_COLOUR;
        private static readonly Color UNSELECTED_COLOUR = InputUtils.DEFAULT_UNSELECTED_COLOUR;

        private const bool SAVE_DOSES = true;
        private const bool DONT_SAVE_DOSES = false;

        /* Stores the Name, Button and DoseView for every Medication */
        private readonly List<Medication> Medications;

        private ResuscitationData ResusData;
        private Timing TimingCount;

        public MedicationPage()
        {
            this.InitializeComponent();

            Medications = new List<Medication>
            {
                new Medication(ADR1View.Text, ADR1Button, ADR1Dose),
                new Medication(ADR2View.Text, ADR2Button, ADR2Dose),
                new Medication(SodBicarbView.Text, SodBicarbButton, SodBicarbDose),
                new Medication(DextroseView.Text, DextroseButton, DextroseDose),
                new Medication(CellTransfusionView.Text, CellTransfusionButton, CellTransfusionDose),
                new Medication(ADRviaETTView.Text, ADRviaETTButton, ADRviaETTDose),
                new Medication(Surfactant120View.Text, Surfactant120Button, Surfactant120Dose),
                new Medication(Surfactant240View.Text, Surfactant240Button, Surfactant240Dose)
            };
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // Take value from previous screen
            ResusData = (ResuscitationData)e.Parameter;
            TimingCount = ResusData.TimingCount;

            if (ResusData.MedicationDoses.Count == 0)
            {
                for (int i = 0; i < Medications.Count; i++)
                {
                    ResusData.MedicationDoses.Add(0);
                }
            }

            GetDosesFromResusData();
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            List<StatusEvent> StatusEvents = new List<StatusEvent>();

            foreach (Medication Medication in Medications)
            {
                if (Medication.StatusEvent != null)
                {
                    StatusEvents.Add(Medication.StatusEvent);
                }
            }

            ResusData.StatusList.AddAll(StatusEvents);

            ResetMedications(SAVE_DOSES);

            if (StatusEvents.Count > 0)
            {
                Frame.Navigate(typeof(Resuscitation), ResusData);
            }
        }

        private void DoseGiven_Click(object sender, RoutedEventArgs e)
        {
            Medication medication = GetMedicationFromButton((Button) sender);

            SolidColorBrush Brush = medication.Button.Background as SolidColorBrush;

            if (Brush.Color == UNSELECTED_COLOUR)
            {
                medication.incrementDose();
                medication.StatusEvent = GenerateStatusEvent(medication);

                medication.Button.Background = new SolidColorBrush(SELECTED_COLOUR);
            }
            else {
                medication.decrementDose();
                medication.StatusEvent = null;

                medication.Button.Background = new SolidColorBrush(UNSELECTED_COLOUR);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            ResetMedications(DONT_SAVE_DOSES);
            Frame.Navigate(typeof(Resuscitation), TimingCount);
        }

        private void GetDosesFromResusData()
        {
            for (int i = 0; i < Medications.Count; i++)
            {
                Medications[i].setNumDoses(ResusData.MedicationDoses[i]);
            }
        }

        private Medication GetMedicationFromButton(Button button)
        {
            foreach (Medication medication in Medications)
            {
                if (button.Equals(medication.Button))
                {
                    return medication;
                }
            }

            return null;
        }

        private void ResetMedications(bool hasSavedDoses)
        {
            for (int i = 0; i < Medications.Count; i++)
            {
                Medication medication = Medications[i];

                medication.resetMedication(hasSavedDoses);
                ResusData.MedicationDoses[i] = medication.getNumDoses();
            }
        }

        private StatusEvent GenerateStatusEvent(Medication medication)
        {
            return new StatusEvent("Medication Given", medication.Name + " (Dose " + medication.getNumDoses() + ")", TimingCount.Time);
        }

        private void TimeView_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Nothing
        }

        private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {
            // Nothing
        }
    }
}
