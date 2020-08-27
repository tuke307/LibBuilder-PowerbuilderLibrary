using LibBuilder.WPFCore.Region;
using LibBuilder.WPFCore.ViewModels;
using MvvmCross.Platforms.Wpf.Views;

namespace LibBuilder.WPFCore.Views
{
    [MvxWpfPresenter("ProcessMainRegion", mvxViewPosition.NewOrExsist)]
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class ProcessSettingsView : MvxWpfView<ProcessSettingsViewModel>
    {
        public ProcessSettingsView()
        {
            InitializeComponent();

            LibDataGrid.Columns[1].Visibility = System.Windows.Visibility.Collapsed;
        }

        private void CompletePath_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            LibDataGrid.Columns[1].Visibility = System.Windows.Visibility.Visible;
        }

        private void CompletePath_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            LibDataGrid.Columns[1].Visibility = System.Windows.Visibility.Collapsed;
        }
    }
}