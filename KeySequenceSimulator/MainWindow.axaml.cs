using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using KeySequenceSimulator.ActionSimulator;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace KeySequenceSimulator
{
    public class MainWindow : Window
    {
        private Panel mainPanel;
        public List<Group> Groups { get; private set; }

        private string saveFile;

        public IGlobalInput GlobalInput { get; private set;  }

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

            Groups = new List<Group>();

            // create global input hook once
            GlobalInput = new GlobalInputWindows();

            this.Closed += MainWindow_Closed;
        }

        private void MainWindow_Closed(object sender, System.EventArgs e)
        {
            GlobalInput.Dispose();
        }

        public void AddGroup(object sender, RoutedEventArgs e)
        {
            // Add group on Click
            Group group = new Group();
            group.mainWindow = this;
            group.SetValue(DockPanel.DockProperty, Dock.Top);
            Groups.Add(group);
            group.SetGroupHeaderText("Group " + Groups.Count);
            mainPanel.Children.Add(group);
        }

        public void RemoveGroup(Group g)
        {
            Groups.Remove(g);
            mainPanel.Children.Remove(g);
        }

        public void Save(object sender, RoutedEventArgs e)
        {
            // if saveFile is null, show a dialog. Otherwise overwrite into saveFile
            if (saveFile != null)
                File.WriteAllText(saveFile, ToJson());
            else
                SaveAs(sender, e);
        }

        public async void SaveAs(object sender, RoutedEventArgs e)
        {
            var dlg = new SaveFileDialog();
            dlg.Filters.Add(new FileDialogFilter() { Name = "Json", Extensions = { "json" } });
            var result = await dlg.ShowAsync(this);

            if (result != null)
            {
                File.WriteAllText(result, ToJson());
                saveFile = result;
            }
        }

        public async void Load(object sender, RoutedEventArgs e)
        {
            // show dialog for opening saved file
            var dlg = new OpenFileDialog();
            dlg.AllowMultiple = false;
            dlg.Filters.Add(new FileDialogFilter() { Name = "Json", Extensions = { "json" } });
            var result = await dlg.ShowAsync(this);

            if (result != null)
            {
                string json = File.ReadAllText(result[0]);
                ParseJson(json);
            }
        }

        private void ParseJson(string json)
        {
            var values = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(json);
            if (values["groups"] != null)
            {
                foreach (var group in values["groups"])
                {
                    AddGroup(null, null);
                    var g = Groups[Groups.Count - 1];
                    g.Hotkey = group["hotkey"] ?? "";
                    foreach (var seq in group["sequences"])
                    {
                        g.AddSequence(null, null);
                        var s = g.Sequences[g.Sequences.Count - 1];
                        s.IsActive = seq["active"] ?? true;
                        foreach (var action in seq["actions"])
                        {
                            s.AddAction(null, null);
                            var a = s.Actions[s.Actions.Count - 1];
                            ActionView.FromJson(action, a);
                        }
                    }
                }
            }

        }

        public string ToJson()
        {
            string json = "{\n\t\"groups\" : [\n";

            for (int i = 0; i < Groups.Count; i++)
            {
                json += "\t" + Groups[i].ToJson() + (i == Groups.Count - 1 ? "\n" : ",\n");
            }

            json += "\t]\n}";
            return json;
        }

        public bool HotkeyAvailable(char hotkey)
        {
            return !GlobalInput.GetRegisteredHotkeys().Contains(hotkey);
        }

        public void Exit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
