using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Collections.Generic;

namespace KeySequenceSimulator
{
    public class MainWindow : Window
    {
        private Panel mainPanel;
        private List<Group> groups = new List<Group>();
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            mainPanel = this.FindControl<Panel>("mainPanel");

            // select and set AcionInput instance
        }

        public void AddGroup(object sender, RoutedEventArgs e)
        {
            // Add group on Click
            Group group = new Group();
            group.mainWindow = this;
            group.SetValue(DockPanel.DockProperty, Dock.Top);
            groups.Add(group);
            group.SetGroupHeaderText("Group " + groups.Count);
            mainPanel.Children.Add(group);
        }

        public void RemoveGroup(Group g)
        {
            groups.Remove(g);
            mainPanel.Children.Remove(g);
        }

        //TODO set platform for IActionSimulator
    }
}
