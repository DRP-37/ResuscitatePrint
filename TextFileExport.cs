using System;
using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;

namespace Resuscitate
{
    class TextFileExport : Export
    {
        private static readonly Color COMPLETE_COLOUR = InputUtils.DEFAULT_SELECTED_COLOUR;

        public async void ExportStatusList(string patientId, StatusList statusList, Button exportButton, TextBlock flyout)
        {
            var savePicker = new Windows.Storage.Pickers.FileSavePicker();

            savePicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            savePicker.FileTypeChoices.Add("Plain Text", new List<string>() { ".txt" });
            savePicker.SuggestedFileName = $"resus_{patientId}";

            Windows.Storage.StorageFile file = await savePicker.PickSaveFileAsync();

            if (file == null)
            {
                flyout.Text = "File not saved.";
                FlyoutBase.ShowAttachedFlyout(exportButton);
                return;
            }

            // Prevent updates to the remote version of the file until
            //   we finish making changes and call CompleteUpdatesAsync.
            Windows.Storage.CachedFileManager.DeferUpdates(file);

            // write to file
            await Windows.Storage.FileIO.WriteTextAsync(file, statusList.ToString());

            // Let Windows know that we're finished changing the file so
            // the other app can update the remote version of the file.
            // Completing updates may require Windows to ask for user input.
            Windows.Storage.Provider.FileUpdateStatus status =
                await Windows.Storage.CachedFileManager.CompleteUpdatesAsync(file);

            if (status == Windows.Storage.Provider.FileUpdateStatus.Complete)
            {
                flyout.Text = "File " + file.Name + " was saved.";
            } else
            {
                flyout.Text = "File " + file.Name + " couldn't be saved.";
            }

            FlyoutBase.ShowAttachedFlyout(exportButton);
            exportButton.Background = new SolidColorBrush(COMPLETE_COLOUR);
        }
    }
}
