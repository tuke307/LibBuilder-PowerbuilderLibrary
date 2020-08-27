// project=LibBuilder.WPFCore, file=Processes.xaml.cs, creation=2020:7:21 Copyright (c)
// 2020 Timeline Financials GmbH & Co. KG. All rights reserved.
using LibBuilder.WPFCore.Region;
using LibBuilder.WPFCore.ViewModels;
using MvvmCross.Platforms.Wpf.Views;

namespace LibBuilder.WPFCore.Views
{
    [MvxWpfPresenter("MainWindowRegion", mvxViewPosition.NewOrExsist)]
    /// <summary>
    /// Interaction logic for ProcessHistory.xaml
    /// </summary>
    public partial class ProcessHistoryView : MvxWpfView<ProcessHistoryViewModel>
    {
        public ProcessHistoryView()
        {
            InitializeComponent();
        }
    }
}