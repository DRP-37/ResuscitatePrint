﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

namespace Resuscitate
{
    public class AudioRecorder
    {

        private MediaCapture _mediaCapture;
        private InMemoryRandomAccessStream _memoryBuffer;
        private String DEFAULT_AUDIO_FILENAME = "resuscitationNotesAudio";
        private String _fileName;

        public bool IsRecording { get; set; }

        public async void Record()
        {
            if (_memoryBuffer != null)
            {
                _memoryBuffer.Dispose();
            }
            _memoryBuffer = new InMemoryRandomAccessStream();
            if (_mediaCapture != null)
            {
                _mediaCapture.Dispose();
            }
            MediaCaptureInitializationSettings settings =
          new MediaCaptureInitializationSettings
          {
              StreamingCaptureMode = StreamingCaptureMode.Audio
          };
            _mediaCapture = new MediaCapture();
            await _mediaCapture.InitializeAsync(settings);
            await _mediaCapture.StartRecordToStreamAsync(
              MediaEncodingProfile.CreateMp3(AudioEncodingQuality.Auto), _memoryBuffer);
            IsRecording = true;
        }

        public async void StopRecording()
        {
            await _mediaCapture.StopRecordAsync();
            IsRecording = false;
            SaveAudioToFile();
        }

        private async void SaveAudioToFile()
        {
            IRandomAccessStream audioStream = _memoryBuffer.CloneStream();
            StorageFolder storageFolder = Package.Current.InstalledLocation;
            StorageFile storageFile = await storageFolder.CreateFileAsync(
              DEFAULT_AUDIO_FILENAME, CreationCollisionOption.GenerateUniqueName);
            this._fileName = storageFile.Name;
            using (IRandomAccessStream fileStream =
              await storageFile.OpenAsync(FileAccessMode.ReadWrite))
            {
                await RandomAccessStream.CopyAndCloseAsync(
                  audioStream.GetInputStreamAt(0), fileStream.GetOutputStreamAt(0));
                await audioStream.FlushAsync();
                audioStream.Dispose();
            }
        }

        public void Play()
        {
            MediaElement playbackMediaElement = new MediaElement();
            playbackMediaElement.SetSource(_memoryBuffer, "MP3");
            playbackMediaElement.Play();
        }

        public async Task PlayFromDisk(CoreDispatcher dispatcher)
        {
            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                MediaElement playbackMediaElement = new MediaElement();
                StorageFolder storageFolder = Package.Current.InstalledLocation;
                try
                {
                    StorageFile storageFile = await storageFolder.GetFileAsync(this._fileName);
                    IRandomAccessStream stream = await storageFile.OpenAsync(FileAccessMode.Read);
                    playbackMediaElement.SetSource(stream, storageFile.FileType);
                    playbackMediaElement.Play();
                } catch
                {
                    var dialog = new MessageDialog("Please record a note before playing");
                    await dialog.ShowAsync();
                }

            });
        }
    }
}
