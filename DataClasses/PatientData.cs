using System;
using System.Collections.Generic;
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
        private string name = "Euan";
        private string dob = "";
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

            //var dialog = new MessageDialog("Connected");
            //await dialog.ShowAsync();

            DocumentReference dr = db.Collection("Data").Document(name);
            Dictionary<string, object> data = new Dictionary<string, object>();

            Dictionary<string, object> list = new Dictionary<string, object>
            {
                { "Name", name },
                { "Date of Birth", dob },
                { "Apgar Score", apgarStrings() }
            };
            data.Add("Data", list);
            await dr.SetAsync(list);

            //dialog = new MessageDialog("Data Added");
            //await dialog.ShowAsync();
        }

        // Data Functions

        public void setName(string name)
        {
            this.name = name;
        }

        public void setDOB(string dob)
        {
            this.dob = dob;
        }

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

        public string[] apgarStrings()
        {
            string[] apgarStrings = new string[5];
            for (int i=0; i < apgars.Count; i++)
            {
                if (apgars[i] == null)
                {
                    apgarStrings[i] = "N/A";
                } else {
                    apgarStrings[i] = apgars[i].ToString();
                }
            }
            return apgarStrings;
        }

    }
}
