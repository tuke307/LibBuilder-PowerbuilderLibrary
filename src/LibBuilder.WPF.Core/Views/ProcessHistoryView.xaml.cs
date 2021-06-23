// project=LibBuilder.WPF.Core, file=ProcessHistoryView.xaml.cs, create=09:16 Copyright
// (c) 2021 Timeline Financials GmbH & Co. KG. All rights reserved.
namespace LibBuilder.WPF.Core.Views
{
    using LibBuilder.WPF.Core.Region;
    using LibBuilder.WPF.Core.ViewModels;
    using MvvmCross.Platforms.Wpf.Views;

    /// <summary>
    /// ProcessHistory.
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