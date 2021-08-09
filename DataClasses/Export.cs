using Windows.UI;
using Windows.UI.Xaml.Controls;

namespace Resuscitate.DataClasses
{
    abstract class Export
    {
        public static readonly Color COMPLETE_COLOUR = InputUtils.DEFAULT_SELECTED_COLOUR;

        public abstract void ExportStatusList(string patientId, ExportData data, Button exportButton, TextBlock flyout);
    }
}
