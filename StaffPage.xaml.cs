using Resuscitate.DataClasses;
using System;
using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Resuscitate
{
    public sealed partial class StaffPage : Page
    {
        private static readonly Color READY_FOR_INPUT_COLOUR = InputUtils.DEFAULT_ADD_COLOUR;
        private static readonly Color INCORRECT_INPUT_COLOUR = InputUtils.DEFAULT_INCORRECT_ADD_COLOUR;

        private ResuscitationData ResusData;
        private StaffList StaffList;
        private List<StaffMemberData> ItemsAdded;

        public StaffPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Always takes a ResuscitationData
            ResusData = (ResuscitationData)e.Parameter;
            StaffList = ResusData.StaffList;

            ItemsAdded = new List<StaffMemberData>();

            StaffName.Text = "";
            StaffPosition.SelectedIndex = -1;
            StaffGrade.SelectedIndex = -1;
            arrivalTimePicker.Time = DateTime.Now.TimeOfDay;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame.CanGoBack)
            {
                PageStackEntry prevStackEntry = rootFrame.BackStack[rootFrame.BackStackDepth - 1];
                this.Frame.Navigate(prevStackEntry.SourcePageType, ResusData);
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(StaffName.Text) || StaffPosition.SelectedIndex < 0 
                || StaffGrade.SelectedIndex < 0)
            {
                FlyoutBase.ShowAttachedFlyout((FrameworkElement) sender);

                AddButton.Background = new SolidColorBrush(INCORRECT_INPUT_COLOUR);
                AddButton.BorderBrush = new SolidColorBrush(INCORRECT_INPUT_COLOUR);
                return;
            }

            AddButton.Background = new SolidColorBrush(READY_FOR_INPUT_COLOUR);
            AddButton.BorderBrush = new SolidColorBrush(READY_FOR_INPUT_COLOUR);

            StaffMemberData StaffData = GenerateStaffData();
            ItemsAdded.Add(StaffData);
            StaffList.Members.Add(StaffData);

            StaffName.Text = "";
            StaffPosition.SelectedIndex = -1;
            StaffGrade.SelectedIndex = -1;
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
                PageStackEntry prevStackEntry = rootFrame.BackStack[rootFrame.BackStackDepth - 1];
                this.Frame.Navigate(prevStackEntry.SourcePageType, ResusData);
            }
        }

        private StaffMemberData GenerateStaffData()
        {
            string Name = StaffName.Text;
            string Position = StaffPosition.SelectedValue.ToString();
            string Grade = StaffGrade.SelectedValue.ToString();
            string TimeOfArrival = arrivalTimePicker.Time.ToString().Substring(0, 5);

            return new StaffMemberData(Name, Position, Grade, TimeOfArrival);
        }
    }
}
