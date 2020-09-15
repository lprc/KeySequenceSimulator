﻿using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Collections.Generic;

namespace KeySequenceSimulator
{
    public class Sequence : UserControl
    {
        private Button btnSequenceNumber;
        private Panel sequencePanel;
        private Button btnAddAction;

        private List<ActionView> actions = new List<ActionView>();

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
            btnAddAction.SetValue(IsVisibleProperty, !repeat);
            if (repeat)
            {
                // remove last arrow
                sequencePanel.Children.RemoveAt(sequencePanel.Children.Count - 2);
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
}
