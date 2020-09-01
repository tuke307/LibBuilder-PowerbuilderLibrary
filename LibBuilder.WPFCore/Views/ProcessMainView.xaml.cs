// project=LibBuilder.WPFCore, file=ProcessMainView.xaml.cs, creation=2020:7:21 Copyright
// (c) 2020 Timeline Financials GmbH & Co. KG. All rights reserved.
namespace LibBuilder.WPFCore.Views
{
    using LibBuilder.WPFCore.Region;
    using LibBuilder.WPFCore.ViewModels;
    using MvvmCross.Platforms.Wpf.Views;

    /// <summary>
    /// </summary>
    /// <seealso cref="MvvmCross.Platforms.Wpf.Views.MvxWpfView{LibBuilder.WPFCore.ViewModels.ProcessMainViewModel}" />
    /// <seealso cref="System.Windows.Markup.IComponentConnector" />
    [MvxWpfPresenter("MainWindowRegion", mvxViewPosition.NewOrExsist)]
    public partial class ProcessMainView : MvxWpfView<ProcessMainViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessMainView" /> class.
        /// </summary>
        public ProcessMainView()
        {
            InitializeComponent();
        }
    }
}