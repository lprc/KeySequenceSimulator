using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;

namespace KeySequenceSimulator
{
    public class Group : UserControl
    {
        private DockPanel contentPanel;

        public Group()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            contentPanel = this.FindControl<DockPanel>("groupContentPanel");
        }

        public void AddSequence(object sender, RoutedEventArgs e)
        {
            // Add sequence on Click
            Sequence seq = new Sequence();
            seq.SetValue(DockPanel.DockProperty, Dock.Top);
            contentPanel.Children.Add(seq);
        }
    }
}
