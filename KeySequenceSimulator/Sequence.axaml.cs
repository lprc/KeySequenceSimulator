using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KeySequenceSimulator
{
    public class Sequence : UserControl
    {
        public Group Group { get; set; }

        private Button btnSequenceNumber;
        private Panel sequencePanel;
        private Button btnAddAction;
        public List<ActionView> Actions { get; set; }

        private volatile bool _repeat;
        public bool Repeat
        {
            get { return _repeat; }
            set { _repeat = value; }
        }

        private volatile bool _isActive;
        public bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; }
        }

        private volatile bool _isRunning;
        public bool IsRunning
        {
            get { return _isRunning; }
            set { _isRunning = value; }
        }

        private bool _uiElementsEnabled = true;
        public bool UIElementsEnabled
        {
            get { return _uiElementsEnabled; }
            set
            {
                if (value)
                    Classes.Remove("Disabled");
                else
                    Classes.Add("Disabled");
            }
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

            Actions = new List<ActionView>();
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
            Actions.Add(action);

            ArrowRight arr = new ArrowRight();
            arr.SetValue(DockPanel.DockProperty, Dock.Right);

            sequencePanel.Children.Insert(sequencePanel.Children.Count - 1, action);
            sequencePanel.Children.Insert(sequencePanel.Children.Count - 1, arr);
        }

        // Removes action from this sequence, if present and if sequence is not running.
        public void RemoveAction(ActionView a)
        {
            if (!IsRunning)
            {
                bool isRepeat = a.SelectedAction == ActionView.ActionType.REPEAT;
                bool removed = Actions.Remove(a);

                if (removed)
                {
                    if (isRepeat)
                        Repeat = false;

                    // First remove arrow, if child is not repeat. Then remove action itself.
                    int idx = sequencePanel.Children.IndexOf(a);

                    if (!isRepeat)
                        sequencePanel.Children.RemoveAt(idx + 1);

                    sequencePanel.Children.RemoveAt(idx);
                }
            }          
        }

        // Move action and its UI Element to the left if it's not the first one and it's not repeat.
        public void MoveActionLeft(ActionView a)
        {
            int idx = Actions.IndexOf(a);

            if (idx > 0 && a.SelectedAction != ActionView.ActionType.REPEAT)
            {
                var tmp = Actions[idx - 1];
                Actions[idx - 1] = Actions[idx];
                Actions[idx] = tmp;

                int idxUi = sequencePanel.Children.IndexOf(a);
                sequencePanel.Children.Move(idxUi, idxUi - 2);
                sequencePanel.Children.Move(idxUi - 1, idxUi);
            }
        }

        public void MoveActionRight(ActionView a)

        {
            int idx = Actions.IndexOf(a);

            if (idx < Actions.Count - 1 && a.SelectedAction != ActionView.ActionType.REPEAT)
            {
                var tmp = Actions[idx + 1];
                Actions[idx + 1] = Actions[idx];
                Actions[idx] = tmp;

                int idxUi = sequencePanel.Children.IndexOf(a);
                sequencePanel.Children.Move(idxUi, idxUi + 2);
                sequencePanel.Children.Move(idxUi + 1, idxUi);
            }
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

        public void SetIsActive(bool isActive)
        {
            IsActive = isActive;
            if (IsActive)
            {
                btnSequenceNumber.Classes.Remove("Inactive");
                this.Classes.Remove("Inactive");
            }
            else
            {
                btnSequenceNumber.Classes.Add("Inactive");
                this.Classes.Add("Inactive");
            }
        }

        public void ToggleActive(object sender, RoutedEventArgs e)
        {
            SetIsActive(!IsActive);
        }

        // executes the sequence. Stops if Group.IsRunning is false
        public void Execute()
        {
            IsRunning = true;
            // update status from ui thread
            Dispatcher.UIThread.Post(() => Group.mainWindow.UpdateStatus());

            Dispatcher.UIThread.Post(() => {
                UIElementsEnabled = false;
                foreach (var action in Actions)
                    action.UIElementsEnabled = false;
            });

            do
            {
                foreach (var action in Actions)
                {
                    if (!Group.IsRunning || !IsActive)
                        return;
                    action.Execute();
                }
            } while (Group.IsRunning && Repeat && IsActive);
            IsRunning = false;

            // update status from ui thread when action is finished
            Dispatcher.UIThread.Post(() => Group.mainWindow.UpdateStatus());

            Dispatcher.UIThread.Post(() => {
                UIElementsEnabled = true;
                foreach (var action in Actions)
                    action.UIElementsEnabled = true;
            });
        }

        public string ToJson()
        {
            string json = "{\n\t\t\"active\" : \"" + IsActive + "\",\n\t\t\"actions\" : [\n";

            for (int i = 0; i < Actions.Count; i++)
            {
                json += Actions[i].ToJson() + (i == Actions.Count - 1 ? "\n" : ",\n");
            }

            json += "\t\t]\n}";
            return json;
        }
    }
}
