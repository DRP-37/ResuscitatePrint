﻿using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Resuscitate.DataClasses;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Microsoft.Toolkit.Uwp.Helpers;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Resuscitate
{
    public sealed partial class MainPage : Page
    {
        private const string HAS_STORE_KEY = ResuscitationData.HAS_STORE_KEY;

        private bool HasLocalStore;
        private static ResuscitationData StoredData;

        public MainPage()
        {
            this.InitializeComponent();

            ApplicationDataContainer AppSettings = ApplicationData.Current.LocalSettings;

            loadStorage();
            HasLocalStore = AppSettings.Values[HAS_STORE_KEY] != null && (bool) AppSettings.Values[HAS_STORE_KEY];

            if (!HasLocalStore)
            {
                ReturnButton.IsEnabled = false;
            }
        } 

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            // Go to main page
            this.Frame.Navigate(typeof(InputTime));
        }

        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            if (HasLocalStore)
            {
                if (StoredData.IsComplete)
                {
                    this.Frame.Navigate(typeof(Resuscitation), StoredData);
                } else
                {
                    this.Frame.Navigate(typeof(InputTime), StoredData);
                }
            }
        }
        
        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SettingsPage));
        }

        public async static void loadStorage()
        {
            LocalObjectStorageHelper storageHelper = ResuscitationData.GenerateStorageHelper();

            if (await storageHelper.FileExistsAsync(ResuscitationData.STORAGE_KEY))
            {
                StoredData = await storageHelper.ReadFileAsync<ResuscitationData>(ResuscitationData.STORAGE_KEY);
            }
        }
    }
}
