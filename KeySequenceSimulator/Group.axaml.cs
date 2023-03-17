using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using KeySequenceSimulator.ActionSimulator;
using System;
using System.Collections.Generic;
using System.Threading;

namespace KeySequenceSimulator
{
    public class Group : UserControl
    {
        public MainWindow mainWindow { get; set; }

        private volatile bool _isRunning;
        public bool IsRunning
        {
            get { return _isRunning; }
            set { _isRunning = value; }
        }

        private string _heading;
        public string Heading
        {
            get { return _heading; }
            set { _heading = value; txtGroupHeader.Text = value; }
        }

        public bool IsActive { get; private set; }

        private Panel contentPanel;
        private Border groupContentBorder;
        private TextBox txtGroupHeader;
        private Button minMaxButton;
        private Button hotkeyButton;
        private Button btnIsActive;

        public KeyboardKey Hotkey { get; set; }
        private bool IsListening { get; set; }

        public List<Sequence> Sequences { get; private set; }

        public Group()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            contentPanel = this.FindControl<Panel>("groupContentPanel");
            groupContentBorder = this.FindControl<Border>("groupContentBorder");
            txtGroupHeader = this.FindControl<TextBox>("txtGroupHeader");
            minMaxButton = this.FindControl<Button>("btnGroupMinimize");
            hotkeyButton = this.FindControl<Button>("btnHotkey");
            btnIsActive = this.FindControl<Button>("btnActive");

            Sequences = new List<Sequence>();

            IsRunning = false;
            IsListening = false;
            IsActive = true;
        }

        // attaches a listener which sets max width to window width minus margin so sequences can be wrapped around
        public void AttachResizeListener()
        {
            if (mainWindow != null)
                mainWindow.GetObservable(TopLevel.ClientSizeProperty).Subscribe(size => this.MaxWidth = size.Width - 10);
        }

        // Listens for a key press and sets the pressed key as new hotkey.
        public void ChangeHotkey(object sender, KeyEventArgs e)
        {
            IsListening = true;
            var newHotkey = Util.AvaloniaKeyToKeyboardKey(e.Key);

            //TODO safety return check
            //if (newHotkey == '\x00')
            //    hotkeyButton.Content = "Invalid char. Try again.";
            if (!mainWindow.HotkeyAvailable(newHotkey) && newHotkey != Hotkey)
            {
                hotkeyButton.Content = "Hotkey already in use. Try again.";
            }
            else
            {
                SetHotkey(newHotkey);

                // remove listener
                mainWindow.KeyUp -= ChangeHotkey;
                IsListening = false;
            }
        }

        // Registers Hook for given hotkey and updates UI if key is available and not the current hotkey.
        public void SetHotkey(KeyboardKey key)
        {
            if (mainWindow.HotkeyAvailable(key) && key != Hotkey)
            {
                // Remove hook for old key
                mainWindow.GlobalInput.RemoveHook(Hotkey);

                Hotkey = key;
                //TODO convert hotkey to char properly
                // register global input hook
                mainWindow.GlobalInput.RegisterHook(Hotkey, () =>
                {
                    // set running to true only if at least one sequence is active. Stop running if at least one sequence is currently running
                    IsRunning = (!Sequences.Exists(s => s.IsRunning) || (!IsRunning && Sequences.Exists(s => s.IsActive))) && mainWindow.IsGloballyActive && IsActive;
                    mainWindow.UpdateStatus();
                    if (!IsRunning)
                        return;

                    // start background thread for each active sequence
                    foreach (var seq in Sequences)
                    {
                        //MessageBox.Show(null, "hook called. seq.IsActive = " + seq.IsActive, "Error", MessageBox.MessageBoxButtons.Ok);
                        if (seq.IsActive)
                        {
                            Thread t = new Thread(() =>
                            {
                                Thread.CurrentThread.IsBackground = true;
                                seq.Execute();
                            });
                            t.Start();
                        }
                    }
                });

                hotkeyButton.Content = "Hotkey: " + Hotkey;
            }
        }

        // Registers Hook for given hotkey and updates UI
        public void SetHotkey(string key)
        {
            KeyboardKey parsedKey;

            // Only set hotkey if it can be parsed into KeyboardKey Enum
            if (Enum.TryParse(key, out parsedKey) && Enum.IsDefined(typeof(KeyboardKey), parsedKey))
                SetHotkey((KeyboardKey)Enum.Parse(typeof(KeyboardKey), key));
        }

        public void SetIsActive(bool isActive)
        {
            IsActive = isActive;
            btnIsActive.Content = IsActive ? "Active" : "Inactive";
            if (IsActive)
                Classes.Remove("inactive");
            else
                Classes.Add("inactive");
        }

        public void ToggleIsActive(object sender, RoutedEventArgs e)
        {
            SetIsActive(!IsActive);
        }

        // adds a sequence
        public void AddSequence(object sender, RoutedEventArgs e)
        {
            // Add sequence on Click
            Sequence seq = new Sequence();
            seq.Group = this;
            seq.SetValue(DockPanel.DockProperty, Dock.Top);
            Sequences.Add(seq);
            seq.SetSequenceButtonNumber(Sequences.Count);
            contentPanel.Children.Add(seq);
        }

        // shows or hides group
        public void MinMaxGroup(object sender, RoutedEventArgs e)
        {
            // show / hide content using animation defined as Style
            var visible = minMaxButton.Content.ToString() == "-";
            minMaxButton.Content = visible ? "o" : "-";

            if (visible)
                groupContentBorder.Classes.Add("hidden");
            else
                groupContentBorder.Classes.Remove("hidden");

            // change tooltip and reopen to refresh content
            ToolTip.SetTip(minMaxButton, visible ? "Maximize Group" : "Minimize Group");
            if (ToolTip.GetIsOpen(minMaxButton))
            {
                ToolTip.SetIsOpen(minMaxButton, false);
                ToolTip.SetIsOpen(minMaxButton, true);
            }
        }

        public void RemoveGroup(object sender, RoutedEventArgs e)
        {
            // Remove this group
            mainWindow.RemoveGroup(this);
        }

        public void ListenForHotkey(object sender, RoutedEventArgs e)
        {
            // Waits for input hotkey
            hotkeyButton.Content = "Waiting for input...";

            // add keylistener
            if (!IsListening)
                mainWindow.KeyUp += ChangeHotkey;
        }

        public string ToJson()
        {
            string json = "{\n\t\"hotkey\" : \"" + Hotkey + "\",\n";
            json += "\t\"active\" : \"" + IsActive + "\",\n";

            json += "\t\"sequences\" : [\n";
            for (int i = 0; i < Sequences.Count; i++)
            {
                json += "\t\t" + Sequences[i].ToJson() + (i == Sequences.Count - 1 ? "\n" : ",\n");
            }

            json += "\t]\n}";
            return json;
        }
    }
}
