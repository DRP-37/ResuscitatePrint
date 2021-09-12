using Microsoft.Toolkit.Uwp.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Windows.Storage;

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

        /* Stopwatch data (is null if not running) */
        public long LastApgarTime { get; set; } = 0;
        public long LastReassessmentTime { get; set; } = 0;
        public long? CPRStartTime { get; set; } = null;

        public ResuscitationData(Timing timing, PatientData patient, StaffList staff, string timeOfBirth)
        {
            this.IsComplete = true;

            this.TimingCount = timing;
            this.PatientData = patient;
            this.StaffList = staff;
            this.StatusList = StatusList.fromTimeOfBirth(timeOfBirth);
            this.MedicationDoses = new List<int>();

            /* 'Start' Stopwatches */
            this.LastReassessmentTime = Environment.TickCount;
            this.LastApgarTime = Environment.TickCount;

            PatientData.Tob = timeOfBirth;
            PatientData.DOB = DateTime.Now.ToString("dd/MM/yyyy");
        }

        /* Used on the InputTime page, where time of birth is unknown */
        public ResuscitationData(PatientData patient, StaffList staff)
        {
            this.IsComplete = false;

            this.PatientData = patient;
            this.StaffList = staff;
        }

        [JsonConstructor]
        private ResuscitationData(bool isComplete, Timing timingCount, PatientData patientData, StaffList staffList,
            StatusList statusList, List<int> medicationDoses, long lastReassessmentTime, long lastApgarTime, long? cPRStartTime)
        {
            this.IsComplete = isComplete;
            this.TimingCount = timingCount;
            this.PatientData = patientData;
            this.StaffList = staffList;
            this.StatusList = statusList;
            this.MedicationDoses = medicationDoses;

            this.LastReassessmentTime = lastReassessmentTime;
            this.LastApgarTime = lastApgarTime;
            this.CPRStartTime = cPRStartTime;
        }

        /* TIMER FUNCTIONS */

        public void StartNewReassessmentTimer()
        {
            LastReassessmentTime = Environment.TickCount;
        }

        public void StartNewApgarTimer()
        {
            LastApgarTime = Environment.TickCount;
        }

        public void StartNewCPRTimer()
        {
            CPRStartTime = Environment.TickCount;
        }

        public void StopCPRTimer()
        {
            CPRStartTime = null;
        }

        public bool CPRIsRunning()
        {
            return CPRStartTime != null;
        }

        public TimeSpan ReassessmentElapsed()
        {
            return TimeSpan.FromMilliseconds(Environment.TickCount - (long)LastReassessmentTime);
        }

        public TimeSpan ApgarElapsed()
        {
            return TimeSpan.FromMilliseconds(Environment.TickCount - (long)LastApgarTime);
        }

        public TimeSpan? CPRElapsed()
        {
            long? miliseconds = CPRElapsedMiliseconds();

            return miliseconds == null ? null : (TimeSpan?)TimeSpan.FromMilliseconds((long)miliseconds);
        }

        public long? CPRElapsedMiliseconds()
        {
            if (CPRStartTime == null)
            {
                return null;
            }

            return Environment.TickCount - (long)CPRStartTime;
        }

        /* STORAGE FUNCTIONS */

        public static LocalObjectStorageHelper GenerateStorageHelper()
        {
            return new LocalObjectStorageHelper(new JsonNetObjectSerializer());
        }

        public async void SaveLocally()
        {
            ApplicationDataContainer AppSettings = ApplicationData.Current.LocalSettings;

            var storageHelper = GenerateStorageHelper();

            await storageHelper.SaveFileAsync(STORAGE_KEY, this);
            AppSettings.Values[HAS_STORE_KEY] = true;
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
