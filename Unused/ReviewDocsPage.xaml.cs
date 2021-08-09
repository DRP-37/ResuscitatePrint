using System;
using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Resuscitate
{
    public sealed partial class ReviewDocsPage : Page
    {
        ExportList ExportList = new ExportList();
        List<String> Approved = new List<string>();
        List<Button> selectedButtons = new List<Button>();

        /*
        private FirestoreDb db;
        CollectionReference collection;
        FirestoreChangeListener listener;
        string path = AppDomain.CurrentDomain.BaseDirectory + @"resuscitate2-47110-firebase-adminsdk-or0ak-c2c668d7ab.json";
        string project = "resuscitate2-47110";
        */

        DispatcherTimer dispatcherTimer;
        public ReviewDocsPage()
        {
            this.InitializeComponent();
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = new TimeSpan(10000);
            dispatcherTimer.Start();
            // This line would this page to cache everything while ignoring the reset when navigating
            //      to the main page from the review page. Use if needed:
            // this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            /*
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
            db = FirestoreDb.Create(project);
            populateExportList();
            collection = db.Collection("Data");
            listener = collection.Listen(snapshot =>
            {
                populateExportList();
            });

            ExportListView.MaxHeight = ((Frame)Window.Current.Content).ActualHeight - 325;
            */
        }

        private async void populateExportList()
        {
            /*
            Query allFiles = db.Collection("Data");
            QuerySnapshot allFilesSnapshot = await allFiles.GetSnapshotAsync();
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                foreach (DocumentSnapshot documentSnapshot in allFilesSnapshot.Documents)
                {
                    Dictionary<string, object> procedure = documentSnapshot.ToDictionary();
                    string id = (string)procedure["Id"];
                    string timeOfBirth = (string)procedure["TimeOfBirth"];
                    string dateOfBirth = (string)procedure["DateOfBirth"];
                    var data = new ExportData(id, $"{dateOfBirth} {timeOfBirth}");

                    if (!ExportList.Data.Contains(data))
                    {
                        ExportList.Data.Add(data);
                    }
                }
            });
            */
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            dispatcherTimer.Stop();
            Frame.Navigate(typeof(SignInPage));

        }


        private async void ApproveButton_Click(object sender, RoutedEventArgs e) {
            /*
            Button selected = (Button)sender;
            string ID = selected.Tag.ToString();
            DocumentReference docRef = db.Collection("Data").Document(ID);
            DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
            FirebaseDataStructure patient = snapshot.ConvertTo<FirebaseDataStructure>();
            bool alreadyApproved = (patient.Approved == "True");

            if (alreadyApproved)
            {
                selected.Content = "Approved";
                selected.Background = new SolidColorBrush(Colors.DarkGray);
            }
            else
            {
                if (!selectedButtons.Contains(selected))
                {
                    
                    selectedButtons.Add(selected);
                    selected.Background = new SolidColorBrush(Colors.LightGreen);
                    Approved.Add(ID);
                }
                else
                {
                    selectedButtons.Remove(selected);
                    selected.Background = new SolidColorBrush(Color.FromArgb(51, 188, 188, 188));
                    Approved.Remove(ID);
                }
            }
            */
        }

        private async void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            /*
            Button selected = (Button)sender;

            string ID = selected.Tag.ToString();

            // Use ID to download text file
            DocumentReference docRef = db.Collection("Data").Document(ID);
            DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
            FirebaseDataStructure patient = snapshot.ConvertTo<FirebaseDataStructure>();

            if (snapshot.Exists)
            {
                //Dictionary<string, object> patientInfo = snapshot.ToDictionary();
                Exporter.exportFile(ID, patient.ToString());
            }
            */
        }

        private async void Confirm_Button_Click(object sender, RoutedEventArgs e)
        {
            /*
            DocumentReference docRef;
            foreach (var id in Approved)
            {
                docRef = db.Collection("Data").Document(id);
                await docRef.UpdateAsync("Approved", "True");
                ResetButtonColors();
            }
            */
        }

        private void ResetButtonColors() {
            foreach (var button in selectedButtons)
            {
                button.Background = new SolidColorBrush(Color.FromArgb(51, 188, 188, 188));
            }
        }
    }
}
