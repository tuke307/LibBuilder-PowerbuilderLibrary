using LibBuilder.ViewModels;
using System.Windows.Controls;

namespace LibBuilder.Views
{
    /// <summary>
    /// Interaction logic for ProcessHistory.xaml
    /// </summary>
    public partial class Processes : UserControl
    {
        public Processes()
        {
            InitializeComponent();

            DataContext = new ProcessHistoryViewModel();
        }
    }
}