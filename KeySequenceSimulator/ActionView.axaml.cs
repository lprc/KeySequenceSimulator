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
        private ComboBox actionCombobox;
        private Panel part2;
        private Border part3;

        private Button keyButton;
        private TextBox sleepText;
        private Button mouseKeyButton;
        private TextBox textText;

        private Key key;
        private MouseButton mouseKey;
        private EventHandler<KeyEventArgs> changeKeyListener;
        private EventHandler<PointerReleasedEventArgs> changeMouseKeyListener;

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
            mouseKeyButton = this.FindControl<Button>("btnMouse");
            textText = this.FindControl<TextBox>("txtText");

            // add listener to combobox
            actionCombobox.SelectionChanged += OnSelectionChanged;

            // handler for changing input key
            changeKeyListener = (sender, e) =>
            {
                key = e.Key;

                // remove listener
                keyButton.KeyUp -= changeKeyListener;
                keyButton.Content = key.ToString();
            };

            // handler for changing input mouse key
            changeMouseKeyListener = (sender, e) =>
            {
                //TODO
                //mouseKey = e.MouseButton;

                // remove listener
                keyButton.KeyUp -= changeKeyListener;
                //keyButton.Content = key.ToString();
                keyButton.Content = "test";
            };
        }

        public void OnSelectionChanged(object sender, RoutedEventArgs e)
        {
            // first hide all inputs
            keyButton.SetValue(IsVisibleProperty, false);
            sleepText.SetValue(IsVisibleProperty, false);
            mouseKeyButton.SetValue(IsVisibleProperty, false);
            textText.SetValue(IsVisibleProperty, false);

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
                    mouseKeyButton.SetValue(IsVisibleProperty, true);
                    break;
                case 8:
                    textText.SetValue(IsVisibleProperty, true);
                    break;
            }
        }

        public void SetKey(object sender, RoutedEventArgs e)
        {
            // Waits for input hotkey
            keyButton.Content = "Waiting for input...";
            mouseKeyButton.Content = "Waiting for input...";

            // add keylistener
            keyButton.KeyUp += changeKeyListener;
            mouseKeyButton.PointerReleased += changeMouseKeyListener;
        }
    }
}
