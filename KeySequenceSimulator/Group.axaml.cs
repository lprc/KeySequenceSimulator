using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace KeySequenceSimulator
{
    public class Group : UserControl
    {
        public Group()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
