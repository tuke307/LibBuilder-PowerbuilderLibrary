// project=LibBuilder.WPFCore, file=ProcessSettingsView.xaml.cs, creation=2020:8:24
// Copyright (c) 2020 Timeline Financials GmbH & Co. KG. All rights reserved.
namespace LibBuilder.WPFCore.Views
{
    using LibBuilder.WPFCore.Region;
    using LibBuilder.WPFCore.ViewModels;
    using MvvmCross.Platforms.Wpf.Views;

    /// <summary>
    /// Interaction logic for MainView.xaml
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