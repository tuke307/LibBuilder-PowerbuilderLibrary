// project=LibBuilder.WPF.Core, file=ProcessMainView.xaml.cs, creation=2020:7:21 Copyright
// (c) 2020 Timeline Financials GmbH & Co. KG. All rights reserved.
namespace LibBuilder.WPF.Core.Views
{
    using LibBuilder.WPF.Core.Region;
    using LibBuilder.WPF.Core.ViewModels;
    using MvvmCross.Platforms.Wpf.Views;

    /// <summary>
    /// </summary>
    /// <seealso cref="MvvmCross.Platforms.Wpf.Views.MvxWpfView{LibBuilder.WPF.Core.ViewModels.ProcessMainViewModel}" />
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