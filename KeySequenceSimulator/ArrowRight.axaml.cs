using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace KeySequenceSimulator
{
    public class ArrowRight : UserControl
    {
        public ArrowRight()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
