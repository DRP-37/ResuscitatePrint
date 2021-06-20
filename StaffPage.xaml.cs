using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
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
    public sealed partial class StaffPage : Page
    {
        private StaffList StaffList = new StaffList();
        private List<StaffMemberData> ItemsAdded;

        public StaffPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ItemsAdded = new List<StaffMemberData>();
            StaffName.Text = "";
            StaffPosition.SelectedIndex = -1;
            StaffGrade.SelectedIndex = -1;
            arrivalTimePicker.Time = DateTime.Now.TimeOfDay;

            // can take a Timing or nothing I think
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            // not yet implemented ig

            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame.CanGoBack)
            {
                rootFrame.GoBack();
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (StaffMemberData staffData in ItemsAdded)
            {
                StaffList.Members.Remove(staffData);
            }

            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame.CanGoBack)
            {
                rootFrame.GoBack();
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(StaffName.Text) || StaffPosition.SelectedIndex < 0 
                || StaffGrade.SelectedIndex < 0)
            {
                FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
                AddButton.Background = new SolidColorBrush(Colors.Red);
                AddButton.BorderBrush = new SolidColorBrush(Colors.Red);
                return;
            }

            AddButton.Background = new SolidColorBrush(Colors.DodgerBlue);
            AddButton.BorderBrush = new SolidColorBrush(Colors.DodgerBlue);

            string Name = StaffName.Text;
            string Position = StaffPosition.SelectedValue.ToString();
            string Grade = StaffGrade.SelectedValue.ToString();
            string TimeOfArrival = arrivalTimePicker.Time.ToString().Substring(0,5);

            StaffMemberData StaffData = new StaffMemberData(Name, Position, Grade, TimeOfArrival);
            StaffList.Members.Add(StaffData);
            ItemsAdded.Add(StaffData);

            StaffName.Text = "";
            StaffPosition.SelectedIndex = -1;
            StaffGrade.SelectedIndex = -1;
        }
    }
}
