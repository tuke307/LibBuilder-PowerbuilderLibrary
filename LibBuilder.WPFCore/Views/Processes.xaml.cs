// project=LibBuilder.WPFCore, file=Processes.xaml.cs, creation=2020:7:21 Copyright (c)
// 2020 Timeline Financials GmbH & Co. KG. All rights reserved.
using LibBuilder.Core.ViewModels;
using System.Windows.Controls;

namespace LibBuilder.WPFCore.Views
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