using System;
using System.Collections.Generic;
using Google.Cloud.Firestore;
using Windows.UI.Popups;

namespace Resuscitate
{
    class Data
    {
        private FirestoreDb db;
        string path = AppDomain.CurrentDomain.BaseDirectory + @"resuscitate-4c0ec-firebase-adminsdk-71nk1-4884e36e03.json";
        string project = "resuscitate-4c0ec";

        string name = "Euan";
        string dob = "";

        ApgarScore[] apgars = new ApgarScore[5];
        int currentApgar = 0;

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
            apgars[currentApgar] = apgar;
            currentApgar++;
        }

        public string[] apgarStrings()
        {
            string[] apgarStrings = new string[5];
            for (int i = 0; i < 5; i++)
            {
                if (apgars[i] == null)
                {
                    apgarStrings[i] = "N/A";
                }
                else
                {
                    apgarStrings[i] = apgars[i].ToString();
                }
            }
            return apgarStrings;
        }

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
                { "Apgar Score", apgarStrings() }
            };
            data.Add("Data", list);
            await dr.SetAsync(list);

            dialog = new MessageDialog("Data Added");
            await dialog.ShowAsync();

            checkData();
        }

        public async void checkData()
        {
            DocumentReference dr = db.Collection("Data").Document(name);
            DocumentSnapshot snap = await dr.GetSnapshotAsync();

            if (snap.Exists)
            {
                Dictionary<string, object> item = snap.ToDictionary();

                foreach (var field in item)
                {
                    var dialog = new MessageDialog(field.Key);
                    await dialog.ShowAsync();
                }
            }

        }
    }
}