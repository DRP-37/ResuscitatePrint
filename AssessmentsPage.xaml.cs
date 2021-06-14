﻿using Resuscitate.DataClasses;
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
    public sealed partial class AssessmentsPage : Page
    {
        public Timing TimingCount { get; set; }
        private Button[] colours;
        private Button[] hrs;
        private Button[] respirations;
        private Button[] tones;
        private Button[] tempRegulations;
        private Button[] cordClamping;
        private int cord = -1;
        private int temp = -1;
        private int hr = -1;
        private int respiration = -1;
        private int tone = -1;
        private int colour = -1;

        InitialAssessment initialAssessment;

        public AssessmentsPage()
        {
            this.InitializeComponent();
            colours = new Button[] { colour0, colour1, colour2 };
            hrs = new Button[] { hr0, hr1, hr2 };
            respirations = new Button[] { resp0, resp1, resp2 };
            tones = new Button[] { tone0, tone1, tone2 };
            tempRegulations = new Button[] { temp0, temp1, temp2 };
            cordClamping = new Button[] { cord0, cord1, cord2 };
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Take value from previous screen
            TimingCount = (Timing)e.Parameter;

            initialAssessment = new InitialAssessment(TimingCount);

            base.OnNavigatedTo(e);
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            // Set data structure
            initialAssessment.Colour = colour;
            initialAssessment.Tone = tone;
            initialAssessment.RespEffort = respiration;
            initialAssessment.HeartRate = hr;

            initialAssessment.Clamping = (CordClamping)cord;
            initialAssessment.TempReg = (TemperatureReg)temp;

            // Add check whether a proper selection has been made
            List<Event> Events = new List<Event>();
            Events.Add(initialAssessment);

            string Time = initialAssessment.Time.ToString();

            List<StatusEvent> StatusEvents = new List<StatusEvent>();
            AddIfNotNull(GenerateStatusEvent("Colour", colours, Time), StatusEvents);
            AddIfNotNull(GenerateStatusEvent("Heart Rate", hrs, Time), StatusEvents);
            AddIfNotNull(GenerateStatusEvent("Muscle", tones, Time), StatusEvents);
            AddIfNotNull(GenerateStatusEvent("Temperature Regulation", tempRegulations, Time), StatusEvents);
            AddIfNotNull(GenerateStatusEvent("Cord Clamping", cordClamping, Time), StatusEvents);
            AddIfNotNull(GenerateStatusEvent("Respiratory Effort", respirations, Time), StatusEvents);

            Frame.Navigate(typeof(Resuscitation), new EventAndTiming(TimingCount, Events, StatusEvents));
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Resuscitation), TimingCount);
        }

        private void TimeView_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Nothing
        }

        private void colour_Click(object sender, RoutedEventArgs e)
        {
            Button selected = sender as Button;
            SolidColorBrush colour = selected.Background as SolidColorBrush;

            if (colour.Color == Colors.LightGreen)
            {
                selected.Background = new SolidColorBrush(Colors.White);
                this.colour = -1;
                return;
            }

            changeColours(colours, selected);
            this.colour = selected.Name[selected.Name.Length - 1] - '0';
            Console.WriteLine(this.colour);
        }

        private void hr_Click(object sender, RoutedEventArgs e)
        {
            Button selected = sender as Button;
            SolidColorBrush colour = selected.Background as SolidColorBrush;

            if (colour.Color == Colors.LightGreen)
            {
                selected.Background = new SolidColorBrush(Colors.White);
                this.hr = -1;
                return;
            }

            changeColours(hrs, selected);
            this.hr = selected.Name[selected.Name.Length - 1] - '0';
        }

        private void tone_Click(object sender, RoutedEventArgs e)
        {
            Button selected = sender as Button;
            SolidColorBrush colour = selected.Background as SolidColorBrush;

            if (colour.Color == Colors.LightGreen)
            {
                selected.Background = new SolidColorBrush(Colors.White);
                this.tone = -1;
                return;
            }

            changeColours(tones, selected);
            this.tone = selected.Name[selected.Name.Length - 1] - '0';
        }

        private void resp_Click(object sender, RoutedEventArgs e)
        {
            Button selected = sender as Button;
            SolidColorBrush colour = selected.Background as SolidColorBrush;

            if (colour.Color == Colors.LightGreen)
            {
                selected.Background = new SolidColorBrush(Colors.White);
                this.respiration = -1;
                return;
            }

            changeColours(respirations, selected);
            this.respiration = selected.Name[selected.Name.Length - 1] - '0';
        }

        private void cord_Click(object sender, RoutedEventArgs e)
        {
            Button selected = sender as Button;
            SolidColorBrush colour = selected.Background as SolidColorBrush;

            if (colour.Color == Colors.LightGreen)
            {
                selected.Background = new SolidColorBrush(Colors.White);
                this.cord = -1;
                return;
            }

            changeColours(cordClamping, selected);
            this.cord = selected.Name[selected.Name.Length - 1] - '0';
        }

        private void temp_Click(object sender, RoutedEventArgs e)
        {
            Button selected = sender as Button;
            SolidColorBrush colour = selected.Background as SolidColorBrush;

            if (colour.Color == Colors.LightGreen)
            {
                selected.Background = new SolidColorBrush(Colors.White);
                this.temp = -1;
                return;
            }

            changeColours(tempRegulations, selected);
            this.temp = selected.Name[selected.Name.Length - 1] - '0';
        }

        private void changeColours(Button[] buttons, Button sender)
        {
            buttons[0].Background = new SolidColorBrush(Colors.White);
            buttons[1].Background = new SolidColorBrush(Colors.White);
            buttons[2].Background = new SolidColorBrush(Colors.White);
            sender.Background = new SolidColorBrush(Colors.LightGreen);
        }

        private StatusEvent GenerateStatusEvent(string name, Button[] buttons, string time)
        {
            Button selected = null;

            foreach (Button button in buttons)
            {
                SolidColorBrush colour = button.Background as SolidColorBrush;

                if (colour.Color == Colors.LightGreen)
                {
                    selected = button;
                }
            }

            if (selected == null)
            {
                return null;
            }

            return new StatusEvent(name, selected.Content.ToString(), time); ;
        }

        private void AddIfNotNull(StatusEvent Event, List<StatusEvent> StatusEvents)
        {
            if (Event != null)
            {
                StatusEvents.Add(Event);
            }
        }
    }
}
