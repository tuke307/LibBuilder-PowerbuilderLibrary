// project=LibBuilder.WPFCore, file=Content.xaml.cs, creation=2020:7:21 Copyright (c) 2020
// Timeline Financials GmbH & Co. KG. All rights reserved.
namespace LibBuilder.WPFCore.Views
{
    using LibBuilder.WPFCore.Business;
    using System.Windows.Controls;

    /// <summary>
    /// Content.
    /// </summary>
    /// <seealso cref="System.Windows.Controls.UserControl" />
    /// <seealso cref="System.Windows.Markup.IComponentConnector" />
    public partial class Content : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Content" /> class.
        /// </summary>
        /// <param name="mainWindowViewModel">The main window view model.</param>
        /// <param name="parameter">The parameter.</param>
        public Content(WPFCore.ViewModels.MainWindowViewModel mainWindowViewModel, Options parameter = null)
        {
            InitializeComponent();

            DataContext = new WPFCore.ViewModels.ContentViewModel(mainWindowViewModel, parameter);

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