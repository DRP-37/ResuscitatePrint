using System;
using System.Collections.Generic;

namespace Resuscitate.DataClasses
{
    /* Stores data that will be saved if app is closed */
    public class ResuscitationData
    {
        public Timing TimingCount { get; }
        public PatientData PatientData { get; }
        public StaffList StaffList { get; }
        public StatusList StatusList { get; }

        /* Number of doses for each medication. If empty, it hasn't been set by visiting MedicationPage yet */
        public List<int> MedicationDoses { get; }

        public ResuscitationData(Timing timing, PatientData patient, StaffList staff, string timeOfBirth)
        {
            this.TimingCount = timing;
            this.PatientData = patient;
            this.StaffList = staff;
            this.StatusList = StatusList.fromTimeOfBirth(timeOfBirth);
            this.MedicationDoses = new List<int>();

            PatientData.Tob = timeOfBirth;
            PatientData.DOB = DateTime.Now.ToString("dd/MM/yyyy");
        }

        /* Used on the InputTime page, where time of birth is unknown*/
        public ResuscitationData(PatientData patient, StaffList staff)
        {
            this.PatientData = patient;
            this.StaffList = staff;
        }
    }
}
