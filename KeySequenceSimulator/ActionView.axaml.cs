using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;
using KeySequenceSimulator.ActionSimulator;

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

        public IActionSimulator ActionSimulator { get; set; }

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

            // init action simulator
            ActionSimulator = new ActionSimulatorWindows();
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

        // executes the action
        public async void Execute()
        {
            MouseKey mk = cbMouseKey.SelectedIndex == 0 ? MouseKey.LEFT : cbMouseKey.SelectedIndex == 1 ? MouseKey.RIGHT : MouseKey.MIDDLE;

            // selects an action to execute
            switch (actionCombobox.SelectedIndex)
            {
                case 0:
                    ActionSimulator.SimulateKey(KeyAction.DOWN, (int)key); //TODO key enum
                    break;
                case 1:
                    ActionSimulator.SimulateKey(KeyAction.UP, (int)key);
                    break;
                case 2:
                    ActionSimulator.SimulateKey(KeyAction.PRESS, (int)key);
                    break;
                case 3: // sleep
                    try
                    {
                        ActionSimulator.SimulateSleep(Int32.Parse(sleepText.Text));
                    }
                    catch (Exception e)
                    {
                        // TODO validate input, only numeric
                        await MessageBox.Show(null, "Error parsing sleep time:\n" + e.Message, "Error", MessageBox.MessageBoxButtons.Ok);
                    }                    
                    break;
                case 4: // mousedown
                    ActionSimulator.SimulateMouseDown(mk, 0, 0); //TODO position
                    break;
                case 5: // mouseup
                    ActionSimulator.SimulateMouseUp(mk, 0, 0); //TODO position
                    break;
                case 6: // mouse click
                    ActionSimulator.SimulateMouseClick(mk, 0, 0); //TODO position
                    break;
                case 7: // mouse doubleclick
                    ActionSimulator.SimulateMouseDoubleClick(mk, 0, 0); //TODO position
                    break;
                case 8: // text
                    ActionSimulator.SimulateText(textText.Text);
                    break;
                case 9: // repeat
                    // nop
                    break;
            }
        }
    }
}
