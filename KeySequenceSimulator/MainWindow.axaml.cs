using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace KeySequenceSimulator
{
    public class MainWindow : Window
    {
        private Button btnAddGroup;
        private Panel mainPanel;
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
            btnAddGroup = this.FindControl<Button>("btnAddGroup");
            mainPanel = this.FindControl<Panel>("mainPanel");
        }

        public void AddGroup(object sender, RoutedEventArgs e)
        {
            // Add sequence on Click
            Group group = new Group();
            group.SetValue(DockPanel.DockProperty, Dock.Top);
            mainPanel.Children.Add(group);
        }
    }
}
