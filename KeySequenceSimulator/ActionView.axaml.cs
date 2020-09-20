using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;
using KeySequenceSimulator.ActionSimulator;
using System.Collections.Generic;

namespace KeySequenceSimulator
{
    public class ActionView : UserControl
    {
        // index of action types coerces with index of combobox
        private enum ActionType
        {
            KEY_DOWN, KEY_UP, KEY_PRESS,
            SLEEP, MOUSE_DOWN, MOUSE_UP, MOUSE_CLICK, MOUSE_DOUBLE_CLICK,
            TEXT, REPEAT
        }

        private ActionType SelectedAction { get; set; }

        public Sequence ParentSequence { get; set; }

        private ComboBox actionCombobox;
        private Panel part2;
        private Border part3;

        private Button keyButton;
        private TextBox sleepText;
        private ComboBox cbMouseKey;
        private TextBox textText;

        private char key;
        public MouseKey SelectedMouseKey { get; private set; }
        public int MouseX { get; private set; }
        public int MouseY { get; private set; }
        private bool IsListening { get; set; }

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
            actionCombobox.SelectionChanged += OnActionSelectionChanged;
            cbMouseKey.SelectionChanged += OnMouseSelectionChanged;

            // init action simulator
            ActionSimulator = new ActionSimulatorWindows();

            IsListening = false;
        }

        public void OnActionSelectionChanged(object sender, RoutedEventArgs e)
        {
            // first hide all inputs
            keyButton.SetValue(IsVisibleProperty, false);
            sleepText.SetValue(IsVisibleProperty, false);
            cbMouseKey.SetValue(IsVisibleProperty, false);
            textText.SetValue(IsVisibleProperty, false);
            part2.SetValue(IsVisibleProperty, true);
            ParentSequence.SetRepeatSequence(false);

            SelectedAction = (ActionType)actionCombobox.SelectedIndex;

            // displays action parts depending on action type
            switch (SelectedAction)
            {
                case ActionType.KEY_DOWN:
                case ActionType.KEY_UP:
                case ActionType.KEY_PRESS:
                    keyButton.SetValue(IsVisibleProperty, true);
                    break;
                case ActionType.SLEEP:
                    sleepText.SetValue(IsVisibleProperty, true);
                    break;
                case ActionType.MOUSE_DOWN:
                case ActionType.MOUSE_UP:
                case ActionType.MOUSE_CLICK:
                case ActionType.MOUSE_DOUBLE_CLICK:
                    cbMouseKey.SetValue(IsVisibleProperty, true);
                    break;
                case ActionType.TEXT:
                    textText.SetValue(IsVisibleProperty, true);
                    break;
                case ActionType.REPEAT: // repeat
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
                    SelectedMouseKey = MouseKey.LEFT;
                    break;
                case 1:
                    SelectedMouseKey = MouseKey.RIGHT;
                    break;
                case 2:
                    SelectedMouseKey = MouseKey.MIDDLE;
                    break;
            }
        }

        public void SetKey(object sender, RoutedEventArgs e)
        {
            // Waits for input hotkey
            keyButton.Content = "Waiting for input...";

            // add keylistener
            if (!IsListening)
                keyButton.KeyUp += changeKeyListener;
        }

        private void changeKeyListener(object sender, KeyEventArgs e)
        {
            key = Util.KeyToChar(e.Key, e.KeyModifiers);
            IsListening = true;
            if (key == '\x00')
                keyButton.Content = "Invalid char. Try again.";
            else
            {
                // remove listener
                keyButton.KeyUp -= changeKeyListener;
                IsListening = false;
                keyButton.Content = key.ToString();
            }
        }

        // executes the action
        public async void Execute()
        {
            // selects an action to execute
            switch (SelectedAction)
            {
                case ActionType.KEY_DOWN:
                    ActionSimulator.SimulateKey(KeyAction.DOWN, key); //TODO key enum
                    break;
                case ActionType.KEY_UP:
                    ActionSimulator.SimulateKey(KeyAction.UP, key);
                    break;
                case ActionType.KEY_PRESS:
                    ActionSimulator.SimulateKey(KeyAction.PRESS, key);
                    break;
                case ActionType.SLEEP:
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
                case ActionType.MOUSE_DOWN:
                    ActionSimulator.SimulateMouseDown(SelectedMouseKey, 0, 0); //TODO position
                    break;
                case ActionType.MOUSE_UP:
                    ActionSimulator.SimulateMouseUp(SelectedMouseKey, 0, 0); //TODO position
                    break;
                case ActionType.MOUSE_CLICK:
                    ActionSimulator.SimulateMouseClick(SelectedMouseKey, 0, 0); //TODO position
                    break;
                case ActionType.MOUSE_DOUBLE_CLICK:
                    ActionSimulator.SimulateMouseDoubleClick(SelectedMouseKey, 0, 0); //TODO position
                    break;
                case ActionType.TEXT:
                    ActionSimulator.SimulateText(textText.Text);
                    break;
                case ActionType.REPEAT:
                    // nop
                    break;
            }
        }
        public string ToJson()
        {
            string json = "{\n";
            json += "\ttype: " + SelectedAction;

            switch (SelectedAction)
            {
                case ActionType.KEY_DOWN:
                case ActionType.KEY_UP:
                case ActionType.KEY_PRESS:
                    json += ",\n\tkey: " + key;
                    break;
                case ActionType.SLEEP:
                    json += ",\n\tduration: " + sleepText.Text;
                    break;
                case ActionType.MOUSE_DOWN:
                case ActionType.MOUSE_UP:
                case ActionType.MOUSE_CLICK:
                case ActionType.MOUSE_DOUBLE_CLICK:
                    json += ",\n\tmousekey: " + SelectedMouseKey + ",\n";
                    json += "\tx: " + MouseX + ",\n";
                    json += "\ty: " + MouseY;
                    break;
                case ActionType.TEXT:
                    json += ",\n\ttext: " + textText.Text;
                    break;
                case ActionType.REPEAT:
                    // nop
                    break;
            }

            json += "\n}";
            return json;
        }
    }
}
