using Avalonia;
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
        }

        public void SetSequenceButtonNumber(int number)
        {
            btnSequenceNumber.Content = System.Convert.ToString(number);
        }

        public void AddAction(object sender, RoutedEventArgs e)
        {
            // Add action and arrow on Click
            ActionView action = new ActionView();
            action.SetValue(DockPanel.DockProperty, Dock.Right);
            actions.Add(action);

            ArrowRight arr = new ArrowRight();
            arr.SetValue(DockPanel.DockProperty, Dock.Right);

            sequencePanel.Children.Insert(sequencePanel.Children.Count - 1, action);
            sequencePanel.Children.Insert(sequencePanel.Children.Count - 1, arr);
        }

    }
}
