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
    public sealed partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Path.Text = Directory.GetCurrentDirectory();
            base.OnNavigatedTo(e);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void Path_TextChanged(object sender, TextChangedEventArgs e)
        {
            MainPage.exportPath = (sender as TextBox).Text;
        }

        private void HospitalName_TextChanged(object sender, TextChangedEventArgs e)
        {
            MainPage.hospitalName = (sender as TextBox).Text;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(Path.Text))
            {
                MainPage.exportPath = Directory.GetCurrentDirectory();
            }
        }
    }
}
