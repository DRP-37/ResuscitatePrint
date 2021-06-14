﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Firebase.Auth;
using Google.Cloud.Firestore;
using Windows.UI.Popups;
using System.Security.Cryptography;
using System.Text;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Resuscitate
{

    public sealed partial class SignInPage : Page
    {
        private SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();

        private FirestoreDb db;
        string path = AppDomain.CurrentDomain.BaseDirectory + @"resuscitate-4c0ec-firebase-adminsdk-71nk1-71d3a47982.json";
        string project = "resuscitate-4c0ec";

        public SignInPage()
        {
            this.InitializeComponent();

        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame.CanGoBack)
            {
                rootFrame.GoBack();
            }
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // check credentials
            // * incorrect password
            var email = UsernameTextBox.Text;
            var password = sha1.ComputeHash(Encoding.ASCII.GetBytes(PasswordTextBox.Password));

            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
            FirestoreDb db = FirestoreDb.Create(project);
            CollectionReference users = db.Collection("Users");
            QuerySnapshot snapshot = await users.GetSnapshotAsync();
            foreach (DocumentSnapshot d in snapshot.Documents)
            {
                Dictionary<string, object> dict = d.ToDictionary();
                if ((string)dict["email"] == email && (string)dict["password"] == Encoding.UTF8.GetString(password))
                {
                    Frame.Navigate(typeof(ReviewDocsPage));
                } else
                {
                    var dialog = new MessageDialog("Incorrect email or password");
                    await dialog.ShowAsync();
                }
            }
        }

        private void RegisterButtonTextBlock_PointerPressed(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SignUpPage));
        }

        private async void checkSignIn(string email, byte[] password)
        {
            

        }
    }
}
