using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Resuscitate
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SignUpPage : Page
    {

        private SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
        private FirestoreDb db;
        string path = AppDomain.CurrentDomain.BaseDirectory + @"resuscitate2-47110-firebase-adminsdk-or0ak-c2c668d7ab.json";
        string project = "resuscitate2-47110";

        public SignUpPage()
        {
            this.InitializeComponent();
        }

        private async void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            // Password does not match up
            var email = UsernameTextBox.Text;

            if (PasswordTextBox.Password != PasswordTextBox_Copy.Password)
            {
                var dialog = new MessageDialog("Passwords do not match");
                await dialog.ShowAsync();
            }
            else
            {
                var password = sha1.ComputeHash(Encoding.ASCII.GetBytes(PasswordTextBox.Password));
                signUp(email, password);

                // Account with this email exists already
                Frame.Navigate(typeof(ReviewDocsPage));
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame.CanGoBack)
            {
                rootFrame.GoBack();
            }
        }

        private async void signUp(string email, byte[] password)
        {
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);

            db = FirestoreDb.Create(project);

            DocumentReference dr = db.Collection("Users").Document(email);
            Dictionary<string, object> data = new Dictionary<string, object>();

            Dictionary<string, object> list = new Dictionary<string, object>
            {
                { "email", email },
                { "password", Encoding.UTF8.GetString(password) }
            };

            data.Add("Data", list);
            await dr.SetAsync(list);
        }
    }
}
