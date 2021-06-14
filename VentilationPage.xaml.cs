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
    public sealed partial class VentilationPage : Page
    {
        public Timing TimingCount { get; set; }
        private Button[] procedures;
        private int? airGiven = null;
        // Ventilation Support:
        // 0: Inflation Breaths: Via Mask
        // 1: Inflation Breaths: Via ETT
        // 2: Ventilation Breaths: Via Mask
        // 3: Ventilation Breaths: Via ETT
        // 4: Mask CPAP
        private int ventilationProcedure;

        private Ventillation ventilation;
        private StatusEvent statusEvent;

        public VentilationPage()
        {
            this.InitializeComponent();
            procedures = new Button[] { InflationMask, InflationETT, VentilationMask, VentilationETT, MaskCPAP };
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Take value from previous screen
            TimingCount = (Timing)e.Parameter;

            ventilation = new Ventillation();
            statusEvent = new StatusEvent();

            base.OnNavigatedTo(e);
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectionMade(procedures))
            {
                bool hasAirGiven = airGiven != null;

                if (hasAirGiven && airGiven > 100)
                {
                    AirGiven.BorderBrush = new SolidColorBrush(Colors.Red);
                    AirGiven.Background = new SolidColorBrush(Colors.LightPink);
                    return;
                }

                List<Event> EventList = new List<Event>();
                List<StatusEvent> StatusList = new List<StatusEvent>();

                // set data structure with ventilation procedure and time stamp of selection
                ventilation.Time = TimingCount;
                ventilation.Oxygen = hasAirGiven ? (float)airGiven : -1;
                ventilation.VentType = (VentillationType)ventilationProcedure;
                EventList.Add(ventilation);

                statusEvent.Name = ventilation.ventToString();
                statusEvent.Data = hasAirGiven ? $"{airGiven}% Air/Oxygen Given" : "N/I";
                statusEvent.Time = ventilation.Time.ToString();
                statusEvent.Event = ventilation;
                StatusList.Add(statusEvent);

                Frame.Navigate(typeof(Resuscitation), new EventAndTiming(TimingCount, EventList, StatusList));
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Resuscitation), TimingCount);
        }

        private void Ventilation_Click(object sender, RoutedEventArgs e)
        {
            Button selected = (sender as Button);
            int index = Array.IndexOf(procedures, selected);
            if (this.ventilationProcedure == index)
            {
                procedures[index].Background = new SolidColorBrush(Colors.White);
                ventilationProcedure = -1;
            }
            else
            {
                UpdateColours(procedures, selected);
                ventilationProcedure = index;
            }
        }

        private void TimeView_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Nothing
        }

        private void AirGiven_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            textBox.Text = new String(textBox.Text.Where(c => char.IsDigit(c) || c == '.').ToArray());
            int temp;
            if (!int.TryParse(AirGiven.Text, out temp))
            {
                // if parsing attempt wasn't successful
                // output message to enter only numbers
            } else
            {
                airGiven = temp;
            }

        }

        private bool SelectionMade(Button[] buttons)
        {
            foreach (Button button in buttons)
            {
                SolidColorBrush colour = button.Background as SolidColorBrush;

                if (colour.Color == Colors.LightGreen)
                {
                    return true;
                }
            }
            return false;
        }

        private void UpdateColours(Button[] buttons, Button sender)
        {
            foreach (Button button in buttons)
            {
                button.Background = new SolidColorBrush(Colors.White);
            }
            sender.Background = new SolidColorBrush(Colors.LightGreen);
        }

        private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {
            // Nothing
        }
    }
}
