// project=LibBuilder.WPF.Core, file=ProcessHistoryView.xaml.cs, creation=2020:7:21
// Copyright (c) 2020 Timeline Financials GmbH & Co. KG. All rights reserved.
namespace LibBuilder.WPF.Core.Views
{
    using LibBuilder.WPF.Core.Region;
    using LibBuilder.WPF.Core.ViewModels;
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