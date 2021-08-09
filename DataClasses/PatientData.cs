using System.Collections.Generic;
using System.Text;

namespace Resuscitate.DataClasses
{
    public class PatientData
    {
        public bool isComplete { get; set; } = false;

        // Patient Data
        public string Surname { get; set; } = "";
        public string Id { get; set; } = "";
        public string DOB { get; set; } = "";
        public string Tob { get; set; } = "";
        public string Sex { get; set; } = "";
        public string Gestation { get; set; } = "";
        public string Weight { get; set; } = "";
        public string History { get; set; } = "";

        private List<Notes> notes = new List<Notes>();

        public void addNote(Notes note)
        {
            notes.Add(note);
        }

        override public string ToString()
        {
            StringBuilder sb = new StringBuilder("Patient Information:\n\n");

            sb.Append("Surname:\t\t");
            AppendToBuilder(Surname, sb);
            sb.Append("ID:\t\t\t");
            AppendToBuilder(Id, sb);
            sb.Append("Date of Birth:\t\t");
            AppendToBuilder(DOB, sb);
            sb.Append("Time of Birth:\t\t");
            AppendToBuilder(Tob, sb);
            sb.Append("Sex:\t\t\t");
            AppendToBuilder(Sex, sb);
            sb.Append("Gestation Period:\t");
            AppendToBuilder(Gestation, sb);
            sb.Append("Est. Weight:\t\t");
            AppendToBuilder(Weight + "kg", sb);
            sb.Append("\nMedical History:\n");
            AppendToBuilder(History, sb);

            for (int i = 0; i < notes.Count; i++)
            {
                sb.AppendLine($"\nNote {i + 1}:");
                sb.AppendLine(notes[i].ToString());
            }

            return sb.ToString();
        }

        private void AppendToBuilder(string item, StringBuilder sb)
        {
            if (string.IsNullOrWhiteSpace(item))
            {
                sb.AppendLine("N/A");
            } else
            {
                sb.AppendLine(item);
            }
        }
    }
}
