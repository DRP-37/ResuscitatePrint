using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;

namespace Resuscitate.DataClasses
{
    class TextFileExport : Export
    {
        public async override void ExportStatusList(string patientId, ExportData data, Button exportButton, TextBlock flyout)
        {
            StorageFile file;
            bool fromFolder = false;

            StorageFolder storageFolder = await GetFileFromToken();

            if (storageFolder == null)
            {
                var savePicker = new Windows.Storage.Pickers.FileSavePicker();

                savePicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
                savePicker.FileTypeChoices.Add("Plain Text", new List<string>() { ".txt" });
                savePicker.SuggestedFileName = $"resus_{patientId}";

                file = await savePicker.PickSaveFileAsync();
            } else
            {
                file = await storageFolder.CreateFileAsync($"resus_{patientId}.txt", CreationCollisionOption.FailIfExists);
                fromFolder = true;
            }

            if (file == null)
            {
                if (fromFolder)
                {
                    flyout.Text = "File already exists.";
                } else { 
                    flyout.Text = "File not saved.";
                }

                FlyoutBase.ShowAttachedFlyout(exportButton);
                return;
            }

            // Prevent updates to the remote version of the file until
            //   we finish making changes and call CompleteUpdatesAsync.
            CachedFileManager.DeferUpdates(file);

            // write to file
            await FileIO.WriteTextAsync(file, data.ToString());

            // Let Windows know that we're finished changing the file so
            // the other app can update the remote version of the file.
            // Completing updates may require Windows to ask for user input.
            Windows.Storage.Provider.FileUpdateStatus status =
                await CachedFileManager.CompleteUpdatesAsync(file);

            if (status == Windows.Storage.Provider.FileUpdateStatus.Complete)
            {
                flyout.Text = "File " + file.Name + " was saved to\n" + getFolderPath(file);
                
            } else
            {
                flyout.Text = "File " + file.Name + " couldn't be saved.";
            }

            FlyoutBase.ShowAttachedFlyout(exportButton);
            exportButton.Background = new SolidColorBrush(COMPLETE_COLOUR);
        }

        static private async Task<StorageFolder> GetFileFromToken()
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

        private string getFolderPath(StorageFile file)
        {
            return file.Path.Substring(0, file.Path.Length - file.Name.Length);
        }
    }
}
