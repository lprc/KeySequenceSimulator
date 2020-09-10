using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using System;

namespace KeySequenceSimulator
{
    public class AddButton : Button, IStyleable
    {
        public AddButton()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        Type IStyleable.StyleKey => typeof(Button);
    }
}
