using LibBuilder.WPF.ViewModels;
using System.Windows.Controls;

namespace LibBuilder.WPF.Views
{
    public partial class Content : UserControl
    {
        public Content(MainWindowViewModel mainWindowViewModel)
        {
            InitializeComponent();

            DataContext = new ContentViewModel(mainWindowViewModel);

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