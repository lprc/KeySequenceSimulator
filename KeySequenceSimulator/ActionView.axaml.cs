using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;
using KeySequenceSimulator.ActionSimulator;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Avalonia.Threading;

namespace KeySequenceSimulator
{
    public class ActionView : UserControl
    {
        // index of action types coerces with index of combobox
        public enum ActionType
        {
            KEY_DOWN, KEY_UP, KEY_PRESS,
            SLEEP, MOUSE_DOWN, MOUSE_UP, MOUSE_CLICK, MOUSE_DOUBLE_CLICK,
            TEXT, REPEAT
        }

        public ActionType SelectedAction { get; set; }

        public Sequence ParentSequence { get; set; }

        private ComboBox actionCombobox;
        private Panel part2;
        private Panel part3;
        private Border borderKeymods;

        private CheckBox chbShift;
        private CheckBox chbCtrl;
        private CheckBox chbAlt;
        private CheckBox chbMeta;

        private Button keyButton;
        private TextBox sleepText;
        private ComboBox cbMouseKey;
        private TextBox textText;
        private TextBox txtX;
        private TextBox txtY;

        public KeyboardKey Key { get; set; }
        public MouseKey SelectedMouseKey { get; private set; }

        private int _mouseX;
        public int MouseX
        {
            get { return _mouseX; }
            private set { _mouseX = value; txtX.Text = value.ToString(); }
        }

        private int _mouseY;
        public int MouseY
        {
            get { return _mouseY; }
            private set { _mouseY = value; txtY.Text = value.ToString(); }
        }
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
            part3 = this.FindControl<Panel>("ActionPart3");
            borderKeymods = this.FindControl<Border>("borderPanelKeymods");

            chbShift = this.FindControl<CheckBox>("chbShift");
            chbCtrl = this.FindControl<CheckBox>("chbCtrl");
            chbAlt = this.FindControl<CheckBox>("chbAlt");
            chbMeta = this.FindControl<CheckBox>("chbMeta");

            keyButton = this.FindControl<Button>("btnKey");
            sleepText = this.FindControl<TextBox>("txtSleep");
            cbMouseKey = this.FindControl<ComboBox>("cbMouse");
            textText = this.FindControl<TextBox>("txtText");
            txtX = this.FindControl<TextBox>("txtX");
            txtY = this.FindControl<TextBox>("txtY");

            // add listeners to comboboxes
            actionCombobox.SelectionChanged += OnActionSelectionChanged;
            cbMouseKey.SelectionChanged += OnMouseSelectionChanged;

            // add listeners to textboxes
            txtX.GetObservable(TextBox.TextProperty).Subscribe(text => _mouseX = (text != null && text.Length > 0) ? ParseMouseX(text) : 0);
            txtY.GetObservable(TextBox.TextProperty).Subscribe(text => _mouseY = (text != null && text.Length > 0) ? ParseMouseY(text) : 0);

            // init action simulator
            ActionSimulator = new ActionSimulatorWindows();

            IsListening = false;
        }

        // Removes this action from the parent sequence
        public void Remove(object sender, RoutedEventArgs e)
        {
            ParentSequence.RemoveAction(this);
        }

