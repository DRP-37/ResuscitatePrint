using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Cloud.Firestore;
using Windows.UI.Popups;

namespace Resuscitate
{
    class Data
    {
        private FirestoreDb db;

        public void sendToFirestore()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"resuscitate-4c0ec-firebase-adminsdk-71nk1-71d3a47982.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);

            string project = "resuscitate-4c0ec";
            db = FirestoreDb.Create(project);

            var dialog = new MessageDialog("Success");
            await dialog.ShowAsync();

            CollectionReference cr = db.Collection("Data");
            Dictionary<string, object> user = new Dictionary<string, object>
            {
                { "First", "Ada" },
                { "Last", "Lovelace" },
                { "Born", 1815 }
            };
            await cr.AddAsync(user);

            dialog = new MessageDialog("Data Added");
            await dialog.ShowAsync();
        }
    }
}
