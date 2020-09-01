// project=LibBuilder.WPFCore, file=ColorSettingsView.xaml.cs, creation=2020:7:21
// Copyright (c) 2020 Timeline Financials GmbH & Co. KG. All rights reserved.
namespace LibBuilder.WPFCore.Views
{
    using LibBuilder.WPFCore.Region;
    using LibBuilder.WPFCore.ViewModels;
    using MvvmCross.Platforms.Wpf.Views;
    using System.Windows.Controls;

    /// <summary>
    /// ColorSettingsView.
    /// </summary>
    /// <seealso cref="MvvmCross.Platforms.Wpf.Views.MvxWpfView{LibBuilder.WPFCore.ViewModels.ColorSettingsViewModel}" />
    /// <seealso cref="System.Windows.Markup.IComponentConnector" />
    [MvxWpfPresenter("MainWindowRegion", mvxViewPosition.NewOrExsist)]
    public partial class ColorSettingsView : MvxWpfView<ColorSettingsViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ColorSettingsView" /> class.
        /// </summary>
        public ColorSettingsView()
        {
            InitializeComponent();
        }
    }
}