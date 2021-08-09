using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;

namespace Resuscitate.DataClasses
{
    public class StatusEvent
    {
        // Current longest name is Ventilation Breaths: via mask (29 chars)
        private const int MAX_NAME_LEN = 35;

        public string Name { get; }
        public string Data { get; }
        public string Time { get; }

        public StatusEvent(string Name, string Data, string Time)
        {
            this.Name = Name;
            this.Data = Data;
            this.Time = Time;
        }

        /* Constructors to convert text from TextBlocks */
        public StatusEvent(TextBlock NameBox, string Data, string Time) : this(NameBox.Text.Replace("\n", " "), Data, Time) { }

        public StatusEvent(string Name, TextBlock DataBox, string Time) : this(Name, DataBox.Text.Replace("\n", " "), Time) { }

        public StatusEvent(TextBlock NameBox, TextBlock DataBox, string Time) : this(NameBox, DataBox.Text.Replace("\n", " "), Time) { }

        /* Constructors to convert text from TextBoxes */
        public StatusEvent(TextBox NameBox, string Data, string Time) : this(NameBox.Text.Replace("\n", " "), Data, Time) { }

        public StatusEvent(string Name, TextBox DataBox, string Time) : this(Name, DataBox.Text.Replace("\n", " "), Time) { }

        public StatusEvent(TextBox NameBox, TextBox DataBox, string Time) : this(NameBox, DataBox.Text.Replace("\n", " "), Time) { }

        /* Static Constructors */

        public static StatusEvent FromTextBlockButtons(string name, Button[] buttons, string Time)
        {
            Button selected = InputUtils.SelectionMade(buttons);

            if (selected == null)
            {
                return null;
            }

            return new StatusEvent(name, (TextBlock)selected.Content, Time);
        }

        /* Methods */

        public override string ToString()
        {
            return Time + " " + Name + ":" + new string(' ', MAX_NAME_LEN - Name.Length) + Data;
        }

        /* Static Utils */

        public static void MaybeAdd(StatusEvent statusEvent, List<StatusEvent> statusEvents)
        {
            if (statusEvent != null)
            {
                statusEvents.Add(statusEvent);
            }
        }
    }
}
