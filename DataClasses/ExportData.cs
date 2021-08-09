using System.Text;
using Windows.UI.Xaml.Controls;

namespace Resuscitate.DataClasses
{
    class ExportData
    {
        public string ID { get; }
        public PatientData PatientData { get; }
        public StaffList StaffData { get; }
        public StatusList StatusList { get; }

        public ExportData(PatientData patientData, StaffList staffList, StatusList statusList)
        {
            ID = patientData.Id;
            PatientData = patientData;
            StaffData = staffList;
            StatusList = statusList;
        }

        public void ExportAsTextFile(Button exportButton, TextBlock flyout)
        {
            new TextFileExport().ExportResusData(ID, this, exportButton, flyout);
        }

        override public string ToString()
        {
            StringBuilder sb = new StringBuilder();

            // Generate Title
            string title = $"REPORT FOR RESUSCITATION OF PATIENT {ID}";

            sb.AppendLine(new string('#', title.Length + 6));
            sb.AppendLine("#" + new string(' ', title.Length + 4) + "#");
            sb.AppendLine("#  " + title + "  #");
            sb.AppendLine("#" + new string(' ', title.Length + 4) + "#");
            sb.AppendLine(new string('#', title.Length + 6) + "\n");

            // Generate Data
            sb.AppendLine(PatientData.ToString());
            sb.AppendLine();
            sb.AppendLine(StaffData.ToString());
            sb.AppendLine();
            sb.AppendLine(StatusList.ToString());

            return sb.ToString();
        }
    }
}
