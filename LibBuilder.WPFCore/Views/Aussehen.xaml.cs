using System.Windows.Controls;

namespace LibBuilder.WPFCore.Views
{
    public partial class Aussehen : UserControl
    {
        public Aussehen()
        {
            InitializeComponent();

            DataContext = new LibBuilder.WPFCore.ViewModels.AussehenViewModel();
        }
    }
}