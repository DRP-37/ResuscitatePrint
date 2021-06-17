using System;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Resuscitate
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ReviewDocsPage : Page
    {

        ExportList ExportList = new ExportList();

        public ReviewDocsPage()
        {
            this.InitializeComponent();

            // This line would this page to cache everything while ignoring the reset when navigating
            //      to the main page from the review page. Use if needed:
            // this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ExportListView.MaxHeight = ((Frame)Window.Current.Content).ActualHeight - 325;

            ExportList.Data.Add(new ExportData("1097387", "10:10"));
            ExportList.Data.Add(new ExportData("1097387", "10:10"));
            ExportList.Data.Add(new ExportData("1097387", "10:10"));
            ExportList.Data.Add(new ExportData("1097387", "10:10"));
            ExportList.Data.Add(new ExportData("1097387", "10:10"));
            ExportList.Data.Add(new ExportData("1097387", "10:10"));
            ExportList.Data.Add(new ExportData("1097387", "10:10"));
            ExportList.Data.Add(new ExportData("1097387", "10:10"));
            ExportList.Data.Add(new ExportData("1097387", "10:10"));
            ExportList.Data.Add(new ExportData("1097387", "10:10"));
            ExportList.Data.Add(new ExportData("1097387", "10:10"));
            ExportList.Data.Add(new ExportData("1097387", "10:10"));
            ExportList.Data.Add(new ExportData("1097387", "10:10"));
            ExportList.Data.Add(new ExportData("1097387", "10:10"));
            ExportList.Data.Add(new ExportData("1097387", "10:10"));
            ExportList.Data.Add(new ExportData("1097387", "10:10"));
            ExportList.Data.Add(new ExportData("1097387", "10:10"));
            ExportList.Data.Add(new ExportData("1097387", "10:10"));
            ExportList.Data.Add(new ExportData("1097387", "10:10"));
            ExportList.Data.Add(new ExportData("1097387", "10:10"));
            ExportList.Data.Add(new ExportData("1097387", "10:10"));
            ExportList.Data.Add(new ExportData("1097387", "10:10"));
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SignInPage));
        }

        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            Button selected = (Button)sender;
            string ID = selected.Tag.ToString();

            // Use ID to download text file
        }
    }
}
