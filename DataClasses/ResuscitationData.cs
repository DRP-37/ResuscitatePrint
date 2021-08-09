using Microsoft.Toolkit.Uwp.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Resuscitate.DataClasses
{
    /* Stores data that will be saved if app is closed */
    public class ResuscitationData
    {
        internal const string HAS_STORE_KEY = "hasDataStore";
        internal const string STORAGE_KEY = "dataStore";

        public bool IsComplete { get; }

        public Timing TimingCount { get; }
        public PatientData PatientData { get; }
        public StaffList StaffList { get; }
        public StatusList StatusList { get; }

        /* Number of doses for each medication. If empty, it hasn't been set by visiting MedicationPage yet */
        public List<int> MedicationDoses { get; }

        public ResuscitationData(Timing timing, PatientData patient, StaffList staff, string timeOfBirth)
        {
            this.IsComplete = true;

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
            this.IsComplete = false;

            this.PatientData = patient;
            this.StaffList = staff;
        }

        [JsonConstructor]
        private ResuscitationData(bool isComplete, Timing timingCount, PatientData patientData, StaffList staffList,
            StatusList statusList, List<int> medicationDoses)
        {
            this.IsComplete = isComplete;
            this.TimingCount = timingCount;
            this.PatientData = patientData;
            this.StaffList = staffList;
            this.StatusList = statusList;
            this.MedicationDoses = medicationDoses;
        }

        public static LocalObjectStorageHelper GenerateStorageHelper()
        {
            return new LocalObjectStorageHelper(new JsonNetObjectSerializer());
        }

        public async void SaveLocally()
        {
            var storageHelper = GenerateStorageHelper();

            await storageHelper.SaveFileAsync(STORAGE_KEY, this);
            MainPage.AppSettings.Values[HAS_STORE_KEY] = true;
        }

        /* Used for storage of ResuscitationData */
        private class JsonNetObjectSerializer : IObjectSerializer
        {
            // Specify your serialization settings
            private readonly JsonSerializerSettings settings = new JsonSerializerSettings();

            public object Serialize<T>(T value) => JsonConvert.SerializeObject(value, typeof(T), Formatting.Indented, settings);

            public T Deserialize<T>(object value) => JsonConvert.DeserializeObject<T>((string)value, settings);
        }
    }
}
