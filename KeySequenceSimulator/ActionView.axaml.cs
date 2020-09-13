using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace KeySequenceSimulator
{
    public class ActionView : UserControl
    {
        public ActionView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
