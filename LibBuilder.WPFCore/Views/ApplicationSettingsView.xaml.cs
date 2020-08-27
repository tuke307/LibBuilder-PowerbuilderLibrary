using LibBuilder.WPFCore.Region;
using LibBuilder.WPFCore.ViewModels;
using MvvmCross.Platforms.Wpf.Views;

namespace LibBuilder.WPFCore.Views
{
    [MvxWpfPresenter("MainWindowRegion", mvxViewPosition.NewOrExsist)]
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class ApplicationSettingsView : MvxWpfView<ApplicationSettingsViewModel>
    {
        public ApplicationSettingsView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ApplicationSettings.Default.Save();
        }
    }
}