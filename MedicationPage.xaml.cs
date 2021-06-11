using Resuscitate.DataClasses;
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
    public sealed partial class MedicationPage : Page
    {
        private static int NUM_MEDICATIONS = 8;
        
        public Timing TimingCount { get; set; }

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
        private bool[] DoseGiven = new bool[NUM_MEDICATIONS];

        private Medication medication;
        private StatusEvent statusEvent;

        public MedicationPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Enabled;
            Medications = new Button[] { ADR1Button, ADR2Button, SodBicarbButton, DextroseButton,
                CellTransfusionButton, ADRviaETTButton, Surfactant120Button, Surfactant240Button };
            DoseViews = new TextBlock[] { ADR1Dose, ADR2Dose, SodBicarbDose, DextroseDose,
                CellTransfusionDose, ADRviaETTDose, Surfactant120Dose, Surfactant240Dose };
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Take value from previous screen
            TimingCount = (Timing)e.Parameter;

            base.OnNavigatedTo(e);

            medication = new Medication();
            statusEvent = new StatusEvent();

            // Reset DoseGiven and buttons' colours
            foreach (Button Medication in Medications)
            {
                Medication.Background = new SolidColorBrush(Colors.White);
            }

            DoseGiven = new bool[NUM_MEDICATIONS];
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectionMade(Medications))
            {
                medication.Time = TimingCount;
                medication.setData(DoseGiven);

                statusEvent.Name = "Medication Given";
                statusEvent.Data = "";
                statusEvent.Time = medication.Time.ToString();
                statusEvent.Event = medication;

                Frame.Navigate(typeof(Resuscitation), TimingCount);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            ResetDoses();

            Frame.Navigate(typeof(Resuscitation), TimingCount);
        }

        private void TimeView_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        private void DoseGiven_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            if (button == null)
                return;

            int reference = Array.IndexOf(Medications, button);

            SolidColorBrush Colour = button.Background as SolidColorBrush;

            if (Colour.Color == Colors.White)
            {
                NumDoses[reference]++;
                DoseGiven[reference] = true;
                button.Background = new SolidColorBrush(Colors.LightGreen);
            } else
            {
                NumDoses[reference]--;
                DoseGiven[reference] = false;
                button.Background = new SolidColorBrush(Colors.White);
            }

            DoseViews[reference].Text = NumDoses[reference].ToString();
        }

        private void ResetDoses()
        {
            for (int i = 0; i < NUM_MEDICATIONS; i++)
            {
                if (DoseGiven[i])
                {
                    NumDoses[i]--;
                    DoseViews[i].Text = NumDoses[i].ToString();
                }
            }
        }

        // Move to its own class later on - needed it many classes
        private bool SelectionMade(Button[] buttons)
        {
            foreach (Button button in buttons)
            {
                SolidColorBrush colour = button.Background as SolidColorBrush;

                if (colour.Color == Colors.LightGreen)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
