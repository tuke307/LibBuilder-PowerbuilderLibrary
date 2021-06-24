// project=LibBuilder.WPF.Core, file=OngoingProcessView.xaml.cs, create=09:16 Copyright
// (c) 2021 Timeline Financials GmbH & Co. KG. All rights reserved.
namespace LibBuilder.WPF.Core.Views
{
    using LibBuilder.WPF.Core.ViewModels;
    using MvvmCross.Platforms.Wpf.Views;
    using Mvx.Wpf.ItemsPresenter;

    /// <summary>
    /// OngoingProcessView.
    /// </summary>
    [MvxWpfPresenter("ProcessMainRegion", mvxViewPosition.New)]
    public partial class OngoingProcessView : MvxWpfView<OngoingProcessViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OngoingProcessView" /> class.
        /// </summary>
        public OngoingProcessView()
        {
            InitializeComponent();
        }
    }
}