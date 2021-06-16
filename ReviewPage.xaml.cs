﻿using Resuscitate.DataClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Graphics.Printing;
using Windows.Storage;
using System.Threading.Tasks;
using Windows.Storage.AccessCache;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Resuscitate
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ReviewPage : Page
    {
        public PatientData patientData;
        public Timing TimingCount;
        private StatusList StatusList;

        public ReviewPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Take value from previous screen
            var RDaT = (ReviewDataAndTiming)e.Parameter;
            patientData = RDaT.PatientData;
            TimingCount = RDaT.Timing;

            if (RDaT.StatusList != null)
            {
                StatusList = RDaT.StatusList;
            }

            PatientInfo.Background = MainPage.patienInformationComplete ? new SolidColorBrush(Colors.LightGreen) :
                new SolidColorBrush(Colors.Red);

            this.NavigationCacheMode = NavigationCacheMode.Enabled;
            base.OnNavigatedTo(e);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame.CanGoBack)
            {
                rootFrame.GoBack();
            }
        }

        private void FinishButton_Click(object sender, RoutedEventArgs e)
        {
            TimingCount.Stop();

            patientData.sendToFirestore();

            // Send data to the firestore
            // This will be replaced with the actual patient data being passed around.
            //PatientData patientData = new PatientData();
            //patientData.sendToFirestore();

            // Clear cache stored locally
            var frame = Window.Current.Content as Frame;
            if (frame != null)
            {
                var cacheSize = ((frame)).CacheSize;
                ((frame)).CacheSize = 0;
                ((frame)).CacheSize = cacheSize;
            }

            this.Frame.Navigate(typeof(MainPage));
        }

        private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {
            // Nothing
        }

        private void TimeView_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Nothing
        }

        private void PatientInfo_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(PatientPage), new ReviewDataAndTiming(TimingCount, null, patientData));
        }

        private void StaffInfo_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(StaffPage), TimingCount);
        }

        private async void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            // Create sample file with the ID of the patient; replace if exists.
            StorageFolder storageFolder = await GetFileFromToken();
            if (storageFolder == null)
            {
                var picker = new Windows.Storage.Pickers.FolderPicker();
                picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
                picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;

                picker.FileTypeFilter.Add("*");

                storageFolder = await picker.PickSingleFolderAsync();
            }

            StorageFile sampleFile = 
                await storageFolder.CreateFileAsync("sample.txt", CreationCollisionOption.ReplaceExisting);
            String doc = "Resuscitation Report for patient: " + patientData.Id + "\n";
            foreach (StatusEvent statEvent in StatusList.Events)
            {
                doc += statEvent.Event + " " + statEvent.Data + " " + statEvent.Time + ";\n";
            }
            await Windows.Storage.FileIO.WriteTextAsync(sampleFile, doc);

            ExportButton.Background = new SolidColorBrush(Colors.LightGreen);
            // DEBUG 
            // Storage­File.Get­File­From­Application­Uri­Async. - get from URI address
            // Directory.GetCurrentDirectory 
            // if a path is not selected by user default to installation path of the app
            System.Diagnostics.Debug.WriteLine(String.Format("File is located at {0}", sampleFile.Path.ToString()));
        }

        public async Task<StorageFolder> GetFileFromToken()
        {
            string token = "";
            if (MainPage.AppSettings.Values.ContainsKey("exportToken"))
            {
                token = (String)MainPage.AppSettings.Values["exportToken"];
            }

            if (token == "") return null;

            if (!StorageApplicationPermissions.FutureAccessList.ContainsItem(token)) return null;
            return await StorageApplicationPermissions.FutureAccessList.GetFolderAsync(token);
        }
    }
}
