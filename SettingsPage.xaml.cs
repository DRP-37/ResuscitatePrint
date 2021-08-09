using System;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Resuscitate
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
            // Load settings
            if (MainPage.AppSettings.Values.ContainsKey("exportPath"))
            {
                Path.Text = (String)MainPage.AppSettings.Values["exportPath"];
            }

            if (MainPage.AppSettings.Values.ContainsKey("hospitalName"))
            {
                HospitalName.Text = ((String)MainPage.AppSettings.Values["hospitalName"]);
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
            // Store in app settings
            MainPage.AppSettings.Values["hospitalName"] = HospitalName.Text;
            MainPage.AppSettings.Values["exportPath"] = Path.Text;


            // Create a token for the storage folder to be able to access it
            // multiple times
            if (storageFolder != null)
            {
                string token = Guid.NewGuid().ToString();
                StorageApplicationPermissions.FutureAccessList.AddOrReplace(token, storageFolder);
                MainPage.AppSettings.Values["exportToken"] = token;
            } else if (storageFolder == null && String.IsNullOrWhiteSpace(Path.Text))
            {
                MainPage.AppSettings.Values["exportToken"] = "";
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