using Windows.UI;
using Windows.UI.Xaml.Controls;

namespace Resuscitate.DataClasses
{
    abstract class Export
    {
        public abstract void ExportResusData(string patientId, ExportData data, Button exportButton, TextBlock flyout);
    }
}
