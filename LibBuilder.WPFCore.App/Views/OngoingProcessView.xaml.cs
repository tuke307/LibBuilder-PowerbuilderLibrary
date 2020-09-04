// project=LibBuilder.WPF.Core, file=OngoingProcessView.xaml.cs, creation=2020:8:24
// Copyright (c) 2020 Timeline Financials GmbH & Co. KG. All rights reserved.
namespace LibBuilder.WPF.Core.Views
{
    using LibBuilder.WPF.Core.Region;
    using LibBuilder.WPF.Core.ViewModels;
    using MvvmCross.Platforms.Wpf.Views;

    /// <summary>
    /// Interaction logic for OngoingProcessView.xaml
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