using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Firebase.Storage;
using Google.Cloud.Firestore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Windows.UI.Popups;

namespace Resuscitate.DataClasses
{
    class PatientData
    {
        private FirestoreDb db;
        string path = AppDomain.CurrentDomain.BaseDirectory + @"resuscitate-4c0ec-firebase-adminsdk-71nk1-71d3a47982.json";
        string project = "resuscitate-4c0ec";

        // Patient Data
        private string name;
        private string dob;
        public string Name { get => name; set => name = value; }
        public string DOB { get => dob; set => dob = value; }
        private List<ApgarScore> apgars = new List<ApgarScore>();
        private List<AirwayPositioning> positionings = new List<AirwayPositioning>();
        private List<Observation> observations = new List<Observation>();
        private InitialAssessment initialAssessment;
        private List<Reassessment> reassessments = new List<Reassessment>();
        private List<OtherProcedures> procedures = new List<OtherProcedures>();
        private List<IntubationAndSuction> intubationAndSuctions = new List<IntubationAndSuction>();
        private List<CardiacCompressions> compressions = new List<CardiacCompressions>();
        private List<LineInsertion> insertions = new List<LineInsertion>();
        private List<Notes> notes = new List<Notes>();

        // Database Functions
        public async void sendToFirestore()
        {
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);

            db = FirestoreDb.Create(project);

            var dialog = new MessageDialog("Connected");
            await dialog.ShowAsync();

            DocumentReference dr = db.Collection("Data").Document(name);
            Dictionary<string, object> data = new Dictionary<string, object>();


            Dictionary<string, object> list = new Dictionary<string, object>
            {
                { "Name", name },
                { "Date of Birth", dob },
                { "Initial Assessment", initialAssessment.ToString() },
                { "Apgar Scores", listToStrings(apgars) },
                { "Observations",  listToStrings(observations) },
                { "Airway Positioning",  listToStrings(positionings) },
                { "Reassessments",  listToStrings(reassessments) },
                { "Other Procedures",  listToStrings(procedures) },
                { "Intubation & Suction",  listToStrings(intubationAndSuctions) },
                { "Compressions",  listToStrings(compressions) },
                { "Insertions",  listToStrings(insertions) },
                { "Notes",  listToStrings(notes) },
            };

            data.Add("Data", list);
            await dr.SetAsync(list);

            //dialog = new MessageDialog("Data Added");
            //await dialog.ShowAsync();
        }

        public async void sendToStorage(string fileName)
        {
            var stream = File.Open(fileName, FileMode.Open);

            var task = new FirebaseStorage("gs://resuscitate-4c0ec.appspot.com")
                .Child("Resuscitation Recordings")
                .Child(name)
                .Child(fileName)
                .PutAsync(stream);

            var downloadUrl = await task;

            var dialog = new MessageDialog("File uploaded");
            await dialog.ShowAsync();
        }

        // Data Functions
        public void addApgar(ApgarScore apgar)
        {
            apgars.Add(apgar);
        }

        public void addAirwayPos(AirwayPositioning pos) {
            positionings.Add(pos);
        }

        public void addObservation(Observation obs) {
            observations.Add(obs);
        }

        public void addInitialAssessment(InitialAssessment assess) {
            initialAssessment = assess;
        }

        public void addReassessment(Reassessment assess) {
            reassessments.Add(assess);
        }

        public void addProcedure(OtherProcedures proc) {
            procedures.Add(proc);
        }

        public void addIAndS(IntubationAndSuction intubation)
        {
            intubationAndSuctions.Add(intubation);
        }

        public void addCompression(CardiacCompressions compression) {
            compressions.Add(compression);
        }

        public void addInsertions(LineInsertion ins) {
            insertions.Add(ins);
        }

        public void addNote(Notes note) {
            notes.Add(note);
        }

        public string[] listToStrings(IEnumerable<Event> eItems)
        {
            List<Event> items = eItems.ToList();

            if (items.Count == 0)
            {
                return new string[] { "N/A" };
            }

            string[] listOfStrings = new string[items.Count];
            for (int i = 0; i < items.Count; i++)
            {
                if (apgars[i] == null)
                {
                    listOfStrings[i] = "N/A";
                }
                else
                {
                    try
                    {
                        listOfStrings[i] = items[i].ToString();
                    }
                    catch (Exception e)
                    {
                        listOfStrings[i] = "N/A";
                    }
                }
            }
            return listOfStrings;
        }

    }
}
