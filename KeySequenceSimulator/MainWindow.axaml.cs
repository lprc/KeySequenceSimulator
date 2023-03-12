using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.Styling;
using KeySequenceSimulator.ActionSimulator;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace KeySequenceSimulator
{
    public class MainWindow : Window
    {
        private Panel mainPanel;
        private TextBlock lblStatus;
        private CheckBox cbIsGloballyActive;
        private MenuItem menuRecentFiles;
        public List<Group> Groups { get; private set; }

        public bool IsGloballyActive { get; private set; }

        private string saveFile;

        public IGlobalInput GlobalInput { get; private set; }

        private const string TITLE_PREFIX = "KeySequenceSimulator";

        private RecentFiles recentFiles = new RecentFiles();

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
            lblStatus = this.FindControl<TextBlock>("lblStatus");
            cbIsGloballyActive = this.FindControl<CheckBox>("cbIsGlobalActive");
            menuRecentFiles = this.FindControl<MenuItem>("menuRecentFiles");

            Groups = new List<Group>();
            IsGloballyActive = true;

            // create global input hook once
            GlobalInput = new GlobalInputWindows();

            this.Closed += MainWindow_Closed;

            // load recent files to menu
            UpdateRecentFilesMenu();

            // use dark theme
            Application.Current.RequestedThemeVariant = Avalonia.Styling.ThemeVariant.Dark;
        }

        private void UpdateRecentFilesMenu()
        {
            List<MenuItem> items = new List<MenuItem>();
            foreach (var file in recentFiles.LoadList())
            {
                var mu = new MenuItem();
                mu.Header = file;
                mu.Click += (sender, e) => LoadFile(file);
                items.Add(mu);
            }

            // add list to menu
            menuRecentFiles.Items = items.ToArray();
        }

        private void MainWindow_Closed(object sender, System.EventArgs e)
        {
            GlobalInput.Dispose();
        }

        public void ToggleGlobalActive(object sender, RoutedEventArgs e)
        {
            IsGloballyActive = !IsGloballyActive;
            cbIsGloballyActive.IsChecked = IsGloballyActive;
            UpdateStatus();
        }

        public void AddGroup(object sender, RoutedEventArgs e)
        {
            // Add group on Click
            Group group = new Group();
            group.mainWindow = this;
            group.AttachResizeListener();
            group.SetValue(DockPanel.DockProperty, Dock.Top);
            Groups.Add(group);
            group.Heading = GenerateGroupName();
            mainPanel.Children.Add(group);
        }

        private string GenerateGroupName()
        {
            for (int i = 1; i <= 100; i++)
                if (!Groups.Exists(g => g.Heading == "Group " + i))
                    return "Group " + i;
            return "";
        }

        public void RemoveGroup(Group g)
        {
            Groups.Remove(g);
            mainPanel.Children.Remove(g);
        }

        private void UpdateTitle()
        {
            Title = TITLE_PREFIX + " - " + saveFile;
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

            if (result != null && result.Length > 0)
            {
                File.WriteAllText(result, ToJson());
                saveFile = result;
                UpdateTitle();

                // add to recent files list
                recentFiles.AddFile(result);
                UpdateRecentFilesMenu();
            }
        }

        public void LoadFile(string file)
        {
            // remove existing groups first
            for (int i = Groups.Count - 1; i >= 0; i--)
                RemoveGroup(Groups[i]);

            string json = File.ReadAllText(file);
            ParseJson(json);
            saveFile = file;
            UpdateTitle();

            // add to recent files list
            recentFiles.AddFile(file);
            UpdateRecentFilesMenu();
        }

        public async void Load(object sender, RoutedEventArgs e)
        {
            // ask if changes should be saved before loading new one
            if (Groups.Count > 0)
            {
                var res = await MessageBox.Show(null, "Do you want to save existing changes?", "Save first?", MessageBox.MessageBoxButtons.YesNo);
                if (res == MessageBox.MessageBoxResult.Yes)
                    Save(null, null);
            }

            // show dialog for opening saved file
            var dlg = new OpenFileDialog();
            dlg.AllowMultiple = false;
            dlg.Filters.Add(new FileDialogFilter() { Name = "Json", Extensions = { "json" } });
            var result = await dlg.ShowAsync(this);

            if (result != null && result.Length > 0)
                LoadFile(result[0]);
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
            string json = "{\n\"groups\" : [\n";

            for (int i = 0; i < Groups.Count; i++)
            {
                json += "\t" + Groups[i].ToJson() + (i == Groups.Count - 1 ? "\n" : ",\n");
            }

            json += "]\n}";
            return json;
        }

        public bool HotkeyAvailable(KeyboardKey hotkey)
        {
            return !GlobalInput.GetRegisteredHotkeys().Contains(hotkey);
        }

        public void Exit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void UpdateStatus()
        {
            string status = "";
            if (!IsGloballyActive)
            {
                status += "Deactivated globally";
                lblStatus.Classes.Add("red");
            }
            else
            {
                List<string> actives = new List<string>();
                for (int i = 0; i < Groups.Count; i++)
                {
                    var g = Groups[i];
                    if (g.IsRunning)
                        for (int j = 0; j < g.Sequences.Count; j++)
                        {
                            var seq = g.Sequences[j];
                            if (seq.IsActive && seq.IsRunning)
                                actives.Add((i + 1) + "." + (j + 1));
                        }
                }
                status += string.Join(", ", actives);
                lblStatus.Classes.Remove("red");
            }
            
            lblStatus.Text = status;
        }
    }
}
