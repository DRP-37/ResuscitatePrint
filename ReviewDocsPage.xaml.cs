using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Storage;
using Firebase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;
using FireSharp.Config;
using FireSharp.Response;
using FireSharp.Interfaces;
using Google.Cloud.Firestore;
using Resuscitate.DataClasses;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Resuscitate
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ReviewDocsPage : Page
    {

        ExportList ExportList = new ExportList();
        private FirestoreDb db;
        string path = AppDomain.CurrentDomain.BaseDirectory + @"resuscitate-4c0ec-firebase-adminsdk-71nk1-71d3a47982.json";
        string project = "resuscitate-4c0ec";

        public ReviewDocsPage()
        {
            this.InitializeComponent();

            // This line would this page to cache everything while ignoring the reset when navigating
            //      to the main page from the review page. Use if needed:
            // this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
            db = FirestoreDb.Create(project);

            ExportListView.MaxHeight = ((Frame)Window.Current.Content).ActualHeight - 325;

            populateExportList();
        }

        private async void populateExportList()
        {
            Query allFiles = db.Collection("Data");
            QuerySnapshot allFilesSnapshot = await allFiles.GetSnapshotAsync();
            foreach (DocumentSnapshot documentSnapshot in allFilesSnapshot.Documents)
            {
                Dictionary<string, object> procedure = documentSnapshot.ToDictionary();
                string id = (string)procedure["Id"];
                string timeOfBirth = (string)procedure["TimeOfBirth"];
                string dateOfBirth = (string)procedure["DateOfBirth"];
                ExportList.Data.Add(new ExportData(id, $"{dateOfBirth} {timeOfBirth}"));
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SignInPage));
        }

        private async void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            Button selected = (Button)sender;
            string ID = selected.Tag.ToString();

            // Use ID to download text file
            DocumentReference docRef = db.Collection("Data").Document(ID);
            DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
            FirebaseDataStructure patient = snapshot.ConvertTo<FirebaseDataStructure>();

            if (snapshot.Exists)
            {
                Dictionary<string, object> patientInfo = snapshot.ToDictionary();
                Exporter.exportFile(ID, patient.ToString());
            }
        }
    }
}
