using LibBuilder.Core.ViewModels;
using LibBuilder.WPFCore.ViewModels;
using System.Windows.Controls;

namespace LibBuilder.WPFCore.Views
{
    public partial class Content : UserControl
    {
        public Content(WPFCore.ViewModels.MainWindowViewModel mainWindowViewModel)
        {
            InitializeComponent();

            DataContext = new WPFCore.ViewModels.ContentViewModel(mainWindowViewModel);

            LibDataGrid.Columns[1].Visibility = System.Windows.Visibility.Collapsed;
        }

        private void CompletePath_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            LibDataGrid.Columns[1].Visibility = System.Windows.Visibility.Collapsed;
        }

        private void CompletePath_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            LibDataGrid.Columns[1].Visibility = System.Windows.Visibility.Visible;
        }
    }
}