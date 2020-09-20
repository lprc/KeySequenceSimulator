﻿using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Collections.Generic;

namespace KeySequenceSimulator
{
    public class Sequence : UserControl
    {
        public Group group { get; set; }

        private Button btnSequenceNumber;
        private Panel sequencePanel;
        private Button btnAddAction;

        private List<ActionView> actions = new List<ActionView>();

        public volatile bool _repeat;
        public bool Repeat
        {
            get { return _repeat; }
            set { _repeat = value; }
        }

        public volatile bool _isActive;
        public bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; }
        }

        public Sequence()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            btnSequenceNumber = this.FindControl<Button>("btnSequenceNumber");
            sequencePanel = this.FindControl<Panel>("sequencePanel");
            btnAddAction = this.FindControl<Button>("btnAddAction");

            IsActive = true;
        }

        public void SetSequenceButtonNumber(int number)
        {
            btnSequenceNumber.Content = System.Convert.ToString(number);
        }

        public void AddAction(object sender, RoutedEventArgs e)
        {
            // Add action and arrow on Click
            ActionView action = new ActionView();
            action.ParentSequence = this;
            action.SetValue(DockPanel.DockProperty, Dock.Right);
            actions.Add(action);

            ArrowRight arr = new ArrowRight();
            arr.SetValue(DockPanel.DockProperty, Dock.Right);

            sequencePanel.Children.Insert(sequencePanel.Children.Count - 1, action);
            sequencePanel.Children.Insert(sequencePanel.Children.Count - 1, arr);
        }

        // replaces add action button with repeat if repeat is true
        public void SetRepeatSequence(bool repeat)
        {
            if (Repeat != repeat)
            {
                Repeat = repeat;
                btnAddAction.SetValue(IsVisibleProperty, !repeat);
                if (repeat)
                {
                    // remove last arrow
                    sequencePanel.Children.RemoveAt(sequencePanel.Children.Count - 2);
                }
                else
                {
                    // add last arrow
                    ArrowRight arr = new ArrowRight();
                    arr.SetValue(DockPanel.DockProperty, Dock.Right);
                    sequencePanel.Children.Insert(sequencePanel.Children.Count - 1, arr);
                }
            } 
        }

        public void ToggleActive(object sender, RoutedEventArgs e)
        {
            IsActive = !IsActive;
            if (IsActive)
                btnSequenceNumber.Classes.Remove("Inactive");
            else
                btnSequenceNumber.Classes.Add("Inactive");
        }

        // executes the sequence. Stops if Group.IsRunning is false
        public void Execute()
        {
            do
            {
                foreach (var action in actions)
                {
                    if (!group.IsRunning || !IsActive)
                        return;
                    action.Execute();
                }
            } while (group.IsRunning && Repeat && IsActive);
        }

        public string ToJson()
        {
            string json = "{\n\tactions: [\n";

            for (int i = 0; i < actions.Count; i++)
            {
                json += actions[i].ToJson() + (i == actions.Count - 1 ? "\n" : ",\n");
            }

            json += "\t]\n}";
            return json;
        }
    }
}
