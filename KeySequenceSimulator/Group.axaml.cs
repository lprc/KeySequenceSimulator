﻿using Avalonia;
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

        public volatile bool _isRunning;
        public bool IsRunning
        {
            get { return _isRunning; }
            set { _isRunning = value; }
        }

        private Panel contentPanel;
        private Border groupContentBorder;
        private TextBlock txtGroupHeader;
        private Button minMaxButton;
        private Button hotkeyButton;

        private char hotkey;
        private bool IsListening { get; set; }

        private List<Sequence> sequences = new List<Sequence>();

        public Group()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            contentPanel = this.FindControl<Panel>("groupContentPanel");
            groupContentBorder = this.FindControl<Border>("groupContentBorder");
            txtGroupHeader = this.FindControl<TextBlock>("txtGroupHeader");
            minMaxButton = this.FindControl<Button>("btnGroupMinimize");
            hotkeyButton = this.FindControl<Button>("btnHotkey");

            IsRunning = false;
            IsListening = false;
        }

        public void ChangeHotkey(object sender, KeyEventArgs e)
        {
            IsListening = true;
            hotkey = Util.KeyToChar(e.Key, e.KeyModifiers);

            if (hotkey == '\x00')
                hotkeyButton.Content = "Invalid char. Try again.";
            else
            {
                //TODO convert hotkey to char properly
                // register global input hook
                mainWindow.GlobalInput.RemoveHook(hotkey);
                mainWindow.GlobalInput.RegisterHook(hotkey, () =>
                {
                    // set running to true only if at least one sequence is active
                    IsRunning = !IsRunning && sequences.Exists(s => s.IsActive);
                    if (!IsRunning)
                        return;

                    // start background thread for each active sequence
                    foreach (var seq in sequences)
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

                // remove listener
                mainWindow.KeyUp -= ChangeHotkey;
                IsListening = false;
                hotkeyButton.Content = "Hotkey: " + hotkey;
            }
        }

        public void SetGroupHeaderText(String text)
        {
            txtGroupHeader.Text = text;
        }

        // adds a sequence
        public void AddSequence(object sender, RoutedEventArgs e)
        {
            // Add sequence on Click
            Sequence seq = new Sequence();
            seq.group = this;
            seq.SetValue(DockPanel.DockProperty, Dock.Top);
            sequences.Add(seq);
            seq.SetSequenceButtonNumber(sequences.Count);
            contentPanel.Children.Add(seq);
        }

        // shows or hides group
        public void MinMaxGroup(object sender, RoutedEventArgs e)
        {
            var visible = groupContentBorder.GetValue(IsVisibleProperty);
            minMaxButton.Content = visible ? "o" : "-";
            groupContentBorder.SetValue(IsVisibleProperty, !visible);
            
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

        public void SetHotkey(object sender, RoutedEventArgs e)
        {
            // Waits for input hotkey
            hotkeyButton.Content = "Waiting for input...";

            // add keylistener
            if (!IsListening)
                mainWindow.KeyUp += ChangeHotkey;
        }

        
    }
}
