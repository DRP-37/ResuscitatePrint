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
