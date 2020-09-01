// project=LibBuilder.WPFCore, file=MainView.xaml.cs, creation=2020:8:24 Copyright (c)
// 2020 Timeline Financials GmbH & Co. KG. All rights reserved.
namespace LibBuilder.WPFCore.Views
{
    using LibBuilder.WPFCore.Region;
    using LibBuilder.WPFCore.ViewModels;
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