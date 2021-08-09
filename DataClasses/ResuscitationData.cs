using System;

namespace Resuscitate.DataClasses
{
    public class ResuscitationData
    {
        public bool isAllData { get; }

        public Timing TimingCount { get; }
        public PatientData PatientData { get; }
        public StaffList StaffList { get; }
        public StatusList StatusList { get; }

        public ResuscitationData(Timing timing, PatientData patient, StaffList staff, string timeOfBirth)
        {
            this.isAllData = true;
            this.TimingCount = timing;
            this.PatientData = patient;
            this.StaffList = staff;
            this.StatusList = StatusList.fromTimeOfBirth(timeOfBirth);

            PatientData.Tob = timeOfBirth;
            PatientData.DOB = DateTime.Now.ToString("dd/MM/yyyy");
        }

        public ResuscitationData(PatientData patient, StaffList staff)
        {
            this.isAllData = false;
            this.PatientData = patient;
            this.StaffList = staff;
        }
    }
}