        public void OnActionSelectionChanged(object sender, RoutedEventArgs e)
        {
            // first hide all inputs
            keyButton.SetValue(IsVisibleProperty, false);
            sleepText.SetValue(IsVisibleProperty, false);
            cbMouseKey.SetValue(IsVisibleProperty, false);
            textText.SetValue(IsVisibleProperty, false);
            part2.SetValue(IsVisibleProperty, true);
            part3.SetValue(IsVisibleProperty, false);
            borderKeymods.SetValue(IsVisibleProperty, false);
            ParentSequence.SetRepeatSequence(false);

            SelectedAction = (ActionType)actionCombobox.SelectedIndex;

            // displays action parts depending on action type
            switch (SelectedAction)
            {
                case ActionType.KEY_DOWN:
                case ActionType.KEY_UP:
                    keyButton.SetValue(IsVisibleProperty, true);
                    break;
                case ActionType.KEY_PRESS:
                    keyButton.SetValue(IsVisibleProperty, true);
                    borderKeymods.SetValue(IsVisibleProperty, true);
                    break;
                case ActionType.SLEEP:
                    sleepText.SetValue(IsVisibleProperty, true);
                    break;
                case ActionType.MOUSE_DOWN:
                case ActionType.MOUSE_UP:
                case ActionType.MOUSE_CLICK:
                case ActionType.MOUSE_DOUBLE_CLICK:
                    cbMouseKey.SetValue(IsVisibleProperty, true);
                    part3.SetValue(IsVisibleProperty, true);
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
            switch (cbMouseKey.SelectedIndex)
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
            //TODO add safety check?
            Key = Util.AvaloniaKeyToKeyboardKey(e.Key);
            IsListening = true;

            // remove listener
            keyButton.KeyUp -= changeKeyListener;
            IsListening = false;
            keyButton.Content = Key.ToString();
        }

        public bool isShiftKeyChecked()
        {
            return chbShift.IsChecked.HasValue && chbShift.IsChecked.Value;
        }

        public bool isCtrlKeyChecked()
        {
            return chbCtrl.IsChecked.HasValue && chbCtrl.IsChecked.Value;
        }

        public bool isAltKeyChecked()
        {
            return chbAlt.IsChecked.HasValue && chbAlt.IsChecked.Value;
        }

        public bool isMetaKeyChecked()
        {
            return chbMeta.IsChecked.HasValue && chbMeta.IsChecked.Value;
        }

        // executes the action
        public async void Execute()
        {
            KeyMod mod = isShiftKeyChecked() ? KeyMod.SHIFT : KeyMod.NONE;
            mod |= isCtrlKeyChecked() ? KeyMod.CTRL : KeyMod.NONE;
            mod |= isAltKeyChecked() ? KeyMod.ALT : KeyMod.NONE;
            mod |= isMetaKeyChecked() ? KeyMod.META : KeyMod.NONE;

            // selects an action to execute
            switch (SelectedAction)
            {
                case ActionType.KEY_DOWN:
                    ActionSimulator.SimulateKey(KeyAction.DOWN, Key);
                    break;
                case ActionType.KEY_UP:
                    ActionSimulator.SimulateKey(KeyAction.UP, Key);
                    break;
                case ActionType.KEY_PRESS:
                    ActionSimulator.SimulateKey(KeyAction.PRESS, Key, mod);
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
                    ActionSimulator.SimulateMouseDown(SelectedMouseKey, ParseMouseX(), ParseMouseY());
                    break;
                case ActionType.MOUSE_UP:
                    ActionSimulator.SimulateMouseUp(SelectedMouseKey, ParseMouseX(), ParseMouseY());
                    break;
                case ActionType.MOUSE_CLICK:
                    ActionSimulator.SimulateMouseClick(SelectedMouseKey, ParseMouseX(), ParseMouseY());
                    break;
                case ActionType.MOUSE_DOUBLE_CLICK:
                    ActionSimulator.SimulateMouseDoubleClick(SelectedMouseKey, ParseMouseX(), ParseMouseY());
                    break;
                case ActionType.TEXT:
                    ActionSimulator.SimulateText(textText.Text);
                    break;
                case ActionType.REPEAT:
                    // nop
                    break;
            }
        }

        private int ParseMouseX(string x = null)
        {

            try
            {
                return Int32.Parse(x ?? txtX.Text);
            }
            catch (Exception e)
            {
                // TODO validate input, only numeric
                Dispatcher.UIThread.Post(() => MessageBox.Show(null, "Error parsing mouse coords:\n" + e.Message, "Error", MessageBox.MessageBoxButtons.Ok));
                return -1;
            }
        }

        private int ParseMouseY(string y = null)
        {
            try
            {
                return Int32.Parse(y ?? txtY.Text);
            }
            catch (Exception e)
            {
                // TODO validate input, only numeric
                Dispatcher.UIThread.Post(() => MessageBox.Show(null, "Error parsing mouse coords:\n" + e.Message, "Error", MessageBox.MessageBoxButtons.Ok));
                return -1;
            }
        }

        public string ToJson()
        {
            string json = "\t\t{\n";
            json += "\t\t\t\"type\" : \"" + SelectedAction + "\"";

            switch (SelectedAction)
            {
                case ActionType.KEY_DOWN:
                case ActionType.KEY_UP:
                    json += ",\n\t\t\t\"key\" : \"" + Key + "\"";
                    break;
                case ActionType.KEY_PRESS:
                    json += ",\n\t\t\t\"key\" : \"" + Key + "\"";
                    json += ",\n\t\t\t\"shift\" : \"" + isShiftKeyChecked() + "\"";
                    json += ",\n\t\t\t\"ctrl\" : \"" + isCtrlKeyChecked() + "\"";
                    json += ",\n\t\t\t\"alt\" : \"" + isAltKeyChecked() + "\"";
                    json += ",\n\t\t\t\"meta\" : \"" + isMetaKeyChecked() + "\"";
                    break;
                case ActionType.SLEEP:
                    json += ",\n\t\t\t\"duration\" : \"" + sleepText.Text + "\"";
                    break;
                case ActionType.MOUSE_DOWN:
                case ActionType.MOUSE_UP:
                case ActionType.MOUSE_CLICK:
                case ActionType.MOUSE_DOUBLE_CLICK:
                    json += ",\n\t\t\t\"mousekey\" : \"" + SelectedMouseKey + "\",\n";
                    json += "\t\t\t\"x\" : \"" + MouseX + "\",\n";
                    json += "\t\t\t\"y\" : \"" + MouseY + "\"";
                    break;
                case ActionType.TEXT:
                    json += ",\n\t\t\t\"text\" : \"" + textText.Text + "\"";
                    break;
                case ActionType.REPEAT:
                    // nop
                    break;
            }

            json += "\n\t\t}";
            return json;
        }

        public void SetSleepText(string text)
        {
            sleepText.Text = text;
        }

        public void SetTextText(string text)
        {
            textText.Text = text;
        }

        public void SetActionTypeCbIndex(int index)
        {
            actionCombobox.SelectedIndex = index;
        }

        public static ActionView FromJson(JObject jsonObject, ActionView initial)
        {
            var av = initial ?? new ActionView();
            av.SelectedAction = (ActionView.ActionType)Enum.Parse(typeof(ActionView.ActionType), jsonObject["type"].ToString(), true);

            // local function that updates the combobox selection of mouse key
            void updateMouseCbSelection (MouseKey selectedMouseKey)
            {
                switch (selectedMouseKey)
                {
                    case MouseKey.LEFT:
                        av.cbMouseKey.SelectedIndex = 0;
                        break;
                    case MouseKey.RIGHT:
                        av.cbMouseKey.SelectedIndex = 1;
                        break;
                    case MouseKey.MIDDLE:
                        av.cbMouseKey.SelectedIndex = 2;
                        break;
                }
            };

            switch (av.SelectedAction)
            {
                case ActionType.KEY_DOWN:
                    av.SetActionTypeCbIndex(0);
                    av.Key = (KeyboardKey)Enum.Parse(typeof(KeyboardKey), jsonObject["key"].ToString());
                    av.keyButton.Content = av.Key.ToString();
                    break;
                case ActionType.KEY_UP:
                    av.SetActionTypeCbIndex(1);
                    av.Key = (KeyboardKey)Enum.Parse(typeof(KeyboardKey), jsonObject["key"].ToString());
                    av.keyButton.Content = av.Key.ToString();
                    break;
                case ActionType.KEY_PRESS:
                    av.SetActionTypeCbIndex(2);
                    av.Key = (KeyboardKey)Enum.Parse(typeof(KeyboardKey), jsonObject["key"].ToString());
                    av.keyButton.Content = av.Key.ToString();

                    // TODO checkboxes ctrl, ...
                    av.chbShift.IsChecked = jsonObject.ContainsKey("shift") && jsonObject["shift"].ToString() == "True";
                    av.chbCtrl.IsChecked = jsonObject.ContainsKey("ctrl") && jsonObject["ctrl"].ToString() == "True";
                    av.chbAlt.IsChecked = jsonObject.ContainsKey("alt") && jsonObject["alt"].ToString() == "True";
                    av.chbMeta.IsChecked = jsonObject.ContainsKey("meta") && jsonObject["meta"].ToString() == "True";
                    break;
                case ActionType.SLEEP:
                    av.SetActionTypeCbIndex(3);
                    av.SetSleepText(jsonObject["duration"].ToString());
                    break;
                case ActionType.MOUSE_DOWN:
                    av.SetActionTypeCbIndex(4);
                    av.SelectedMouseKey = (MouseKey)Enum.Parse(typeof(MouseKey), jsonObject["mousekey"].ToString(), true);
                    updateMouseCbSelection(av.SelectedMouseKey);
                    av.MouseX = Int32.Parse(jsonObject["x"].ToString());
                    av.MouseY = Int32.Parse(jsonObject["y"].ToString());
                    break;
                case ActionType.MOUSE_UP:
                    av.SetActionTypeCbIndex(5);
                    av.SelectedMouseKey = (MouseKey)Enum.Parse(typeof(MouseKey), jsonObject["mousekey"].ToString(), true);
                    updateMouseCbSelection(av.SelectedMouseKey);
                    av.MouseX = Int32.Parse(jsonObject["x"].ToString());
                    av.MouseY = Int32.Parse(jsonObject["y"].ToString());
                    break;
                case ActionType.MOUSE_CLICK:
                    av.SetActionTypeCbIndex(6);
                    av.SelectedMouseKey = (MouseKey)Enum.Parse(typeof(MouseKey), jsonObject["mousekey"].ToString(), true);
                    updateMouseCbSelection(av.SelectedMouseKey);
                    av.MouseX = Int32.Parse(jsonObject["x"].ToString());
                    av.MouseY = Int32.Parse(jsonObject["y"].ToString());
                    break;
                case ActionType.MOUSE_DOUBLE_CLICK:
                    av.SetActionTypeCbIndex(7);
                    av.SelectedMouseKey = (MouseKey)Enum.Parse(typeof(MouseKey), jsonObject["mousekey"].ToString(), true);
                    updateMouseCbSelection(av.SelectedMouseKey);
                    av.MouseX = Int32.Parse(jsonObject["x"].ToString());
                    av.MouseY = Int32.Parse(jsonObject["y"].ToString());
                    break;
                case ActionType.TEXT:
                    av.SetActionTypeCbIndex(8);
                    av.SetTextText(jsonObject["text"].ToString());
                    break;
                case ActionType.REPEAT:
                    av.SetActionTypeCbIndex(9);
                    break;
            }

            return av;
        }
    }
}
