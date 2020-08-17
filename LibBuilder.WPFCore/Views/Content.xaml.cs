// project=LibBuilder.WPFCore, file=Content.xaml.cs, creation=2020:7:21 Copyright (c) 2020
// Timeline Financials GmbH & Co. KG. All rights reserved.
using LibBuilder.Core.ViewModels;
using LibBuilder.WPFCore.ViewModels;
using System.Collections.Generic;
using System.Windows.Controls;

namespace LibBuilder.WPFCore.Views
{
    public partial class Content : UserControl
    {
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