using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Resuscitate.DataClasses
{
    class Medication
    {
        public string Name { get; }
        public Button Button { get; }
        public TextBlock DoseView { get; }

        public StatusEvent StatusEvent { get; set; }

        public int NumDoses { get; set; }

        public Medication(string name, Button button, TextBlock view)
        {
            this.Name = name;
            this.Button = button;
            this.DoseView = view;

            this.NumDoses = 0;
            this.StatusEvent = null;
        }

        public int getNumDoses()
        {
            return NumDoses;
        }

        public void setNumDoses(int numDoses)
        {
            NumDoses = numDoses;
            DoseView.Text = NumDoses.ToString();
        }

        public void incrementDose()
        {
            NumDoses++;
            DoseView.Text = NumDoses.ToString();
        }

        public void decrementDose()
        {
            NumDoses--;
            DoseView.Text = NumDoses.ToString();
        }

        public void resetMedication(bool hasSavedDose)
        {
            if (!hasSavedDose)
            {
                decrementDose();
            }

            this.StatusEvent = null;
        }
    }
}
