using LibBuilder.ViewModels;
using System.Windows.Controls;

namespace LibBuilder.Views
{
    public partial class Aussehen : UserControl
    {
        public Aussehen()
        {
            InitializeComponent();

            DataContext = new AussehenViewModel();
        }
    }
}