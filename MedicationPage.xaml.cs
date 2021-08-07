using Resuscitate.DataClasses;
using System;
using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Resuscitate
{
    public sealed partial class MedicationPage : Page
    {
        private const int NUM_MEDICATIONS = 8;

        private static readonly Color SELECTED_COLOUR = InputUtils.DEFAULT_SELECTED_COLOUR;
        private static readonly Color UNSELECTED_COLOUR = InputUtils.DEFAULT_UNSELECTED_COLOUR;

        private Timing TimingCount;
        private TextBlock[] NameViews;

        // Doses:
        // 0: Adrenaline 1 in 10,000 (0.1 ml/kg) IV  
        // 1: Adrenaline 1 in 10,000 (0.3 ml/kg) IV  
        // 2: Sodium Bicarbonate 4.2% (2 to 4 mls/kg) IV 
        // 3: Dextrose (2.5mls/kg) IV 
        // 4: Red cell transfusion 
        // 5: Adrenaline via ETT 
        // 6: Surfactant via ETT 120mg 
        // 7: Surfactant via ETT 240mg
        private Button[] Medications;
        private TextBlock[] DoseViews;
        private int[] NumDoses = new int[NUM_MEDICATIONS];
        private StatusEvent[] MedicationEvents;

        public MedicationPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Enabled;
            Medications = new Button[] { ADR1Button, ADR2Button, SodBicarbButton, DextroseButton,
                CellTransfusionButton, ADRviaETTButton, Surfactant120Button, Surfactant240Button };
            DoseViews = new TextBlock[] { ADR1Dose, ADR2Dose, SodBicarbDose, DextroseDose,
                CellTransfusionDose, ADRviaETTDose, Surfactant120Dose, Surfactant240Dose };
            NameViews = new TextBlock[] { ADR1View, ADR2View, SodBicarbView, DextroseView,
                CellTransfusionView, ADRviaETTView, Surfactant120View, Surfactant240View};
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // Take value from previous screen
            TimingCount = (Timing)e.Parameter;

            MedicationEvents = new StatusEvent[NUM_MEDICATIONS];

            // Reset DoseGiven and buttons' colours
            foreach (Button Medication in Medications)
            {
                Medication.Background = new SolidColorBrush(UNSELECTED_COLOUR);
            }
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            List<Event> Events = new List<Event>();

            List<StatusEvent> StatusEvents = new List<StatusEvent>();

            foreach (StatusEvent Event in MedicationEvents)
            {
                if (Event != null)
                {
                    StatusEvents.Add(Event);
                }
            }

            if (StatusEvents.Count > 0)
            {
                Frame.Navigate(typeof(Resuscitation), new EventAndTiming(TimingCount, Events, StatusEvents));
            }
        }

        private void DoseGiven_Click(object sender, RoutedEventArgs e)
        {
            Button selected = (Button)sender;

            int selectedIndex = Array.IndexOf(Medications, selected);

            SolidColorBrush Colour = selected.Background as SolidColorBrush;

            if (Colour.Color == UNSELECTED_COLOUR)
            {
                NumDoses[selectedIndex]++;
                MedicationEvents[selectedIndex] = GenerateStatusEvent(selectedIndex);
                selected.Background = new SolidColorBrush(SELECTED_COLOUR);
            }
            else {
                NumDoses[selectedIndex]--;
                MedicationEvents[selectedIndex] = null;
                selected.Background = new SolidColorBrush(UNSELECTED_COLOUR);
            }

            DoseViews[selectedIndex].Text = NumDoses[selectedIndex].ToString();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Resuscitation), TimingCount);
            ResetDoses();
        }

        // int index refers to the index of a Button in Button[] Medications
        private StatusEvent GenerateStatusEvent(int index)
        {
            return new StatusEvent("Medication Given", NameViews[index].Text + " (Dose " + NumDoses[index] + ")", TimingCount.Time);
        }

        private void ResetDoses()
        {
            for (int i = 0; i < NUM_MEDICATIONS; i++)
            {
                if (MedicationEvents[i] != null)
                {
                    NumDoses[i]--;
                    DoseViews[i].Text = NumDoses[i].ToString();
                }
            }
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
