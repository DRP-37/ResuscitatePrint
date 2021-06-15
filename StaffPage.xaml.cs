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

        public StaffPage()
        {
            this.InitializeComponent();

            StaffPosition.SelectedIndex = -1;
            StaffGrade.SelectedIndex = -1;
            arrivalTimePicker.Time = DateTime.Now.TimeOfDay;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame.CanGoBack)
            {
                rootFrame.GoBack();
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (StaffName.Text == null || StaffPosition.SelectedIndex < 0 
                || StaffGrade.SelectedIndex < 0)
            {
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

            StaffList.Members.Add(new StaffMemberData(Name, Position, Grade, TimeOfArrival));

            StaffName.Text = "";
            StaffPosition.SelectedIndex = -1;
            StaffGrade.SelectedIndex = -1;
        }
    }
}
