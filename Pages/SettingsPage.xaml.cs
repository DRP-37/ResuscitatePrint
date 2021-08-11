using System;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Resuscitate.Pages
{
    public sealed partial class SettingsPage : Page
    {
        StorageFolder storageFolder;

        public SettingsPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ApplicationDataContainer AppSettings = ApplicationData.Current.LocalSettings;

            // Load settings
            if (AppSettings.Values.ContainsKey("exportPath"))
            {
                Path.Text = (string) AppSettings.Values["exportPath"];
            }

            if (AppSettings.Values.ContainsKey("hospitalName"))
            {
                HospitalName.Text = (string) AppSettings.Values["hospitalName"];
            }
            base.OnNavigatedTo(e);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }


        private void HospitalName_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            ApplicationDataContainer AppSettings = ApplicationData.Current.LocalSettings;

            // Store in app settings
            AppSettings.Values["hospitalName"] = HospitalName.Text;
            AppSettings.Values["exportPath"] = Path.Text;

            // Create a token for the storage folder to be able to access it
            // multiple times
            if (storageFolder != null)
            {
                string token = Guid.NewGuid().ToString();
                StorageApplicationPermissions.FutureAccessList.AddOrReplace(token, storageFolder);
                AppSettings.Values["exportToken"] = token;

            } else if (storageFolder == null && String.IsNullOrWhiteSpace(Path.Text))
            {
                AppSettings.Values["exportToken"] = "";
            }

            Frame.Navigate(typeof(MainPage));
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            // Use a folder picker to gain access to the file system 
            var picker = new Windows.Storage.Pickers.FolderPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;

            picker.FileTypeFilter.Add("*");

            storageFolder = await picker.PickSingleFolderAsync();

            if (storageFolder != null)
            {
                Path.Text = storageFolder.Path;
            } else
            {
                Path.Text = "";
            }
        }
    }
}