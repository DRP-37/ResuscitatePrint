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
    public sealed partial class Resuscitation : Page
    {
        readonly DispatcherTimer Timer = new DispatcherTimer();
        private int Count_Seconds = 0;

        public Resuscitation()
        {
            this.InitializeComponent();

            // Start timer
            Timer.Tick += Timer_Tick;
            Timer.Interval = new TimeSpan(0, 0, 1);
            Timer.Start();
        }

        private void Timer_Tick(object sender, object e)
        {
            // Update Time
            Count_Seconds ++;

            // Update View with Time
            String minsStr, secsStr;
            int mins = Count_Seconds / 60;
            int secs = Count_Seconds % 60;

            minsStr = mins < 10 ? "0" + mins.ToString() : mins.ToString();
            secsStr = secs < 10 ? "0" + secs.ToString() : secs.ToString();

            TimeView.Text = minsStr + ":" + secsStr;
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame.CanGoBack)
            {
                rootFrame.GoBack();
            }
        }

        private void TimeView_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
