using LibBuilder.Core.ViewModels;
using System.Windows.Controls;

namespace LibBuilder.WPF.Views
{
    public partial class Processes : UserControl
    {
        public Processes()
        {
            InitializeComponent();

            DataContext = new ProcessHistoryViewModel();
        }
    }
}