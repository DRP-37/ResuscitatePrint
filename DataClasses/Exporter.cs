using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.UI.Xaml.Media;

namespace Resuscitate.DataClasses
{
    class Exporter
    {
        static public async void exportFile(string id, string doc)
        {
            StorageFolder storageFolder = await GetFileFromToken();
            if (storageFolder == null)
            {
                var picker = new Windows.Storage.Pickers.FolderPicker();
                picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
                picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;

                picker.FileTypeFilter.Add("*");

                storageFolder = await picker.PickSingleFolderAsync();
            }

            if (storageFolder == null)
            {
                // Pop Up you need to choose folder
                return;
            }

            StorageFile sampleFile =
                await storageFolder.CreateFileAsync($"{id}_resuscitation.txt", CreationCollisionOption.ReplaceExisting);
            
            await FileIO.WriteTextAsync(sampleFile, doc);

            // DEBUG 
            // Storage­File.Get­File­From­Application­Uri­Async. - get from URI address
            // Directory.GetCurrentDirectory 
            // if a path is not selected by user default to installation path of the app
            System.Diagnostics.Debug.WriteLine(String.Format("File is located at {0}", sampleFile.Path.ToString()));
        }

        static public async Task<StorageFolder> GetFileFromToken()
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
