// project=LibBuilder.WPFCore, file=ProcessHistoryView.xaml.cs, creation=2020:7:21
// Copyright (c) 2020 Timeline Financials GmbH & Co. KG. All rights reserved.
namespace LibBuilder.WPFCore.Views
{
    using LibBuilder.WPFCore.Region;
    using LibBuilder.WPFCore.ViewModels;
    using MvvmCross.Platforms.Wpf.Views;

    /// <summary>
    /// Interaction logic for ProcessHistory.xaml
    /// </summary>
    [MvxWpfPresenter("MainWindowRegion", mvxViewPosition.NewOrExsist)]
    public partial class ProcessHistoryView : MvxWpfView<ProcessHistoryViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessHistoryView" /> class.
        /// </summary>
        public ProcessHistoryView()
        {
            InitializeComponent();
        }
    }
}