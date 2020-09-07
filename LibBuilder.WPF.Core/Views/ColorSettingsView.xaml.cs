// project=LibBuilder.WPF.Core, file=ColorSettingsView.xaml.cs, creation=2020:7:21
// Copyright (c) 2020 Timeline Financials GmbH & Co. KG. All rights reserved.
namespace LibBuilder.WPF.Core.Views
{
    using LibBuilder.WPF.Core.Region;
    using LibBuilder.WPF.Core.ViewModels;
    using MvvmCross.Platforms.Wpf.Views;
    using System.Windows.Controls;

    /// <summary>
    /// ColorSettingsView.
    /// </summary>
    /// <seealso cref="MvvmCross.Platforms.Wpf.Views.MvxWpfView{LibBuilder.WPF.Core.ViewModels.ColorSettingsViewModel}" />
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