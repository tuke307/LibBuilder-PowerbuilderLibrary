// project=LibBuilder.WPF.Core, file=MainView.xaml.cs, create=09:16 Copyright (c) 2021
// Timeline Financials GmbH & Co. KG. All rights reserved.
namespace LibBuilder.WPF.Core.Views
{
    using LibBuilder.WPF.Core.Region;
    using LibBuilder.WPF.Core.ViewModels;
    using MvvmCross.Platforms.Wpf.Views;

    /// <summary>
    /// MainView.
    /// </summary>
    [MvxWpfPresenter("NavigationRegion", mvxViewPosition.NewOrExsist)]
    public partial class MainView : MvxWpfView<MainViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainView" /> class.
        /// </summary>
        public MainView()
        {
            InitializeComponent();
        }
    }
}