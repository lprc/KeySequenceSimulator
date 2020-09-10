using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;
using System.Collections.Generic;

namespace KeySequenceSimulator
{
    public class Group : UserControl
    {
        public MainWindow MainWindow { get; set; }

        private Panel contentPanel;
        private Border groupContentBorder;
        private TextBlock txtGroupHeader;
        private Button minMaxButton;

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
        }

        public void RemoveGroup(object sender, RoutedEventArgs e)
        {
            // Remove this group
            MainWindow.RemoveGroup(this);
        }
    }
}
