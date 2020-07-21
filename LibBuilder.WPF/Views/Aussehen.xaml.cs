using System.Windows.Controls;

namespace LibBuilder.WPF.Views
{
    public partial class Aussehen : UserControl
    {
        public Aussehen()
        {
            InitializeComponent();

            DataContext = new WPF.ViewModels.AussehenViewModel();
        }
    }
}