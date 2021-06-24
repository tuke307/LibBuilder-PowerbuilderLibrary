// project=LibBuilder.WPF.Core, file=ProcessSettingsView.xaml.cs, create=09:16 Copyright
// (c) 2021 Timeline Financials GmbH & Co. KG. All rights reserved.
namespace LibBuilder.WPF.Core.Views
{
    using LibBuilder.WPF.Core.ViewModels;
    using MvvmCross.Platforms.Wpf.Views;
    using Mvx.Wpf.ItemsPresenter;

    /// <summary>
    /// ProcessSettingsView.
    /// </summary>
    [MvxWpfPresenter("ProcessMainRegion", mvxViewPosition.NewOrExsist)]
    public partial class ProcessSettingsView : MvxWpfView<ProcessSettingsViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessSettingsView" /> class.
        /// </summary>
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