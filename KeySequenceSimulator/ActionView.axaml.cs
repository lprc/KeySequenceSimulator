using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;

namespace KeySequenceSimulator
{
    public class ActionView : UserControl
    {
        public Sequence ParentSequence { get; set; }

        private ComboBox actionCombobox;
        private Panel part2;
        private Border part3;

        private Button keyButton;
        private TextBox sleepText;
        private ComboBox cbMouseKey;
        private TextBox textText;

        private Key key;
        private MouseButton mouseButton;

        public ActionView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            actionCombobox = this.FindControl<ComboBox>("cbActionSelection");
            part2 = this.FindControl<Panel>("ActionPart2");
            part3 = this.FindControl<Border>("ActionPart3");

            keyButton = this.FindControl<Button>("btnKey");
            sleepText = this.FindControl<TextBox>("txtSleep");
            cbMouseKey = this.FindControl<ComboBox>("cbMouse");
            textText = this.FindControl<TextBox>("txtText");

            // add listeners to comboboxes
            actionCombobox.SelectionChanged += OnSelectionChanged;
            cbMouseKey.SelectionChanged += OnMouseSelectionChanged;
        }

        public void OnSelectionChanged(object sender, RoutedEventArgs e)
        {
            // first hide all inputs
            keyButton.SetValue(IsVisibleProperty, false);
            sleepText.SetValue(IsVisibleProperty, false);
            cbMouseKey.SetValue(IsVisibleProperty, false);
            textText.SetValue(IsVisibleProperty, false);
            part2.SetValue(IsVisibleProperty, true);
            ParentSequence.SetRepeatSequence(false);

            // displays action parts depending on action type
            switch (actionCombobox.SelectedIndex)
            {
                case 0:
                case 1:
                case 2:
                    keyButton.SetValue(IsVisibleProperty, true);
                    break;
                case 3:
                    sleepText.SetValue(IsVisibleProperty, true);
                    break;
                case 4:
                case 5:
                case 6:
                case 7:
                    cbMouseKey.SetValue(IsVisibleProperty, true);
                    break;
                case 8:
                    textText.SetValue(IsVisibleProperty, true);
                    break;
                case 9: // repeat
                    part2.SetValue(IsVisibleProperty, false);
                    ParentSequence.SetRepeatSequence(true);
                    break;
            }
        }

        public void OnMouseSelectionChanged(object sender, RoutedEventArgs e)
        {
            // save mouse button
            switch (actionCombobox.SelectedIndex)
            {
                case 0:
                    mouseButton = MouseButton.Left;
                    break;
                case 1:
                    mouseButton = MouseButton.Right;
                    break;
                case 2:
                    mouseButton = MouseButton.Middle;
                    break;
                default:
                    mouseButton = MouseButton.None;
                    break;
            }
        }

        public void SetKey(object sender, RoutedEventArgs e)
        {
            // Waits for input hotkey
            keyButton.Content = "Waiting for input...";

            // add keylistener
            keyButton.KeyUp += changeKeyListener;
        }

        private void changeKeyListener(object sender, KeyEventArgs e)
        {
            key = e.Key;

            // remove listener
            keyButton.KeyUp -= changeKeyListener;
            keyButton.Content = key.ToString();
        }
    }
}
