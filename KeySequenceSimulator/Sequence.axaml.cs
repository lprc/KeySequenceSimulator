using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace KeySequenceSimulator
{
    public class Sequence : UserControl
    {
        private Button btnSequenceNumber;
        public Sequence()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            btnSequenceNumber = this.FindControl<Button>("btnSequenceNumber");
        }

        public void SetSequenceButtonNumber(int number)
        {
            btnSequenceNumber.Content = System.Convert.ToString(number);
        }
    }
}
