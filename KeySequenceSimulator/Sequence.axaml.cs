using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace KeySequenceSimulator
{
    public class Sequence : UserControl
    {
        public Sequence()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
