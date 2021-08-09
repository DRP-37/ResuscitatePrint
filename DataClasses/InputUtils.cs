using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Resuscitate.DataClasses
{
    class InputUtils
    {
        // Default button colours
        public static readonly Color DEFAULT_SELECTED_COLOUR = ConvertHexColour("#9923E8CA");
        public static readonly Color DEFAULT_UNSELECTED_COLOUR = Colors.White;

        public static readonly Color DEFAULT_INCORRECT_BUTTON_COLOUR = Colors.Red;
        public static readonly Color DEFAULT_NAVIGATION_BUTTON_COLOUR = ConvertHexColour("#FFF2F2F2");

        public static readonly Color DEFAULT_ADD_COLOUR = ConvertHexColour("#FF24BAA3");
        public static readonly Color DEFAULT_INCORRECT_ADD_COLOUR = Colors.Red;

        public static readonly Color DEFAULT_CPR_SELECTED_COLOUR = ConvertHexColour("#FFDB4325");
        public static readonly Color DEFAULT_CPR_UNSELECTED_COLOUR = Colors.LightGray;

        // Default text input colours
        public static readonly Color DEFAULT_CORRECT_INPUT_BACKGROUND = Colors.White;
        public static readonly Color DEFAULT_CORRECT_INPUT_BORDER = Colors.Black;

        public static readonly Color DEFAULT_INCORRECT_INPUT_BACKGROUND = Colors.LightPink;
        public static readonly Color DEFAULT_INCORRECT_INPUT_BORDER = Colors.PaleVioletRed;

        private InputUtils() { }

        /* BUTTON UTILS */

        /* Simulates a button click in which there can only be one button selected at once.
         * Returns NULL if the selected was the same as the sender.
         * DOES NOT update a StatusEvent */
        public static Button ClickWithColours(Button sender, Button[] buttons, Color selectedColour, Color unselectedColour)
        {
            Button selected = SelectionMade(buttons);

            if (selected != null && selected.Equals(sender)) {
                selected.Background = new SolidColorBrush(unselectedColour);

                return null;
            } else
            {
                UpdateOneSelected(buttons, sender, selectedColour, unselectedColour);

                return sender;
            }
        }

        public static Button ClickWithDefaults(Button sender, Button[] buttons)
        {
            return ClickWithColours(sender, buttons, DEFAULT_SELECTED_COLOUR, DEFAULT_UNSELECTED_COLOUR);
        }

        /* Same as ClickWithColours but allows all buttons to be selected at once.
         * Returns the sender if the sender was already selected, otherwise null */
        public static Button ClickAnyWithColours(Button sender, Button[] buttons, Color selectedColour, Color unselectedColour)
        {
            SolidColorBrush senderColour = (SolidColorBrush) sender.Background;

            if (senderColour.Equals(selectedColour))
            {
                sender.Background = new SolidColorBrush(unselectedColour);

                return null;
            } else
            {
                sender.Background = new SolidColorBrush(selectedColour);

                return sender;
            }
        }

        public static Button ClickAnyWithDefaults(Button sender, Button[] buttons)
        {
            return ClickAnyWithColours(sender, buttons, DEFAULT_SELECTED_COLOUR, DEFAULT_UNSELECTED_COLOUR);
        }

        public static Button SelectionMade(Button[] buttons, Color selectedColour)
        {
            foreach (Button button in buttons)
            {
                SolidColorBrush colour = button.Background as SolidColorBrush;

                if (colour.Color == selectedColour)
                {
                    return button;
                }
            }

            return null;
        }

        public static Button SelectionMade(Button[] buttons)
        {
            return SelectionMade(buttons, DEFAULT_SELECTED_COLOUR);
        }

        public List<Button> SelectedButtons(Button[] buttons, Color selectedColour)
        {
            List<Button> Selecteds = new List<Button>();

            foreach (Button button in buttons)
            {
                SolidColorBrush colour = button.Background as SolidColorBrush;

                if (colour.Color == selectedColour)
                {
                    Selecteds.Add(button);
                }
            }

            return Selecteds;
        }

        public List<Button> SelectedButtons(Button[] buttons)
        {
            return SelectedButtons(buttons, DEFAULT_SELECTED_COLOUR);
        }

        public static void UpdateOneSelected(Button[] buttons, Button newSelection,
            Color selectedColour, Color unselectedColour)
        {
            foreach (Button button in buttons)
            {
                button.Background = new SolidColorBrush(unselectedColour);
            }

            newSelection.Background = new SolidColorBrush(selectedColour);
        }

        public static void UpdateOneSelected(Button[] buttons, Button newSelection)
        {
            UpdateOneSelected(buttons, newSelection, DEFAULT_SELECTED_COLOUR, DEFAULT_UNSELECTED_COLOUR);
        }

        public static void ResetButtons(Button[] buttons, Color unselectedColour)
        {
            foreach (Button button in buttons)
            {
                button.Background = new SolidColorBrush(unselectedColour);
            }
        }

        public static void ResetButtons(Button[] buttons)
        {
            ResetButtons(buttons, DEFAULT_UNSELECTED_COLOUR);
        }

        /* TEXT UTILS */

        public static void UpdateValidColours(TextBox inputBox, bool validInput, 
            Color correctBackground, Color correctBorder, Color incorrectBackground, Color incorrectBorder)
        {
            if (validInput || string.IsNullOrWhiteSpace(inputBox.Text))
            {
                inputBox.Background = new SolidColorBrush(correctBackground);
                inputBox.BorderBrush = new SolidColorBrush(correctBorder);
            } else
            {
                inputBox.Background = new SolidColorBrush(incorrectBackground);
                inputBox.BorderBrush = new SolidColorBrush(incorrectBorder);
            }
        }

        public static void UpdateValidColours(TextBox inputBox, bool validInput)
        {
            UpdateValidColours(inputBox, validInput,
                DEFAULT_CORRECT_INPUT_BACKGROUND, DEFAULT_CORRECT_INPUT_BORDER,
                DEFAULT_INCORRECT_INPUT_BACKGROUND, DEFAULT_INCORRECT_INPUT_BORDER);
        }

        public static void ResetTextBoxColour(TextBox inputBox, Color correctBackground, Color correctBorder)
        {
            inputBox.Background = new SolidColorBrush(correctBackground);
            inputBox.BorderBrush = new SolidColorBrush(correctBorder);
        }

        public static void ResetTextBoxColour(TextBox inputBox)
        {
            ResetTextBoxColour(inputBox, DEFAULT_CORRECT_INPUT_BACKGROUND, DEFAULT_CORRECT_INPUT_BORDER);
        }

        public static void SetInvalidtTextBoxColour(TextBox inputBox, Color incorrectBackground, Color incorrectBorder)
        {
            inputBox.Background = new SolidColorBrush(incorrectBackground);
            inputBox.BorderBrush = new SolidColorBrush(incorrectBorder);
        }

        public static void SetInvalidTextBoxColour(TextBox inputBox)
        {
            ResetTextBoxColour(inputBox, DEFAULT_INCORRECT_INPUT_BACKGROUND, DEFAULT_INCORRECT_INPUT_BORDER);
        }

        /* COLOUR UTILS */

        public static Color ConvertHexColour(string hexColour)
        {
            hexColour = hexColour.Replace("#", string.Empty);
            // from #RRGGBB string
            var s = (byte)System.Convert.ToUInt32(hexColour.Substring(0, 2), 16);
            var r = (byte)System.Convert.ToUInt32(hexColour.Substring(2, 2), 16);
            var g = (byte)System.Convert.ToUInt32(hexColour.Substring(4, 2), 16);
            var b = (byte)System.Convert.ToUInt32(hexColour.Substring(6, 2), 16);

            //get the color
            return Color.FromArgb(s, r, g, b);
        }
    }
}
