// project=LibBuilder.WPF.Core, file=MainView.xaml.cs, creation=2020:8:24 Copyright (c)
// 2020 Timeline Financials GmbH & Co. KG. All rights reserved.
namespace LibBuilder.WPF.Core.Views
{
    using LibBuilder.WPF.Core.Region;
    using LibBuilder.WPF.Core.ViewModels;
    using MvvmCross.Platforms.Wpf.Views;

    /// <summary>
    /// Interaction logic for MainView.xaml
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