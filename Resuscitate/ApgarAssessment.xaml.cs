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
    public sealed partial class ApgarAssessment : Page
    {
        int hr;
        int resp;
        int tone;
        int response;
        int colour;

        ApgarScore score;
        public ApgarAssessment()
        {
            this.InitializeComponent();
        }

        private void confirmButton_Click(object sender, RoutedEventArgs e)
        {
            score.HeartRate = hr;
            score.Respiration = resp;
            score.Tone = tone;
            score.Response = response;
            score.Colour = colour;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            score = new ApgarScore((TimeSpan)e.Parameter);
        }
    }
}